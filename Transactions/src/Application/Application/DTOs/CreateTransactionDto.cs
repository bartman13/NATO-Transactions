using Domain.Enums;

namespace Application.DTOs;

public class CreateTransactionDto
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public TransactionType TransactionType { get; set; }
}
