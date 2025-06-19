namespace Agora.Core.Models;

/// <summary>
/// Entity representing a transaction between two users.
/// A transaction may optionally be linked to a post and has a status and participants (buyer and seller).
/// </summary>
public class Transaction : BaseEntity
{
    /// <summary>
    /// Title of the transaction.
    /// </summary>
    public required string Title { get; set; }
    
    /// <summary>
    /// Price of the transaction in Kairos credits.
    /// </summary>
    public required int Price { get; set; }
    
    /// <summary>
    /// Foreign key referencing the related <c>Post</c>, if any.
    /// It corresponds to a post's unique identifier <c>Id</c>.
    /// </summary>
    public required long? PostId { get; set; }
    
    /// <summary>
    /// Navigation property to the related post, if applicable.
    /// </summary>

    public Post? Post { get; set; }
    
    /// <summary>
    /// Foreign key referencing the associated <c>TransactionStatus</c>.
    /// It corresponds to a transaction status's unique identifier <c>Id</c>.
    /// </summary>
    public required long TransactionStatusId { get; set; }
    
    /// <summary>
    /// Navigation property that refers to the <c>TransactionStatus</c> representing the current status of the transaction.
    /// </summary>
    public TransactionStatus? TransactionStatus { get; set; }
    
    /// <summary>
    /// Foreign key referencing the associated <c>User</c> as the one having initiated the transaction.
    /// It corresponds to a user's unique identifier <c>Id</c>.
    /// </summary>
    public required string InitiatorId { get; set; }
    
    /// <summary>
    /// Navigation property that refers to the <c>User</c> who initiated this transaction.
    /// </summary>
    public User? Initiator { get; set; }
    
    /// <summary>
    /// Foreign key referencing the associated <c>User</c> as the buyer in the transaction.
    /// It corresponds to a user's unique identifier <c>Id</c>.
    /// /// </summary>
    public required string BuyerId { get; set; }
    
    /// <summary>
    /// Indicates whether the buyer has confirmed the transaction.
    /// </summary>
    public required bool BuyerConfirmed { get; set; }
    
    /// <summary>
    /// Navigation property that refers to the <c>User</c> who is the buyer in this transaction.
    /// </summary>
    public User? Buyer { get; set; }
    
    /// <summary>
    /// Foreign key referencing the associated <c>User</c> as the seller in the transaction.
    /// It corresponds to a user's unique identifier <c>Id</c>.
    /// </summary>
    public required string SellerId { get; set; }
    
    /// <summary>
    /// Indicates whether the seller has confirmed the transaction.
    /// </summary>
    public required bool SellerConfirmed { get; set; }
    
    /// <summary>
    /// Navigation property that refers to the <c>User</c> who is the seller in this transaction.
    /// </summary>
    public User? Seller { get; set; }

    /// <summary>
    /// Date and time (UTC) when the transaction was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Date and time (UTC) when the transaction was last updated, if applicable.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
    
    /// <summary>
    /// Date of the transaction, as agreed by the participants, if applicable.
    /// </summary>
    public DateOnly? TransactionDate { get; set; }
    
    /// <summary>
    /// Date and time (UTC) when the transaction was completed, i.e. when the transaction status was changed
    /// to a <c>TransactionStatus</c> with <c>IsFinal</c> equals to <c>true</c>, if applicable.
    /// </summary>
    public DateTime? CompletedAt { get; set; }
    
    /// <summary>
    /// Deconstructs this transaction into basic components.
    /// </summary>
    public void Deconstruct(out int price, out Post? post, out string buyerId, out User? buyer, out string sellerId)
    {
        price = Price;
        post = Post;
        buyerId = BuyerId;
        buyer = Buyer;
        sellerId = SellerId;
    }
}