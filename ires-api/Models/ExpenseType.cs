
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires_api.Models
{
    [Table("expensetypes")]
    public class ExpenseType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long expensetypeid { get; set; }
        public int companyid { get; set; }
        public string expensetypedesc { get; set; } = string.Empty;
        public int expensetypecat { get; set; }
        public bool isactive { get; set; }
        public long createdbyid { get; set; }
        public DateTime? datecreated { get; set; }
        public long updatedbyid { get; set; }
        public DateTime? dateupdated { get; set; }
        public ExpenseTypeCategory? category { get; set; }
    }
}