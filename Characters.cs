using System.Runtime.InteropServices;

public class Character{
    public string Name {get; set;}
    public int yPos {get; set;}
    public int xPos {get; set;}
    public Cell? PlayerCell {get; set;}
    public int Turns; 

    public Character(string character, int RowPos, int ColumnPos, Grid grid, int turns)
    {   
        Name = character;
        yPos = RowPos;
        xPos = ColumnPos;
        PlayerCell = grid[yPos, xPos];
        Turns = turns;
    }
    public static void Move(Character c, Grid grid)
    {
        var temp = c.Turns;
        while(c.Turns > 0) {
            
            ConsoleKeyInfo cki = Console.ReadKey(true);
            if (cki.Key == ConsoleKey.W && c.PlayerCell!.IsLinked(c.PlayerCell.North!)) {
                c.yPos -= 1;
                c.PlayerCell = grid[c.yPos, c.xPos];
                ReturnVictim();
                c.Turns -= 1;
            }
            if (cki.Key == ConsoleKey.S && c.PlayerCell!.IsLinked(c.PlayerCell.South!)) {
                c.yPos += 1;
                c.PlayerCell = grid[c.yPos, c.xPos];
                ReturnVictim();
                c.Turns -= 1;
            }
            if (cki.Key == ConsoleKey.A && c.PlayerCell!.IsLinked(c.PlayerCell.West!)) {
                c.xPos -= 1;
                c.PlayerCell = grid[c.yPos, c.xPos];
                ReturnVictim();
                c.Turns -= 1;
            }
            if (cki.Key == ConsoleKey.D && c.PlayerCell!.IsLinked(c.PlayerCell.East!)) {
                c.xPos += 1;
                c.PlayerCell = grid[c.yPos, c.xPos];
                ReturnVictim();
                c.Turns -= 1;
            }
            
            if (cki.Key == ConsoleKey.Escape) {
                Menu.PauseMenu();
                int option = int.Parse(Console.ReadLine()!);
                if (option == 1) {
                    Program.PaintMaze(grid);
                    continue;
                }
                if (option == 2) {
                    Environment.Exit(0);
                }    
            }
            Program.PaintMaze(grid);
        }
        c.Turns = temp;
    }

    private static bool VictimReached() {
        if (Program.badGuy!.PlayerCell == Program.goodGuy!.PlayerCell)
            return true;
        else return false;
    }

    private static void ReturnVictim() {
        if (VictimReached()) {
            Console.WriteLine($"{Program.badGuy} ha alcanzado a {Program.goodGuy}");
            Program.goodGuy!.PlayerCell = Program.grid![0,0];
            Program.goodGuy.yPos = 0;
            Program.goodGuy.xPos = 0;
        }
    }
}