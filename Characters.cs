using System.Runtime.InteropServices;

namespace Project;
public class Character {
    public string Name;
    public int yPos;
    public int xPos;
    public Cell? PlayerCell {get; set;}
    public int Turns;
    public int AbilityCooldown;

    public Character(string character, int RowPos, int ColumnPos, int turns, int cool)
    {   
        Name = character;
        yPos = RowPos;
        xPos = ColumnPos;
        PlayerCell = Program.grid![yPos, xPos];
        Turns = turns;
        AbilityCooldown = cool;
    }
}