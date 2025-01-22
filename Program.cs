using System.Runtime.InteropServices;
using System.Text;
using Spectre.Console;

namespace Project;
class Program
{
    public static Grid? grid;
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
        MazeRezolve.CreateMaze(grid);

        if (charSelection1.StartsWith("Jason") || charSelection2.StartsWith("Jason"))
            badGuy = new Character ("Jason", grid.Rows - 1, 0, grid, 3);

        else if (charSelection1.StartsWith("Fred") || charSelection2.StartsWith("Fred"))
            badGuy = new Character ("Freddy Krueger", grid.Rows - 1, 0, grid, 2);
        
        else if (charSelection1.StartsWith("Luci") || charSelection2.StartsWith("Luci"))
            badGuy = new Character ("Lucifer", grid.Rows - 1, 0, grid, 3);
        
        // Instanciar sobreviviente
        if (charSelection1.StartsWith("Joel") || charSelection2.StartsWith("Joel"))
            goodGuy = new Character ("Joel", 0, 0, grid, 3);

        else if (charSelection1.StartsWith("Sam") || charSelection2.StartsWith("Sam"))
            goodGuy = new Character ("Sam Bridges", 0, 0, grid, 5);
        
        else if (charSelection1.StartsWith("Constan") || charSelection2.StartsWith("Constan"))
            goodGuy = new Character ("Constantine", 0, 0, grid, 4);
        
        Console.WriteLine($"{badGuy!.Name}");
        Console.WriteLine (goodGuy!.Name);
        /*if (seleccion == 1) {
            badGuy = new Character("Jason", grid.Rows - 1, 0, grid, 3);
            Console.WriteLine(" A Jugador2 le toco Joel");
            Thread.Sleep(1500);
            goodGuy = new Character("Joel", 0, 0, grid, 5);
        }
        else if (seleccion == 4) {
            goodGuy = new Character("Joel", 0, 0, grid, 5);
            Console.WriteLine(" A Jugador2 le toco Jason");
            Thread.Sleep(1500);
            badGuy = new Character("Jason", (grid.Rows- 1)/2 , 0, grid, 3);
        }*/
        PaintMaze(grid);
        /*GameStatus();
        
        rounds = 0;
        while (rounds<5) {                       
            Character.Move(goodGuy!, grid);  ;             
            Character.Move(badGuy!, grid);
            rounds ++;
        }*/
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
                // Ubicacion de personajes
                string body = "   ";
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
            table.AddColumn("Stamina");
            table.AddColumn("Reutilizacion de habilidad");
            
            string stamina_bad = Convert.ToString(badGuy!.Turns);
            string stamina_good = Convert.ToString(goodGuy!.Turns);
            table.AddRow([badGuy!.Name, stamina_bad, "2"]);
            table.AddRow([goodGuy!.Name, stamina_good,"6"]);
            AnsiConsole.Write(table);
            Console.WriteLine("Presione (esc) para pausar juego");
    }
}