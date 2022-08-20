using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSorting
{
    public class DataDBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlite("Data Source=scansdata.db");

           
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PersonalInfoModel>().HasIndex(p => p.MeliCode).IsUnique();
        }

        public DbSet<PersonalInfoModel> Employees { get; set; }
    }
}
