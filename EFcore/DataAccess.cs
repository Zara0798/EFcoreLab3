using EFCore_LAB3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFcore
{
    public class DataAccess
    {
        private readonly TempFuktContext _context;

        public DataAccess(TempFuktContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /*public void LoadDataFromCsv(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines.Skip(1))  // Skip header line
            {
                var columns = line.Split(',');

                DateTime datum = DateTime.Parse(columns[0]);
                string plats = columns[1];
                double temp = double.Parse(columns[2]);
                double luftfuktighet = double.Parse(columns[3]);

            }
        }*/

        //Uppdaderade LäsInCvsOchSpara-metodn för att hantera felaktiga rader i CSV-filen och skriva ut felmeddelanden
        public void LäsInCsvOchSpara(string filePath)
        {
            try
            {
                var rows = File.ReadLines(filePath)
                               .Skip(1)  // Skippa headern
                               .Select(line => line.Split(','))
                               .Where(columns => columns.Length == 4)  // Kontrollera att alla kolumner finns
                               .Select(columns =>
                               {
                                   try
                                   {
                                       return new TempFuktData
                                       {
                                           Datum = DateTime.Parse(columns[0]),
                                           Plats = columns[1],
                                           Temp = double.Parse(columns[2]),
                                           Luftfuktighet = int.Parse(columns[3])
                                       };
                                   }
                                   catch (FormatException ex)
                                   {
                                       Console.WriteLine($"Felaktigt format på rad: {string.Join(',', columns)}. Fel: {ex.Message}");
                                       return null;  // Om rad har felaktigt format, returnera null
                                   }
                               })
                               .Where(data => data != null)  // Ignorera ogiltiga rader
                               .ToList();

                _context.TempFuktData.AddRange(rows);
                _context.SaveChanges();

                Console.WriteLine("Data från CSV har lästs in och sparats.");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"CSV-fil kunde inte hittas: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ett fel inträffade vid läsning av CSV-filen: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett oväntat fel inträffade: {ex.Message}");
            }
        }


        public class TempFuktBerakningar
        {
            private TempFuktContext _context;

            public TempFuktBerakningar(TempFuktContext context)
            {
                _context = context;
            }

        }

    }
}


