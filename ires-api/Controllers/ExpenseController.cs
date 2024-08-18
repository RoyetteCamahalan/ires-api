using ires.AppService.Common;
using ires.Domain.Common;
using ires.Domain.Contracts;
using ires.Domain.DTO;
using ires.Domain.DTO.AccountPayable;
using ires.Domain.DTO.Expense;
using ires.Domain.DTO.ExpenseType;
using ires.Domain.DTO.Vendor;
using ires.Domain.Enumerations;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController(IExpenseService _expenseService, IAccountService _accountService) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> Get(long id)
        {
            var serverResponse = new ServerResponse<ExpenseViewModel>
            {
                Data = await _expenseService.GetExpenseByID(id)
            };
            return Ok(serverResponse);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequest request)
        {
            var serverResponse = new ServerResponse<PaginatedResult<ExpenseViewModel>>
            {
                Data = await _expenseService.GetExpenses(request)
            };
            return Ok(serverResponse);

        }

        [HttpGet("getexpensereport")]
        public async Task<IActionResult> GetExpenseReport([FromQuery] PaginationRequest request)
        {
            var serverResponse = new ServerResponse<ExpenseReportViewModel>();
            var result = (await _expenseService.GetExpenses(request)).data;
            result = result.Where(x => x.status != ExpenseStatus.@void).ToList();
            var expenseReport = new ExpenseReportViewModel
            {
                totalExpense = result.Select(x => x.amount).Sum(),
                expenses = result
            };
            serverResponse.Data = expenseReport;
            return Ok(serverResponse);

        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ExpenseRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<ExpenseViewModel>();
            if (requestDto.usepettycash)
            {
                var office = await _accountService.GetOfficeByID(requestDto.accountid);
                if (office.pettycashbalance < requestDto.amount)
                {
                    serverResponse.Success = false;
                    serverResponse.message = "Insufficient petty cash balance";
                    return BadRequest(serverResponse);
                }
            }
            serverResponse.Data = await _expenseService.Create(requestDto);
            return Ok(serverResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ExpenseRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            if (requestDto.usepettycash)
            {
                var expense = await _expenseService.GetExpenseByID(requestDto.expenseid);
                decimal forDeduction;
                if (expense.accountid == requestDto.accountid)
                    forDeduction = requestDto.amount - expense.amount;
                else
                    forDeduction = requestDto.amount;

                var office = await _accountService.GetOfficeByID(requestDto.accountid);
                if (office.pettycashbalance < forDeduction && forDeduction > 0)
                {
                    serverResponse.Success = false;
                    serverResponse.message = "Insufficient petty cash balance";
                    return BadRequest(serverResponse);
                }
            }
            await _expenseService.Update(requestDto);
            return Ok(serverResponse);
        }

        [HttpPut("voidexpense")]
        public async Task<IActionResult> VoidExpense([FromBody] IDRequestDto requestDto)
        {
            await _expenseService.VoidExpense(requestDto.id);
            return Ok(new ServerResponse<bool>());
        }




        [HttpGet("getexpensetype")]
        public async Task<IActionResult> GetExpenseType(long id)
        {
            var serverResponse = new ServerResponse<ExpenseTypeViewModel>();
            var result = await _expenseService.GetExpenseTypeByID(id);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.message = "Record not found";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);

        }

        [HttpGet("getexpensetypes")]
        public async Task<IActionResult> GetExpenseTypes([FromQuery] PaginationRequest request)
        {
            var serverResponse = new ServerResponse<PaginatedResult<ExpenseTypeViewModel>>
            {
                Data = await _expenseService.GetExpenseTypes(request)
            };
            return Ok(serverResponse);

        }
        [HttpPost("createexpensetype")]
        public async Task<IActionResult> CreateExpenseType([FromBody] ExpenseTypeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<ExpenseTypeViewModel>();
            var result = await _expenseService.GetExpenseTypeByName(requestDto.expensetypedesc);
            if (result != null)
            {
                serverResponse.Success = false;
                serverResponse.message = "Record already exist.";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = await _expenseService.CreateExpenseType(requestDto);
            return Ok(serverResponse);
        }

        [HttpPut("updateexpensetype")]
        public async Task<IActionResult> UpdateExpenseType([FromBody] ExpenseTypeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            await _expenseService.UpdateExpenseType(requestDto);
            return Ok(serverResponse);
        }



        #region "Vendors"

        [HttpGet("getvendor")]
        public async Task<IActionResult> GetVendor(long id)
        {
            var serverResponse = new ServerResponse<VendorViewModel>
            {
                Data = await _expenseService.GetVendorByID(id)
            };
            return Ok(serverResponse);

        }

        [HttpGet("getvendors")]
        public async Task<IActionResult> GetVendors([FromQuery] PaginationRequest request)
        {
            var serverResponse = new ServerResponse<PaginatedResult<VendorViewModel>>
            {
                Data = await _expenseService.GetVendors(request)
            };
            return Ok(serverResponse);

        }
        [HttpPost("createvendor")]
        public async Task<IActionResult> CreateVendor([FromBody] VendorRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<VendorViewModel>();
            var result = await _expenseService.GetVendorByName(requestDto.vendorname);
            if (result != null)
            {
                serverResponse.Success = false;
                serverResponse.message = "Record already exist.";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = await _expenseService.CreateVendor(requestDto);
            return Ok(serverResponse);
        }

        [HttpPut("updatevendor")]
        public async Task<IActionResult> UpdateVendor([FromBody] VendorRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            await _expenseService.UpdateVendor(requestDto);
            return Ok(serverResponse);
        }
        #endregion

        #region "Accounts Payable"

        [HttpGet("getaccountpayable")]
        public async Task<IActionResult> GetAccountPayable(long id)
        {
            var serverResponse = new ServerResponse<AccountPayableViewModel>
            {
                Data = await _expenseService.GetAccountPayableByID(id)
            };
            return Ok(serverResponse);
        }

        [HttpGet("getaccountpayables")]
        public async Task<IActionResult> GetAccountPayables([FromQuery] PaginationRequest request)
        {
            var serverResponse = new ServerResponse<PaginatedResult<AccountPayableViewModel>>
            {
                Data = await _expenseService.GetAccountPayables(request)
            };
            return Ok(serverResponse);

        }
        [HttpPost("createaccountpayable")]
        public async Task<IActionResult> CreateAccountPayable([FromBody] AccountPayableRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<AccountPayableViewModel>
            {
                Data = await _expenseService.CreateAccountPayable(requestDto)
            };
            return Ok(serverResponse);
        }

        [HttpPut("updateaccountpayable")]
        public async Task<IActionResult> UpdateAccountPayable([FromBody] AccountPayableRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            await _expenseService.UpdateAccountPayable(requestDto);
            return Ok(serverResponse);
        }

        [HttpPut("voidaccountpayable")]
        public async Task<IActionResult> VoidAccountPayable([FromBody] IDRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            await _expenseService.VoidAccountPayable(requestDto.id);
            return Ok(serverResponse);
        }
        #endregion
    }
}
