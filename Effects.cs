namespace Project;
public class Effects {

    public static void MantainPlayer(Character c, int time){
        c.Turns -= time;
    }

    public static void SwitchPos() {
        
        Console.WriteLine($"Constantine llevo a cabo un conjuro de intercambio de posicion con {Program.badGuy!.Name}");
        Thread.Sleep(2000);

        var xTemp = Program.badGuy.xPos;
        var yTemp = Program.badGuy.yPos;
        var CellTemp = Program.badGuy.PlayerCell;

        Program.badGuy.xPos = Program.goodGuy!.xPos;
        Program.badGuy.yPos = Program.goodGuy.yPos;
        Program.badGuy.PlayerCell = Program.goodGuy.PlayerCell;

        Program.goodGuy.xPos = xTemp;
        Program.goodGuy.yPos = yTemp;
        Program.goodGuy.PlayerCell = CellTemp;
    }
    public static void PutToSleep() {
        for (int i = 0; i < 2; i++) {
            Console.WriteLine($"Freddy indujo a {Program.goodGuy!.Name} al sueño y duerme por {3-i} rondas");
            
            // xq no corre ?
            // if (i == 2)
            //    Console.WriteLine($"Freddy indujo a {Program.goodGuy!.Name} al sueño y duerme por 1 ronda");

            Thread.Sleep(2000);
            Program.badGuy!.AbilityCooldown = Program.cool_bad;

            Console.Clear();
            Program.PaintMaze(Program.grid!);
            Program.GameStatus();
            Character.Play(Program.badGuy!);
            Program.rounds++;
        }
    }

    public static void SmashWall() {
        Console.WriteLine("Jason puede romper una pared en el siguiente movimiento");
        Console.WriteLine("Introduce una direccion valida hacia la cual moverte");
        
        var ypos = Program.badGuy!.yPos;
        var xpos = Program.badGuy.xPos;
        var cell = Program.badGuy.PlayerCell;

        ConsoleKeyInfo cki = Console.ReadKey(true);

        if (cki.Key == ConsoleKey.W || cki.Key == ConsoleKey.UpArrow)
        {   
            if (cell.North != null) {
                if (cell.IsLinked(cell.North)) {
                    Console.WriteLine("No hay pared en esta direccion que Jason pueda romper. Aun asi continua su marcha");
                    Thread.Sleep(2000);
                }
                if (!cell.IsLinked(cell.North)) {
                    cell.Link(cell.North);
                    Program.badGuy!.AbilityCooldown = Program.cool_bad;
                }
                
                Program.badGuy.yPos -= 1;
                ypos = Program.badGuy.yPos;
            
                Program.badGuy.PlayerCell = Program.grid![ypos, xpos];
                Program.badGuy.Turns -= 1;
            }
            if (cell.North == null) {
                Console.WriteLine("Jason no puede romper esta pared");
                Thread.Sleep(2000);
            }
        }
            
        if (cki.Key == ConsoleKey.S || cki.Key == ConsoleKey.DownArrow)
        {   
            if (cell.South != null) {
                if (cell.IsLinked(cell.South)) {
                    Console.WriteLine("No hay pared en esta direccion que Jason pueda romper. Aun asi continua su marcha");
                    Thread.Sleep(2000);
                }
                if (!cell.IsLinked(cell.South)) {
                    cell.Link(cell.South);
                    Program.badGuy!.AbilityCooldown = Program.cool_bad;
                }

                Program.badGuy.yPos += 1;
                ypos = Program.badGuy.yPos;

                Program.badGuy.PlayerCell = Program.grid![ypos, xpos];
                Program.badGuy.Turns -= 1;
            }
            if (cell.South == null) {
                Console.WriteLine("Jason no puede romper esta pared");
                Thread.Sleep(2000);
            }
        }
        
        if (cki.Key == ConsoleKey.D || cki.Key == ConsoleKey.RightArrow)
        {
            if (cell.East != null) {
                if (cell.IsLinked(cell.East)) {
                    Console.WriteLine("No hay pared en esta direccion que Jason pueda romper. Aun asi continua su marcha");
                    Thread.Sleep(2000);
                }
                if (!cell.IsLinked(cell.East)) {
                    cell.Link(cell.East);
                    Program.badGuy!.AbilityCooldown = Program.cool_bad;
                }

                Program.badGuy.xPos += 1;
                xpos = Program.badGuy.xPos;

                Program.badGuy.PlayerCell = Program.grid![ypos, xpos];
                Program.badGuy.Turns -= 1;
            }
            if (cell.East == null) {
                Console.WriteLine("Jason no puede romper esta pared");
                Thread.Sleep(2000);
            }
        }
        
        if (cki.Key == ConsoleKey.A || cki.Key == ConsoleKey.LeftArrow)
        {
            if (cell.West != null) {
                if (cell.IsLinked(cell.West)) {
                    Console.WriteLine("No hay pared en esta direccion que Jason pueda romper. Aun asi continua su marcha");
                    Thread.Sleep(2000);
                }
                if (!cell.IsLinked(cell.West)) {
                    cell.Link(cell.West);
                    Program.badGuy!.AbilityCooldown = Program.cool_bad;
                }
                Program.badGuy.xPos -= 1;
                xpos = Program.badGuy.xPos;

                Program.badGuy.PlayerCell = Program.grid![ypos, xpos];
                Program.badGuy.Turns -= 1;
            }
            if (cell.West == null) {
                Console.WriteLine("Jason no puede romper esta pared");
                Thread.Sleep(2000);
            }
        }
    }

    
    public static void RebuildMaze() {
        Console.WriteLine("Lucifer cambio la estructura del laberinto tras desatar su ira");
        Thread.Sleep(2000);

        foreach (var cell in Program.grid!.Cells) {
            if (cell.IsLinked(cell.North!))
                cell.Unlink(cell.North!);
            if (cell.IsLinked(cell.South!))
                cell.Unlink(cell.South!);
            if (cell.IsLinked(cell.East!))
                cell.Unlink(cell.East!);
            if (cell.IsLinked(cell.West!))
                cell.Unlink(cell.West!);
        }

        MazeResolve.CreateMaze(Program.grid);
    }
}