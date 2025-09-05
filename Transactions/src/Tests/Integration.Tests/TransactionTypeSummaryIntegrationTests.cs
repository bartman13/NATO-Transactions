using Application.Services;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Repositories;

public class TransactionTypeSummaryIntegrationTests : IClassFixture<TransactionTestFixture>
{
    private readonly TransactionTestFixture _fixture;

    public TransactionTypeSummaryIntegrationTests(TransactionTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetTotalAmountPerUser_ShouldReturnCorrectSums()
    {
        // Arrange
        var repository = new TransactionRepository(_fixture.DbContext);
        var userRepository = new UserRepository(_fixture.DbContext);
        var service = new TransactionService(repository, userRepository, _fixture.Mapper);

        await _fixture.DbContext.SaveChangesAsync();

        // Act
        var result = await service.GetTotalAmountPerUserAsync();

        // Assert
        result.Should().HaveCount(2);

        var user1Summary = result.First(x => x.UserId == _fixture.User1);
        user1Summary.TotalAmount.Should().Be(400);

        var user2Summary = result.First(x => x.UserId == _fixture.User2);
        user2Summary.TotalAmount.Should().Be(200);
    }

    [Fact]
    public async Task GetTotalAmountPerTransactionType_ShouldReturnCorrectSums()
    {
        // Arrange
        var repository = new TransactionRepository(_fixture.DbContext);
        var userRepository = new UserRepository(_fixture.DbContext);
        var service = new TransactionService(repository, userRepository, _fixture.Mapper);

        // Act
        var result = await service.GetTotalAmountPerTransactionType();

        // Assert
        result.Should().HaveCount(Enum.GetValues<TransactionType>().Length);

        var debitSummary = result.First(x => x.TransactionType == TransactionType.Debit);
        debitSummary.TotalAmount.Should().Be(300);

        var creditSummary = result.First(x => x.TransactionType == TransactionType.Credit);
        creditSummary.TotalAmount.Should().Be(300);
    }

    [Fact]
    public async Task GetHighVolumeTransactions_ShouldReturnEmptyIfNoTransactionAboveThreshold()
    {
        // Arrange
        var repository = new TransactionRepository(_fixture.DbContext);
        var userRepository = new UserRepository(_fixture.DbContext);
        var service = new TransactionService(repository, userRepository, _fixture.Mapper);

        decimal threshold = 1000;

        // Act
        var result = await service.GetHighVolumeTransactions(threshold);

        // Assert
        result.Should().BeEmpty();
    }
}