using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore_LAB3.Models
{
    public class TempFuktData
    {
        public int Id { get; set; } // Primärnyckel
        public DateTime Datum { get; set; } // Datum och tid för mätningen
        public string Plats { get; set; } // Ute eller Inne
        public double Temp { get; set; } // Temperatur i Celsius
        public int Luftfuktighet { get; set; } // Luftfuktighet i procent
    }
    public class TempFuktService
    {
        private TempFuktContext _context;

        public TempFuktService(TempFuktContext context)
        {
            _context = context;
        }

        // Metod för att beräkna medeltemperatur för en viss månad och plats
        public double BeraknaMedelTempForManad(int ar, int manad, string plats)
        {
            // Hämta temperaturdata för den specifika månaden och platsen
            var dataForManad = _context.TempFuktData
                                        .Where(t => t.Datum.Year == ar && t.Datum.Month == manad && t.Plats == plats)
                                        .ToList();

            if (dataForManad.Count == 0)
                return 0; // Om inga data finns, returnera 0 som default

            // Beräkna medeltemperaturen för månaden
            return dataForManad.Average(t => t.Temp);
        }

    }
}
