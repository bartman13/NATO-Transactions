namespace Application.DTOs;

public class UserTransactionSummaryDto
{
    public Guid UserId { get; set; }
    public decimal TotalAmount { get; set; }
}
