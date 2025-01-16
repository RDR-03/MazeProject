using System.Runtime.InteropServices;
using System.Text;
using Spectre.Console;

class Program
{
    public static Grid? grid;
    public static Character? badGuy;
    public static Character? goodGuy;
    
    static void Main(string[] args)
    {   
         
        Menu.StartMenu();
        grid = new Grid(10,10);
        MazeRezolve.CreateMaze(grid);
        
        Console.WriteLine("Jugador1 Seleccione Un Personaje");
        Console.WriteLine("1) Jason");
        Console.WriteLine("4) Joel");
        int seleccion = int.Parse(Console.ReadLine()!);
        
        if (seleccion == 1) {
            badGuy = new Character("Jason", 6, 8, grid, 3);
            Console.WriteLine("Jugador2 le toco Joel");
            goodGuy = new Character("Joel", 9, 5, grid, 5);
        }
        else if (seleccion == 4) {
            goodGuy = new Character("Joel", 9, 5, grid, 5);
            Console.WriteLine("Jugador2 le toco Jason");
            badGuy = new Character("Jason", 6, 8, grid, 3);
        }
        PaintMaze(grid);
        
        int turns = 0;
        while (turns<5) {
            turns ++;
            Console.WriteLine($"\nEs el turno de {goodGuy!.Name}");            
            Character.Move(goodGuy!, grid);
            
            Console.WriteLine($"\nEs el turno {badGuy!.Name}");
            Character.Move(badGuy!, grid);
        }

        // badGuy = new Character("Freddy Krueger", 0, 3, grid, 1);
        // badGuy = new Character("Lucifer", 5, 0, grid, 2);
        // goodGuy = new Character("", , , ,)
        // goodGuy = new Character("", , , ,)
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
                        case "Chris Redfield":
                            body = "CR ";
                            break;
                        case "Lara Croft":
                            body = "LC ";
                            break;
                    } 
                }
                top += body + east;
                
                // Paredes inferiores de cada celda
                string south = cell.IsLinked(cell.South!) ? "   " : "---";
                const string corner = " + ";
                bottom += south + corner;
            }
            // Borde derecho de las filas
            output.AppendLine(top);
            output.AppendLine(bottom);
        }
        Console.WriteLine(output.ToString());
    }
}