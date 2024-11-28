using EFcore;
using EFcore.Core;
using EFCore_LAB3.Models;
using System;
using System.Globalization;
using System.Threading.Tasks;
using EFCore_LAB3.Models;
using System.Linq;

class Program
{
    static async Task Main(string[] args)
    {
        using var context = new TempFuktContext();

        // Skapa databasen om den inte redan finns
        context.CreateDatabaseIfNotExists();

        var dataAccess = new DataAccess(context);
        var tempFuktService = new EFcore.Core.TempFuktService(context);

        // Läs in CSV-filen och spara datan
        Console.WriteLine("Läser in CSV-data...");
        dataAccess.LäsInCsvOchSpara("TempFuktData.csv");

        bool running = true; // Loop för att köra programmet

        while (running)
        {
            Console.WriteLine("\n--- Meny ---");
            Console.WriteLine("1. Ange ett datum och visa medeltemperatur för utomhus och inomhus");
            Console.WriteLine("2. Sortera dagar från minst till störst risk för mögel (Utomhus)");
            Console.WriteLine("3. Sortera dagar från minst till störst risk för mögel (Inomhus)");
            Console.WriteLine("4. Testa mögelriskberäkning för givna värden (Temperatur = 20°C, Luftfuktighet = 80%)");
            Console.WriteLine("5. ");

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
        // Beräkna och visa medeltemperatur för inomhus på det valda datumet
        var validatedDailyAveragesInomhus = tempFuktService.BeräknaMedelTempPerDag(datum, "Inne");
        Console.WriteLine($"Medeltemperaturen för {datum:yyyy-MM-dd} (Inomhus) är {validatedDailyAveragesInomhus:F2}°C");

        // Sortering av dagar från minst till störst risk för mögel (Inomhus)
        Console.WriteLine("\nSortering av dagar från minst till störst risk för mögel (Inomhus):");
        var moldSortedInomhus = tempFuktService.SorteraAllaDagarMogelriskInomhus();
        foreach (var day in moldSortedInomhus.Take(5))
        {
            Console.WriteLine($"{day.Datum:yyyy-MM-dd}: Mögelrisk: {day.Mogelrisk:F2}");
        }

        Console.WriteLine("\nTryck på valfri tangent för att avsluta...");
        Console.ReadKey();



        // Manuella tester
        Console.WriteLine("\n--- Manuella Tester ---\n");

        //// Test: Beräkna medeltemperatur för en specifik månad (Januari 2021, Utomhus)
        //var avgTempJan2021 = tempFuktService.BeraknaMedelTempForManad(2021, 1, "Ute");
        //Console.WriteLine("Test - Beräkna medeltemperatur för Januari 2021 (Utomhus):");
        //Console.WriteLine($"Förväntat värde: (kontrollera manuellt beroende på data), Beräknat värde: {avgTempJan2021:F2}°C\n");

        // Test: Beräkna mögelrisk för givna värden (Temperatur = 20°C, Luftfuktighet = 80%)
        double testTemperatur = 20.0;
        double testLuftfuktighet = 80.0;
        var mogelrisk = tempFuktService.BeräknaMogelrisk(testTemperatur, testLuftfuktighet);
        Console.WriteLine("Test - Beräkna mögelrisk för Temperatur = 20°C och Luftfuktighet = 80%:");
        Console.WriteLine($"Förväntat värde: 16.0, Beräknat värde: {mogelrisk:F2}\n");

        // Test: Beräkna medeltemperatur för en specifik dag (1 Januari 2021, Utomhus)
        var avgTempDagUtomhus = tempFuktService.BeräknaMedelTempPerDag(new DateTime(2021, 1, 1), "Ute");
        Console.WriteLine("Test - Beräkna medeltemperatur för 1 Januari 2021 (Utomhus):");
        Console.WriteLine($"Förväntat värde: (kontrollera manuellt beroende på data), Beräknat värde: {avgTempDagUtomhus:F2}°C\n");

        // Test: Beräkna medeltemperatur för en specifik dag (1 Januari 2021, Inomhus)
        var avgTempDagInomhus = tempFuktService.BeräknaMedelTempPerDag(new DateTime(2021, 1, 1), "Inne");
        Console.WriteLine("Test - Beräkna medeltemperatur för 1 Januari 2021 (Inomhus):");
        Console.WriteLine($"Förväntat värde: (kontrollera manuellt beroende på data), Beräknat värde: {avgTempDagInomhus:F2}°C\n");

        Console.WriteLine("\nTryck på valfri tangent för att avsluta...");
        Console.ReadKey();

    }
}

