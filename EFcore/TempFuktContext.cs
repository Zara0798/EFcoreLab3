using EFCore_LAB3.Models;
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
        public DbSet<TempFuktData> TempFuktData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(@"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = ZaraDuaa; Integrated Security = True;");

       //Skapa databas om den inte redan finns
    public void CreateDatabaseIfNotExists()
        {
            if (!Database.CanConnect())
            {
                Database.EnsureCreated();
            }
        }

        public double GetAverageTemperature(DateTime date, string location) // för att räkna medeltemp...
        {
            using TempFuktContext context = new TempFuktContext();
            return context.TempFuktData
                .Where(d => d.Datum.Date == date.Date && d.Plats == location)
                .Average(d => d.Temp);
        }
        // Beräkning av mögelindex
        //För att beräkna risk för mögel, behöver vi en formel
        public double BeräknaMogelrisk(double temperatur, double luftfuktighet)
        {
            return (temperatur * luftfuktighet) / 100;
        }

    }
}

