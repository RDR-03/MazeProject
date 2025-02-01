using System.Runtime.InteropServices;
using Spectre.Console;

namespace Project;
public class Character {
    public string Name;
    public int yPos;
    public int xPos;
    public Cell? PlayerCell {get; set;}
    public int Turns;
    public int AbilityCooldown;
    public int Life;

    public Character(string character, int RowPos, int ColumnPos, int turns, int cool, int captures = 3)
    {   
        Name = character;
        yPos = RowPos;
        xPos = ColumnPos;
        PlayerCell = Program.grid![yPos, xPos];
        Turns = turns;
        AbilityCooldown = cool;
        Life = captures;
    }

    public static void ShowInfo1() {
        Console.Clear();
        Table table = new Table();
        table.Title = new TableTitle (text:"Asesinos");

        table.AddColumn("Personaje");
        table.AddColumn("Habilidad");
        table.AddColumn("Tiempo para restaurar habilidad");
        table.AddColumn("Velocidad");
        table.AddRow(["[red]Jason[/]","Rompe una pared en la ronda", "3 rondas", "3"]);
        table.AddRow(["[red]Freddy Krueger[/]","Anula 3 rondas del superviviente", "4 rondas", "2"]);
        table.AddRow(["[red]Lucifer[/]","Reconfigura el laberinto", "5 rondas", "4"]);

        AnsiConsole.Write(table);
            
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\nPresione (esc) o (backspace) para retornar");
        ConsoleKeyInfo cki = Console.ReadKey(true);
        if (cki.Key == ConsoleKey.Backspace || cki.Key == ConsoleKey.Escape)
            Menu.CharacterType();
        else
            ShowInfo1();
    }
        
    public static void ShowInfo2() {
        Console.Clear();
        Table table = new Table();
        table.Title = new TableTitle (text:"Sobrevivientes");
        
        table.AddColumn("Personaje");
        table.AddColumn("Habilidad");
        table.AddColumn("Tiempo para restaurar habilidad");
        table.AddColumn("Velocidad");
        table.AddRow(["[blueviolet]Joel[/]","Ignora trampas por una ronda (pasiva)", "2 rondas", "3"]);
        table.AddRow(["[blueviolet]Alan Wake[/]","Regresa \"ciertas cosas\" a la posición inicial (pasiva)", "4 rondas", "2"]);
        table.AddRow(["[blueviolet]Constantine[/]","Intercambia de lugar con su perseguidor", "6 rondas", "4"]);
    
        AnsiConsole.Write(table);

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\nPresione (esc) o (backspace) para retornar");
        ConsoleKeyInfo cki = Console.ReadKey(true);
        if (cki.Key == ConsoleKey.Backspace || cki.Key == ConsoleKey.Escape)
            Menu.CharacterType();
        else
            ShowInfo2();
    }
}