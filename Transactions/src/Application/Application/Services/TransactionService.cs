using Application.DTOs;
using AutoMapper;
using Domain.Abstractions;
using Domain.Entities;

namespace Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, IUserRepository userRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<TransactionDto> AddTransactionAsync(CreateTransactionDto dto)
        {
            var userExists = await _userRepository.ExistsAsync(dto.UserId);
            if (!userExists)
                throw new KeyNotFoundException($"User with Id {dto.UserId} does not exist.");

            var transaction = _mapper.Map<Transaction>(dto);
            transaction.CreatedAt = DateTime.UtcNow;

            var added = await _transactionRepository.AddAsync(transaction);

            return _mapper.Map<TransactionDto>(added);
        }

        public async Task<IReadOnlyCollection<TransactionDto>> GetAllTransactionsAsync()
        {
            var transactions = await _transactionRepository.GetAllAsync();
            return _mapper.Map<IReadOnlyCollection<TransactionDto>>(transactions);
        }

        public async Task<IReadOnlyCollection<UserTransactionSummaryDto>> GetTotalAmountPerUserAsync()
        {
            var transactions = await _transactionRepository.GetAllAsync();

            return transactions
                .GroupBy(t => t.UserId).
                Select(g => new UserTransactionSummaryDto { 
                    UserId = g.Key, 
                    TotalAmount = g.Sum(t => t.Amount) })
                .ToList();
        }
    }
}
