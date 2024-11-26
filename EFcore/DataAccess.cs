﻿using EFCore_LAB3.Models;
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

        public DataAccess()
        {
            _context = _context;
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
        public void LäsInCsvOchSpara(string filePath)
        {
            var rows = File.ReadLines(filePath)
                           .Skip(1)  // Skippa headern
                           .Select(line => line.Split(','))
                           .Where(columns => columns.Length == 4)
                           .Select(columns => new TempFuktData
                           {
                               Datum = DateTime.Parse(columns[0]),
                               Plats = columns[1],
                               Temp = double.Parse(columns[2]),
                               Luftfuktighet = int.Parse(columns[3])
                           }).ToList();

            _context.TempFuktData.AddRange(rows);
            _context.SaveChanges();
        }

        public class TempFuktBerakningar
        {
            private TempFuktContext _context;

            public TempFuktBerakningar(TempFuktContext context)
            {
                _context = context;
            }

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
    }
}


