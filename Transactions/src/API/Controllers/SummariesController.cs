using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Transactions.Controllers;

[Route("[controller]")]
[ApiController]
public class SummariesController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public SummariesController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet("total-per-user")]
    public async Task<IActionResult> GetTotalAmountPerUser()
    {
        // Fetch all transactions in memory
        var transactions = await _transactionService.GetAllTransactionsAsync();

        // Group by user and sum amounts efficiently
        var summary = transactions
            .GroupBy(t => t.UserId)
            .Select(g => new UserTransactionSummaryDto
            {
                UserId = g.Key,
                TotalAmount = g.Sum(t => t.Amount)
            })
            .ToList();

        return Ok(summary);
    }
}
