using ires_api.DTO;
using ires_api.Models;

namespace ires_api.Services.Interface
{
    public interface IExpenseService
    {
        public ExpenseType CreateExpenseType(ExpenseType expenseType);
        public ExpenseType UpdateExpenseType(ExpenseTypeRequestDto requestDto);
        public ExpenseType GetExpenseTypeByID(long ID);
        public ExpenseType GetExpenseTypeByName(int companyID, string name);
        public ICollection<ExpenseType> GetExpenseTypes(int companyID, string search);
    }
}
