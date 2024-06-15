using ires.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("survey")]
    public class Survey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public int companyid { get; set; }
        public long custid { get; set; }
        public string owner { get; set; } = string.Empty;
        public string titleno { get; set; } = string.Empty;
        public string surveyno { get; set; } = string.Empty;
        public DateTime? surveydate { get; set; }
        public string propertyname { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public decimal landarea { get; set; }
        public string details { get; set; } = string.Empty;
        public decimal contractprice { get; set; }
        public decimal balance { get; set; }
        public SurveyStatus status { get; set; }
        public long createdbyid { get; set; }
        public DateTime? datecreated { get; set; }
        public long updatedbyid { get; set; }
        public DateTime? dateupdated { get; set; }

        public Client? client { get; set; }

        public ICollection<OtherCharge>? otherCharges { get; set; }

    }
}
