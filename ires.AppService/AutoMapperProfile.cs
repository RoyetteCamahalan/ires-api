

using AutoMapper;
using ires.Domain.DTO;
using ires.Domain.DTO.AccountPayable;
using ires.Domain.DTO.Attachment;
using ires.Domain.DTO.Bank;
using ires.Domain.DTO.BankAccount;
using ires.Domain.DTO.CashDisbursement;
using ires.Domain.DTO.Client;
using ires.Domain.DTO.Company;
using ires.Domain.DTO.CompanySetting;
using ires.Domain.DTO.CreditNote;
using ires.Domain.DTO.Employee;
using ires.Domain.DTO.Expense;
using ires.Domain.DTO.ExpenseType;
using ires.Domain.DTO.Office;
using ires.Domain.DTO.OtherCharge;
using ires.Domain.DTO.OtherFee;
using ires.Domain.DTO.Payment;
using ires.Domain.DTO.PettyCash;
using ires.Domain.DTO.Project;
using ires.Domain.DTO.RentalCharge;
using ires.Domain.DTO.RentalContract;
using ires.Domain.DTO.RentalContractDetail;
using ires.Domain.DTO.RentalUnit;
using ires.Domain.DTO.Survey;
using ires.Domain.DTO.User;
using ires.Domain.DTO.Vendor;
using ires.Infrastructure.Entities;
using ires.Infrastructure.Keyless;

namespace ires.AppService
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Attachment, AttachmentViewModel>();

            CreateMap<AccountPayableRequestDto, AccountPayable>();
            CreateMap<AccountPayable, AccountPayableViewModel>();

            CreateMap<BankRequestDto, Bank>();
            CreateMap<Bank, BankViewModel>();

            CreateMap<BankAccountRequestDto, BankAccount>();
            CreateMap<BankAccount, BankAccountViewModel>();

            CreateMap<BankTransferRequestDto, BankTransfer>();
            CreateMap<BankTransfer, BankTransferViewModel>();

            CreateMap<Bill, BillViewModel>();

            CreateMap<CashDisbursementRequestDto, CashDisbursement>();
            CreateMap<CashDisbursement, CashDisbursementViewModel>();

            CreateMap<ClientRequestDto, Client>();
            CreateMap<Client, ClientViewModel>();
            CreateMap<Company, CompanyViewModel>();
            CreateMap<Company, CompanyPlanViewModel>();

            CreateMap<CompanySetting, CompanySettingViewModel>();

            CreateMap<CreditMemoTypeRequestDto, CreditMemoType>();
            CreateMap<CreditMemoType, CreditMemoTypeViewModel>();

            CreateMap<EmployeeRequestDto, Employee>();
            CreateMap<Employee, EmployeeViewModel>();

            CreateMap<EmployeeViewModel, UserLoginViewModel>()
                .ForMember(dest => dest.company,
                opts => opts.MapFrom(src =>
                src.company));

            CreateMap<Expense, ExpenseViewModel>();
            CreateMap<ExpenseRequestDto, Expense>();

            CreateMap<ExpenseType, ExpenseTypeViewModel>();
            CreateMap<ExpenseTypeRequestDto, ExpenseType>();

            CreateMap<ExpenseTypeCategory, ExpenseTypeCategoryViewModel>();

            CreateMap<Module, ModuleDto>();

            CreateMap<Notification, NotificationViewModel>();

            CreateMap<OfficeRequestDto, Office>();
            CreateMap<Office, OfficeViewModel>();

            CreateMap<OtherFeeRequestDto, OtherFee>();
            CreateMap<OtherFee, OtherFeeViewModel>();

            CreateMap<PettyCashAccountHistory, PettyCashAccountHistoryViewModel>();

            CreateMap<RentalProjectRequestDto, ProjectRequestDto>();
            CreateMap<Project, RentalProjectViewModel>();
            CreateMap<ProjectRequestDto, Project>();



            CreateMap<RentalUnitRequestDto, RentalProperty>().ForMember(x => x.status, opt => opt.Ignore());
            CreateMap<RentalProperty, RentalUnitViewModel>();

            CreateMap<RentalContractRequestDto, RentalContract>()
                .ForMember(x => x.rentalContractDetails, opt => opt.Ignore())
                .ForMember(x => x.term, opt => opt.MapFrom(y => y.term ?? 0))
                .ForMember(x => x.noofmonthadvance, opt => opt.MapFrom(y => y.noofmonthadvance ?? 0))
                .ForMember(x => x.deposit, opt => opt.MapFrom(y => y.deposit ?? 0))
                .ForMember(x => x.monthlypenalty, opt => opt.MapFrom(y => y.monthlypenalty ?? 0))
                .ForMember(x => x.penaltyextension, opt => opt.MapFrom(y => y.penaltyextension ?? 0));
            CreateMap<RentalContract, RentalContractViewModel>();

            CreateMap<RentalContractDetailRequestDto, RentalContractDetail>();
            CreateMap<RentalContractDetail, RentalContractDetailViewModel>();
            CreateMap<RentalAccountHistory, RentalHistoryViewModel>();

            CreateMap<RentalChargeRequestDto, RentalCharge>();
            CreateMap<RentalCharge, RentalChargeViewModel>();

            CreateMap<UserPrivilege, UserPrivilegeViewModel>();

            CreateMap<SurveyRequestDto, Survey>();
            CreateMap<Survey, SurveyViewModel>();

            CreateMap<OtherChargeRequestDto, OtherCharge>();
            CreateMap<OtherCharge, OtherChargeViewModel>();
            CreateMap<PaymentRequestDto, Payment>();
            CreateMap<Payment, PaymentViewModel>();
            //.ForMember(dest => dest.client, opts => opts.MapFrom(src => src.client));
            CreateMap<PaymentCheckRequestDto, PaymentCheck>();
            CreateMap<PaymentCheck, PaymentCheckViewModel>();
            CreateMap<PaymentDetail, PaymentDetailViewModel>();
            CreateMap<Payable, PayableViewModel>();
            CreateMap<SubscriptionPlan, SubscriptionPlanViewModel>();

            CreateMap<VendorRequestDto, Vendor>();
            CreateMap<Vendor, VendorViewModel>();
        }
    }
}
