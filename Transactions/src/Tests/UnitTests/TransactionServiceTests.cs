using Moq;
using FluentAssertions;
using Application.Services;
using Domain.Abstractions;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Logging;
using Application.Mapping;

namespace UnitTests;

public class TransactionServiceTests
{
    private readonly TransactionService _service;
    private readonly Mock<ITransactionRepository> _transactionRepositoryMock = new Mock<ITransactionRepository>();
    private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();

    private static readonly Guid userId1 = Guid.NewGuid();
    private static readonly Guid userId2 = Guid.NewGuid();

    private readonly List<Transaction> transactions = new()
    {
            new Transaction { Id = Guid.NewGuid(), UserId = userId1, Amount = 100, TransactionType = TransactionType.Debit, CreatedAt = DateTime.UtcNow },
            new Transaction { Id = Guid.NewGuid(), UserId = userId1, Amount = 50, TransactionType = TransactionType.Credit, CreatedAt = DateTime.UtcNow },
            new Transaction { Id = Guid.NewGuid(), UserId = userId2, Amount = 200, TransactionType = TransactionType.Debit, CreatedAt = DateTime.UtcNow }
    };

    public TransactionServiceTests()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(
            m => m.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<object>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()));

        var mockLoggerFactory = new Mock<ILoggerFactory>();
        mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(() => mockLogger.Object);

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UserMappingProfile>();
            cfg.AddProfile<TransactionMappingProfile>();
        }, mockLoggerFactory.Object);

        IMapper mapper = mapperConfig.CreateMapper();

        _service = new TransactionService(_transactionRepositoryMock.Object, _userRepositoryMock.Object, mapper);
    }

    [Fact]
    public async Task GetTotalAmountPerUserAsync_ShouldReturnCorrectSums()
    {
        // Arrange
        _transactionRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(transactions);

        // Act
        var result = await _service.GetTotalAmountPerUserAsync();

        // Assert
        result.Should().HaveCount(2);
        result.First(r => r.UserId == userId1).TotalAmount.Should().Be(150);
        result.First(r => r.UserId == userId2).TotalAmount.Should().Be(200);
    }

    [Fact]
    public async Task GetTotalAmountPerTransactionType_ShouldReturnCorrectSums()
    {
        // Arrange
        _transactionRepositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(transactions);

        // Act
        var result = await _service.GetTotalAmountPerTransactionType();

        // Assert
        result.Should().HaveCount(Enum.GetValues<TransactionType>().Length);

        var debitSummary = result.First(x => x.TransactionType == TransactionType.Debit);
        debitSummary.TotalAmount.Should().Be(300);

        var creditSummary = result.First(x => x.TransactionType == TransactionType.Credit);
        creditSummary.TotalAmount.Should().Be(50);
    }

    [Fact]
    public async Task GetTotalAmountPerTransactionType_ShouldReturnZeroForMissingTypes()
    {
        // Arrange: only Debit transactions
        var transactions = new List<Transaction>
        {
            new Transaction { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Amount = 100, TransactionType = TransactionType.Debit, CreatedAt = DateTime.UtcNow }
        };

        _transactionRepositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(transactions);

        // Act
        var result = await _service.GetTotalAmountPerTransactionType();

        // Assert
        var debitSummary = result.First(x => x.TransactionType == TransactionType.Debit);
        debitSummary.TotalAmount.Should().Be(100);

        var creditSummary = result.First(x => x.TransactionType == TransactionType.Credit);
        creditSummary.TotalAmount.Should().Be(0);
    }
}
