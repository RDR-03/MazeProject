using Project;

public class Objects {
    public int yPos;
    public int xPos;
    public Cell? ObjectCell {get; set;}

    public Objects (int RowPos, int ColumnPos)
    {
        yPos = RowPos;
        xPos = ColumnPos;
        ObjectCell = Program.grid![yPos, xPos];
    }
}