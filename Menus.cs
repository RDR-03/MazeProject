public class Menu
{
    public static void StartMenu() {
        Console.Clear();
        Console.WriteLine("Escape the Undead\n");
        Dictionary<string, bool> options = new Dictionary<string, bool>()
        {
            {"Comenzar a jugar", true},
            {"Saber acerca del juego", false},
            {"Salir", false}
        };
        
        options["Comenzar a jugar"] = false;
        options["Saber acerca del juego"] = true;
        options["Salir"] = false;

        if (true)
            PlayerSelection();
        if (true)
            GameInfo();
        if (true)
           Environment.Exit(0);
    }

    public static void PauseMenu() {
        Console.Clear();
        Console.WriteLine("Menu de Pausa");
        Console.WriteLine("1)Continuar");
        Console.WriteLine("2)Salir");
        Console.Write("Selecciona una opcion: ");
    }
    
    private static void PlayerSelection() {
        Console.Clear();
    }
    public static void GameInfo() {
        Console.Clear();
    }
    
}