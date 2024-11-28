using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFcore.Core
{
    public class MogelriskResult
    {
        public DateTime Datum { get; set; }
        public double MedelTemperatur { get; set; }
        public double MedelLuftfuktighet { get; set; }
        public double Mogelrisk { get; set; }
    }
}
