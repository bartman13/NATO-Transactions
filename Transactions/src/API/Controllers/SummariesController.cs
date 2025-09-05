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
        var summary = await _transactionService.GetTotalAmountPerUserAsync();
        return Ok(summary);
    }

    [HttpGet("total-per-transaction-type")]
    public async Task<IActionResult> GetTotalAmountPerTransactionType()
    {
        var summary = await _transactionService.GetTotalAmountPerTransactionType();
        return Ok(summary);
    }

    [HttpGet("high-volume")]
    public async Task<IActionResult> GetHighVolumeTransactions([FromQuery] decimal threshold)
    {
        var result = await _transactionService.GetHighVolumeTransactions(threshold);
        return Ok(result);
    }
}
