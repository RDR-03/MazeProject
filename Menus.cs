using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using Spectre.Console;

namespace Project;
public class Menu
{
    static string[]? options;
    static int counter = 0;
    static ConsoleKeyInfo cki;
    static readonly int x = Console.CursorLeft;
    static readonly int y = Console.CursorTop;

    public static void StartMenu() {
        Console.Clear();
        Console.WriteLine("¿Escaparás?\n");
        options = new string[]
        {
            "Comenzar a jugar",
            "Sobre el juego",
            "Salir"
        };
        Console.CursorVisible = false;
        string highlighted = Highlight(options, counter);

        Cycle();
        if (counter == 0) return;
        if (counter == 1) {
            counter = 0;
            GameInfo();
        }
        if (counter == 2) Environment.Exit(0);
    }
    public static void PauseMenu() {
        Console.Clear();
        Console.WriteLine("Menú de Pausa\n");
        options = new string[]
        {
            "Continuar",
            "Sobre el juego",
            "Salir"
        };
        Console.CursorVisible = false;
        string highlighted = Highlight(options, counter);
        
        Cycle();
        if (counter == 0) {
            Program.PaintMaze(Program.grid!);
            return;
        }
        if (counter == 1) {
            counter = 0;
            GameInfo();
        }
        if (counter == 2) Environment.Exit(0);
    }
    private static void GameInfo() {
        Console.Clear();
        Console.WriteLine("Sobre el juego\n");
        options = new string[]
        {
            "Controles",
            "Información General",
            "Personajes",
            "Atrás"
        };
        
        string highlighted = Highlight(options, counter);
       
        Cycle();
        if (counter == 0) {
            Controls();
        } 
        if (counter == 1) {
            GeneralInfo();
        }
        if (counter == 2) {
            counter = 0;
            CharacterType();
        }
        if (counter == 3) {
            counter = 1;
            if (Program.game_Initialized == true)
                PauseMenu();
            else StartMenu();
        }
    }
    private static string Highlight (string[] items, int option) {
        string actualSelection = string.Empty;
        int _highlighted = 0;
        Array.ForEach(items, element =>
        {
            if (_highlighted == option) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"> {element} <");
                Console.ForegroundColor = ConsoleColor.White;
                actualSelection = element;
            }
            else {
                Console.Write(new string(' ', Console.WindowWidth));
                Console.CursorLeft = 0;
                Console.WriteLine(element);
            }
            _highlighted ++;
        });
        
        return actualSelection;
    }
    private static void Cycle() {
        while((cki = Console.ReadKey(true)).Key != ConsoleKey.Enter)
        {
            if (cki.Key == ConsoleKey.W || cki.Key == ConsoleKey.UpArrow) {
                if (counter == 0) continue;
                counter --;
            }
            if (cki.Key == ConsoleKey.S || cki.Key == ConsoleKey.DownArrow) {
                if (counter == options!.Length - 1) continue;
                counter ++;
            }
        
            Console.CursorLeft = x;
            Console.CursorTop = y;
            string highlighted = Highlight(options!, counter);
        }
    }
    
    public static (string, string) PlayerSelection() {
        Console.Clear();
        
        var H_S = new Style(Color.Red, Color.Default, Decoration.SlowBlink);
        var firstSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Jugador 1 Seleccione Un Personaje")
                .PageSize(8)
                .HighlightStyle(H_S)
                .AddChoiceGroup("Asesinos", 
                    ["Jason (Ja)", "Freddy Krueger (FK)", "Lucifer (Lu)"])
                .AddChoiceGroup("Sobrevivientes",
                    ["Joel (Jo)", "Alan Wake (AW)", "Constantine (Co)"])
        );

        string remainingGroup;
        string[] remainingChoices;
        if (firstSelection.StartsWith("Jason") || firstSelection.StartsWith("Freddy") ||
            firstSelection.StartsWith("Lucifer"))
        {
            remainingGroup = "Sobreviviente";
            remainingChoices = ["Joel (Jo)", "Alan Wake (AW)", "Constantine (Co)"];
        }
        else {
            remainingGroup = "Asesino";
            remainingChoices = ["Jason (Ja)", "Freddy Krueger (FK)", "Lucifer (Lu)"];
        }

        var secondSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title ($"Jugador 2 Seleccione Un {remainingGroup}")
            .PageSize(8)
            .HighlightStyle (H_S)
            .AddChoices(remainingChoices)
        );

        return (firstSelection,secondSelection);
    }
    public static void CharacterType() {
        Console.Clear();
        Console.WriteLine("Seleccione que grupo desea conocer\n");

        options = new string[]
        {
            "Asesinos",
            "Sobrevivientes",
            "Atrás"
        };
        Console.CursorVisible = false;
        string highlighted = Highlight(options, counter);

        Cycle();
        if (counter == 0) {
            Character.ShowInfo1();
        } 
        if (counter == 1) {
            Character.ShowInfo2();
        }
        if (counter == 2) {
            GameInfo();
        }
    }
    private static void GeneralInfo() {
        Console.Clear();
        Console.WriteLine("Descripción del juego");
        Console.Write("El juego ¿Escaparás? basa su trama en las persecuciones de las historias de terror y suspenso.\n");
        Console.WriteLine("");
        Console.Write(@"Los perseguidores están inspirados en villanos conocidos de las historias de terror.
Siguiendo una estela parecida, los sobrevivientes que se encuentran en el juego son reconocidos en tramas oscuras y llenas de suspenso
(sin ser necesariamente del género de terror)");
        Console.WriteLine("");
        Console.WriteLine("\nObjetivos");
        Console.Write("Los personajes que pertenecen al grupo de Asesinos consiguen la victoria si alcanzan determinada cantidad de veces al personaje perteneciente al grupo contrario.\n");
        Console.Write("Los personajes que pertenecen al grupo de Sobrevivientes ganan si logran escapar del laberinto tras conseguir la llave que desbloquea la salida.\n");
    
        Console.WriteLine("\nPresione (esc) o (backspace) para retornar");
        ConsoleKeyInfo cki = Console.ReadKey(true);
        if (cki.Key == ConsoleKey.Backspace || cki.Key == ConsoleKey.Escape)
            GameInfo();
        else
            GeneralInfo();
    }
    private static void Controls() {
        Console.Clear();
        Table table = new Table();
        table.Title = new TableTitle (text:"Controles");
        
        table.AddColumn("Acción");
        table.AddColumn("Tecla");
        table.AddRow(["Mover hacia arriba","W  o Flecha hacia arriba"]);
        table.AddRow(["Mover hacia abajo","S  o Flecha hacia abajo"]);
        table.AddRow(["Mover hacia la derecha","D  o Flecha hacia la derecha"]);
        table.AddRow(["Mover hacia la izquierda","A  o Flecha hacia la izquierda"]);
        table.AddRow(["Utilizar habilidad","H"]);
        table.AddRow(["Pausar","esc"]);
        AnsiConsole.Write(table);

        Console.WriteLine("\nPresione (esc) o (backspace) para retornar");
        ConsoleKeyInfo cki = Console.ReadKey(true);
        if (cki.Key == ConsoleKey.Backspace || cki.Key == ConsoleKey.Escape)
            GameInfo();
        else
            Controls();
    }
} 