using ires_api.Data;
using ires_api.DTO;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ires_api.Services.Repository
{
    public class ExpenseRepository : IExpenseService
    {
        private readonly DataContext _dataContext;

        public ExpenseRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public ExpenseType CreateExpenseType(ExpenseType expenseType)
        {
            expenseType.expensetypeid = 0;
            expenseType.datecreated = DateTime.Now;
            _dataContext.expenseTypes.Add(expenseType);
            _dataContext.SaveChanges();
            return expenseType;
        }

        public ExpenseType GetExpenseTypeByID(long ID)
        {
            return _dataContext.expenseTypes.Include(x => x.category).FirstOrDefault(x => x.expensetypeid == ID);
        }

        public ExpenseType GetExpenseTypeByName(int companyID, string name)
        {
            return _dataContext.expenseTypes.FirstOrDefault(x => x.companyid == companyID && x.expensetypedesc == name);
        }

        public ICollection<ExpenseType> GetExpenseTypes(int companyID, string search)
        {
            return _dataContext.expenseTypes.Include(x => x.category).Where(x => x.companyid == companyID && x.expensetypedesc.Contains(search))
                .OrderBy(x => x.expensetypedesc).ToList();
        }

        public ExpenseType UpdateExpenseType(ExpenseTypeRequestDto requestDto)
        {
            var expenseType = GetExpenseTypeByID(requestDto.expensetypeid);
            if (expenseType != null)
            {
                expenseType.expensetypedesc = requestDto.expensetypedesc;
                expenseType.expensetypecat = requestDto.expensetypecat;
                expenseType.isactive = requestDto.isactive;
                expenseType.updatedbyid = requestDto.updatedbyid;
                expenseType.dateupdated = DateTime.Now;
                _dataContext.SaveChanges();
            }
            return expenseType;
        }
    }
}
