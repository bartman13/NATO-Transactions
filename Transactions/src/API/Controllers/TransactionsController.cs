using Microsoft.AspNetCore.Mvc;

namespace Transactions.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ILogger<TransactionsController> _logger;

    public TransactionsController(ILogger<TransactionsController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "Transaction")]
    public string Get()
    {
        return "Ok";
    }
}
