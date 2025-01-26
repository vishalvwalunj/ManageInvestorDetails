using Dapper;
using DapperASPNetCore.Contracts;
using DapperASPNetCore.Entities;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DapperASPNetCore.Repository
{
    public class InvestorRepository : IInvestorRepository
    {
        private readonly IDbConnection _dbConnection;

        public InvestorRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Investors>> GetAllInvestorsWithFundsAsync()
        {
            var investors = await _dbConnection.QueryAsync<Investors>("SELECT * FROM Investors");

            var investorFunds = await _dbConnection.QueryAsync<InvestorFunds>(
                "SELECT * FROM InvestorFunds");

            foreach (var investor in investors)
            {
                investor.Funds = investorFunds
                    .Where(x => x.InvestorId == investor.InvestorId)
                    .Select(x => new Funds { FundId = x.FundId })
                    .ToList();
            }

            return investors;
        }

        public async Task<Investors> GetInvestorWithFundsAsync(int investorId)
        {
            var investor = await _dbConnection.QuerySingleOrDefaultAsync<Investors>(
                "SELECT * FROM Investors WHERE InvestorId = @InvestorId", new { InvestorId = investorId });

            var investorFunds = await _dbConnection.QueryAsync<InvestorFunds>(
                "SELECT * FROM InvestorFunds WHERE InvestorId = @InvestorId", new { InvestorId = investorId });

            if (investor != null)
            {
                investor.Funds = investorFunds
                    .Select(x => new Funds { FundId = x.FundId })
                    .ToList();
            }

            return investor;
        }

        public async Task AddInvestorAsync(Investors investor)
        {
            var query = "INSERT INTO Investors (Name, Email) VALUES (@Name, @Email)";
            await _dbConnection.ExecuteAsync(query, new { investor.Name, investor.Email });
        }

        public async Task AddFundToInvestorAsync(int investorId, int fundId)
        {
            var query = "INSERT INTO InvestorFunds (InvestorId, FundId) VALUES (@InvestorId, @FundId)";
            await _dbConnection.ExecuteAsync(query, new { InvestorId = investorId, FundId = fundId });
        }

        public async Task DeleteInvestorAsync(int investorId)
        {
            await _dbConnection.ExecuteAsync("DELETE FROM InvestorFunds WHERE InvestorId = @InvestorId", new { InvestorId = investorId });
            await _dbConnection.ExecuteAsync("DELETE FROM Investors WHERE InvestorId = @InvestorId", new { InvestorId = investorId });
        }

        public async Task<Funds> GetFundsByNameAsync(string fundName)
        {
            var fund = await _dbConnection.QuerySingleOrDefaultAsync<Funds>(
               "SELECT * FROM Funds WHERE FundName = @FundName", new { FundName = fundName });

            return fund;
        }
    }
}
