namespace Project;
public class Efects {
    public static void MantainPlayer(Character c, int time){
        c.Turns -= time;
    }

    public static void SwitchPos() {
        var xTemp = Program.badGuy.xPos;
        var yTemp = Program.badGuy.yPos;
        var CellTemp = Program.badGuy.PlayerCell;

        Program.badGuy.xPos = Program.goodGuy.xPos;
        Program.badGuy.yPos = Program.goodGuy.yPos;
        Program.badGuy.PlayerCell = Program.goodGuy.PlayerCell;

        Program.goodGuy.xPos = xTemp;
        Program.goodGuy.yPos = yTemp;
        Program.goodGuy.PlayerCell = CellTemp;
    }

}