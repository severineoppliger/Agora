using Agora.Core.BusinessRules.Interfaces;
using Agora.Core.Common;
using Agora.Core.Enums;
using Agora.Core.Interfaces.Repositories;
using Agora.Core.Models;
using Agora.Core.Models.Filters;

namespace Agora.Core.BusinessRules;

public class BusinessRulesValidator(
    IPostRepository postRepo,
    IPostCategoryRepository postCategoryRepo,
    ITransactionRepository transactionRepo
    ) : IBusinessRulesValidator
{
    public Result ValidateUser(AppUser user)
    {
        throw new NotImplementedException();
        // TODO
    }

    public async Task<Result> ValidateNewPostCategoryAsync(PostCategory postCategory)
    {
        if (await postCategoryRepo.NameExistsAsync(postCategory.Name))
        {
            return Result.Failure(ErrorType.Invalid, ErrorMessages.AlreadyExists("post category name", postCategory.Name));
        }

        return Result.Success();
    }

    public async Task<Result> ValidatePostCategoryUpdateAsync(PostCategory postCategory, string newName)
    {
        if (postCategory.Name == newName)
        {
            return Result.Failure(ErrorType.Invalid, ErrorMessages.NewMustBeDifferentFromCurrent("post category name"));
        }
        
        if (await postCategoryRepo.NameExistsAsync(postCategory.Name))
        {
            return Result.Failure(ErrorType.Invalid, ErrorMessages.AlreadyExists("post category name", postCategory.Name));
        }

        return Result.Success();
    }

    public async Task<Result> ValidatePostCategoryDeletionAsync(PostCategory postCategory)
    {
        return await postRepo.IsCategoryInUseAsync(postCategory.Id)
            ? Result.Failure(ErrorType.Invalid, ErrorMessages.PostCategory.InUse)
            : Result.Success();
    }


    public async Task<Result> ValidateNewPostAsync(Post newPost, UserContext userContext)
    {
        PostFilter postFilter = new PostFilter()
        {
            UserId = userContext.UserId,
            StatusNames = [PostStatus.Active.ToString(), PostStatus.Inactive.ToString()]
        };

        IReadOnlyList<Post> postsOfUser = await postRepo.GetAllPostsAsync(postFilter);
        List<string> postTitlesOfUser = postsOfUser.Select(p => p.Title).ToList();
        if (postTitlesOfUser.Contains(newPost.Title))
        {
            return Result.Failure(ErrorType.Invalid, ErrorMessages.Post.SameTitle);
        }

        return Result.Success();
    }

    public async Task<Result> ValidatePostUpdateAsync(Post oldPost, string? newTitle, UserContext userContext)
    {
        if (newTitle is not null && newTitle != oldPost.Title)
        {
            PostFilter postFilter = new PostFilter()
            {
                UserId = userContext.UserId,
                StatusNames = [PostStatus.Active.ToString(), PostStatus.Inactive.ToString()]
            };

            IReadOnlyList<Post> postsOfUser = await postRepo.GetAllPostsAsync(postFilter);
            List<string> postTitlesOfUser = postsOfUser.Select(p => p.Title).ToList();
            if (postTitlesOfUser.Contains(newTitle))
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

    public Result ValidatePostStatusChangeAsync(Post oldPost, PostStatus newStatus)
    {
        return oldPost.Status == newStatus
            ? Result.Failure(ErrorType.Invalid, ErrorMessages.Post.SameStatus(newStatus))
            : Result.Success();
    }
    
    
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
                            ? Result.Failure(ErrorType.Unauthorized, ErrorMessages.Transaction.InitiatorCantAcceptOrRefuseOwnTransaction)
                            : Result.Success();
                    }
                    case TransactionStatusEnum.Cancelled:
                        return userContext.UserId != transaction.InitiatorId
                            ? Result.Failure(ErrorType.Unauthorized, ErrorMessages.Transaction.InitiatorCanCancelOwnTransaction)
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
                        : Result.Failure(ErrorType.Unauthorized, ErrorMessages.Transaction.AdminShouldResolveAConflict);
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
                        return Result.Failure(ErrorType.Unauthorized, ErrorMessages.Transaction.OtherPartShouldComplete);
                }
                break;
        }

        return Result.Failure(ErrorType.Invalid, ErrorMessages.Transaction.InvalidTransactionStatusChange(oldStatus.ToString(), newStatus.ToString()));
    }
    
     public Result ValidateTransaction(Transaction transaction)
    {
        (int price, Post? post, string buyerId, AppUser? buyer, string sellerId) = transaction;
        
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
    
}