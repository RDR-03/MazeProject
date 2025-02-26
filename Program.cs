using System.Runtime.InteropServices;
using System.Text;
using Spectre.Console;
using System.Media;

namespace Project;
class Program
{
    public static Grid? grid;
    public static Objects? maze_exit;
    public static Objects? Shelter;
    public static Objects? Key;
    public static Objects? FKey;
    
    public static Trap[]? _Traps;
    public static Trap? Trap1;
    public static Trap? Trap2;
    public static Trap? Trap3;
    public static Trap? Trap4;
    public static Trap? Trap5;
    public static Trap? Trap6;
    
    public static Character? badGuy;
    public static Character? goodGuy;
    public static int cool_bad;
    public static int cool_good;
    public static bool game_Initialized;
    
    static void Main(string[] args)
    {               
        if (OperatingSystem.IsWindows()) {
            SoundPlayer main = new SoundPlayer();
            main.SoundLocation = "Haunting_Menu.wav";
            main.Load();
            main.PlayLooping();
        }
        Menu.Start();
        
        (string, string) Selections = Menu.PlayerSelection();
        string charSelection1 = Selections.Item1;
        string charSelection2 = Selections.Item2;
        
        grid = new Grid(13,13);
        MazeResolve.CreateMaze(grid);
        MazeResolve.CreateMaze(grid);

        // Instanciar asesino
        if (charSelection1.StartsWith("Jason") || charSelection2.StartsWith("Jason"))
            badGuy = new Character ("Jason", grid.Rows - 1, 0, 3, 3);

        else if (charSelection1.StartsWith("Fred") || charSelection2.StartsWith("Fred"))
            badGuy = new Character ("Freddy Krueger", grid.Rows - 1, 0, 2, 4);
        
        else if (charSelection1.StartsWith("Luci") || charSelection2.StartsWith("Luci"))
            badGuy = new Character ("Lucifer", grid.Rows - 1, 0, 4, 5);
        
        // Instanciar sobreviviente
        if (charSelection1.StartsWith("Joel") || charSelection2.StartsWith("Joel"))
            goodGuy = new Character ("Joel", 0, 0, 3, 2);

        else if (charSelection1.StartsWith("Alan") || charSelection2.StartsWith("Alan"))
            goodGuy = new Character ("Alan Wake", 0, 0, 2, 4, 4);
        
        else if (charSelection1.StartsWith("Constan") || charSelection2.StartsWith("Constan"))
            goodGuy = new Character ("Constantine", 0, 0, 4, 6);
        
        // Objetos del laberinto
        Random rand = new Random();
        maze_exit = new Objects (grid.Rows/2, grid.Columns - 1);
        Key = new Objects (3, rand.Next(grid.Columns - 1));
        FKey = new Objects (rand.Next(grid.Rows - 1), rand.Next(grid.Columns - 1));
        Shelter = new Objects (rand.Next(1,grid.Rows - 2), rand.Next(1,grid.Columns/2));
        
        _Traps = [
            Trap1 = new Trap ("Retener",rand.Next(2,grid.Rows - 1), rand.Next(0, grid.Columns/2)),
            Trap2 = new Trap ("Retener",rand.Next(0,grid.Rows - 2), rand.Next(grid.Columns/2, grid.Columns - 1)),
            Trap3 = new Trap ("Acercar",rand.Next(4,grid.Rows - 1), rand.Next(0, grid.Columns/2)),
            Trap4 = new Trap ("Acercar",rand.Next(6,grid.Rows - 4), rand.Next(grid.Columns/2, grid.Columns - 1)),
            Trap5 = new Trap ("Ralentizar",rand.Next(0,grid.Rows - 1), rand.Next(0, grid.Columns/2)),
            Trap6 = new Trap ("Ralentizar",rand.Next(4,grid.Rows - 4), rand.Next(0, grid.Columns - 1)),
        ];
        
        PaintMaze(grid);
        GameStatus();
        
        if (OperatingSystem.IsWindows()) {
            SoundPlayer main = new SoundPlayer();
            main.SoundLocation = "Haunting_Play.wav";
            main.Load();
            main.PlayLooping();
        }
        
        cool_bad = badGuy!.AbilityCooldown;
        cool_good = goodGuy!.AbilityCooldown;

        while (true) {
            game_Initialized = true;
            Game.Play(goodGuy!);
            Game.Play(badGuy!);
           
            if (goodGuy.AbilityCooldown > 0)
                goodGuy.AbilityCooldown --;
            if (badGuy.AbilityCooldown > 0)
                badGuy.AbilityCooldown --;
        }
    }
    
