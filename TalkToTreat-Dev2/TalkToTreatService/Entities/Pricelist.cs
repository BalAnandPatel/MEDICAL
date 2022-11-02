using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkToTreatService.Entities
{
    public class Pricelist
    {
        public string StoreFrontId { get; set; }
        public long ExternalRefId { get; set; }
        public string Name { get; set; }
        public string CurrencyCode { get; set; }
        public string StockCode { get; set; }
        public decimal CostPrice { get; set; }
        public decimal RRP { get; set; }
        public decimal SellPriceIncVat { get; set; }
        public decimal SellPriceExcVat { get; set; }
        public decimal Worth { get; set; }
        public DateTime PriceEffectiveFrom { get; set; }

    }
}
