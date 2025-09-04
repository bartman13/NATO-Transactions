using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Transactions.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _service;

    public TransactionsController(ITransactionService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> AddTransaction([FromBody] CreateTransactionDto dto)
    {
        var result = await _service.AddTransactionAsync(dto);
        return CreatedAtAction(nameof(GetTransactions), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetTransactions()
    {
        var result = await _service.GetAllTransactionsAsync();
        return Ok(result);
    }
}
