using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFCore_LAB3.Models;

namespace EFcore.Core
    {
        public class TempFuktService
        {
            private readonly TempFuktContext _context;

            public TempFuktService(TempFuktContext context)
            {
                _context = context;
            }

            // Metod för att beräkna medeltemperatur för en viss månad och plats
            public double BeraknaMedelTempForManad(int ar, int manad, string plats)
            {
                var dataForManad = _context.TempFuktData
                                            .Where(t => t.Datum.Year == ar && t.Datum.Month == manad && t.Plats == plats)
                                            .ToList();

                if (dataForManad.Count == 0)
                    return 0;

                return dataForManad.Average(t => t.Temp);
            }

            // Metod för att beräkna medeltemperatur för en specifik dag och plats
            public double BeraknaMedelTempPerDag(DateTime datum, string plats)
            {
                var dagData = _context.TempFuktData
                                      .Where(t => t.Datum.Date == datum.Date && t.Plats == plats)
                                      .ToList();

                return dagData.Average(t => t.Temp);
            }

            // Metod för att beräkna mögelrisk
            public double BeräknaMogelrisk(double temperatur, double luftfuktighet)
            {
                return (temperatur * luftfuktighet) / 100;
            }
        }
    }
}
