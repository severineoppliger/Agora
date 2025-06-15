using System.ComponentModel.DataAnnotations;
using Agora.Core.Enums;

namespace Agora.API.DTOs.Transaction;

public class ChangeTransactionStatusDto
{
    [Required]
    public TransactionStatusEnum NewStatus;
}