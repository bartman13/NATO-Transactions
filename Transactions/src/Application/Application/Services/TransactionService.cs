using Application.DTOs;
using AutoMapper;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Enums;

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
            var users = await _userRepository.GetAllAsync();
            var transactions = await _transactionRepository.GetAllAsync();

            // transaction by user takes O(n) efficiency
            var transactionsByUser = transactions
                .GroupBy(x => x.UserId)
                .ToDictionary(g => g.Key, g => g.ToList());


            return users.Select(x => new UserTransactionSummaryDto
            {
                UserId = x.Id,
                TotalAmount = transactionsByUser.GetValueOrDefault(x.Id)?.Sum(y => y.Amount) ?? 0
            }).ToList();
        }

        public async Task<IReadOnlyCollection<TransactionTypeSummaryDto>> GetTotalAmountPerTransactionType()
        {
            var transactions = await _transactionRepository.GetAllAsync();

            var transactionsByTransactionType = transactions
               .GroupBy(x => x.TransactionType)
               .ToDictionary(g => g.Key, g => g.ToList());

            return Enum.GetValues<TransactionType>().Select(x => new TransactionTypeSummaryDto
            {
                TransactionType = x,
                TotalAmount = transactionsByTransactionType.GetValueOrDefault(x)?.Sum(y => y.Amount) ?? 0
            }).ToList();
        }

        public async Task<IReadOnlyCollection<TransactionDto>> GetHighVolumeTransactions(decimal threshold)
        {
            var transactions = await _transactionRepository.GetAllAsync();

            return transactions
                .Where(t => t.Amount > threshold)
                .Select(_mapper.Map<TransactionDto>)
                .ToList();
        }
    }
}
