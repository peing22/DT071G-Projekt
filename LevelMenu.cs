internal class LevelMenu
{
    // Statisk metod för att visa levelmenyn
    public static void DisplayMenu(string name, Action description, Dictionary<int, MenuItem> menuItems)
    {
        // Rensar konsol och skriver ut namn och XP-indikator
        Clear();
        WriteLine("-----------------------------------------------------------------");
        Write($" {name}         XP-indikator ");
        Player.CurrentPlayer.ProgressBar();
        WriteLine("-----------------------------------------------------------------\n");

        // Anropar metod för att visa beskrivning och ställer en fråga till spelaren
        description?.Invoke();
        WriteLine("Vad vill du göra?\n");

        // Loopar igenom varje menyobjekt i lexikonet av menyobjekt
        foreach (var menuItem in menuItems)
            {
                // Skriver ut menyobjektets numeriska nyckel och beskrivning
                WriteLine($"{menuItem.Key}. {menuItem.Value.Description}");
            }

        // Skriver ut extra menyalternativ som inte ingår i lexikonet av menyobjekt
        WriteLine($"{menuItems.Count + 1}. Dricka en läkande trolldryck");
        WriteLine($"{menuItems.Count + 2}. Se aktuell status");
        WriteLine($"{menuItems.Count + 3}. Spara spelet");
        WriteLine($"{menuItems.Count + 4}. Avsluta\n");

        // Efterfrågar inmatning
        Write("Välj ett alternativ (1-" + (menuItems.Count + 4) + "): ");

        // Om inmatningen är en siffra anropas den metod som matchar spelarens val
        if (int.TryParse(ReadLine(), out int choice))
        {
            if (choice >= 1 && choice <= menuItems.Count)
            {
                menuItems[choice]?.Action?.Invoke();
            }
            else if (choice == menuItems.Count + 1)
            {
                Player.CurrentPlayer.DrinkPotion();
            }
            else if (choice == menuItems.Count + 2)
            {
                Player.CurrentPlayer.PlayerStatus();
            }
            else if (choice == menuItems.Count + 3)
            {
                Game.SaveGame();
            }
            else if (choice == menuItems.Count + 4)
            {
                Game.QuitGame();
            }
            // Om spelarens val inte matchar något av alternativen ovan skrivs felmeddelande ut
            else
            {
                Game.WriteOptionErrorMessage();
            }
        }
        // Om inmatningen inte är en siffra skrivs felmeddelande ut
        else
        {
            Game.WriteOptionErrorMessage();
        }

        // Om spelaren kan nå nästa level
        if (Player.CurrentPlayer.CanLevelUp())
        {
            // Anropar metod för att öka spelarens level
            Player.CurrentPlayer.LevelUp();
        }
    }
}