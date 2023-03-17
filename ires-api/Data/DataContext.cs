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
        }
        public DbSet<Company> companies { get; set; }
        public DbSet<Employee> employees { get; set; }
        public DbSet<Project> projects { get; set; }
        public DbSet<RentalProperty> rentalProperties { get; set; }
        public DbSet<ApplicationModule> applicationModules { get; set; }
        public DbSet<UserPrivilege> userPrivileges { get; set; }
        public DbSet<Client> clients { get; set; }
        public DbSet<Lot> lots { get; set; }
        public DbSet<Survey> surveys { get; set; }
        public DbSet<OtherCharge> otherCharges { get; set; }
        public DbSet<Payment> payments { get; set; }
    }
}
