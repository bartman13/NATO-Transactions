using Domain.Enums;

namespace Domain.Entities;

public class Transaction : BaseEntity
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public TransactionType TransactionType { get; set; }
}
