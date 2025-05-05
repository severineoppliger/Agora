namespace Agora.Core.Interfaces;

public interface ITransactionStatusFilter
{
    public string? NameOrDescription { get; set; }
    public bool? IsFinal { get; set; }
    public bool? IsSuccess { get; set; }
}