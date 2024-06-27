using DinkToPdf;
using DinkToPdf.Contracts;
using ires.Domain;
using ires.Domain.Contracts;
using ires.Domain.DTO;
using ires.Domain.DTO.Company;
using ires.Domain.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IBillService _billService;
        private readonly IConverter _converter;

        public BillingController(IConfiguration configuration, IBillService billService, IConverter converter)
        {
            _configuration = configuration;
            _billService = billService;
            _converter = converter;
        }
        [HttpGet]
        public async Task<IActionResult> Get(int currentPage, int filter)
        {
            var serverResponse = new ServerResponse<PaginatorDto<BillViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            var billDtos = await _billService.GetBills(identity.companyid ?? 0, filter);
            var paginator = new PaginatorDto<BillViewModel>(currentPage);
            paginator.Paginate(billDtos);
            serverResponse.Data = paginator;
            return Ok(serverResponse);

        }
        [HttpGet("getplans")]
        public async Task<IActionResult> GetPlans()
        {
            var serverResponse = new ServerResponse<ICollection<CompanyPlanViewModel>>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            var plan = await _billService.GetSubscriptionPlans(identity.companyid ?? 0);
            serverResponse.Data = new List<CompanyPlanViewModel> { plan };
            return Ok(serverResponse);
        }

        [HttpPost("processpayment")]
        public async Task<IActionResult> ProcessPayment(IDRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<BillViewModel>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            var bill = await _billService.StartPayment(identity.companyid ?? 0, requestDto.id, new PayMongoConfig(_configuration));
            serverResponse.Data = bill;
            return Ok(serverResponse);
        }

        [HttpPost("completepayment")]
        public async Task<IActionResult> CompletePayment(IDRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<BillViewModel>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (identity == null)
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            var bill = await _billService.CompletePayment(identity.companyid ?? 0, requestDto.id, new PayMongoConfig(_configuration));
            if (bill != null && bill.status == BillStatus.paid)
                serverResponse.Data = bill;
            else
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            return Ok(serverResponse);
        }

        [HttpPut("updatebillingcycle")]
        public IActionResult UpdateBillingCycle(RegisterCompanyRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            requestDto.id = identity.companyid ?? 0;
            requestDto.updatedbyid = identity.employeeid;
            _billService.UpdateBillingCycle(requestDto);
            serverResponse.Data = true;
            return Ok(serverResponse);
        }

        [HttpPut("upgradeplan")]
        public async Task<IActionResult> UpgradePlan(RegisterCompanyRequestDto requestDto)
        {
            var serverResponse = new ServerResponse<bool>();
            var identity = IdentityProfile.getIdentity(this.HttpContext);
            if (!await _billService.UpgradePlan(identity.companyid ?? 0, requestDto.planid, identity.employeeid))
            {
                serverResponse.Success = false;
                serverResponse.Message = "Unable to process request";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = true;
            return Ok(serverResponse);
        }
        [HttpPost("getinvoicedocument/{billid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetInvoice(long billid)
        {
            var serverResponse = new ServerResponse<string>();
            var filePath = await GenerateInvoiceAsync(billid);
            if (filePath == "")
            {
                serverResponse.Success = false;
                serverResponse.Message = "Failed to get invoice";
                return BadRequest(serverResponse);
            }
            serverResponse.Data = filePath;
            return Ok(serverResponse);
        }
        private async Task<string> GenerateInvoiceAsync(long billid)
        {
            var data = await _billService.GetBillByID(billid);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/attachments/invoices/" + data.companyid);
            string fileurl = $"{this.Request.Scheme}://{this.Request.Host}/attachments/invoices/" + data.companyid + "/inv-" + billid + ".pdf";
            string fullpath = path + "/inv-" + billid + ".pdf";
            if (System.IO.File.Exists(fullpath))
                return fileurl;
            try
            {
                var html = System.IO.File.ReadAllText(@"./Templates/Invoice.html");
                var body = html.Replace("{main_link}", _configuration["uiBaseURL"])
                    .Replace("{customername}", data.company.name)
                    .Replace("{logo_path}", Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/banner.png"))
                    .Replace("{customeraddress}", data.company.address)
                    .Replace("{invoiceno}", data.id.ToString())
                    .Replace("{invoicedate}", (data.billdate ?? DateTime.Now).ToString(Constants.dateFormat))
                    .Replace("{duedate}", (data.duedate ?? DateTime.Now).ToString(Constants.dateFormat))
                    .Replace("{description}", data.particular)
                    .Replace("{amount}", data.amount.ToString(Constants.moneyFormat));

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var doc = new HtmlToPdfDocument()
                {
                    GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    PaperSize = PaperKind.A4,
                    DocumentTitle = "Invoice-" + billid,
                    Out = fullpath,
                },
                    Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = body,
                        WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = @"./Templates/main.css" },
                    }
                }
                };
                _converter.Convert(doc);
                return fileurl;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
