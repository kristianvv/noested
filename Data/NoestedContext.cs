using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Noested.Models;

namespace Noested.Data
{
    public class NoestedContext : DbContext
    {
        public NoestedContext (DbContextOptions<NoestedContext> options)
            : base(options)
        {
        }

        public DbSet<Noested.Models.Test> Test { get; set; } = default!;

        public DbSet<Noested.Models.ServiceOrder> ServiceOrder { get; set; } = default!;
    }
}
