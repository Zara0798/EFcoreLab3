using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore_LAB3.Models
{
    public class TempFuktContext : DbContext
    {
        public DbSet<TempFuktData> TempFuktDatas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = ZaraDuaa; Integrated Security = True;");
        }
    }
}
