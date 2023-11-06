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
        public DbSet<Customer> Customer { get; set; } = default!;
        public DbSet<ServiceOrder> ServiceOrder { get; set; } = default!;
        public DbSet<Checklist> Checklist { get; set; } = default!;
        public DbSet<WinchChecklist> WinchChecklist { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Enum-converters
            var orderConv = new EnumToStringConverter<OrderStatus>();
            var warrantyConv = new EnumToStringConverter<WarrantyType>();
            var productConv = new EnumToStringConverter<ProductType>();
            var CompStatConv = new EnumToStringConverter<ComponentStatus>();

            // Conversion across all entities
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(OrderStatus))
                    {
                        property.SetValueConverter(orderConv);
                    }
                    else if (property.ClrType == typeof(WarrantyType))
                    {
                        property.SetValueConverter(warrantyConv);
                    }
                    else if (property.ClrType == typeof(ProductType))
                    {
                        property.SetValueConverter(productConv);
                    }
                    else if (property.ClrType == typeof(ComponentStatus))
                    {
                        property.SetValueConverter(CompStatConv);
                    }
                }
            }
        }
    }
}