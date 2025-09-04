using Domain.Entities;

namespace Domain.Abstractions;

public interface ITransactionRepository
{
    Task<Transaction> AddAsync(Transaction transaction);
    Task<IReadOnlyCollection<Transaction>> GetAllAsync();
}
