using System.Runtime.InteropServices;

namespace Project;
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
            
            Console.WriteLine($"Es el turno de {c.Name}");
            ConsoleKeyInfo cki = Console.ReadKey(true);
            if ((cki.Key == ConsoleKey.W || cki.Key == ConsoleKey.UpArrow)
                && c.PlayerCell!.IsLinked(c.PlayerCell.North!))
            {
                c.yPos -= 1;
                c.PlayerCell = grid[c.yPos, c.xPos];
                ReturnVictim();
                c.Turns -= 1;
            }
            if ((cki.Key == ConsoleKey.S || cki.Key == ConsoleKey.DownArrow)
                && c.PlayerCell!.IsLinked(c.PlayerCell.South!))
            {
                c.yPos += 1;
                c.PlayerCell = grid[c.yPos, c.xPos];
                ReturnVictim();
                c.Turns -= 1;
            }
            if ((cki.Key == ConsoleKey.A || cki.Key == ConsoleKey.LeftArrow)
                && c.PlayerCell!.IsLinked(c.PlayerCell.West!))
            {
                c.xPos -= 1;
                c.PlayerCell = grid[c.yPos, c.xPos];
                ReturnVictim();
                c.Turns -= 1;
            }
            if ((cki.Key == ConsoleKey.D || cki.Key == ConsoleKey.RightArrow)
                && c.PlayerCell!.IsLinked(c.PlayerCell.East!))
            {
                c.xPos += 1;
                c.PlayerCell = grid[c.yPos, c.xPos];
                ReturnVictim();
                c.Turns -= 1;
            }
            
            if (cki.Key == ConsoleKey.Escape) 
                Menu.PauseMenu();
            
            Program.PaintMaze(grid);
            Program.GameStatus();
        }
        c.Turns = temp;
    }
    public static bool VictimReached() {
        if (Program.badGuy!.PlayerCell == Program.goodGuy!.PlayerCell)
            return true;
        else return false;
    }
    private static void ReturnVictim() {
        if (VictimReached()) {
            Console.WriteLine($"{Program.badGuy!.Name} capturo a {Program.goodGuy!.Name}");
            Thread.Sleep(1500);
            Program.goodGuy!.PlayerCell = Program.grid![0,0];
            Program.goodGuy.yPos = 0;
            Program.goodGuy.xPos = 0;
        }
    }
}