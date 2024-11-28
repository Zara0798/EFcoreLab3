using EFcore;
using EFcore.Core;
using EFCore_LAB3.Models;
using System;
using System.Globalization;
using System.Threading.Tasks;
using EFCore_LAB3.Models;
class Program
{
    static async Task Main(string[] args)
    {
        using var context = new TempFuktContext();
        var dataAccess = new DataAccess(context);
        var tempFuktService = new EFcore.Core.TempFuktService(context);

        // Läs in CSV-filen och spara datan
        Console.WriteLine("Läser in CSV-data...");
        dataAccess.LäsInCsvOchSpara("TempFuktData.csv");

        // Fråga användaren efter ett datum för att visa medeltemperatur och luftfuktighet
        Console.WriteLine("Ange ett datum (yyyy-MM-dd) för att visa medeltemperatur och luftfuktighet:");
        string inputDatum = Console.ReadLine();
        DateTime datum;

        while (!DateTime.TryParseExact(inputDatum, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out datum))
        {
            Console.WriteLine("Felaktigt format. Vänligen ange datum i formatet yyyy-MM-dd:");
            inputDatum = Console.ReadLine();
        }

        // Beräkna och visa medeltemperatur för utomhus på det valda datumet
        var validatedDailyAverages = tempFuktService.BeräknaMedelTempPerDag(datum, "Ute");
        Console.WriteLine($"Medeltemperaturen för {datum:yyyy-MM-dd} (Utomhus) är {validatedDailyAverages:F2}°C");

        // Sortering av dagar från minst till störst risk för mögel (Utomhus)
        Console.WriteLine("\nSortering av dagar från minst till störst risk för mögel (Utomhus):");
        var moldSortedUtomhus = tempFuktService.SorteraAllaDagarMogelriskUtomhus();
        foreach (var day in moldSortedUtomhus.Take(5))
        {
            Console.WriteLine($"{day.Datum:yyyy-MM-dd}: Mögelrisk: {day.Mogelrisk:F2}");
        }

        // Sortering av dagar från minst till störst risk för mögel (Inomhus)
        Console.WriteLine("\nSortering av dagar från minst till störst risk för mögel (Inomhus):");
        var moldSortedInomhus = tempFuktService.SorteraAllaDagarMogelriskInomhus();
        foreach (var day in moldSortedInomhus.Take(5))
        {
            Console.WriteLine($"{day.Datum:yyyy-MM-dd}: Mögelrisk: {day.Mogelrisk:F2}");
        }

        Console.WriteLine("\nTryck på valfri tangent för att avsluta...");
        Console.ReadKey();
    }
}

