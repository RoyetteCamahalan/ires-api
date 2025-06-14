using ires.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ires.Core.ViewModels
{
    public class LotViewModel
    {
        public long lot_id { get; set; }
        public long propertyid { get; set; }
        public string blockno { get; set; } = string.Empty;
        public string lotno { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public decimal area { get; set; }
        public decimal pricepersquare { get; set; }
        public decimal default_price { get; set; }
        public decimal min_down { get; set; }
        public decimal compercentage { get; set; }
        public decimal commissionableamount { get; set; }
        public decimal comatdown { get; set; }
        public int blocknoint { get; set; }
        public int lotnoint { get; set; }
        public string titleno { get; set; } = string.Empty;
        public LotStatus status { get; set; }
        public LotModelViewModel? lotModel { get; set; }
    }
}
