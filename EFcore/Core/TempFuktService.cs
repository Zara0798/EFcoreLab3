using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFCore_LAB3.Models;
using static EFcore.DataAccess;

namespace EFcore.Core
{
    public class TempFuktService
    {
        private readonly TempFuktContext _context;

        public TempFuktService(TempFuktContext context)
        {
            _context = context;
        }

        public double BeräknaMedelTempPerDag(DateTime datum, string plats)
        {
            var dagData = _context.TempFuktData
                                  .Where(t => t.Datum.Date == datum.Date && t.Plats == plats)
                                  .ToList();

            return dagData.Average(t => t.Temp);
        }

        public double BeräknaMedelLuftfuktighetPerDag(DateTime datum, string plats)
        {
            var dagData = _context.TempFuktData
                                  .Where(t => t.Datum.Date == datum.Date && t.Plats == plats)
                                  .ToList();

            return dagData.Average(t => t.Luftfuktighet);
        }

        public List<TempFuktData> SorteraVarmasteTillKallaste(DateTime datum, string plats)
        {
            return _context.TempFuktData
                           .Where(t => t.Datum.Date == datum.Date && t.Plats == plats)
                           .OrderByDescending(t => t.Temp)
                           .ToList();
        }

        public List<TempFuktData> SorteraTorrasteTillFuktigaste(DateTime datum, string plats)
        {
            return _context.TempFuktData
                           .Where(t => t.Datum.Date == datum.Date && t.Plats == plats)
                           .OrderByDescending(t => t.Luftfuktighet)
                           .ToList();
        }
        public double BeräknaMogelrisk(double temperatur, double luftfuktighet)
        {
            return (temperatur * luftfuktighet) / 100;
        }
        // Sortera by temp luftfuktighet alla dagar:
        public List<object> SorteraAllaDagarTemperatur(string plats)
        {
            return _context.TempFuktData
                .Where(t => t.Plats == plats)
                .GroupBy(t => t.Datum.Date)
                .Select(g => new
                {
                    Datum = g.Key,
                    MedelTemperatur = g.Average(t => t.Temp)
                })
                .OrderByDescending(x => x.MedelTemperatur)
                .ToList<object>();
        }

        public List<object> SorteraAllaDagarLuftfuktighet(string plats)
        {
            return _context.TempFuktData
                .Where(t => t.Plats == plats)
                .GroupBy(t => t.Datum.Date)
                .Select(g => new
                {
                    Datum = g.Key,
                    MedelLuftfuktighet = g.Average(t => t.Luftfuktighet)
                })
                .OrderByDescending(x => x.MedelLuftfuktighet)
                .ToList<object>();
        }

        // Slå ihop metoderna för att sortera dagar efter mögelrisk i en metod, mer strukturerad data
        private List<MogelriskResult> SorteraMogelrisk(string plats)
        {
            return _context.TempFuktData
                .Where(t => t.Plats == plats)
                .GroupBy(t => t.Datum.Date) // Gruppera per dag
                .Select(g => new MogelriskResult
                {
                    Datum = g.Key,
                    MedelTemperatur = g.Average(t => t.Temp),
                    MedelLuftfuktighet = g.Average(t => t.Luftfuktighet),
                    Mogelrisk = (g.Average(t => t.Temp) * g.Average(t => t.Luftfuktighet)) / 100
                })
                .OrderBy(x => x.Mogelrisk) // Sortera efter minst till störst risk för mögel
                .ToList();
        }
        // Ny metod för att sortera dagar efter minst till störst risk för mögel för utomhus
        public List<MogelriskResult> SorteraAllaDagarMogelriskUtomhus()
        {
            return SorteraMogelrisk("Ute");
        }

        // Ny metod för att sortera dagar efter minst till störst risk för mögel för inomhus
        public List<MogelriskResult> SorteraAllaDagarMogelriskInomhus()
        {
            return SorteraMogelrisk("Inne");
        }

        public class MogelriskResult
        {
            public DateTime Datum { get; set; }
            public double MedelTemperatur { get; set; }
            public double MedelLuftfuktighet { get; set; }
            public double Mogelrisk { get; set; }
        }
    }
}