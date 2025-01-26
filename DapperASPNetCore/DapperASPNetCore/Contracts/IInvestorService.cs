using DapperASPNetCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DapperASPNetCore.Contracts
{
    public interface IInvestorService
    {
        Task<IEnumerable<Investors>> GetAllInvestorsWithFundsAsync();
        Task<Investors> GetInvestorWithFundsAsync(int investorId);
        Task AddFundToInvestorAsync(int investorId, string fundName);
        Task AddInvestorAsync(string Name, string email);
        Task DeleteInvestorAsync(int investorId);
    }
}
