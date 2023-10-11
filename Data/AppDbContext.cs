using System;
namespace Noested.Data;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Noested.Models;


public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}

