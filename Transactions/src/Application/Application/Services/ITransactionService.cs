using Application.DTOs;

namespace Application.Services;

public interface ITransactionService
{
    Task<TransactionDto> AddTransactionAsync(CreateTransactionDto dto);

    Task<IReadOnlyCollection<TransactionDto>> GetAllTransactionsAsync();

    Task<IReadOnlyCollection<UserTransactionSummaryDto>> GetTotalAmountPerUserAsync();

    Task<IReadOnlyCollection<TransactionTypeSummaryDto>> GetTotalAmountPerTransactionType();

}
