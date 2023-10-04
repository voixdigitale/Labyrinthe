using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;

ConsoleKeyInfo cki;
string cell;

int sizeX = 10;
int sizeY = 10;
int keys = 0;
int[] playerPos = new int[] { 0, 0 };
bool gameFinished = false;

string[,] labyrinthe = new string[,] { //Attention, pour l'accéder il faut faire labyrinthe[y, x]
    { "joueur", "vide", "║║║",    "vide",  "vide", "vide", "vide", "vide", "║║║",  "clé" },
    { "vide",   "═══",  "╩═╝",    "vide",  "╔═╦",  "═══",  "╦═╗", "vide",  "╚═╝",  "vide" },
    { "vide",   "vide", "vide",   "vide",  "╚═╝",  "vide", "╚═╝", "vide",  "vide", "vide" },
    { "═══",    "╦═╗",  "vide",   "╔═╗",   "clé",  "vide", "vide", "═══",  "╦═╗",  "vide" },
    { "vide",   "╚═╝",  "vide",   "║║║",   "vide", "╔═╗",  "vide","vide",  "║║║",  "vide" },
    { "vide",   "vide", "vide",   "╚═╩",   "═══",  "╩═╩",  "╦╦╗", "vide",  "║║║",  "vide" },
    { "vide",   "╔═╗",  "vide",   "vide",  "vide", "clé",  "║║║", "vide",  "╚═╝",  "vide" },
    { "vide",   "║║║",  "enemmi", "╔═╦",   "═══",  "╦═╦",  "╬╬╣", "enemmi","vide", "vide" },
    { "vide",   "╚═╝",  "vide",   "╚═╝",   "clé",  "║║╠",  "╩═╝", "vide",  "═══",  "═══" },
    { "vide",   "vide", "vide",   "vide",  "vide", "╚═╝", "vide", "vide",  "vide", "vide" }
};

ConsoleColor color = Console.ForegroundColor;
ConsoleColor bgColor = Console.BackgroundColor;

Draw();

do {
    cki = Console.ReadKey();
    HandleInput(cki.Key);
} while (cki.Key != ConsoleKey.Escape);


void HandleInput(ConsoleKey key) {
    if (key == ConsoleKey.DownArrow) {
        MoveDown();
    } else if (key == ConsoleKey.UpArrow) {
        MoveUp();
    } else if (key == ConsoleKey.LeftArrow) {
        MoveLeft();
    } else if (key == ConsoleKey.RightArrow) {
        MoveRight();
    }
    UpdatePlayerPos();
}

void UpdateCell(int x, int y, string cell, ConsoleColor color, ConsoleColor bgColor, bool isUI = false) {
    (int cursorLeft, int cursorTop) = Console.GetCursorPosition();
    ConsoleColor originalBG = Console.BackgroundColor;
    ConsoleColor originalFG = Console.ForegroundColor;

    Console.BackgroundColor = bgColor;
    Console.ForegroundColor = color;
    
    Console.SetCursorPosition(x*3, y);
    string cellContent = cell switch {
        "joueur" => " █ ",
        "vide" => "   ",
        "clé" => " ⌐ ",
        "enemmi" => " Ö ",
        _ => cell
    };
    Console.Write(cellContent);
    Console.SetCursorPosition(cursorLeft, cursorTop);

    if (!isUI) labyrinthe[y, x] = cell;

    Console.BackgroundColor = originalBG;
    Console.ForegroundColor = originalFG;
}

void MoveDown() {
    int x = playerPos[0];
    int y = playerPos[1];

    if (y + 1 < sizeY) {
        if (canStepOn(labyrinthe[y + 1, x])) {
            if (labyrinthe[y + 1, x] == "clé") {
                AddKey();
            }

            UpdateCell(x, y, "vide", color, bgColor);
            UpdateCell(x, y + 1, "joueur", color, bgColor);
            playerPos[1]++;
        }
    }
}

void MoveUp() {
    int x = playerPos[0];
    int y = playerPos[1];

    if (y - 1 >= 0) {
        if (canStepOn(labyrinthe[y - 1, x])) {
            if (labyrinthe[y - 1, x] == "clé") {
                AddKey();
            }

            UpdateCell(x, y, "vide", color, bgColor);
            UpdateCell(x, y - 1, "joueur", color, bgColor);
            playerPos[1]--;
        }
    }
}

