using Agora.Core.Commands;
using Agora.Core.Enums;
using Agora.Core.Interfaces.Repositories;
using Agora.Core.Models;
using Agora.Core.Models.DomainQueryParameters;
using Agora.Core.Models.Entities;
using Agora.Core.Shared;
using Agora.Core.Validation.Interfaces;

namespace Agora.Core.Validation;

/// <summary>
/// Default implementation of <see cref="IBusinessRulesValidator"/>.
/// </summary>
public class BusinessRulesValidator(
    IPostRepository postRepo,
    IPostCategoryRepository postCategoryRepo,
    ITransactionStatusRepository transactionStatusRepo,
    ITransactionRepository transactionRepo
    ) : IBusinessRulesValidator
{
    #region PostCategory
    /// <inheritdoc />
    public async Task<Result> ValidateNewPostCategoryAsync(PostCategory postCategory)
    {
        if (await postCategoryRepo.NameExistsAsync(postCategory.Name))
        {
            return Result.Failure(ErrorType.Invalid, ErrorMessages.AlreadyExists("post category name", postCategory.Name));
        }

        return Result.Success();
    }

    /// <inheritdoc />
    public async Task<Result> ValidatePostCategoryUpdateAsync(PostCategory postCategory, UpdatePostCategoryDetailsCommand newDetails)
    {
        if (postCategory.Name == newDetails.Name)
        {
            return Result.Failure(ErrorType.Invalid, ErrorMessages.NewMustBeDifferentFromCurrent("post category name"));
        }
        
        if (await postCategoryRepo.NameExistsAsync(newDetails.Name))
        {
            return Result.Failure(ErrorType.Invalid, ErrorMessages.AlreadyExists("post category name", postCategory.Name));
        }

        return Result.Success();
    }

    /// <inheritdoc />
    public async Task<Result> ValidatePostCategoryDeletionAsync(PostCategory postCategory)
    {
        return await postRepo.IsCategoryInUseAsync(postCategory.Id)
            ? Result.Failure(ErrorType.Invalid, ErrorMessages.PostCategory.InUse)
            : Result.Success();
    }

    #endregion
    
    #region Post
    /// <inheritdoc />
    public async Task<Result> ValidateNewPostAsync(Post newPost, UserContext userContext)
    {
        PostQueryParameters postQueryParameters = new PostQueryParameters()
        {
            UserId = userContext.UserId,
            StatusNames = [PostStatus.Active.ToString(), PostStatus.Inactive.ToString()]
        };

        IReadOnlyList<Post> postsOfUser = await postRepo.GetAllPostsAsync(postQueryParameters);
        List<string> postTitlesOfUser = postsOfUser.Select(p => p.Title).ToList();
        if (postTitlesOfUser.Contains(newPost.Title))
        {
            return Result.Failure(ErrorType.Invalid, ErrorMessages.Post.SameTitle);
        }

        return Result.Success();
    }

    /// <inheritdoc />
    public async Task<Result> ValidatePostUpdateAsync(Post oldPost, UpdatePostDetailsCommand newDetails, UserContext userContext)
    {
        if (newDetails.Title is not null && newDetails.Title != oldPost.Title)
        {
            PostQueryParameters postQueryParameters = new PostQueryParameters()
            {
                UserId = userContext.UserId,
                StatusNames = [PostStatus.Active.ToString(), PostStatus.Inactive.ToString()]
            };

            IReadOnlyList<Post> postsOfUser = await postRepo.GetAllPostsAsync(postQueryParameters);
            List<string> postTitlesOfUser = postsOfUser.Select(p => p.Title).ToList();
            if (postTitlesOfUser.Contains(newDetails.Title))
            {
                return Result.Failure(ErrorType.Invalid, ErrorMessages.Post.SameTitle);
            }
        }
        
        // Admin can force update
        if (userContext.IsAdmin) return Result.Success();
        
        if (await transactionRepo.IsPostInOnGoingTransactionAsync(oldPost.Id))
        {
            return Result.Failure(ErrorType.Invalid, ErrorMessages.Post.InvolvedInOngoingTransaction);
        }
        
        return Result.Success();
    }

    /// <inheritdoc />
    public Result ValidatePostStatusChangeAsync(Post oldPost, PostStatus newStatus)
    {
        return oldPost.Status == newStatus
            ? Result.Failure(ErrorType.Invalid, ErrorMessages.Post.SameStatus(newStatus))
            : Result.Success();
    }
    
    /// <inheritdoc />
    public async Task<Result> ValidatePostDeletionAsync(Post post, UserContext userContext)
    {
        if (post.Status == PostStatus.Deleted)
        {
            return Result.Failure(ErrorType.Invalid, ErrorMessages.AlreadyDeleted("post"));
        }

        // Admin can force deletion
        if (userContext.IsAdmin) return Result.Success();
        
        if (await transactionRepo.IsPostInOnGoingTransactionAsync(post.Id))
        {
            return Result.Failure(ErrorType.Invalid, ErrorMessages.Post.InvolvedInOngoingTransaction);
        }

        return Result.Success();
    }

    #endregion
    
    #region TransactionStatus
    /// <inheritdoc />
    public async Task<Result> ValidateTransactionStatusUpdateAsync(TransactionStatus oldTransactionStatus, UpdateTransactionStatusDetailsCommand newDetails)
    {
        if (newDetails.Name != null)
        {
            if (oldTransactionStatus.Name == newDetails.Name)
            {
                return Result.Failure(ErrorType.Invalid,
                    ErrorMessages.NewMustBeDifferentFromCurrent("transaction status name"));
            }

            if (await transactionStatusRepo.NameExistsAsync(newDetails.Name))
            {
                return Result.Failure(ErrorType.Invalid,
                    ErrorMessages.AlreadyExists("transaction status name", newDetails.Name));
            }
        }

        return Result.Success();
    }
    #endregion

    #region Transaction
    /// <inheritdoc />
    public Result ValidateTransactionStatusChange(
        Transaction transaction,
        TransactionStatusEnum oldStatus, 
        TransactionStatusEnum newStatus,
        UserContext userContext)
    {
        switch (oldStatus)
        {
            case TransactionStatusEnum.Pending:
                switch (newStatus)
                {
                    case TransactionStatusEnum.Accepted or TransactionStatusEnum.Refused:
                    {
                        return userContext.UserId == transaction.InitiatorId
                            ? Result.Failure(ErrorType.Forbidden, ErrorMessages.Transaction.InitiatorCantAcceptOrRefuseOwnTransaction)
                            : Result.Success();
                    }
                    case TransactionStatusEnum.Cancelled:
                        return userContext.UserId != transaction.InitiatorId
                            ? Result.Failure(ErrorType.Forbidden, ErrorMessages.Transaction.InitiatorCanCancelOwnTransaction)
                            : Result.Success();
                }
                break; 
            case TransactionStatusEnum.Accepted:
                if (newStatus is TransactionStatusEnum.Failed or TransactionStatusEnum.InDispute or TransactionStatusEnum.PartiallyValidated){ 
                    return Result.Success();
                }
                break;
            case TransactionStatusEnum.InDispute:
                if (newStatus is TransactionStatusEnum.ResolvedAccepted or TransactionStatusEnum.ResolvedCancelled)
                {
                    return userContext.IsAdmin
                        ? Result.Success()
                        : Result.Failure(ErrorType.Forbidden, ErrorMessages.Transaction.AdminShouldResolveAConflict);
                }
                break;
            case TransactionStatusEnum.PartiallyValidated:
                switch (newStatus)
                {
                    case TransactionStatusEnum.InDispute:
                        return Result.Success();
                    case TransactionStatusEnum.Completed:
                        if (transaction.BuyerConfirmed && userContext.UserId == transaction.SellerId ||
                            transaction.SellerConfirmed && userContext.UserId == transaction.BuyerId)
                        {
                            return Result.Success();
                        }
                        return Result.Failure(ErrorType.Forbidden, ErrorMessages.Transaction.OtherPartShouldComplete);
                }
                break;
        }

        return Result.Failure(ErrorType.Invalid, ErrorMessages.Transaction.InvalidTransactionStatusChange(oldStatus.ToString(), newStatus.ToString()));
    }
    
    /// <inheritdoc />
     public Result ValidateNewTransaction(Transaction transaction)
    {
        (int price, Post? post, string buyerId, User? buyer, string sellerId) = transaction;
        
        List<ErrorDetail> errors = new();
           
        if (post != null && post.OwnerUserId != buyerId && post.OwnerUserId != sellerId)
        {
            errors.Add(new ErrorDetail(ErrorType.Invalid,ErrorMessages.Transaction.NotPostOwner));
        }

        if (buyerId == sellerId)
        {
            errors.Add(new ErrorDetail(ErrorType.Invalid,ErrorMessages.Transaction.BuyerEqualsSeller));
        }

        if (buyer != null && price > buyer.Credit)
        {
            errors.Add(new ErrorDetail(ErrorType.Invalid,ErrorMessages.Transaction.CreditInsufficient));
        }

        return errors.Any()
            ? Result.Failure(errors)
            : Result.Success();
    }

    /// <inheritdoc />
    public Result ValidateTransactionUpdate(Transaction transaction)
    {
        (int price, Post? post, string buyerId, User? buyer, string sellerId) = transaction;

        List<ErrorDetail> errors = new();
        
        List<TransactionStatusEnum> updateAllowedForStatus =
        [
            TransactionStatusEnum.Pending,
            TransactionStatusEnum.Accepted
        ];
        
        if (!updateAllowedForStatus.Contains(transaction.TransactionStatus!.EnumValue))
        {
            errors.Add(new ErrorDetail(ErrorType.Invalid, ErrorMessages.Transaction.UpdateOnlyWhenPendingOrAccepted));
        }
        
        if (post != null && post.OwnerUserId != buyerId && post.OwnerUserId != sellerId)
        {
            errors.Add(new ErrorDetail(ErrorType.Invalid,ErrorMessages.Transaction.NotPostOwner));
        }

        
        if (buyer != null && price > buyer.Credit)
        {
            errors.Add(new ErrorDetail(ErrorType.Invalid,ErrorMessages.Transaction.CreditInsufficient));
        }
        
        return errors.Any()
            ? Result.Failure(errors)
            : Result.Success();
    }

    #endregion
    
    /// <inheritdoc />
    public Result ValidateSortBy(string? sortByValue, HashSet<string> allowedValues)
    {
        return string.IsNullOrWhiteSpace(sortByValue) || allowedValues.Contains(sortByValue)
            ? Result.Success()
            : Result.Failure(ErrorType.Invalid, ErrorMessages.IsInvalid("SortBy query parameter", sortByValue));
    }
}