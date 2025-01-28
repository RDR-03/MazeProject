using System.Runtime.InteropServices;
using System.Text;
using Spectre.Console;

namespace Project;
class Program
{
    public static Grid? grid;
    public static Objects? maze_exit;
    public static Objects? Shelter;
    public static Objects? Car;
    public static Objects? Trap1;
    public static Objects? Trap2;
    public static Objects? Trap3;
    
    public static Character? badGuy;
    public static Character? goodGuy;
    static int rounds;
    
    static void Main(string[] args)
    {               
        Menu.StartMenu();
        (string, string) Selections = Menu.PlayerSelection();
        string charSelection1 = Selections.Item1;
        string charSelection2 = Selections.Item2;
        
        grid = new Grid(13,13);
        MazeResolve.CreateMaze(grid);
        MazeResolve.CreateMaze(grid);

        // Instanciar asesino
        if (charSelection1.StartsWith("Jason") || charSelection2.StartsWith("Jason"))
            badGuy = new Character ("Jason", grid.Rows - 1, 0, 3);

        else if (charSelection1.StartsWith("Fred") || charSelection2.StartsWith("Fred"))
            badGuy = new Character ("Freddy Krueger", grid.Rows - 1, 0, 2);
        
        else if (charSelection1.StartsWith("Luci") || charSelection2.StartsWith("Luci"))
            badGuy = new Character ("Lucifer", grid.Rows - 1, 0, 3);
        
        // Instanciar sobreviviente
        if (charSelection1.StartsWith("Joel") || charSelection2.StartsWith("Joel"))
            goodGuy = new Character ("Joel", 0, 0, 3);

        else if (charSelection1.StartsWith("Sam") || charSelection2.StartsWith("Sam"))
            goodGuy = new Character ("Sam Bridges", 0, 0, 5);
        
        else if (charSelection1.StartsWith("Constan") || charSelection2.StartsWith("Constan"))
            goodGuy = new Character ("Constantine", 0, 0, 4);
        
        // Objetos del laberinto
        Random rand = new Random();
        maze_exit = new Objects ("Exit", grid.Rows/2, grid.Columns - 1);
        Car = new Objects ("Car", 3, rand.Next(grid.Columns-1));
        Shelter = new Objects ("Shelter", rand.Next(1,grid.Columns - 2), rand.Next(1,grid.Columns/2));
        Trap1 = new Objects ("T1",rand.Next(2,grid.Rows - 2), rand.Next(2,grid.Columns - 2));
        Trap2 = new Objects ("T2",rand.Next(3,grid.Rows - 3), rand.Next(3,grid.Columns - 3));
        Trap3 = new Objects ("T3",rand.Next(4,grid.Rows - 4), rand.Next(4,grid.Columns - 4));
        
        PaintMaze(grid);
        GameStatus();
        
        rounds = 0;
        while (true) {                       
            Character.Move(goodGuy!);          
            Character.Move(badGuy!);
            rounds ++;
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
                if (cell == Trap1!.ObjectCell)
                    body = "T1 ";
                if (cell == Trap2!.ObjectCell)
                    body = "T2 ";
                if (cell == Trap3!.ObjectCell)
                    body = "T3 ";
                
                if (cell == goodGuy!.PlayerCell) {
                    switch (goodGuy.Name) 
                    {
                        case "Joel":
                            body = "Jo ";
                            break;
                        case "Constantine":
                            body = "Co ";
                            break;
                        case "Sam Bridges":
                            body = "SB ";
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
                    body = "Re ";
                if (cell == Car!.ObjectCell)
                    body = "Car";
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
            table.AddColumn("Reutilizacion de habilidad");
            
            string stamina_bad = Convert.ToString(badGuy!.Turns);
            string stamina_good = Convert.ToString(goodGuy!.Turns);
            table.AddRow([badGuy!.Name, stamina_bad, "2"]);
            table.AddRow([goodGuy!.Name, stamina_good,"6"]);
            AnsiConsole.Write(table);
            Console.WriteLine("Presione (esc) para pausar juego");
    }
}