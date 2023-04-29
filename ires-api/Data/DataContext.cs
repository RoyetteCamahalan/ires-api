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
            modelBuilder.Entity<RentalProperty>()
                .HasOne(r => r.project)
                .WithMany(p => p.rentalProperties)
                .HasForeignKey(r => r.projectid);
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

            modelBuilder.Entity<Company>()
                .HasOne(s => s.subscriptionPlan)
                .WithMany(c => c.companies)
                .HasForeignKey(e => e.planid);

            modelBuilder.Entity<ExpenseType>()
                .HasOne(s => s.category)
                .WithMany(c => c.expenseTypes)
                .HasForeignKey(e => e.expensetypecat);


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

        }
        public DbSet<ApplicationModule> applicationModules { get; set; }
        public DbSet<Attachment> attachments { get; set; }
        public DbSet<Bank> banks { get; set; }
        public DbSet<BankAccount> bankAccounts { get; set; }
        public DbSet<BankTransfer> bankTransfers { get; set; }
        public DbSet<Bill> bills { get; set; }
        public DbSet<Client> clients { get; set; }
        public DbSet<Company> companies { get; set; }
        public DbSet<Employee> employees { get; set; }
        public DbSet<ExpenseType> expenseTypes { get; set; }
        public DbSet<ExpenseTypeCategory> expenseTypeCategories { get; set; }
        public DbSet<Log> logs { get; set; }
        public DbSet<Lot> lots { get; set; }
        public DbSet<Office> offices { get; set; }
        public DbSet<OtherCharge> otherCharges { get; set; }
        public DbSet<Payment> payments { get; set; }
        public DbSet<PaymentCheck> paymentChecks { get; set; }
        public DbSet<PaymentDetail> paymentDetails { get; set; }
        public DbSet<Project> projects { get; set; }
        public DbSet<RentalProperty> rentalProperties { get; set; }
        public DbSet<Survey> surveys { get; set; }
        public DbSet<SubscriptionPlan> subscriptionPlans { get; set; }
        public DbSet<UserPrivilege> userPrivileges { get; set; }
    }
}
