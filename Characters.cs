using System.Runtime.InteropServices;

namespace Project;
public class Character {
    public string Name;
    public int yPos;
    public int xPos;
    public Cell? PlayerCell {get; set;}
    public int Turns;
    public int AbilityCooldown;

    private static bool shelterVisited;
    private static bool carTaken;


    public Character(string character, int RowPos, int ColumnPos, int turns, int cool)
    {   
        Name = character;
        yPos = RowPos;
        xPos = ColumnPos;
        PlayerCell = Program.grid![yPos, xPos];
        Turns = turns;
        AbilityCooldown = cool;
    }
    
    public static void Play(Character c)
    {
        var temp_turns = c.Turns;

        while(c.Turns > 0) {
            
            Console.WriteLine($"Es el turno de {c.Name}");
            
            if (c.Name != "Joel" && c.AbilityCooldown == 0)
                Console.WriteLine("Habilidad disponible. Presione (H) para utilizarla");
            
            if (Program.goodGuy!.PlayerCell == Program.Shelter!.ObjectCell)
                shelterVisited = true;
              
            ExitReached();
            
            ConsoleKeyInfo cki = Console.ReadKey(true);
            Move(cki, c);
            
            // Activar habilidad
            if (c.Name != "Joel" && c.AbilityCooldown == 0 && cki.Key == ConsoleKey.H) {
                RunAbility(c);

                if (c.Name == "Lucifer")
                    c.AbilityCooldown = Program.cool_bad;
            //  Los cooldowns de los otros asesinos estan en los propios efectos
                
                if (c == Program.goodGuy)
                    c.AbilityCooldown = Program.cool_good;
            }
            
            // Pausa
            if (cki.Key == ConsoleKey.Escape) 
                Menu.PauseMenu();
            
            ReturnVictim();

            TrapAction(c);
            
            // Acciones tras obtener el carro
            if (carTaken) {
                Program.Car!.xPos = Program.goodGuy.xPos;
                Program.Car.yPos = Program.goodGuy.yPos;
                Program.Car.ObjectCell = Program.goodGuy.PlayerCell;
            }
            
            Program.PaintMaze(Program.grid!);
            Program.GameStatus();
        }
        c.Turns = temp_turns;
    }
    private static void Move(ConsoleKeyInfo cki,Character c) {
        if ((cki.Key == ConsoleKey.W || cki.Key == ConsoleKey.UpArrow)
            && c.PlayerCell!.IsLinked(c.PlayerCell.North!))
        {
            c.yPos -= 1;
            c.PlayerCell = Program.grid![c.yPos, c.xPos];
            c.Turns -= 1;
        }
        if ((cki.Key == ConsoleKey.S || cki.Key == ConsoleKey.DownArrow)
            && c.PlayerCell!.IsLinked(c.PlayerCell.South!))
        {
            c.yPos += 1;
            c.PlayerCell = Program.grid![c.yPos, c.xPos];
            c.Turns -= 1;
        }
        if ((cki.Key == ConsoleKey.A || cki.Key == ConsoleKey.LeftArrow)
            && c.PlayerCell!.IsLinked(c.PlayerCell.West!))
        {
            c.xPos -= 1;
            c.PlayerCell = Program.grid![c.yPos, c.xPos];
            c.Turns -= 1;
        }
        if ((cki.Key == ConsoleKey.D || cki.Key == ConsoleKey.RightArrow)
            && c.PlayerCell!.IsLinked(c.PlayerCell.East!))
        {
            c.xPos += 1;
            c.PlayerCell = Program.grid![c.yPos, c.xPos];
            c.Turns -= 1;
        }
    }
    private static bool HidingInShelter() {
        return Program.goodGuy!.PlayerCell == Program.Shelter!.ObjectCell;
    }
    private static bool VictimReached() {
        return Program.badGuy!.PlayerCell == Program.goodGuy!.PlayerCell;
    }
    private static void ReturnVictim() {
        if (VictimReached() && !HidingInShelter()) {
            Console.WriteLine($"\n{Program.badGuy!.Name} alcanzo a {Program.goodGuy!.Name}");
            
            if (carTaken) {
                int x_temp = Program.goodGuy.xPos!;
                Program.Car!.xPos = x_temp;
                int y_temp = Program.goodGuy.yPos!;
                Program.Car.yPos = y_temp;
                Cell c_temp = Program.goodGuy.PlayerCell!;
                Program.Car.ObjectCell = c_temp;
                
                carTaken = false;
            }

            if (!shelterVisited)
            {
                Thread.Sleep(1500);
                Program.goodGuy!.PlayerCell = Program.grid![0,0];
                Program.goodGuy.yPos = 0;
                Program.goodGuy.xPos = 0;
            }
            else if (shelterVisited)
            {
                Console.WriteLine($"{Program.goodGuy.Name} regresa al refugio para recuperarse del ataque");
                Thread.Sleep(1500);
                Program.goodGuy!.PlayerCell = Program.Shelter!.ObjectCell;
                Program.goodGuy.yPos = Program.Shelter.yPos;
                Program.goodGuy.xPos = Program.Shelter.xPos;
            }
        }
        else if (VictimReached() && HidingInShelter()) {
            Console.WriteLine($"\n{Program.goodGuy!.Name} esta refugiado y {Program.badGuy!.Name} no logra encontrarlo");
            Thread.Sleep(1500);
        }
    }
    private static void ExitReached() {
        if (Program.badGuy!.PlayerCell == Program.maze_exit!.ObjectCell) {
            Console.WriteLine($"\n{Program.badGuy.Name} ha alcanzado la salida antes que {Program.goodGuy!.Name}");
            Console.WriteLine($"{Program.badGuy.Name} es el ganador de la partida");
            Thread.Sleep(1500);
            Environment.Exit(0);
        }
        if (Program.goodGuy!.PlayerCell == Program.maze_exit.ObjectCell) {
            Console.WriteLine($"\n{Program.goodGuy.Name} logro escapar del laberinto");
            Console.WriteLine($"{Program.goodGuy.Name} es el ganador de la partida");
            Thread.Sleep(1500);
            Environment.Exit(0);
        }
    }
    private static void RunAbility(Character c) {
        if (c.Name.StartsWith("Fred")) {
            Effects.PutToSleep();
        }
        if (c.Name.StartsWith("Luci")) {
            Effects.RebuildMaze();
        }
        if (c.Name.StartsWith("Jas")) {
            Effects.SmashWall();
        }
        if (c.Name.StartsWith("Sam")) {

        }
        if (c.Name.StartsWith("Const"))
            Effects.SwitchPos();
    }
    private static void TrapAction(Character c) {
        if (FellInTrap(c)) {
            
            // Habilidad pasiva de Joel
            if (c.Name == "Joel" && c.AbilityCooldown == 0) {
                Console.WriteLine("La intuicion de Joel le permite eludir las trampas durante esta ronda");
                Thread.Sleep(2000);
                if (c.Turns == 0)
                    c.AbilityCooldown = Program.cool_good;
            }
            else
            {
                if (Trap.Type == "Reten") {
                    var rand = new Random();
                    Console.WriteLine("\nHa caido en un agujero. Pierde su ronda intentando salir.");
                    Thread.Sleep(2000);
                    Effects.MantainPlayer(c, c.Turns);
                    //Trap.TrapCell = Program.grid![0, 0]!;
                }
                if (Trap.Type == "Consuelo")
                {
                
                }
                if (Trap.Type == "Devuelve")
                {

                }
            }
        }
    }
    public static bool FellInTrap(Character c) {
        foreach (var trap in Program._Traps!){
            if (trap.TrapCell == c.PlayerCell)
                return true;
        }
        return false;
    }
}