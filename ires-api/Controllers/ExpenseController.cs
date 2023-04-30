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
        public IActionResult GetExpenseType(long id)
        {
            var serverResponse = new ServerResponse<ExpenseTypeDto>();
            var result = _expenseService.GetExpenseTypeByID(id);
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
        public IActionResult GetExpenseTypes(int currentPage, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<ExpenseTypeDto>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            var result = _expenseService.GetExpenseTypes(identity.companyid ?? 0, search ?? "");
            var paginator = new PaginatorDto<ExpenseTypeDto>(currentPage);
            paginator.Paginate(_mapper.Map<List<ExpenseTypeDto>>(result));
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }
        [HttpPost("createexpensetype")]
        public IActionResult CreateExpenseType([FromBody] ExpenseTypeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<ExpenseTypeDto>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = _expenseService.GetExpenseTypeByName(requestDto.companyid, requestDto.expensetypedesc);
            if (result != null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Record already exist.";
                return BadRequest(serverResponse);
            }
            requestDto.companyid = identity.companyid ?? 0;
            requestDto.createdbyid = identity.employeeid;
            result = _expenseService.CreateExpenseType(_mapper.Map<ExpenseType>(requestDto));
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
        public IActionResult UpdateExpenseType([FromBody] ExpenseTypeRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<ExpenseTypeDto>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            requestDto.updatedbyid = identity.employeeid;
            var result = _expenseService.UpdateExpenseType(requestDto);
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
        public IActionResult GetVendor(long id)
        {
            var serverResponse = new ServerResponse<VendorDto>();
            var result = _expenseService.GetVendorByID(id);
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
        public IActionResult GetVendors(int currentPage, string? search = "")
        {
            var serverResponse = new ServerResponse<PaginatorDto<VendorDto>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            var result = _expenseService.GetVendors(identity.companyid ?? 0, search ?? "");
            var paginator = new PaginatorDto<VendorDto>(currentPage);
            paginator.Paginate(_mapper.Map<List<VendorDto>>(result));
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }
        [HttpPost("createvendor")]
        public IActionResult CreateVendor([FromBody] VendorRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<VendorDto>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            var result = _expenseService.GetVendorByName(requestDto.companyid, requestDto.vendorname);
            if (result != null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Record already exist.";
                return BadRequest(serverResponse);
            }
            requestDto.companyid = identity.companyid ?? 0;
            requestDto.createdbyid = identity.employeeid;
            result = _expenseService.CreateVendor(_mapper.Map<Vendor>(requestDto));
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
        public IActionResult UpdateVendor([FromBody] VendorRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<VendorDto>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            requestDto.updatedbyid = identity.employeeid;
            var result = _expenseService.UpdateVendor(requestDto);
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
    }
}
