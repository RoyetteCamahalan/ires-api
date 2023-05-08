using AutoMapper;
using ires_api.DTO;
using ires_api.Models;
using ires_api.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public ExpenseController(IExpenseService expenseService, ILogService logService, IMapper mapper)
        {
            _expenseService = expenseService;
            _logService = logService;
            _mapper = mapper;
        }
        [HttpGet("getexpensetype")]
        public async Task<IActionResult> GetExpenseType(long id)
        {
            var serverResponse = new ServerResponse<ExpenseTypeDto>();
            var result = await _expenseService.GetExpenseTypeByID(id);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Record not found";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<ExpenseTypeDto>(result);
            return Ok(serverResponse);

        }

        [HttpGet("getexpensetypes")]
        public async Task<IActionResult> GetExpenseTypes(int currentPage, bool viewAll, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<ExpenseTypeDto>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _expenseService.GetExpenseTypes(identity.companyid ?? 0, viewAll, search ?? "");
            var paginator = new PaginatorDto<ExpenseTypeDto>(currentPage);
            paginator.Paginate(_mapper.Map<List<ExpenseTypeDto>>(result));
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }
        [HttpPost("createexpensetype")]
        public async Task<IActionResult> CreateExpenseType([FromBody] ExpenseTypeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<ExpenseTypeDto>();
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
            result = await _expenseService.CreateExpenseType(_mapper.Map<ExpenseType>(requestDto));
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<ExpenseTypeDto>(result);
            _logService.SaveLog(result.companyid, identity.employeeid, 0, "Expense Type", "Create New Expense Type : " + result.expensetypeid + "-" + requestDto.expensetypedesc, 0);
            return Ok(serverResponse);
        }

        [HttpPut("updateexpensetype")]
        public async Task<IActionResult> UpdateExpenseType([FromBody] ExpenseTypeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<ExpenseTypeDto>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            requestDto.updatedbyid = identity.employeeid;
            var result = await _expenseService.UpdateExpenseType(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<ExpenseTypeDto>(result);
            _logService.SaveLog(result.companyid, identity.employeeid, 0, "Expense Type", "Update Expense Type ID : " + requestDto.expensetypeid, 0);
            return Ok(serverResponse);
        }



        #region "Vendors"

        [HttpGet("getvendor")]
        public async Task<IActionResult> GetVendor(long id)
        {
            var serverResponse = new ServerResponse<VendorDto>();
            var result = await _expenseService.GetVendorByID(id);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Record not found";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<VendorDto>(result);
            return Ok(serverResponse);

        }

        [HttpGet("getvendors")]
        public async Task<IActionResult> GetVendors(int currentPage, bool viewAll, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<VendorDto>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _expenseService.GetVendors(identity.companyid ?? 0, viewAll, search ?? "");
            var paginator = new PaginatorDto<VendorDto>(currentPage);
            paginator.Paginate(_mapper.Map<List<VendorDto>>(result));
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }
        [HttpPost("createvendor")]
        public async Task<IActionResult> CreateVendor([FromBody] VendorRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<VendorDto>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _expenseService.GetVendorByName(requestDto.companyid, requestDto.vendorname);
            if (result != null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Record already exist.";
                return BadRequest(serverResponse);
            }
            requestDto.companyid = identity.companyid ?? 0;
            requestDto.createdbyid = identity.employeeid;
            result = await _expenseService.CreateVendor(_mapper.Map<Vendor>(requestDto));
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<VendorDto>(result);
            _logService.SaveLog(result.companyid, identity.employeeid, 0, "Vendor", "Create New Vendor : " + result.vendorid + "-" + requestDto.vendorname, 0);
            return Ok(serverResponse);
        }

        [HttpPut("updatevendor")]
        public async Task<IActionResult> UpdateVendor([FromBody] VendorRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<VendorDto>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            requestDto.updatedbyid = identity.employeeid;
            var result = await _expenseService.UpdateVendor(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<VendorDto>(result);
            _logService.SaveLog(result.companyid, identity.employeeid, 0, "Vendor", "Update Vendor ID : " + requestDto.vendorid, 0);
            return Ok(serverResponse);
        }
        #endregion

        #region "Accounts Payable"

        [HttpGet("getaccountpayable")]
        public async Task<IActionResult> GetAccountPayable(long id)
        {
            var serverResponse = new ServerResponse<AccountPayableDto>();
            var result = await _expenseService.GetAccountPayableByID(id);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Record not found";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<AccountPayableDto>(result);
            return Ok(serverResponse);
        }

        [HttpGet("getaccountpayables")]
        public async Task<IActionResult> GetAccountPayables(int currentPage, DateTime startDate, DateTime endDate, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<AccountPayableDto>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = await _expenseService.GetAccountPayables(identity.companyid ?? 0, search ?? "", startDate, endDate);
            var paginator = new PaginatorDto<AccountPayableDto>(currentPage);
            paginator.Paginate(_mapper.Map<List<AccountPayableDto>>(result));
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }
        [HttpPost("createaccountpayable")]
        public async Task<IActionResult> CreateAccountPayable([FromBody] AccountPayableRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<AccountPayableDto>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            requestDto.companyid = identity.companyid ?? 0;
            requestDto.createdbyid = identity.employeeid;
            var result = await _expenseService.CreateAccountPayable(_mapper.Map<AccountPayable>(requestDto));
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<AccountPayableDto>(result);
            _logService.SaveLog(result.companyid, identity.employeeid, 0, "Accounts Payable", "Create New Record : " + result.chargeid, 0);
            return Ok(serverResponse);
        }

        [HttpPut("updateaccountpayable")]
        public async Task<IActionResult> UpdateAccountPayable([FromBody] AccountPayableRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<AccountPayableDto>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            requestDto.updatedbyid = identity.employeeid;
            var result = await _expenseService.UpdateAccountPayable(requestDto);
            if (result == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = _mapper.Map<AccountPayableDto>(result);
            _logService.SaveLog(result.companyid, identity.employeeid, 0, "Accounts Payable", "Update Record ID : " + requestDto.chargeid, 0);
            return Ok(serverResponse);
        }

        [HttpPut("voidaccountpayable")]
        public async Task<IActionResult> VoidAccountPayable([FromBody] IDRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            serverResponse.Success = await _expenseService.VoidAccountPayable(requestDto.id);
            serverResponse.Data = serverResponse.Success;
            if (!serverResponse.Success)
            {
                serverResponse.Message = "Record not found";
                return BadRequest(serverResponse);
            }
            _logService.SaveLog(identity.companyid ?? 0, identity.employeeid, 0, "Accounts Payable", "Void Record : " + requestDto.id, 0);
            return Ok(serverResponse);
        }
        #endregion
    }
}
