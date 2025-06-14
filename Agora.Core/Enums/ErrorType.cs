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
    /// The current user is not authorized to perform the requested action.
    /// </summary>
    Unauthorized,

    /// <summary> The request was invalid due to data validation or business rule violations. </summary>
    Invalid,
    
    /// <summary>
    /// A database or persistence-related error occurred.
    /// </summary>
    Persistence
}