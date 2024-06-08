using ires_api.Models;
using Microsoft.EntityFrameworkCore;

namespace ires_api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.company)
                .WithMany(c => c.employees)
                .HasForeignKey(e => e.companyid);
            modelBuilder.Entity<Survey>()
                .HasOne(s => s.client)
                .WithMany(c => c.surveys)
                .HasForeignKey(e => e.custid);
            modelBuilder.Entity<OtherCharge>()
                .HasOne(o => o.survey)
                .WithMany(s => s.otherCharges)
                .HasForeignKey(o => o.surveyid);

            modelBuilder.Entity<OtherCharge>()
                .HasOne(o => o.fee)
                .WithMany(s => s.otherCharges)
                .HasForeignKey(o => o.chargetype);

            modelBuilder.Entity<PaymentCheck>()
                .HasOne(c => c.bank)
                .WithMany(b => b.checks)
                .HasForeignKey(c => c.bankid);

            modelBuilder.Entity<PaymentCheck>()
                .HasOne(c => c.payment)
                .WithOne(b => b.paymentCheck).HasForeignKey<PaymentCheck>(c => c.paymentid);

            modelBuilder.Entity<AccountPayable>()
                .HasOne(c => c.vendor).WithMany().HasForeignKey(c => c.vendorid);

            modelBuilder.Entity<AccountPayable>()
                .HasOne(c => c.expenseType).WithMany().HasForeignKey(c => c.expensetypeid);

            modelBuilder.Entity<BankAccount>()
                .HasOne(s => s.bank)
                .WithMany(c => c.bankAccounts)
                .HasForeignKey(e => e.bankid);

            modelBuilder.Entity<BankTransfer>()
                .HasOne(c => c.payment)
                .WithOne(b => b.bankTransfer).HasForeignKey<BankTransfer>(c => c.paymentid);

            modelBuilder.Entity<BankTransfer>()
                .HasOne(c => c.bank)
                .WithMany(b => b.bankTransfers).HasForeignKey(c => c.bankid);

            modelBuilder.Entity<Booking>()
                .HasOne(c => c.car).WithMany().HasForeignKey(c => c.carid);
            modelBuilder.Entity<Booking>()
                .HasOne(c => c.client).WithMany().HasForeignKey(c => c.clientid);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.carType).WithMany().HasForeignKey(c => c.typeid);

            modelBuilder.Entity<CarMaintenance>()
                .HasOne(c => c.car).WithMany().HasForeignKey(c => c.carid);
            modelBuilder.Entity<CarMaintenance>()
                .HasOne(c => c.maintenanceType).WithMany().HasForeignKey(c => c.typeid);

            modelBuilder.Entity<CarType>().HasData(
                    new CarType { id = 1, name = "Hatchback", isactive = true },
                    new CarType { id = 2, name = "Sedan", isactive = true },
                    new CarType { id = 3, name = "Minivan", isactive = true },
                    new CarType { id = 4, name = "Crossover", isactive = true },
                    new CarType { id = 5, name = "Pickup", isactive = true },
                    new CarType { id = 6, name = "SUV", isactive = true },
                    new CarType { id = 7, name = "Van", isactive = true },
                    new CarType { id = 8, name = "Others", isactive = true }
                );

            modelBuilder.Entity<CashDisbursement>()
                .HasOne(c => c.office).WithMany().HasForeignKey(c => c.accountid);

            modelBuilder.Entity<CashDisbursement>()
                .HasOne(c => c.refOffice).WithMany().HasForeignKey(c => c.refaccountid).IsRequired(false);

            modelBuilder.Entity<Company>()
                .HasOne(s => s.subscriptionPlan)
                .WithMany(c => c.companies)
                .HasForeignKey(e => e.planid);

            modelBuilder.Entity<Expense>()
                .HasOne(s => s.office)
                .WithMany()
                .HasForeignKey(e => e.accountid);

            modelBuilder.Entity<Expense>()
                .HasOne(s => s.expenseType)
                .WithMany()
                .HasForeignKey(e => e.expensetypeid);

            modelBuilder.Entity<Expense>()
                .HasOne(s => s.vendor)
                .WithMany()
                .HasForeignKey(e => e.payeeid);

            modelBuilder.Entity<ExpenseType>()
                .HasOne(s => s.category)
                .WithMany(c => c.expenseTypes)
                .HasForeignKey(e => e.expensetypecat);

            modelBuilder.Entity<MaintenanceType>().HasData(
                    new MaintenanceType { id = 1, name = "Repair and Maintenance", isactive = true },
                    new MaintenanceType { id = 2, name = "Registration Renewal", isactive = true },
                    new MaintenanceType { id = 3, name = "Personal Use", isactive = true },
                    new MaintenanceType { id = 4, name = "Others", isactive = true }
                );

            modelBuilder.Entity<PaymentDetail>()
                .HasOne(c => c.payment)
                .WithMany(b => b.paymentDetails)
                .HasForeignKey(c => c.paymentid);

            modelBuilder.Entity<Payment>()
                .HasOne(s => s.client)
                .WithMany(c => c.payments)
                .HasForeignKey(e => e.custid);

            modelBuilder.Entity<Payment>()
                .HasOne(s => s.createdBy)
                .WithMany(c => c.encodedPayments)
                .HasForeignKey(e => e.encodedby);

            modelBuilder.Entity<PlanModule>()
                .HasOne(s => s.module)
                .WithMany()
                .HasForeignKey(e => e.moduleid);

            modelBuilder.Entity<RentalProperty>()
                .HasOne(r => r.project)
                .WithMany(p => p.rentalProperties)
                .HasForeignKey(r => r.projectid);

            modelBuilder.Entity<RentalContract>()
                .HasOne(r => r.client)
                .WithMany()
                .HasForeignKey(r => r.custid);

            modelBuilder.Entity<RentalContractDetail>()
                .HasOne(r => r.rentalContract)
                .WithMany(d => d.rentalContractDetails)
                .HasForeignKey(r => r.contractid);

            modelBuilder.Entity<RentalContractDetail>()
                .HasOne(r => r.rentalProperty)
                .WithMany()
                .HasForeignKey(r => r.propertyid);

            modelBuilder.Entity<UserPrivilege>()
                .HasOne(s => s.module)
                .WithMany()
                .HasForeignKey(e => e.moduleid);

        }
        public DbSet<AccountPayable> accountPayables { get; set; }
        public DbSet<Attachment> attachments { get; set; }
        public DbSet<Bank> banks { get; set; }
        public DbSet<BankAccount> bankAccounts { get; set; }
        public DbSet<BankTransfer> bankTransfers { get; set; }
        public DbSet<Bill> bills { get; set; }
        public DbSet<Booking> bookings { get; set; }
        public DbSet<CashDisbursement> cashDisbursements { get; set; }
        public DbSet<Car> cars { get; set; }
        public DbSet<CarMaintenance> carMaintenances { get; set; }
        public DbSet<CarType> carTypes { get; set; }
        public DbSet<Client> clients { get; set; }
        public DbSet<Company> companies { get; set; }
        public DbSet<Employee> employees { get; set; }
        public DbSet<Expense> expenses { get; set; }
        public DbSet<ExpenseType> expenseTypes { get; set; }
        public DbSet<ExpenseTypeCategory> expenseTypeCategories { get; set; }
        public DbSet<Log> logs { get; set; }
        public DbSet<Lot> lots { get; set; }
        public DbSet<MaintenanceType> maintenanceTypes { get; set; }
        public DbSet<Module> modules { get; set; }
        public DbSet<Notification> notifications { get; set; }
        public DbSet<Office> offices { get; set; }
        public DbSet<OtherCharge> otherCharges { get; set; }
        public DbSet<Payment> payments { get; set; }
        public DbSet<PaymentCheck> paymentChecks { get; set; }
        public DbSet<PaymentDetail> paymentDetails { get; set; }
        public DbSet<PlanModule> planModules { get; set; }
        public DbSet<Project> projects { get; set; }
        public DbSet<RentalProperty> rentalProperties { get; set; }
        public DbSet<RentalContract> rentalContracts { get; set; }
        public DbSet<RentalContractDetail> rentalContractDetails { get; set; }
        public DbSet<Survey> surveys { get; set; }
        public DbSet<SubscriptionPlan> subscriptionPlans { get; set; }
        public DbSet<UserPrivilege> userPrivileges { get; set; }
        public DbSet<Vendor> vendors { get; set; }
    }
}
