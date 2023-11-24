using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Noested.Models;

namespace Noested.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().ToTable("Users").HasKey(x => x.Id);
            base.OnModelCreating(modelBuilder);
        }


        public DbSet<UserEntity> Users { get; set; } = default!;
        public DbSet<Customer> Customer { get; set; } = default!;
        public DbSet<ServiceOrder> ServiceOrder { get; set; } = default!;
        public DbSet<Checklist> Checklist { get; set; } = default!;
        public DbSet<WinchChecklist> WinchChecklist { get; set; } = default!;
    }
}