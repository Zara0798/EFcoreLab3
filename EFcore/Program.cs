using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EFCore_LAB3.Models;

class Program
{
    static async Task Main(string[] args)
    {
        using var context = new TempFuktContext();
        string csvPath = "TempFuktData.csv";  // Make sure the file is in your project's output directory

        // Create database if it doesn't exist
        await context.Database.EnsureCreatedAsync();

        // Import data if table is empty
        if (!context.TempFuktData.Any())
        {
            var lines = File.ReadAllLines(csvPath).Skip(1);

            foreach (var line in lines)
            {
                var values = line.Split(',');
                var record = new TempFuktData
                {
                    Datum = DateTime.Parse(values[0]),
                    Plats = values[1],
                    Temp = (double)decimal.Parse(values[2].Replace(".", ",")), //ughhhh
                    Luftfuktighet = (int)decimal.Parse(values[3].Replace(".", ","))
                };
                context.TempFuktData.Add(record);
            }

            await context.SaveChangesAsync();
            Console.WriteLine($"Data imported successfully!");
            Console.WriteLine($"Total records imported: {lines.Count()}");
        }
        else
        {
            Console.WriteLine($"Current record count: {context.TempFuktData.Count()}");
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
