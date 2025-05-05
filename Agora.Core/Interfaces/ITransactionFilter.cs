namespace Agora.Core.Interfaces;

public interface ITransactionFilter
{
    public int? MinPrice { get; set; }
    public int? MaxPrice { get; set; }
    public string? PostTitle { get; set; }
    public string? TransactionStatusName { get; set; }
    public string? UsersInvolvedUsername { get; set; }
}