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

        public DataAccess()
        {
            _context = new TempFuktContext();
        }
        
        // Metod för att fylla databasen från en CSV-fil
       public void ImportCsvData(string filePath)
