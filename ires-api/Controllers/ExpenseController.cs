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
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly IAccountService _accountService;

        public ExpenseController(IExpenseService expenseService, IAccountService accountService)
        {
            _expenseService = expenseService;
            _accountService = accountService;
        }


        [HttpGet]
        public async Task<IActionResult> Get(long id)
        {
            var serverResponse = new ServerResponse<ExpenseViewModel>();
            var result = await _expenseService.GetExpenseByID(id);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Record not found";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll(int currentPage, DateTime startDate, DateTime endDate, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<ExpenseViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _expenseService.GetExpenses(identity.companyid ?? 0, search ?? "", startDate, endDate);
            var paginator = new PaginatorDto<ExpenseViewModel>(currentPage);
            paginator.Paginate(result);
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }

        [HttpGet("getexpensereport")]
        public async Task<IActionResult> GetExpenseReport(DateTime startDate, DateTime endDate)
        {
            var serverResponse = new ServerResponse<ExpenseReportViewModel>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _expenseService.GetExpenses(identity.companyid ?? 0, "", startDate, endDate);
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
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (requestDto.usepettycash)
            {
                var office = await _accountService.GetOfficeByID(requestDto.accountid);
                if (office.pettycashbalance < requestDto.amount)
                {
                    serverResponse.Success = false;
                    serverResponse.Message = "Insufficient petty cash balance";
                    return BadRequest(serverResponse);
                }
            }
            requestDto.companyid = identity.companyid ?? 0;
            requestDto.createdbyid = identity.employeeid;
            var result = await _expenseService.Create(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ExpenseRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);

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
                    serverResponse.Message = "Insufficient petty cash balance";
                    return BadRequest(serverResponse);
                }
            }
            requestDto.updatedbyid = identity.employeeid;
            if (!await _expenseService.Update(requestDto))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }

        [HttpPut("voidexpense")]
        public async Task<IActionResult> VoidExpense([FromBody] IDRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            serverResponse.Success = await _expenseService.VoidExpense(requestDto.id, identity.employeeid);
            serverResponse.Data = serverResponse.Success;
            if (!serverResponse.Success)
            {
                serverResponse.Message = "Record not found";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }




        [HttpGet("getexpensetype")]
        public async Task<IActionResult> GetExpenseType(long id)
        {
            var serverResponse = new ServerResponse<ExpenseTypeViewModel>();
            var result = await _expenseService.GetExpenseTypeByID(id);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Record not found";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);

        }

        [HttpGet("getexpensetypes")]
        public async Task<IActionResult> GetExpenseTypes(int currentPage, bool viewAll, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<ExpenseTypeViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _expenseService.GetExpenseTypes(identity.companyid ?? 0, viewAll, search ?? "");
            var paginator = new PaginatorDto<ExpenseTypeViewModel>(currentPage);
            paginator.Paginate(result);
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }
        [HttpPost("createexpensetype")]
        public async Task<IActionResult> CreateExpenseType([FromBody] ExpenseTypeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<ExpenseTypeViewModel>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _expenseService.GetExpenseTypeByName(requestDto.companyid, requestDto.expensetypedesc);
            if (result != null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Record already exist.";
                return BadRequest(serverResponse);
            }
            requestDto.companyid = identity.companyid ?? 0;
            requestDto.createdbyid = identity.employeeid;
            result = await _expenseService.CreateExpenseType(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);
        }

        [HttpPut("updateexpensetype")]
        public async Task<IActionResult> UpdateExpenseType([FromBody] ExpenseTypeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            requestDto.updatedbyid = identity.employeeid;
            if (!await _expenseService.UpdateExpenseType(requestDto))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }



        #region "Vendors"

        [HttpGet("getvendor")]
        public async Task<IActionResult> GetVendor(long id)
        {
            var serverResponse = new ServerResponse<VendorViewModel>();
            var result = await _expenseService.GetVendorByID(id);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Record not found";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);

        }

        [HttpGet("getvendors")]
        public async Task<IActionResult> GetVendors(int currentPage, bool viewAll, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<VendorViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _expenseService.GetVendors(identity.companyid ?? 0, viewAll, search ?? "");
            var paginator = new PaginatorDto<VendorViewModel>(currentPage);
            paginator.Paginate(result);
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }
        [HttpPost("createvendor")]
        public async Task<IActionResult> CreateVendor([FromBody] VendorRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<VendorViewModel>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _expenseService.GetVendorByName(identity.companyid ?? 0, requestDto.vendorname);
            if (result != null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Record already exist.";
                return BadRequest(serverResponse);
            }
            requestDto.companyid = identity.companyid ?? 0;
            requestDto.createdbyid = identity.employeeid;
            result = await _expenseService.CreateVendor(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);
        }

        [HttpPut("updatevendor")]
        public async Task<IActionResult> UpdateVendor([FromBody] VendorRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            requestDto.updatedbyid = identity.employeeid;
            if (!await _expenseService.UpdateVendor(requestDto))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }
        #endregion

        #region "Accounts Payable"

        [HttpGet("getaccountpayable")]
        public async Task<IActionResult> GetAccountPayable(long id)
        {
            var serverResponse = new ServerResponse<AccountPayableViewModel>();
            var result = await _expenseService.GetAccountPayableByID(id);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Record not found";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);
        }

        [HttpGet("getaccountpayables")]
        public async Task<IActionResult> GetAccountPayables(int currentPage, DateTime startDate, DateTime endDate, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<AccountPayableViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _expenseService.GetAccountPayables(identity.companyid ?? 0, search ?? "", startDate, endDate);
            var paginator = new PaginatorDto<AccountPayableViewModel>(currentPage);
            paginator.Paginate(result);
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }
        [HttpPost("createaccountpayable")]
        public async Task<IActionResult> CreateAccountPayable([FromBody] AccountPayableRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<AccountPayableViewModel>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            requestDto.companyid = identity.companyid ?? 0;
            requestDto.createdbyid = identity.employeeid;
            var result = await _expenseService.CreateAccountPayable(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = result;
            return Ok(serverResponse);
        }

        [HttpPut("updateaccountpayable")]
        public async Task<IActionResult> UpdateAccountPayable([FromBody] AccountPayableRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            requestDto.updatedbyid = identity.employeeid;
            if (!await _expenseService.UpdateAccountPayable(requestDto))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }

        [HttpPut("voidaccountpayable")]
        public async Task<IActionResult> VoidAccountPayable([FromBody] IDRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            serverResponse.Success = await _expenseService.VoidAccountPayable(requestDto.id, identity.employeeid);
            serverResponse.Data = serverResponse.Success;
            if (!serverResponse.Success)
            {
                serverResponse.Message = "Record not found";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }
        #endregion
    }
}
