namespace Agora.Core.Constants;

/// <summary>
/// Provides the list of valid <c>SortBy</c> values (property of QueryParameters) for different entities.
/// </summary>
public class SortByOptions
{
    /// <summary>
    /// Allowed <c>SortBy</c> values for the <c>User</c> entity (case-insensitive).
    /// </summary>
    public static readonly HashSet<string> User = ["id", "username", "email", "credit", "createdat", "lastloginat"];

    /// <summary>
    /// Allowed <c>SortBy</c> values for the <c>Post</c> entity (case-insensitive).
    /// </summary>
    public static readonly HashSet<string> Post = ["id", "title", "price", "type", "status", "postcategoryid",
        "postcategoryname", "user", "createdat", "updatedat"];

    /// <summary>
    /// Allowed <c>SortBy</c> values for the <c>Transaction</c> entity (case-insensitive).
    /// </summary>
    public static readonly HashSet<string> Transaction = ["id", "title", "price", "transactionstatus", "buyer",
        "seller", "createdat"];

    /// <summary>
    /// Allowed <c>SortBy</c> values for the <c>PostCategory</c> entity (case-insensitive).
    /// </summary>
    public static readonly HashSet<string> PostCategory = ["id","name"];

    /// <summary>
    /// Allowed <c>SortBy</c> values for the <c>TransactionStatus</c> entity (case-insensitive).
    /// </summary>
    public static readonly HashSet<string> TransactionStatus = ["id", "name",  "isfinal",  "issuccess"];
}
