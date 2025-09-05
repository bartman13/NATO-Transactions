using Domain.Enums;

namespace Application.DTOs;

public class TransactionTypeSummaryDto
{
    public TransactionType TransactionType { get; set; }
    public decimal TotalAmount { get; set; }
}
