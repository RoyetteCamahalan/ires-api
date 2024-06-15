using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("expensetypecategory")]
    public class ExpenseTypeCategory
    {
        [Key]
        public int expensecatid { get; set; }
        public string description { get; set; } = string.Empty;
        public bool isactive { get; set; }
        public List<ExpenseType> expenseTypes { get; set; } = new List<ExpenseType>();
    }
}
