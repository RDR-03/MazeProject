namespace Project;
public class Trap
{
    public static string? Type;
    public int xpos;
    public int ypos;
    public Cell? TrapCell;
    public static Dictionary<Character,bool> fell;

    public Trap(string function, int yPos, int xPos) {
        Type = function;
        xpos = xPos;
        ypos = yPos;
        TrapCell = Program.grid![ypos,xpos];
        fell = new Dictionary<Character,bool>
        {
            {Program.goodGuy!, false},
            {Program.badGuy!, false}
        };
    }
}