using Agora.Core.Extensions;

namespace Agora.Core.Common;

public static class ErrorMessages
{
    public static string NotFound(string objectName) => $"{objectName.CapitalizeFirstLetter()} is not found.";
    public static string IsInvalid(string objectName, string objectValue) => $"{objectName.CapitalizeFirstLetter()} '{objectValue}' is invalid.";

    public static string NewMustBeDifferentFromCurrent(string objectName) =>
        $"New {objectName} must be different from the current {objectName}.";
    public static string SavedButNotRetrieved(string entityName) =>
        $"{entityName} was saved but could not be retrieved.";

    public static string AlreadyExists(string objectName, string objectValue) => 
        $"{objectName.CapitalizeFirstLetter()} {objectValue} already exists. {objectName.CapitalizeFirstLetter()} must be unique";
    public static string RelatedEntityDoesNotExist(string entityName, long? entityId) =>
        $"Related {entityName} with ID {entityId} doesn't exist.";

    public static string UnauthorizedAction(string entityName) => 
        $"You are not authorized to perform this action on this {entityName}.";

    public static string ErrorWhenSavingToDb(string entityName) =>
        $"Problem occured when saving the {entityName} in DB.";
    
    public static string UnknownErrorDuringAction(string entityName, string action) => 
        $"Unknown problem occured during {action} of the {entityName}.";
    
    
    public static class PostCategory
    {
        public const string InUse = "Cannot delete a post category that is used by one or more posts.";
    }

    public static class Post
    {
        public const string NotOwner = "Current user is not the owner of the post.";
        public const string SameTitle = "User has already posted a post with same title.";
    }
    
    public static class Transaction
    {
        public const string NotInvolved = "Current user is not involved in the transaction.";
        public const string NotPostOwner = "Buyer or seller must be the owner of the post.";
        public const string BuyerEqualsSeller = "Buyer and seller cannot be the same user.";
        public const string CreditInsufficient = "Buyer does not have enough credit.";
        public const string InitiatorCantAcceptOrRefuseOwnTransaction =
            "A user can't accept or refuse the transaction he/she has initiated. This must be done by the other part.";
        public const string InitiatorCanCancelOwnTransaction =
            "A user can cancel a transaction only if he/has initiated it.";
        public const string AdminShouldResolveAConflict =
            "Only an administrator can resolved a transaction in dispute.";
        public const string OtherPartShouldComplete =
            "Only the other part is allowed to complete the transaction";
        public static string InvalidTransactionStatusChange(string oldStatus, string newStatus) =>
            "The transaction status change from {oldStatus} to {newStatus} is invalid.";
    }
    
    public static class TransactionStatus
    { 
        public const string MustBeFinalIfSuccess = "Transaction status must be final if it's a success.";
        
    }

    
    public static class User
    {
        public const string IdNotFoundInClaims = "User ID not found in claims.";
        public const string InvalidCredentials = "Invalid email or password.";
        public static string EmailAlreadyRegistered(string email) =>
            $"An account with the email '{email}' is already registered.";
        public static string BuyerOrSellerDoesNotExist(string buyerOrSeller, string id) => 
            $"{buyerOrSeller.CapitalizeFirstLetter()} (user with id {id}) doesn't exist.";
        public const string NotAuthenticated = "User is not authenticated.";
        public const string NotAuthorized = "User is not authorized to perform this action.";
    }
}