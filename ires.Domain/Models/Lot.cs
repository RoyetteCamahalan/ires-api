using ires.Domain.Enumerations;

namespace ires.Domain.Models
{
    public class Lot
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
        public long model_id { get; set; }
        public decimal compercentage { get; set; }
        public decimal commissionableamount { get; set; }
        public decimal housearea { get; set; }
        public decimal parkingarea { get; set; }
        public decimal comatdown { get; set; }
        public int blocknoint { get; set; }
        public int lotnoint { get; set; }
        public string titleno { get; set; } = string.Empty;
        public LotStatus status { get; set; }


        public LotModel? lotModel { get; set; }
    }
}
