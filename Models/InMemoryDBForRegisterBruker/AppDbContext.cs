using System;
namespace innlogging.Models.Data;
using innlogging.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;



    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }

