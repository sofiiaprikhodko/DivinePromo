using DivinePromo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DivinePromo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Subscriber> Subscribers { get; set; }
    }
}