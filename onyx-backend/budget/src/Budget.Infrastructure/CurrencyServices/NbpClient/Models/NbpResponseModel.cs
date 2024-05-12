using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Infrastructure.CurrencyServices.NbpClient.Models
{
    internal sealed record NbpResponse
    {
        public string Table { get; set; }
        public string Currency { get; set; }
        public string Code { get; set; }
        public List<Rate> Rates { get; set; }
    }

    internal sealed record Rate
    {
        public string No { get; set; }
        public DateTime EffectiveDate { get; set; }
        public decimal Mid { get; set; }
    }

}
