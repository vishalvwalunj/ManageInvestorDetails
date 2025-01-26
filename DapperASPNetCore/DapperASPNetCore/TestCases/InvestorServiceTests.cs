using DapperASPNetCore.Contracts;
using DapperASPNetCore.Entities;
using DapperASPNetCore.Service;
using Moq;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

namespace DapperASPNetCore.TestCases
{
    public class InvestorServiceTests
    {
        private readonly Mock<IInvestorRepository> _repositoryMock;
        private readonly IInvestorService _service;

        public InvestorServiceTests()
        {
            _repositoryMock = new Mock<IInvestorRepository>();
            _service = new InvestorService(_repositoryMock.Object);
        }

        [Fact]
        public async Task AddInvestor_Should_Call_Repository_AddInvestor()
        {
            var investor = new Investors { Name = "Smith", Email = "tom.smith@example.com" };

            await _service.AddInvestorAsync(investor.Name, investor.Email);

            _repositoryMock.Verify(r => r.AddInvestorAsync(It.IsAny<Investors>()), Times.Once);
        }

        [Fact]
        public async Task GetInvestor_WithValidId_ShouldReturnInvestor()
        {
            // Arrange
            var investorId = 1;
            var investor = new Investors
            {
                InvestorId = investorId,
                Name = "Kimberly",
                Email = "kimberly.maldonado@example.com",
                Funds = new List<Funds>
                {
                new Funds { FundId = 1, FundName = "Ullamcorper Viverra Fund" }
            }
            };

            _repositoryMock.Setup(r => r.GetInvestorWithFundsAsync(investorId))
                .ReturnsAsync(investor);

            // Act
            var result = await _service.GetInvestorWithFundsAsync(investorId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(investorId, result.InvestorId);
            Assert.Single(result.Funds); // Check if funds are correctly associated
            Assert.Equal("Ullamcorper Viverra Fund", result.Funds[0].FundName);
        }

        [Fact]
        public async Task GetInvestor_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var investorId = 99; // Assuming this ID doesn't exist in the DB

            _repositoryMock.Setup(r => r.GetInvestorWithFundsAsync(investorId))
                .ReturnsAsync((Investors)null);

            // Act
            var result = await _service.GetInvestorWithFundsAsync(investorId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteInvestor_ShouldDeleteInvestor_WhenExists()
        {
            // Arrange
            var investorId = 1;
            _repositoryMock.Setup(r => r.DeleteInvestorAsync(investorId))
                .Returns(Task.CompletedTask);  // Simulating the deletion process.

            // Act
            await _service.DeleteInvestorAsync(investorId);

            // Assert
            _repositoryMock.Verify(r => r.DeleteInvestorAsync(investorId), Times.Once);
        }

        [Fact]
        public async Task DeleteInvestor_ShouldNotDelete_WhenInvestorDoesNotExist()
        {
            // Arrange
            var investorId = 99; // Assuming this investor ID does not exist in DB
            _repositoryMock.Setup(r => r.DeleteInvestorAsync(investorId))
                .Throws(new Exception("Investor not found"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _service.DeleteInvestorAsync(investorId));
        }



        [Fact]
        public async Task AddFundToInvestor_ShouldAddFund_WhenFundExists()
        {
            // Arrange
            var investorId = 1;
            var fundName = "Ullamcorper Viverra Fund";
            var fund = new Funds { FundId = 1, FundName = fundName };

            _repositoryMock.Setup(r => r.GetFundsByNameAsync(fundName))
                .ReturnsAsync(fund);  // Simulating the fund retrieval

            _repositoryMock.Setup(r => r.AddFundToInvestorAsync(investorId, fund.FundId))
                .Returns(Task.CompletedTask);  // Simulating successful fund addition

            // Act
            await _service.AddFundToInvestorAsync(investorId, fundName);

            // Assert
            _repositoryMock.Verify(r => r.AddFundToInvestorAsync(investorId, fund.FundId), Times.Once);
        }

        [Fact]
        public async Task AddFundToInvestor_ShouldThrowException_WhenFundDoesNotExist()
        {
            // Arrange
            var investorId = 1;
            var fundName = "NonExistent Fund";  // This fund doesn't exist
            _repositoryMock.Setup(r => r.GetFundsByNameAsync(fundName))
                .ReturnsAsync((Funds)null);  // Simulating non-existing fund

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _service.AddFundToInvestorAsync(investorId, fundName));
        }


        [Fact]
        public async Task AddInvestor_ShouldAddInvestor_WhenDataIsValid()
        {
            // Arrange
            var newInvestor = new Investors
            {
                Name = "Tom",
                Email = "tom.smith@example.com"
            };

            _repositoryMock.Setup(r => r.AddInvestorAsync(It.IsAny<Investors>()))
                .Returns(Task.CompletedTask);  // Simulating the successful addition

            // Act
            await _service.AddInvestorAsync(newInvestor.Name, newInvestor.Email);

            // Assert
            _repositoryMock.Verify(r => r.AddInvestorAsync(It.IsAny<Investors>()), Times.Once);
        }



        [Fact]
        public async Task AddInvestor_ShouldThrowException_WhenEmailAlreadyExists()
        {
            // Arrange
            var newInvestor = new Investors
            {
                Name = "Jane",
                Email = "tom.smith@example.com"  // This email already exists
            };

            _repositoryMock.Setup(r => r.AddInvestorAsync(It.IsAny<Investors>()))
                .Throws(new Exception("Email already in use"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _service.AddInvestorAsync(newInvestor.Name, newInvestor.Email));
        }


        [Fact]
        public async Task GetAllInvestors_ShouldReturnInvestorsWithFunds_WhenExists()
        {
            // Arrange
            var investors = new List<Investors>
    {
        new Investors
        {
            InvestorId = 1,
            Name = "Kimberly",
            Email = "kimberly.maldonado@example.com",
            Funds = new List<Funds> { new Funds { FundId = 1, FundName = "Ullamcorper Viverra Fund" } }
        },
        new Investors
        {
            InvestorId = 2,
            Name = "Tom",
            Email = "tom.smith@example.com",
            Funds = new List<Funds> { new Funds { FundId = 2, FundName = "Dolor Sit Amet Fund" } }
        }
    };

            _repositoryMock.Setup(r => r.GetAllInvestorsWithFundsAsync())
                .ReturnsAsync(investors);

            // Act
            var result = await _service.GetAllInvestorsWithFundsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());  // Check for multiple investors
            Assert.Equal("Ullamcorper Viverra Fund", result.First().Funds[0].FundName);
        }

        [Fact]
        public async Task GetAllInvestors_ShouldReturnEmpty_WhenNoInvestorsExist()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAllInvestorsWithFundsAsync())
                .ReturnsAsync(new List<Investors>());

            // Act
            var result = await _service.GetAllInvestorsWithFundsAsync();

            // Assert
            Assert.Empty(result);  // No investors should be returned
        }

    }
}
