using Agora.Core.Models;

namespace Agora.Core.BusinessRules.Interfaces;

public interface IBusinessRulesValidator
{
    public List<string> ValidateUser(AppUser user);
    public List<string> ValidatePostCategory(PostCategory postCategory);
    public List<string> ValidatePost(Post post, IList<string> postTitlesOfUser);
    public List<string> ValidateTransactionStatus(TransactionStatus transactionStatus);
    public List<string> ValidateTransaction(Transaction transaction);
}