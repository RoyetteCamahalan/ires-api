using ires.Domain.Enumerations;

namespace ires.Domain.Models
{
    public class Car
    {
        public long id { get; set; }
        public int companyid { get; set; }
        public string name { get; set; } = string.Empty;
        public int typeid { get; set; }
        public string platenumber { get; set; } = string.Empty;
        public string model { get; set; } = string.Empty;
        public string year { get; set; } = string.Empty;
        public CarStatus status { get; set; }
        public long createdbyid { get; set; }
        public DateTime datecreated { get; set; }
        public long updatedbyid { get; set; }
        public DateTime? dateupdated { get; set; }

        public CarType carType { get; set; } = new CarType();
    }
}
