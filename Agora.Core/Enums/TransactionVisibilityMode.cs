namespace Agora.Core.Enums;

/// <summary>
/// Specifies the visibility mode for retrieving transactions.
/// </summary>
public enum TransactionVisibilityMode
{
    /// <summary>
    /// Shows transactions in which current user is involved as buyer or seller.
    /// </summary>
    CurrentUserTransactions,

    /// <summary>
    /// Shows all transactions, intended for administrators.
    /// </summary>
    AdminView
}