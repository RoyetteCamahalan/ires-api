using AutoMapper;
using ires_api.DTO;
using ires_api.DTO.AccountPayable;
using ires_api.DTO.Attachment;
using ires_api.DTO.Bank;
using ires_api.DTO.BankAccount;
using ires_api.DTO.CashDisbursement;
using ires_api.DTO.Client;
using ires_api.DTO.Company;
using ires_api.DTO.Employee;
using ires_api.DTO.Expense;
using ires_api.DTO.ExpenseType;
using ires_api.DTO.Office;
using ires_api.DTO.OtherCharge;
using ires_api.DTO.Payment;
using ires_api.DTO.Project;
using ires_api.DTO.RentalContract;
using ires_api.DTO.RentalContractDetail;
using ires_api.DTO.RentalUnit;
using ires_api.DTO.Survey;
using ires_api.DTO.User;
using ires_api.DTO.Vendor;
using ires_api.Models;

namespace ires_api
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Attachment, AttachmentDto>();

            CreateMap<AccountPayable, AccountPayableDto>();
            CreateMap<AccountPayable, AccountPayableRequestDto>().ReverseMap();

            CreateMap<Bank, BankDto>();
            CreateMap<Bank, BankRequestDto>().ReverseMap();
            CreateMap<BankAccount, BankAccountDto>();
            CreateMap<BankAccount, BankAccountRequestDto>().ReverseMap();

            CreateMap<BankTransfer, BankTransferRequestDto>().ReverseMap();
            CreateMap<BankTransfer, BankTransfer>().ReverseMap();

            CreateMap<Bill, BillDto>();

            CreateMap<CashDisbursement, CashDisbursementDto>();
            CreateMap<CashDisbursement, CashDisbursementRequestDto>().ReverseMap();

            CreateMap<Client, ClientDto>().ReverseMap();
            CreateMap<Company, CompanyDto>()
                .ForMember(dest => dest.isexpired,
                opts => opts.MapFrom(src =>
                src.subscriptionexpiry < DateTime.Now.AddDays(-1)));
            CreateMap<Company, CompanyPlanDto>()
                .ForMember(dest => dest.isexpired,
                opts => opts.MapFrom(src =>
                src.subscriptionexpiry < DateTime.Now.AddDays(-1)));

            CreateMap<Employee, EmployeeDto>();

            CreateMap<Employee, EmployeeRequestDto>().ReverseMap();

            CreateMap<Employee, UserLoginDto>()
                .ForMember(dest => dest.company,
                opts => opts.MapFrom(src =>
                src.company));

            CreateMap<Expense, ExpenseDto>();
            CreateMap<Expense, ExpenseRequestDto>().ReverseMap();

            CreateMap<ExpenseType, ExpenseTypeDto>();
            CreateMap<ExpenseType, ExpenseTypeRequestDto>().ReverseMap();

            CreateMap<ExpenseTypeCategory, ExpenseTypeCategoryDto>();

            CreateMap<Module, ModuleDto>();

            CreateMap<Notification, NotificationDto>();

            CreateMap<Office, OfficeDto>();
            CreateMap<Office, OfficeRequestDto>().ReverseMap();


            CreateMap<RentalProjectRequestDto, ProjectRequestDto>();
            CreateMap<Project, RentalProjectViewModel>();
            CreateMap<Project, ProjectRequestDto>()
                .ReverseMap();



            CreateMap<RentalUnitRequestDto, RentalProperty>().ForMember(x => x.status, opt => opt.Ignore());
            CreateMap<RentalProperty, RentalUnitViewModel>().ReverseMap();

            CreateMap<RentalContractRequestDto, RentalContract>();
            CreateMap<RentalContract, RentalContractViewModel>().ReverseMap();

            CreateMap<RentalContractDetailRequestDto, RentalContractDetail>();
            CreateMap<RentalContractDetail, RentalContractDetailViewModel>().ReverseMap();

            CreateMap<UserPrivilege, UserPrivilegeDto>();

            CreateMap<Survey, SurveyDto>();
            CreateMap<Survey, SurveyRequestDto>().ReverseMap();

            CreateMap<OtherCharge, OtherChargeDto>();
            CreateMap<OtherCharge, OtherChargeRequestDto>().ReverseMap();
            CreateMap<Payment, PaymentRequestDto>().ReverseMap();
            CreateMap<Payment, PaymentDto>();
            //.ForMember(dest => dest.client, opts => opts.MapFrom(src => src.client));
            CreateMap<PaymentCheck, PaymentCheckRequestDto>().ReverseMap();
            CreateMap<SubscriptionPlan, PlanDto>();

            CreateMap<Vendor, VendorDto>();
            CreateMap<Vendor, VendorRequestDto>().ReverseMap();
        }
    }
}
