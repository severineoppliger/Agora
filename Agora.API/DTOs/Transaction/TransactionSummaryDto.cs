namespace Agora.API.DTOs.Transaction;

public class TransactionSummaryDto
{
    public long Id { get; set; }
    public string Title { get; set; } = String.Empty;
    public int Price { get; set; }
    public string PostTitle { get; set; } = String.Empty;
    public string TransactionStatusName { get; set; } = String.Empty;
    public string BuyerUsername { get; set; } = String.Empty;
    public string SellerUsername { get; set; } = String.Empty;
}