using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using Spectre.Console;

namespace Project;
public class Menu
{
    string[] options;
    static int counter = 0;
    string title;

    private Menu (string title, string[] options) {
        this.title = title;
        this.options = options;
    }
    private void DisplayOptions() {
        Console.WriteLine(title);
        for (int i = 0; i < options.Length; i++) {
            string current_option = options[i];
            if (i == counter) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($">> {current_option} <<");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
                Console.WriteLine(current_option);
        }
    }
    private int Cycle() {
        ConsoleKey keyPressed;
        do {
            Console.Clear();
            Console.CursorVisible = false;
            DisplayOptions();

            ConsoleKeyInfo cki = Console.ReadKey(true);
            keyPressed = cki.Key;

            if (keyPressed == ConsoleKey.W || keyPressed == ConsoleKey.UpArrow) {
                if (counter == 0) continue;
                counter --;
            }
            if (keyPressed == ConsoleKey.S || keyPressed == ConsoleKey.DownArrow) {
                if (counter == options!.Length - 1) continue;
                counter ++;
            }

        } while (keyPressed != ConsoleKey.Enter);

        return counter;
    }
    
    public static void Start() {
        Console.Clear();
        string prompt = @"
█     █░ ██▓ ██▓     ██▓       ▓██   ██▓ ▒█████   █    ██    ▓█████   ██████  ▄████▄   ▄▄▄       ██▓███  ▓█████ 
▓█░ █ ░█░▓██▒▓██▒    ▓██▒        ▒██  ██▒▒██▒  ██▒ ██  ▓██▒   ▓█   ▀ ▒██    ▒ ▒██▀ ▀█  ▒████▄    ▓██░  ██▒▓█   ▀ 
▒█░ █ ░█ ▒██▒▒██░    ▒██░         ▒██ ██░▒██░  ██▒▓██  ▒██░   ▒███   ░ ▓██▄   ▒▓█    ▄ ▒██  ▀█▄  ▓██░ ██▓▒▒███   
░█░ █ ░█ ░██░▒██░    ▒██░         ░ ▐██▓░▒██   ██░▓▓█  ░██░   ▒▓█  ▄   ▒   ██▒▒▓▓▄ ▄██▒░██▄▄▄▄██ ▒██▄█▓▒ ▒▒▓█  ▄ 
░░██▒██▓ ░██░░██████▒░██████▒     ░ ██▒▓░░ ████▓▒░▒▒█████▓    ░▒████▒▒██████▒▒▒ ▓███▀ ░ ▓█   ▓██▒▒██▒ ░  ░░▒████▒
░ ▓░▒ ▒  ░▓  ░ ▒░▓  ░░ ▒░▓  ░      ██▒▒▒ ░ ▒░▒░▒░ ░▒▓▒ ▒ ▒    ░░ ▒░ ░▒ ▒▓▒ ▒ ░░ ░▒ ▒  ░ ▒▒   ▓▒█░▒▓▒░ ░  ░░░ ▒░ ░
  ▒ ░ ░   ▒ ░░ ░ ▒  ░░ ░ ▒  ░    ▓██ ░▒░   ░ ▒ ▒░ ░░▒░ ░ ░     ░ ░  ░░ ░▒  ░ ░  ░  ▒     ▒   ▒▒ ░░▒ ░      ░ ░  ░
  ░   ░   ▒ ░  ░ ░     ░ ░       ▒ ▒ ░░  ░ ░ ░ ▒   ░░░ ░ ░       ░   ░  ░  ░  ░          ░   ▒   ░░          ░   
    ░     ░      ░  ░    ░  ░    ░ ░         ░ ░     ░           ░  ░      ░  ░ ░            ░  ░            ░  ░
                                 ░ ░                                          ░                                  ";
        
        string[] options = {"Comenzar a jugar","Sobre el juego","Salir"};
        Menu main = new Menu(prompt,options);
        int selected_option = main.Cycle();
        
        if (selected_option == 1) {
            counter = 0;
            Info();
        }
        if (selected_option == 2) Environment.Exit(0);
    }
    public static void Pause() {
        Console.Clear();
        string prompt = "Menú de Pausa\n";
        string[] options = {"Continuar","Sobre el juego","Salir"};
        Menu pause = new Menu(prompt,options);
        int selected_option = pause.Cycle();
        
        if (selected_option == 1) {
            counter = 0;
            Info();
        }
        if (selected_option == 2) Environment.Exit(0);
    }
    public static void Info() {
        Console.Clear();
        string prompt = "Sobre el juego\n";
        string[] options = {"Controles","Información General","Personajes","Atrás"};
        Menu info = new Menu(prompt,options);
        int selected_option = info.Cycle();
        
        if (selected_option == 0) Controls();
        if (selected_option == 1) GeneralInfo();
        if (selected_option == 2) {
            counter = 0;
            CharacterType();
        }
        if (selected_option == 3) {
            counter = 1;
            if (Program.game_Initialized == false)
                Start();
            else Pause();
        }
    }

    public static void CharacterType() {
        Console.Clear();
        string prompt = "Seleccione que grupo desea conocer\n";
        string[] options ={"Asesinos","Sobrevivientes","Atrás"};
        Menu type = new Menu(prompt,options);
        int selected_option = type.Cycle();
        
        if (selected_option == 0) Character.ShowInfo1();
        if (selected_option == 1) Character.ShowInfo2();
        if (selected_option == 2) Info();
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
            {
                Info();
            }
        else GeneralInfo();
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
        {
            Info();
        }
        else Controls();
    }
} 