using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperASPNetCore.Entities
{
	public class Funds
	{
        public int FundId { get; set; }
        public string FundName { get; set; }
        public decimal AmountInvested { get; set; }

    }
}
