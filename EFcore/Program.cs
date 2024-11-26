using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EFCore_LAB3.Models;
using System.Diagnostics.Metrics;
using System.Globalization;

class Program
{
    static async Task Main(string[] args)
    {
        using var context = new TempFuktContext();
        // First, delete the existing database to start fresh
        await context.Database.EnsureDeletedAsync();

        // Then create a new database
        await context.Database.EnsureCreatedAsync();

        string csvPath = "TempFuktData.csv";

        var lines = File.ReadAllLines(csvPath).Skip(1);
        int counter = 0;

        foreach (var line in lines)
        {
            var values = line.Split(',');
            var record = new TempFuktData
            {
                Datum = DateTime.ParseExact(values[0], "yyyy-MM-dd H:mm", CultureInfo.InvariantCulture),
                Plats = values[1],
                Temp = (double)decimal.Parse(values[2].Replace(".", ",").Replace("−", "-")),
                Luftfuktighet = (int)decimal.Parse(values[3].Replace(".", ",").Replace("−", "-"))
            };
            context.TempFuktData.Add(record);
            counter++;
        }

        await context.SaveChangesAsync();
        Console.WriteLine($"Success! {counter} records imported.");

        // Verify the data
        var totalRecords = await context.TempFuktData.CountAsync();
        Console.WriteLine($"\nTotal records in database: {totalRecords}");

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();

        // Add this after your data import
        var dateRange = context.TempFuktData
        .Select(x => x.Datum)
        .OrderBy(d => d);

        var firstDate = dateRange.First();
        var lastDate = dateRange.Last();
        var distinctDates = context.TempFuktData
            .Select(x => x.Datum.Date)
            .Distinct()
            .Count();

        Console.WriteLine($"\nDate Range in Database:");
        Console.WriteLine($"First Date: {firstDate}");
        Console.WriteLine($"Last Date: {lastDate}");
        Console.WriteLine($"Number of distinct dates: {distinctDates}");

        // Let's also look at some sample dates
        var sampleDates = context.TempFuktData
            .Select(x => x.Datum)
            .Distinct()
            .Take(5)
            .ToList();

        Console.WriteLine("\nSample of different dates:");
        foreach (var date in sampleDates)
        {
            Console.WriteLine(date.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}