    public static void PaintMaze(Grid g)
    {
        Console.Clear();
        
        // Tope del tablero
        StringBuilder output = new StringBuilder(" + ");
        for (int i = 0; i < g.Columns; i++) {
            output.Append("--- + ");
        }
        output.AppendLine();

        foreach (var row in g.Row) {
            var top = " | ";
            var bottom = " + ";
            foreach (var cell in row) {
                
                // Paredes de la derecha de cada celda
                string east = cell.IsLinked(cell.East!) ? "   " : " | ";
                
                // Ubicacion de personajes y objetos
                string body = "   ";
                foreach (var trap in _Traps!) {
                    if (trap.TrapCell == cell && trap.Fell == false)
                        body = "T1 ";
                }
                
                if (cell == Key!.ObjectCell && Game.KeyTaken == false)
                    body = "Key";
                if (cell == FKey!.ObjectCell && Game.FKT == false)
                    body = "Key";
                if (cell == goodGuy!.PlayerCell) {
                    switch (goodGuy.Name) 
                    {
                        case "Joel":
                            body = "Jo ";
                            break;
                        case "Constantine":
                            body = "Co ";
                            break;
                        case "Alan Wake":
                            body = "AW ";
                            break;
                    } 
                }
                if (cell == badGuy!.PlayerCell) {
                    switch (badGuy.Name) 
                    {
                        case "Jason":
                            body = "Ja ";
                            break;
                        case "Freddy Krueger":
                            body = "FK ";
                            break;
                        case "Lucifer":
                            body = "Lu ";
                            break;
                    } 
                }
                if (cell == Shelter!.ObjectCell)
                    body = "Ref";
                if (cell == maze_exit!.ObjectCell)
                    body = "Sal";
                
                top += body + east;
                
                // Paredes inferiores de cada celda
                string south = cell.IsLinked(cell.South!) ? "   " : "---";
                const string corner = " + ";
                bottom += south + corner;
            }
            // Estructura del laberinto excepto el tope
            output.AppendLine(top);
            output.AppendLine(bottom);
        }
        Console.WriteLine(output.ToString());
    }
    public static void GameStatus() { 
        Table table = new Table();
        table.Title = new TableTitle (text:"");
        
        table.AddColumn("Jugadores");
        table.AddColumn("Velocidad");
        table.AddColumn("Tiempo de reposo de la habilidad");
        table.AddColumn("Capturas restantes");

        table.Columns[1].Alignment(Justify.Center);
        table.Columns[2].Alignment(Justify.Center);
        table.Columns[3].Alignment(Justify.Center);
        
        string capture_left = Convert.ToString(goodGuy!.Life);
        string stamina_bad = Convert.ToString(badGuy!.Turns);
        string stamina_good = Convert.ToString(goodGuy!.Turns);
        string cooldown_bad = Convert.ToString(badGuy.AbilityCooldown);
        string cooldown_good = Convert.ToString(goodGuy.AbilityCooldown);
        
        table.AddRow([badGuy!.Name, stamina_bad, cooldown_bad, capture_left]);
        table.AddRow([goodGuy!.Name, stamina_good, cooldown_good]);
        AnsiConsole.Write(table);
        Console.WriteLine("Presione (esc) para pausar juego");
    }
}