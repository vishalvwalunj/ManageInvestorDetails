using DapperASPNetCore.Contracts;
using DapperASPNetCore.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DapperASPNetCore.Controllers
{
    [Route("api/investors")]
	[ApiController]
	public class InvestorsController : ControllerBase
	{
        private readonly IInvestorService _service;

        public InvestorsController(IInvestorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInvestors()
        {
            var investors = await _service.GetAllInvestorsWithFundsAsync();
            return Ok(investors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvestor(int id)
        {
            var investor = await _service.GetInvestorWithFundsAsync(id);
            if (investor == null)
            {
                return NotFound();
            }
            return Ok(investor);
        }

        [HttpPost]
        public async Task<IActionResult> AddInvestor([FromBody] Investors investor)
        {
            await _service.AddInvestorAsync(investor.Name, investor.Email);
            return CreatedAtAction(nameof(GetInvestor), new { id = investor.InvestorId }, investor);
        }

        [HttpPost("{id}/funds")]
        public async Task<IActionResult> AddFundToInvestor(int id, [FromBody] string fundName)
        {
            await _service.AddFundToInvestorAsync(id, fundName);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvestor(int id)
        {
            await _service.DeleteInvestorAsync(id);
            return NoContent();
        }
    }
}
