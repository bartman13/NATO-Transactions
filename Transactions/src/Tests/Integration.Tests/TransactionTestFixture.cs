using Application.Mapping;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

public class TransactionTestFixture : IDisposable
{
    public AppDbContext DbContext { get; }
    public IMapper Mapper { get; }

    public Guid User1 = Guid.NewGuid();
    public Guid User2 = Guid.NewGuid();

    public TransactionTestFixture()
    {
        // Setup InMemory DB
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        DbContext = new AppDbContext(options);

        // Seed sample data
        DbContext.Transactions.AddRange(new[]
        {
            new Transaction { Id = Guid.NewGuid(), UserId = User1, Amount = 100, TransactionType = TransactionType.Debit, CreatedAt = DateTime.UtcNow },
            new Transaction { Id = Guid.NewGuid(), UserId = User2, Amount = 200, TransactionType = TransactionType.Debit, CreatedAt = DateTime.UtcNow },
            new Transaction { Id = Guid.NewGuid(), UserId = User1, Amount = 300, TransactionType = TransactionType.Credit, CreatedAt = DateTime.UtcNow }
        });
        DbContext.SaveChanges();

        // Setup AutoMapper
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(
            m => m.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<object>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception?, string>>()));
        var mockLoggerFactory = new Mock<ILoggerFactory>();
        mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(() => mockLogger.Object);
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UserMappingProfile>();
            cfg.AddProfile<TransactionMappingProfile>();
        }, mockLoggerFactory.Object);

        Mapper = mapperConfig.CreateMapper();
    }

    public void Dispose()
    {
        DbContext.Dispose();
    }
}