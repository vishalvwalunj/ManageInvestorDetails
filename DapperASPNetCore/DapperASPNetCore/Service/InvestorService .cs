using DapperASPNetCore.Contracts;
using DapperASPNetCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DapperASPNetCore.Service
{
    public class InvestorService : IInvestorService
    {
        private readonly IInvestorRepository _repository;

        public InvestorService(IInvestorRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Investors>> GetAllInvestorsWithFundsAsync()
        {
            return await _repository.GetAllInvestorsWithFundsAsync();
        }

        public async Task<Investors> GetInvestorWithFundsAsync(int investorId)
        {
            return await _repository.GetInvestorWithFundsAsync(investorId);
        }

        public async Task AddFundToInvestorAsync(int investorId, string fundName)
        {
            var fund = await _repository.GetFundsByNameAsync(fundName);
            if (fund != null)
            {
                await _repository.AddFundToInvestorAsync(investorId, fund.FundId);
            }
        }

        public async Task AddInvestorAsync(string Name, string email)
        {
            var investor = new Investors {Name = Name, Email = email };
            await _repository.AddInvestorAsync(investor);
        }

        public async Task DeleteInvestorAsync(int investorId)
        {
            await _repository.DeleteInvestorAsync(investorId);
        }

       
    }
}
