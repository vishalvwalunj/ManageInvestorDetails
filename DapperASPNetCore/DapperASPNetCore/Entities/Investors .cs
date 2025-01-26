using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperASPNetCore.Entities
{
	public class Investors
    {
        public int InvestorId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public List<Funds> Funds { get; set; } = new List<Funds>();
	}
}
