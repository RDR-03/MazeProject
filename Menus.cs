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
        Console.WriteLine("Escape the Undead\n");
        options = new string[]
        {
            "Comenzar a jugar",
            "Saber acerca del juego",
            "Salir"
        };
        Console.CursorVisible = false;
        string highlighted = Highlight(options, counter);
        //while(true) {
            Cycle();
            if (counter == 0) {
               return;
            } 
            if (counter == 1) {
                GameInfo();
                return;
            }
            if (counter == 2) Environment.Exit(0);
       // }
    }
    public static void PauseMenu() {
        Console.Clear();
        Console.WriteLine("Menu de Pausa\n");
        options = new string[]
        {
            "Continuar",
            "Sobre el juego",
            "Salir"
        };
        
        Console.CursorVisible = false;
        string highlighted = Highlight(options, counter);
        while(true) {
            Cycle();
            if (counter == 0) {
                Program.PaintMaze(Program.grid!);
                return;
            }
            if (counter == 1) GameInfo();
            if (counter == 2) Environment.Exit(0);
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
                .Title("Jugardor 1 Seleccione Un Personaje")
                .PageSize(8)
                .HighlightStyle(H_S)
                .AddChoiceGroup("Asesinos", 
                    ["Jason (Ja)", "Freddy Krueger (FK)", "Lucifer (Lu)"])
                .AddChoiceGroup("Sobrevivientes",
                    ["Joel (Jo)", "Sam Bridges (SB)", "Constantine (Co)"])
        );

        string remainingGroup;
        string[] remainingChoices;
        if (firstSelection.StartsWith("Jason") || firstSelection.StartsWith("Freddy") ||
            firstSelection.StartsWith("Lucifer"))
        {
                remainingGroup = "Sobreviviente";
                remainingChoices = ["Joel (Jo)", "Sam Bridges (SB)", "Constantine (Co)"];
        }
        else {
            remainingGroup = "Asesino";
            remainingChoices = ["Jason (Ja)", "Freddy Krueger (FK)", "Lucifer (Lu)"];
        }

        var secondSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title ($"Seleccione Ahora Un {remainingGroup}")
            .PageSize(8)
            .HighlightStyle (H_S)
            .AddChoices(remainingChoices)
        );

        return (firstSelection,secondSelection);
    }
    private static void GameInfo() {
        Console.Clear();
    }
} 