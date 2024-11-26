using EFCore_LAB3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFcore
{
    public static class CsvReader
    {
        public static List<TempFuktData> ReadCsvFile(string filePath)
        {
            return File.ReadAllLines(filePath)
                .Skip(1)
                .Select(line =>
                {
                    var values = line.Split(',');
                    return new TempFuktData
                    {
                        Datum = DateTime.Parse(values[0]),
                        Plats = values[1],
                        Temp = (double)decimal.Parse(values[2]),
                        Luftfuktighet = (int)decimal.Parse(values[3])
                    };
                })
                .ToList();
        }
    }
}