void MoveLeft() {
    int x = playerPos[0];
    int y = playerPos[1];

    if (x - 1 >= 0) {
        if (canStepOn(labyrinthe[y, x - 1])) {
            if (labyrinthe[y, x - 1] == "clé") {
                AddKey();
            }

            UpdateCell(x, y, "vide", color, bgColor);
            UpdateCell(x - 1, y, "joueur", color, bgColor);
            playerPos[0]--;
        }
    }
}

void MoveRight() {
    int x = playerPos[0];
    int y = playerPos[1];

    if (x + 1 < sizeX ){
       if (canStepOn(labyrinthe[y, x + 1])) {
            if (labyrinthe[y, x + 1] == "clé") {
                AddKey();
            }

            UpdateCell(x, y, "vide", color, bgColor);
            UpdateCell(x + 1, y, "joueur", color, bgColor);
            playerPos[0]++;
        }
    } else if (x == sizeX - 1 && y == sizeY - 1 && keys == 4) { //Victory
        UpdateCell(x, y, "vide", color, bgColor);
        UpdateCell(x + 2, y, "joueur", color, bgColor, true);
        ReplaceInputMessage();
        PlayVictorySong();
        Environment.Exit(0);
    }
}

void AddKey() {
    keys++;
    UpdateCell(sizeX + 1, 1, $"Clés : {keys}", color, bgColor, true);
    PlayGetKeySound();

    if (keys == 4) {
        OpenExit();    
    }
}

void OpenExit() {
    UpdateCell(sizeX, 9, "▓▓", ConsoleColor.Yellow, ConsoleColor.Black, true);
    PlayOpenDoorSound();
    
    UpdateCell(sizeX, 9, "     -> Exit", color, bgColor, true);
}

Boolean canStepOn(string cellType) {
    if (cellType == "vide" || cellType == "clé") return true;
    return false;
}

void Draw() {
    Console.Clear();
    for (int i = 0; i < sizeX; i++) {
        for (int j = 0; j < sizeY; j++) {
            cell = labyrinthe[i, j] switch {
                "joueur" => " █ ",
                "vide" => "   ",
                "clé" => " ⌐ ",
                "enemmi" => " Ö ",
                _ => labyrinthe[i, j]
            };

            if (labyrinthe[i, j] == "clé") {
                Console.ForegroundColor = ConsoleColor.Yellow;
            } else if (labyrinthe[i, j] == "enemmi") {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.Write(cell);

            if (j == sizeX - 1) {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("▓▓\n");
            }

            Console.ForegroundColor = color;
        }
    }
    for (int i = 0; i < sizeX * 3 + 2; i++) {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("▓");
    }
    Console.ForegroundColor = color;

    UpdateCell(sizeX, 9, "▓▓", ConsoleColor.DarkRed, ConsoleColor.Black, true);
    Console.WriteLine($"\n\nPosition joueur : {playerPos[0]}, {playerPos[1]}");
    Console.WriteLine("\nUtilise les flèches du clavier pour bouger, ESC pour sortir :");
}

void UpdatePlayerPos() {
    UpdateCell(0, 12, $"Position joueur : {playerPos[0]}, {playerPos[1]}", color, bgColor, true);
}

void ReplaceInputMessage() {
    UpdateCell(0, 14, "Félicitations et merci d'avoir joué !                       ", ConsoleColor.Magenta, bgColor, true);
}

void PlayGetKeySound() {
    Console.Beep(800, 25);
    Console.Beep(1400, 100);
}

void PlayOpenDoorSound() {
    for (int i = 0; i < 15; i++) {
        Console.Beep(300, 25);
    }
}

void PlayVictorySong() {
    Console.Beep(130, 100);
    Console.Beep(262, 100);
    Console.Beep(330, 100);
    Console.Beep(392, 100);
    Console.Beep(523, 100);
    Console.Beep(660, 100);
    Console.Beep(784, 300);
    Console.Beep(660, 300);
    Console.Beep(146, 100);
    Console.Beep(262, 100);
    Console.Beep(311, 100);
    Console.Beep(415, 100);
    Console.Beep(523, 100);
    Console.Beep(622, 100);
    Console.Beep(831, 300);
    Console.Beep(622, 300);
    Console.Beep(155, 100);
    Console.Beep(294, 100);
    Console.Beep(349, 100);
    Console.Beep(466, 100);
    Console.Beep(588, 100);
    Console.Beep(699, 100);
    Console.Beep(933, 300);
    Console.Beep(933, 100);
    Console.Beep(933, 100);
    Console.Beep(933, 100);
    Console.Beep(1047, 400);
}