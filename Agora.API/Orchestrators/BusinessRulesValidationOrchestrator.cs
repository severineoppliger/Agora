using System.ComponentModel.DataAnnotations;
using Agora.API.Orchestrators.Interfaces;
using Agora.Core.BusinessRules.Interfaces;
using Agora.Core.Interfaces;
using Agora.Core.Models;

namespace Agora.API.Orchestrators;

/* Complete navigation properties before validating entities */
public class BusinessRulesValidationOrchestrator(
    IBusinessRulesValidator businessRulesValidator,
    IPostRepository postRepository,
    IUserRepository userRepository)
    : IBusinessRulesValidationOrchestrator
{
    public Task<IList<string>> ValidateAndProcessUserAsync(User user)
    {
        throw new NotImplementedException();
        // TODO
    }

    public Task<IList<string>> ValidateAndProcessPostCategoryAsync(PostCategory postCategory)
    {
        throw new NotImplementedException();
        // TODO
    }

    public async Task<IList<string>> ValidateAndProcessPostAsync(Post post)
    {
        IReadOnlyList<Post> postsOfUser = await postRepository.GetAllPostsOfUserAsync(post.UserId);
        List<string> postTitlesOfUser = postsOfUser.Select(p => p.Title).ToList();
        
        return businessRulesValidator.ValidatePost(post, postTitlesOfUser);
    }

    public Task<IList<string>> ValidateAndProcessTransactionStatusAsync(TransactionStatus transactionStatus)
    {
        throw new NotImplementedException();
        // TODO
    }

    public async Task<IList<string>> ValidateAndProcessTransactionAsync(Transaction transaction)
    {
        transaction.Buyer ??= await userRepository.GetUserByIdAsync(transaction.BuyerId)
            ?? throw new InvalidOperationException($"Buyer (user with id {transaction.BuyerId}) doesn't exist.");
        
        long? postId = transaction.PostId;
        if (postId != null)
        {
            transaction.Post ??= await postRepository.GetPostByIdAsync(postId.Value)
                                 ?? throw new ValidationException($"Related post {postId} doesn't exist.");
        }
        
        return businessRulesValidator.ValidateTransaction(transaction);
    }
}