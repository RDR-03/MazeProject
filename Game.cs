namespace Project;
public class Game
{
    private static bool shelterVisited;
    private static bool FallenInTrap = false;
    public static bool KeyTaken;
    public static bool FKT;

    public static void Play(Character c)
    {
        var temp_turns = c.Turns;

        while(c.Turns > 0) {
            if (Program.goodGuy!.PlayerCell == Program.Shelter!.ObjectCell) {
                Console.WriteLine($"{Program.goodGuy.Name} entró en el refugio");
                shelterVisited = true;
            }
            if (Program.goodGuy!.PlayerCell == Program.Key!.ObjectCell && KeyTaken == false) {
                Console.WriteLine($"{Program.goodGuy.Name} encontró una llave");
                KeyTaken = true;
            }
            if (Program.goodGuy!.PlayerCell == Program.FKey!.ObjectCell && FKT == false) {
                Console.WriteLine($"{Program.goodGuy.Name} encontró una llave");
                FKT = true;
            }
            ExitReached();

            Console.WriteLine($"Es el turno de {c.Name}");
            if (c.Name != "Joel" && c.Name != "Alan Wake" && c.AbilityCooldown == 0)
                Console.WriteLine("Habilidad disponible. Presione (H) para utilizarla");
            
            ConsoleKeyInfo cki = Console.ReadKey(true);
            Move(cki, c);
            
            // Activar habilidad
            if (c.Name != "Joel" && c.AbilityCooldown == 0 && cki.Key == ConsoleKey.H) {
                RunAbility(c);

                //  Los cooldowns de los otros asesinos estan en los propios efectos
                if (c.Name == "Lucifer")
                    c.AbilityCooldown = Program.cool_bad;

                if (c == Program.goodGuy)
                    c.AbilityCooldown = Program.cool_good;
            }
            
            // Pausa
            if (cki.Key == ConsoleKey.Escape) 
                Menu.PauseMenu();
            
            ReturnVictim();
            TrapAction(c);
            
            Program.PaintMaze(Program.grid!);
            Program.GameStatus();
        }
        c.Turns = temp_turns;
        if (FallenInTrap)
            c.AbilityCooldown = Program.cool_good;
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
    private static void RunAbility(Character c) {
        if (c.Name.StartsWith("Fred"))
            Effects.PutToSleep();

        if (c.Name.StartsWith("Luci"))
            Effects.RebuildMaze();

        if (c.Name.StartsWith("Jas"))
            Effects.SmashWall();
        
        if (c.Name.StartsWith("Const"))
            Effects.SwitchPos();
    }
    private static bool HidingInShelter() {
        return Program.goodGuy!.PlayerCell == Program.Shelter!.ObjectCell;
    }
    private static void ReturnVictim() {
        if (VictimReached() && !HidingInShelter()) {
           
            // Habilidad pasiva de Alan
            if (Program.goodGuy!.Name.StartsWith("Ala") && Program.goodGuy.AbilityCooldown == 0) {
                Console.WriteLine("Alan abre los ojos y pone en duda de si lo ocurrido hasta el momento fue un sueño");
                
                if (FKT == true  && KeyTaken == true) {
                    Console.WriteLine("Incluso se da cuenta de que no tiene la llave que supuestamente había encontrado");
                    KeyTaken = false;
                    FKT = false;
                }
                else if (KeyTaken == true) {
                    Console.WriteLine("Incluso se da cuenta de que no tiene la llave que supuestamente había encontrado");
                    KeyTaken = false;
                }
                else if (FKT == true) {
                    Console.WriteLine("Incluso se da cuenta de que no tiene la llave que supuestamente había encontrado");
                    FKT = false;
                }
                Thread.Sleep(5000);
                Program.badGuy!.xPos = 0;
                Program.badGuy.yPos = Program.grid!.Rows - 1;
                Program.badGuy.PlayerCell = Program.grid[Program.grid.Rows - 1, 0];
                Program.goodGuy.AbilityCooldown = Program.cool_good;
            }

            else {
                Console.WriteLine($"\n{Program.badGuy!.Name} alcanzó a {Program.goodGuy!.Name}");
                if (!shelterVisited)
                {
                    Thread.Sleep(1500);
                    Program.goodGuy!.PlayerCell = Program.grid![0,0];
                    Program.goodGuy.yPos = 0;
                    Program.goodGuy.xPos = 0;
                    Program.goodGuy.Life -= 1;
                }
                else if (shelterVisited)
                {
                    Console.WriteLine($"{Program.goodGuy.Name} regresa al refugio para recuperarse del ataque");
                    Thread.Sleep(1500);
                    Program.goodGuy!.PlayerCell = Program.Shelter!.ObjectCell;
                    Program.goodGuy.yPos = Program.Shelter.yPos;
                    Program.goodGuy.xPos = Program.Shelter.xPos;
                    Program.goodGuy.Life -= 1;
                }

                if (Program.goodGuy.Life == 0) {
                    Console.WriteLine($"{Program.badGuy.Name} acabó con la vida de {Program.goodGuy.Name}");
                    Console.WriteLine($"{Program.badGuy.Name} ganó la partida");
                    Environment.Exit(0);
                }
            }
        }
        else if (VictimReached() && HidingInShelter()) {
            Console.WriteLine($"\n{Program.goodGuy!.Name} está refugiado y {Program.badGuy!.Name} no logra encontrarlo");
            Thread.Sleep(2000);
        }
    }
    private static bool VictimReached() {
        return Program.badGuy!.PlayerCell == Program.goodGuy!.PlayerCell;
    }
    private static void ExitReached() {
        if (Program.goodGuy!.PlayerCell == Program.maze_exit!.ObjectCell) {
            if (KeyTaken == true) {
                Console.WriteLine($"\n{Program.goodGuy.Name} logró escapar del laberinto");
                Console.WriteLine($"{Program.goodGuy.Name} es el ganador de la partida");
                Thread.Sleep(2000);
                Environment.Exit(0);
            }
            else if (FKT == true) {
                Console.WriteLine("La llave que trae no es la que abre el portón");
                Thread.Sleep(2000);
            }
            else
                Console.WriteLine($"{Program.goodGuy.Name} no posee la llave necesaria para abrir el portón");

        }
    }
    private static void TrapAction(Character c) {
        if (FellInTrap(c)) {
            
            // Habilidad pasiva de Joel
            if (c.Name == "Joel" && c.AbilityCooldown == 0) {
                Console.WriteLine("La intuición y experiencia de Joel le permite eludir las trampas durante esta ronda");
                Thread.Sleep(2000);
                FallenInTrap = true;
            }
            else
            {
                for (int i = 0; i < Program._Traps!.Length; i++) {
                    if (c.PlayerCell == Program._Traps[i].TrapCell && Program._Traps[i].Fell == false) {
                        if (Program._Traps[i].Type == "Retener") {
                            Console.WriteLine("\nHa caído en un hueco que estaba oculto. Pierde su ronda intentando salir");
                            Effects.MantainPlayer(c, c.Turns);
                        }
                        if (Program._Traps[i].Type == "Ralentizar") {
                            Console.WriteLine($"{c.Name} entró en un terreno pantanoso que le ralentiza el movimiento");
                            Effects.MantainPlayer(c, 1);
                        }
                        if (c == Program.goodGuy && Program._Traps[i].Type == "Acercar")
                        {
                            Console.WriteLine($"{Program.goodGuy.Name} reveló su posición al enemigo, el cual se acerca inmediante al lugar");
                            
                            int new_ypos = 0;
                            int new_xpos = 0;
                            while (new_xpos < 0 || new_xpos > Program.grid!.Columns-1
                                || new_ypos < 0 || new_ypos > Program.grid.Rows - 1)
                            {
                                int [] dy = [1, 0, -1, 0];
                                int [] dx = [0, 1, 0, -1];

                                Random rand = new Random();
                                int index = rand.Next(0, dx.Length - 1);

                                new_ypos = c.yPos + dy[index];
                                new_xpos = c.xPos + dx[index];
                            }
                            Program.badGuy!.yPos = new_ypos;
                            Program.badGuy.xPos = new_xpos;
                            Program.badGuy.PlayerCell = Program.grid[new_ypos, new_xpos];
                        }
                        Thread.Sleep(2000);
                        Program._Traps[i].Fell = true;
                    }
                }
            } 
        }
    }
    public static bool FellInTrap(Character c) {
        foreach (var trap in Program._Traps!) {
            if (trap.TrapCell == c.PlayerCell)
                return true;
        }
        return false;
    }
}