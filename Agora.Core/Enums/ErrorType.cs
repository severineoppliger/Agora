namespace Agora.Core.Enums;


/// <summary>
/// Represents the category of an error for more precise handling and messaging.
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// A generic or unknown error occurred.
    /// </summary>
    Unknown,

    /// <summary>
    /// The requested entity was not found in the database or source.
    /// </summary>
    NotFound,
    
    /// <summary>
    /// The current user is not authenticated, but the requested action requires authentication.
    /// </summary>
    Unauthorized,

    /// <summary>
    /// The current user is authenticated but does not have permission to perform the requested action.
    /// </summary>
    Forbidden,

    /// <summary> The request was invalid due to data validation or business rule violations. </summary>
    Invalid,
    
    /// <summary>
    /// A database or persistence-related error occurred.
    /// </summary>
    Persistence
}