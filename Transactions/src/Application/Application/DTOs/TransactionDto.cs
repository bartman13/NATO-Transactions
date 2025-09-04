using Domain.Enums;

namespace Application.DTOs;

public class TransactionDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public TransactionType TransactionType { get; set; }
    public DateTime CreatedAt { get; set; }
}
