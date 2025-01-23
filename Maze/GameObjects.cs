using Project;

public class Objects {
    public static string? Name {get; set;}
    public int yPos {get; set;}
    public int xPos {get; set;}
    public Cell? ObjectCell {get; set;}

    public Objects (string n, int RowPos, int ColumnPos)
    {
        Name = n;
        yPos = RowPos;
        xPos = ColumnPos;
        ObjectCell = Program.grid![yPos, xPos];
    }

    public static void MantainPlayer(){
        
    }
}