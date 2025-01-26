using DapperASPNetCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DapperASPNetCore.Contracts
{
    public interface IInvestorRepository
    {
        Task<IEnumerable<Investors>> GetAllInvestorsWithFundsAsync();
        Task<Investors> GetInvestorWithFundsAsync(int investorId);
        Task AddInvestorAsync(Investors investor);
        Task AddFundToInvestorAsync(int investorId, int fundId);
        Task DeleteInvestorAsync(int investorId);
        Task<Funds> GetFundsByNameAsync(string fundName);
    }
}
