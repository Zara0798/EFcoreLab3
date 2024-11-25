//Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = (localdb)\MSSQLLocalDB; Integrated Security = True;

using EFCore_LAB3.Models;

class Program
{
    static void Main(string[] args)
    {
        // Initiera databaskontexten
        using (var context = new TempFuktContext())
        {
            // Kontrollera att databasen existerar
            context.Database.EnsureCreated();
            Console.WriteLine("Databas skapad eller redan existerande.");
        }

        // Efter detta kan vi lägga till andra funktioner, t.ex. import av CSV-data.
    }
}
