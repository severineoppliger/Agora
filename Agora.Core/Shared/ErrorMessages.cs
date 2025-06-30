using Agora.Core.Enums;
using Agora.Core.Extensions;

namespace Agora.Core.Shared;

/// <summary>
/// Provides common error message templates used throughout the application.
/// </summary>
public static class ErrorMessages
{
    public const string EmptyDto = "At least one field must be provided.";
    public static string NotFound(string objectName, string? objectValue= null)
    {
        return (objectValue is null)
            ? $"{objectName.CapitalizeFirstLetter()} is not found."
            : $"{objectName.CapitalizeFirstLetter()} '{objectValue}' is not found.";

    }

    public static string IsInvalid(string objectName, string objectValue) => $"{objectName.CapitalizeFirstLetter()} '{objectValue}' is invalid.";

    public static string NewMustBeDifferentFromCurrent(string objectName) =>
        $"New {objectName} must be different from the current {objectName}.";
    public static string SavedButNotRetrieved(string entityName) =>
        $"{entityName} was saved but could not be retrieved.";

    public static string AlreadyExists(string objectName, string objectValue) => 
        $"{objectName.CapitalizeFirstLetter()} '{objectValue}' already exists. {objectName.CapitalizeFirstLetter()} must be unique";
    
    public static string AlreadyDeleted(string objectName) => 
        $"This {objectName} is already deleted.";
    
    public static string RelatedEntityDoesNotExist(string entityName, long? entityId)
    {
        return (entityId is null)
            ? $"Related {entityName} doesn't exist."
            : $"Related {entityName} with ID {entityId} doesn't exist.";
    }

    public static string ErrorWhenSavingToDb(string entityName) =>
        $"Problem occured when saving the {entityName} in DB.";
    
    public static string UnknownErrorDuringAction(string entityName, string action) => 
        $"Unknown problem occured during {action} of the {entityName}.";
    
    /// <summary>
    /// Contains error messages related to <c>PostCategory</c> entity.
    /// </summary>
    public static class PostCategory
    {
        public const string InUse = "Cannot delete a post category that is used by one or more posts.";
    }

    /// <summary>
    /// Contains error messages related to <c>Post</c> entity.
    /// </summary>
    public static class Post
    {
        public const string SameTitle = "User has already posted a post with same title.";
        public const string InvolvedInOngoingTransaction = "This post is involved in an ongoing transaction." +
                                                           "It can't be modified or deleted.";
        public static string SameStatus(PostStatus status) => $"Post is already {status}.";
    }
    
    /// <summary>
    /// Contains error messages related to <c>Transaction</c> entity.
    /// </summary>
    public static class Transaction
    {
        public const string NotInvolved = "Current user is not involved in the transaction.";
        public const string NotPostOwner = "Buyer or seller must be the owner of the post.";
        public const string BuyerEqualsSeller = "Buyer and seller cannot be the same user.";
        public const string CreditInsufficient = "Buyer does not have enough credit.";
        public const string UpdateOnlyWhenPendingOrAccepted = "You can only update transaction details when the status is Pending or Accepted.";
        public const string InitiatorCantAcceptOrRefuseOwnTransaction =
            "A user can't accept or refuse the transaction he/she has initiated. This must be done by the other part.";
        public const string InitiatorCanCancelOwnTransaction =
            "A user can cancel a transaction only if he/has initiated it.";
        public const string AdminShouldResolveAConflict =
            "Only an administrator can resolved a transaction in dispute.";
        public const string OtherPartShouldComplete =
            "Only the other part is allowed to complete the transaction";
        public static string InvalidTransactionStatusChange(string oldStatus, string newStatus) =>
            $"The transaction status change from {oldStatus} to {newStatus} is invalid.";
    }

    /// <summary>
    /// Contains error messages related to <c>User</c> entity.
    /// </summary>
    public static class User
    {
        public static string InvalidIdFormat(string id) => $"Invalid user ID format: {id}. Must be a valid GUID.";
        public const string IdNotFoundInClaims = "User ID not found in claims.";
        public const string PasswordWithoutSpaceBeforeOrAfter = "Password must not start or end with a space.";
        public const string InvalidCredentials = "Invalid email or password.";
        public static string EmailAlreadyRegistered(string email) =>
            $"An account with the email '{email}' is already registered.";
        public static string BuyerOrSellerDoesNotExist(string buyerOrSeller, string id) => 
            $"{buyerOrSeller.CapitalizeFirstLetter()} (user with id {id}) doesn't exist.";
        public const string NotAuthenticated = "User should be authenticated to perform this action.";
        public const string NotAuthorized = "User is not allowed to perform this action.";
        public const string RegisterRequiresNoAuthentication =
            "You are already logged in. Please log out first to register a new account.";
    }
}