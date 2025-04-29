using Agora.Core.BusinessRules.Interfaces;
using Agora.Core.Models;

namespace Agora.Core.BusinessRules;

public class BusinessRulesValidator : IBusinessRulesValidator
{
    public List<string> ValidateUser(User user)
    {
        throw new NotImplementedException();
        // TODO
    }

    public List<string> ValidatePostCategory(PostCategory postCategory)
    {
        throw new NotImplementedException();
        // TODO
    }

    public List<string> ValidatePost(Post post)
    {
        throw new NotImplementedException();
        // TODO
    }

    public List<string> ValidateTransactionStatus(TransactionStatus transactionStatus)
    {
        throw new NotImplementedException();
        // TODO
    }

    public List<string> ValidateTransaction(Transaction transaction)
    {
        (int price, Post? post, long buyerId, User? buyer, long sellerId) = transaction;
        
        List<string> businessRulesErrors = new();
        if (post != null && post.UserId != buyerId && post.UserId != sellerId)
            businessRulesErrors.Add("Buyer or seller must be the owner of the post.");
        if (buyerId == sellerId)
            businessRulesErrors.Add("Buyer and seller cannot be the same user.");
        if (price <= 0)
            businessRulesErrors.Add("Price must be positive.");
        if (buyer != null && price > buyer.Credit)
            businessRulesErrors.Add("Buyer does not have enough credit.");
        // TODO implement a maximum that the seller can obtain
        // TODO implement a maximum for a transaction price
        return businessRulesErrors;
    }
}