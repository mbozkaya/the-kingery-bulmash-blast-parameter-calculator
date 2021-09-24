using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.DBModel;

namespace Web.Entity
{
    public class ECDBContext : DbContext
    {
        public ECDBContext()
        {
        }
        public ECDBContext(DbContextOptions<ECDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExplosiveType>().ToTable("ExplosiveTypes");
            modelBuilder.Entity<CalculationResult>().ToTable("CalculationResults");
            modelBuilder.Entity<CalculationEntry>().ToTable("CalculationEntries");

        }

        public DbSet<ExplosiveType> ExplosiveTypes { get; set; }
        public DbSet<CalculationResult> CalculationResults { get; set; }
        public DbSet<CalculationEntry> CalculationEntries { get; set; }

    }
}
