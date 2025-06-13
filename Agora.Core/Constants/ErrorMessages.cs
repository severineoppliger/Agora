using Agora.Core.Extensions;

namespace Agora.Core.Constants;

public static class ErrorMessages
{
    public static string NotFound(string entityName) => $"{entityName.CapitalizeFirstLetter()} is not found.";

    public static string SavedButNotRetrieved(string entityName) =>
        $"{entityName} was saved but could not be retrieved.";

    public static string AlreadyExists(string entityName) => $"{entityName.CapitalizeFirstLetter()} already exists.";

    public static string UnauthorizedAction(string entityName) => 
        $"You are not authorized to perform this action on this {entityName}.";
    
    public static string UnknownErrorDuringAction(string entityName, string action) => 
        $"Unknown problem during {action} of the {entityName}.";
    
    
    public static class PostCategory
    {
        public const string InUse = "Cannot delete a post category that is used by one or more posts.";
    }

    public static class Post
    {
        public const string NotOwner = "Current user is not the owner of the post.";
    }
    
    public static class Transaction
    {
        public const string NotInvolved = "Current user is not involved in the transaction.";
    }
    
    public static class User
    {
        public const string IdNotFoundInClaims = "User ID not found in claims.";
        public const string InvalidCredentials = "Invalid email or password.";
        public const string NotAuthenticated = "User is not authenticated.";
        public const string NotAuthorized = "User is not authorized.";
    }
}