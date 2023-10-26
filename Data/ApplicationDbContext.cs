using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Noested.Models;

namespace Noested.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Noested.Models.Customer> Customer { get; set; } = default!;
        public DbSet<Noested.Models.ServiceOrder> ServiceOrder { get; set; } = default!;
        public DbSet<Noested.Models.Checklist> Checklist { get; set; } = default!;
    }
}