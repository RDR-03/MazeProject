namespace Project;
public class Trap
{
    public static string? Type;
    public int xpos;
    public int ypos;
    public Cell? TrapCell;
    public bool Fell;

    public Trap(string function, int yPos, int xPos, bool f = false) {
        Type = function;
        xpos = xPos;
        ypos = yPos;
        TrapCell = Program.grid![ypos,xpos];
        Fell = f;
    }
}