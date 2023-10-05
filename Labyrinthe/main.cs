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
int[,] enemies = new int[,] {
    { 2, 7 },
    { 7, 7 },
};
int[] enemyWalkDir = new int[] { -1, -1 };

string[,] labyrinthe = new string[,] { //Attention, pour l'accéder il faut faire labyrinthe[y, x]
    { "joueur", "vide", "║║║",    "vide",  "vide", "vide", "vide", "vide", "║║║",  "clé" },
    { "vide",   "═══",  "╩═╝",    "vide",  "╔═╦",  "═══",  "╦═╗", "vide",  "╚═╝",  "vide" },
    { "vide",   "vide", "vide",   "vide",  "╚═╝",  "vide", "╚═╝", "vide",  "vide", "vide" },
    { "═══",    "╦═╗",  "vide",   "╔═╗",   "clé",  "vide", "vide", "═══",  "╦═╗",  "vide" },
    { "vide",   "╚═╝",  "vide",   "║║║",   "vide", "╔═╗",  "vide","vide",  "║║║",  "vide" },
    { "vide",   "vide", "vide",   "╚═╩",   "═══",  "╩═╩",  "╦╦╗", "vide",  "║║║",  "vide" },
    { "vide",   "╔═╗",  "vide",   "vide",  "vide", "clé",  "║║║", "vide",  "╚═╝",  "vide" },
    { "vide",   "║║║",  "ennemi", "╔═╦",   "═══",  "╦═╦",  "╬╬╣", "ennemi","vide", "vide" },
    { "vide",   "╚═╝",  "vide",   "╚═╝",   "clé",  "║║╠",  "╩═╝", "vide",  "═══",  "═══" },
    { "vide",   "vide", "vide",   "vide",  "vide", "╚═╝", "vide", "vide",  "vide", "vide" }
};

ConsoleColor color = Console.ForegroundColor;
ConsoleColor bgColor = Console.BackgroundColor;

Setup();

do {
    cki = Console.ReadKey();
    HandleInput(cki.Key);
} while (cki.Key != ConsoleKey.Escape);

