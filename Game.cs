/*
Importerar klass för att slippa upprepa "Console" och namespace för 
att hantera konvertering mellan JSON och objekt.
*/
using static System.Console;
using System.Text.Json;

internal class Game
{
    // Skapar ny instans med lista av spelare
    private static List<Player> Players = new();

    /*
    Statisk metod för att läsa ut och deserialisera innehåll från en JSON-fil 
    till listan med spelare, förutsatt att JSON-filen exixterar.
    */
    public static void LoadPlayers()
    {
        if (File.Exists("savedgames.json"))
        {
            string json = File.ReadAllText("savedgames.json");
            Players = JsonSerializer.Deserialize<List<Player>>(json)!;
        }
    }

    // Statisk metod för att starta ett nytt spel
    public static void NewGame()
    {
        // Så länge isInputValid är false körs while-loopen
        bool isInputValid = false;
        while (!isInputValid)
        {
            // Rensar konsol, efterfrågar användarens namn och sparar värde i variabel
            Clear();
            Write("Ange ditt namn: ");
            string? name = ReadLine();

            // Om namn är null, tomt eller blanksteg skrivs felmeddelande ut och en ny iteration av loopan startas
            if (string.IsNullOrWhiteSpace(name))
            {
                Clear();
                Write("Ogiltigt namn! Tryck på valfri tangent...");
                ReadKey();
            }
            // Om namn är korrekt angivet anropas metod för att kunna räkna antalet sparade spelare och sätta värde för spelarens id
            else
            { 
                LoadPlayers();
                Player.CurrentPlayer.Id = Players.Count + 1;

                // Sätter värde för spelarens namn
                Player.CurrentPlayer.Name = name;

                // isInputValid sätts till true för att stoppa loopen
                isInputValid = true;

                // Rensar konsol och skriver ut inledning...
                Clear();
                Print($"Hej {Player.CurrentPlayer.Name}! Berättelse om spelet...");
                ReadKey();

                // Anropar metod
                GameMenu();
            }
        }
    }

    // Statisk metod för att ladda ett sparat spel
    public static void LoadGame()
    {
        // Rensar konsol och anropar metod
        Clear();
        LoadPlayers();

        // Om antalet sparade spelare är noll skrivs meddelande ut
        if (Players.Count == 0)
        {
            Write("Det finns inga sparade spel. Tryck på valfri tangent...");
        }
        // Om antalet spelare inte är noll
        else
        {
            // Så länge isChoiceValid är false körs while-loopen
            bool isChoiceValid = false;
            while (!isChoiceValid)
            {
                // Skriver ut sparade spel och efterfrågar inmatning
                WriteLine("Sparade spel:\n");
                for (int i = 0; i < Players.Count; i++)
                {
                    WriteLine($"{i + 1}. {Players[i].Name} (level {Players[i].Level})");
                }
                Write($"\nVälj ett alternativ (1-{Players.Count}): ");

                // Om inmatning inte är korrekt skrivs felmeddelande ut
                if (!int.TryParse(ReadLine(), out int choice) || choice < 1 || choice > Players.Count)
                {
                    Clear();
                    Write("Ogiltigt alternativ! Tryck på valfri tangent...");
                    ReadKey();
                }
                // Om inmatning är korrekt uppdateras CurrentPlayer med vald spelare
                else
                {
                    Player.CurrentPlayer = Players[choice - 1];

                    // isChoiceValid sätts till true för att stoppa loopen
                    isChoiceValid = true;
                }
            }

            // Rensar konsol och skriver ut hälsning...
            Clear();
            Print($"Välkommen tillbaka, {Player.CurrentPlayer.Name}...");
            ReadKey();

            // Anropar metod
            GameMenu();
        }
        ReadKey();
    }

    // Statisk metod för att spara ett spel
    public static void SaveGame()
    {
        // Anropar metod
        LoadPlayers();

        // Om CurrentPlayer redan existerar i listan med spelare ersätts den befintliga instansen
        var existingPlayer = Players.Find(p => p.Id == Player.CurrentPlayer.Id);
        if (existingPlayer != null)
        {
            Players[Players.IndexOf(existingPlayer)] = Player.CurrentPlayer;
        }
        // Om CurrentPlayer inte existerar adderas den nya insatsen till listan med spelare
        else
        {
            Players.Add(Player.CurrentPlayer);
        }

        // Serialiserar listan med spelare till en JSON-sträng och sparar den till en JSON-fil
        string json = JsonSerializer.Serialize(Players);
        File.WriteAllText("savedgames.json", json);
    }

    // Statisk metod för att avsluta programmet
    public static void Quit()
    {
        Clear();
        Environment.Exit(0);
    }

    // Statisk metod för att skriva ut meny
    public static void GameMenu()
    {
        // Skapar en loop för menyn
        while (true)
        {
            // Rensar konsol och skriver ut meny
            Clear();
            WriteLine("Vad vill du göra?\n");
            WriteLine("1. Spara");
            WriteLine("2. Avsluta\n");

            // Efterfrågar inmatning
            Write("Välj ett alternativ (1-2): ");

            // Om inmatning är en siffra körs switch-satsen
            if (int.TryParse(ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        SaveGame();
                        break;
                    case 2:
                        Quit();
                        break;
                    default:
                        Clear();
                        Write("\nOgiltigt alternativ! Tryck på valfri tangent...");
                        ReadKey();
                        break;
                }
            }
            // Om inmatning inte är en siffra skrivs felmeddelande ut
            else
            {
                Clear();
                Write("\nOgiltigt alternativ! Tryck på valfri tangent...");
                ReadKey();
            }
        }
    }

    /* 
    Statisk metod för att skapa en "skriv ut långsamt"-effekt på 
    konsolen genom att skriva ut varje tecken i textsträngen med 
    en fördröjning på 20 millisekunder mellan varje tecken.
    */
    public static void Print(string text)
    {
        foreach (char character in text)
        {
            Write(character);
            Thread.Sleep(20);
        }
    }
}