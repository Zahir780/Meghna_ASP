using System.Linq;
using ESL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ESL.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            foreach (var relationship in modelbuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(modelbuilder);
        }

        #region Admin Module
        public DbSet<Company> Companies { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Notification> tbNotification { get; set; }
        public DbSet<UserAuthentication> tbUserAuthentication { get; set; }
        public DbSet<ChatMessage> tbChatMessage { get; set; }
        public DbSet<Bank> tbBankName { get; set; }
        public DbSet<BankBranch> tbBankBranch { get; set; }
        #endregion Admin Module 

        #region HRM Module
        public DbSet<Department> tbDepartment { get; set; }
        public DbSet<Section> tbSection { get; set; }
        public DbSet<DesignationRank> tbDesignationRank { get; set; }
        public DbSet<Designation> tbDesignation { get; set; }
        public DbSet<Employee> tbEmployeeInfo { get; set; }
        public DbSet<Shift> tbShiftInfo { get; set; }
        public DbSet<Holiday> tbHoliday { get; set; }

        #endregion HRM Module


    }
}