void Setup() {
    Console.Clear();
    for (int y = 0; y < sizeY; y++) {
        for (int x = 0; x < sizeX; x++) {
            cell = DecodeCell(labyrinthe[y, x]);

            if (labyrinthe[y, x] == "clé") {
                Console.ForegroundColor = ConsoleColor.Yellow;
            } else if (labyrinthe[y, x] == "ennemi") {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.Write(cell);

            if (x == sizeX - 1) {
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
    UpdateCell(0, 13, $"Position ennemi : {enemies[0, 0]}, {enemies[0, 1]}", color, bgColor, true);
    Console.WriteLine("\n\nUtilise les flèches du clavier pour bouger, ESC pour sortir :");
}

string DecodeCell(string cell) => cell switch {
    "joueur" => " █ ",
    "vide" => "   ",
    "clé" => " ⌐ ",
    "ennemi" => " Ö ",
    _ => cell
};

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

    for (int enemyNum = 0; enemyNum < enemies.GetLength(0); enemyNum++) {
        MoveEnemy(enemyNum);
    }
}

void UpdateCell(int x, int y, string cell, ConsoleColor color, ConsoleColor bgColor, bool isUI = false) {
    (int cursorLeft, int cursorTop) = Console.GetCursorPosition();
    ConsoleColor originalBG = Console.BackgroundColor;
    ConsoleColor originalFG = Console.ForegroundColor;

    Console.BackgroundColor = bgColor;
    Console.ForegroundColor = color;
    
    Console.SetCursorPosition(x*3, y);
    string cellContent = DecodeCell(cell);
    Console.Write(cellContent);
    Console.SetCursorPosition(cursorLeft, cursorTop);

    if (!isUI) labyrinthe[y, x] = cell;

    Console.BackgroundColor = originalBG;
    Console.ForegroundColor = originalFG;
}

void MoveEnemy(int enemyNum) {
    int x = enemies[enemyNum, 0];
    int y = enemies[enemyNum, 1];
    
    //Check possible moves
    bool[] nwse = new bool[] {
            (y - 1 >= 0) && IsWalkable(labyrinthe[y - 1, x]) && labyrinthe[y - 1, x] != "clé",
            (x - 1 >= 0) && IsWalkable(labyrinthe[y, x - 1]) && labyrinthe[y, x - 1] != "clé",
            (y + 1 < sizeY) && IsWalkable(labyrinthe[y + 1, x]) && labyrinthe[y + 1, x] != "clé",
            (x + 1 < sizeX) && IsWalkable(labyrinthe[y, x + 1]) && labyrinthe[y, x + 1] != "clé"
    };

    int possibleMoves = 0;

    //Extremely inefficient
    for (int i = 0; i < nwse.Length; i++) {
        if (nwse[i]) { possibleMoves++; }
    }
    //And this is why I'd prefer using Lists...
    int[] directions = new int[possibleMoves];

    possibleMoves = 0;
    for (int i = 0; i < nwse.Length; i++) {
        if (nwse[i]) {
            directions[possibleMoves] = i;
            possibleMoves++;
        }
    }

    Random rand = new Random();
    int chosenDirection;
        
    if (Array.Exists(directions, checkDir => checkDir == enemyWalkDir[enemyNum])) {
        chosenDirection = enemyWalkDir[enemyNum];
    } else {
        chosenDirection = directions[rand.Next(0, possibleMoves)];
    }

    enemyWalkDir[enemyNum] = chosenDirection;

    switch (chosenDirection) {
        case 0:
            if (labyrinthe[y - 1, x] == "joueur") DisplayGameOverAndExit();
            enemies[enemyNum, 1]--;
            UpdateCell(x, y, "vide", color, bgColor);
            UpdateCell(x, y - 1, "ennemi", ConsoleColor.Red, bgColor);
            break;
        case 1:
            if (labyrinthe[y, x - 1] == "joueur") DisplayGameOverAndExit();
            enemies[enemyNum, 0]--;
            UpdateCell(x, y, "vide", color, bgColor);
            UpdateCell(x - 1, y, "ennemi", ConsoleColor.Red, bgColor);
            break;
        case 2:
            if (labyrinthe[y + 1, x] == "joueur") DisplayGameOverAndExit();
            enemies[enemyNum, 1]++;
            UpdateCell(x, y, "vide", color, bgColor);
            UpdateCell(x, y + 1, "ennemi", ConsoleColor.Red, bgColor);
            break;
        case 3:
            if (labyrinthe[y, x + 1] == "joueur")  DisplayGameOverAndExit();
            enemies[enemyNum, 0]++;
            UpdateCell(x, y, "vide", color, bgColor);
            UpdateCell(x + 1, y, "ennemi", ConsoleColor.Red, bgColor);
            break;
    }
    UpdateCell(0, 13 + enemyNum, $"Position ennemi : {enemies[enemyNum, 0]}, {enemies[enemyNum, 1]}", color, bgColor, true);
}

void MoveDown() {
    int x = playerPos[0];
    int y = playerPos[1];

    if (y + 1 < sizeY) {
        if (IsWalkable(labyrinthe[y + 1, x])) {
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
        if (IsWalkable(labyrinthe[y - 1, x])) {
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
        if (IsWalkable(labyrinthe[y, x - 1])) {
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
       if (IsWalkable(labyrinthe[y, x + 1])) {
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

Boolean IsWalkable(string cellType) {
    if (cellType == "vide" || cellType == "clé" || cellType == "joueur" || cellType == "ennemi") return true;
    return false;
}

void UpdatePlayerPos() {
    UpdateCell(0, 12, $"Position joueur : {playerPos[0]}, {playerPos[1]}", color, bgColor, true);
}

void ReplaceInputMessage() {
    UpdateCell(0, 15, "Félicitations et merci d'avoir joué !                       ", ConsoleColor.Magenta, bgColor, true);
}

void DisplayGameOverAndExit() {
    UpdateCell(0, 3, "╔═╗ ╔═╗ ╔╦╗ ╔══  ╔═╗ ╦ ╦ ╔══ ╔══╗", ConsoleColor.Red, bgColor, true);
    UpdateCell(0, 4, "║ ╦ ╠═╣ ║ ║ ╠═   ║ ║ ║ ║ ╠═  ╠═╦╝", ConsoleColor.Red, bgColor, true);
    UpdateCell(0, 5, "╚═╝ ╩ ╩ ╩ ╩ ╚══  ╚═╝  ╚╝ ╚══ ╩ ╩═", ConsoleColor.Red, bgColor, true);
    ReplaceInputMessage();
    Environment.Exit(0);
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