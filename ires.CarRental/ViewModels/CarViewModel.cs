using ires.Domain.Enumerations;

namespace ires.Application.ViewModels
{
    public class CarViewModel
    {
        public long id { get; set; }
        public int companyid { get; set; }
        public string name { get; set; } = string.Empty;
        public int typeid { get; set; }
        public string platenumber { get; set; } = string.Empty;
        public string model { get; set; } = string.Empty;
        public string year { get; set; } = string.Empty;
        public CarStatus status { get; set; }

        public CarTypeViewModel carType { get; set; } = new CarTypeViewModel();
    }
}
