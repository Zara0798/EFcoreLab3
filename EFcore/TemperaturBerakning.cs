using EFCore_LAB3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFcore
{
    public static class TemperaturBerakning
    {
        // Metod för att beräkna medeltemperatur för en viss månad och plats
        public static double BeraknaMedelTempForManad(TempFuktContext context, int ar, int manad, string plats)
        {
            var dataForManad = context.TempFuktData
                                      .Where(t => t.Datum.Year == ar && t.Datum.Month == manad && t.Plats == plats)
                                      .ToList();
            if (dataForManad.Count == 0)
                return 0; // Om inga data finns, returnera 0 som default

            // Beräkna medeltemperaturen
            return dataForManad.Average(t => t.Temp);
        }

    }
    public class MeteorologiskSasong
    {
        private TempFuktContext _context;

        public MeteorologiskSasong(TempFuktContext context)
        {
            _context = context;
        }

        // Metod för att avgöra säsong baserat på medeltemperatur
        public string AvgoraSasong(int ar, string plats)
        {
            double septemberTemp = TemperaturBerakning.BeraknaMedelTempForManad(_context, ar, 9, plats);
            double oktoberTemp = TemperaturBerakning.BeraknaMedelTempForManad(_context, ar, 10, plats);
            double novemberTemp = TemperaturBerakning.BeraknaMedelTempForManad(_context, ar, 11, plats);

            double decemberTemp = TemperaturBerakning.BeraknaMedelTempForManad(_context, ar, 12, plats);
            double januariTemp = TemperaturBerakning.BeraknaMedelTempForManad(_context, ar, 1, plats);
            double februariTemp = TemperaturBerakning.BeraknaMedelTempForManad(_context, ar, 2, plats);

            // Om medeltemperaturen för höst är under 10°C
            if ((septemberTemp + oktoberTemp + novemberTemp) / 3 < 10)
            {
                return "Höst";
            }

            // Om medeltemperaturen för vinter är under 0°C
            if ((decemberTemp + januariTemp + februariTemp) / 3 < 0)
            {
                return "Vinter";
            }

            return "Ingen säsong";
        }
    }
}
