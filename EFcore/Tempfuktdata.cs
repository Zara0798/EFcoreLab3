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
}
