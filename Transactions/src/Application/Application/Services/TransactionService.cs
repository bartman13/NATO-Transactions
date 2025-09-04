using Application.DTOs;
using AutoMapper;
using Domain.Abstractions;
using Domain.Entities;

namespace Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _repository;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TransactionDto> AddTransactionAsync(CreateTransactionDto dto)
        {
            var transaction = _mapper.Map<Transaction>(dto);
            transaction.CreatedAt = DateTime.UtcNow;

            var added = await _repository.AddAsync(transaction);

            return _mapper.Map<TransactionDto>(added);
        }

        public async Task<IReadOnlyCollection<TransactionDto>> GetAllTransactionsAsync()
        {
            var transactions = await _repository.GetAllAsync();
            return _mapper.Map<IReadOnlyCollection<TransactionDto>>(transactions);
        }
    }
}
