using Project;

public class Objects {
    public static string? Name;
    public int yPos;
    public int xPos;
    public Cell? ObjectCell {get; set;}

    public Objects (string n, int RowPos, int ColumnPos)
    {
        Name = n;
        yPos = RowPos;
        xPos = ColumnPos;
        ObjectCell = Program.grid![yPos, xPos];
    }
}