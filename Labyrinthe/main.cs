using System.Numerics;
using System.Runtime.InteropServices;

ConsoleKeyInfo cki;
int sizeX = 10;
int sizeY = 10;
int[] playerPos = new int[] { 0, 0 };
string[,] labyrinthe = new string[,] { //Attention, pour l'accéder il faut faire labyrinthe[y, x]
    { "joueur", "vide", "║║║", "vide",  "vide", "vide", "vide", "vide", "║║║",  "clé" },
    { "vide",   "═══",  "╩═╝", "vide",  "╔═╦",  "═══",  "╦═╗", "vide",  "╚═╝",  "vide" },
    { "vide",   "vide", "vide", "vide", "╚═╝",  "vide", "╚═╝", "vide",  "vide", "vide" },
    { "═══",    "╦═╗",  "vide", "╔═╗",  "clé",  "vide", "vide", "═══",  "╦═╗",  "vide" },
    { "vide",   "╚═╝",  "vide", "║║║",  "vide", "╔═╗",  "vide","vide",  "║║║",  "vide" },
    { "vide",   "vide", "vide", "╚═╩",  "═══",  "╩═╩",  "╦╦╗", "vide",  "║║║",  "vide" },
    { "vide",   "╔═╗",  "vide", "vide", "vide", "clé",  "║║║", "vide",  "╚═╝",  "vide" },
    { "vide",   "║║║",  "vide", "╔═╦",  "═══",  "╦═╦",  "╬╬╣", "vide",  "vide", "vide" },
    { "vide",   "╚═╝",  "vide", "╚═╝",  "clé",  "║║╠",  "╩═╝", "vide",  "═══",  "═══" },
    { "vide",   "vide", "vide", "vide", "vide", "╚═╝", "vide", "vide",  "vide", "vide" }
};
string cell;
int keys = 0;

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

void UpdateCell(int x, int y, string cell, bool isUI = false) {
    (int cursorLeft, int cursorTop) = Console.GetCursorPosition();
    Console.SetCursorPosition(x*3, y);
    string cellContent = cell switch {
        "joueur" => " █ ",
        "vide" => "   ",
        "clé" => " ⌐ ",
        _ => cell
    };
    Console.Write(cellContent);
    Console.SetCursorPosition(cursorLeft, cursorTop);

    if (!isUI) labyrinthe[y, x] = cell;
}

void MoveDown() {
    int x = playerPos[0];
    int y = playerPos[1];

    if (y + 1 < sizeY) {
        if (canStepOn(labyrinthe[y + 1, x])) {
            if (labyrinthe[y + 1, x] == "clé") {
                AddKey();
            }

            UpdateCell(x, y, "vide");
            UpdateCell(x, y + 1, "joueur");
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

            UpdateCell(x, y, "vide");
            UpdateCell(x, y - 1, "joueur");
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

            UpdateCell(x, y, "vide");
            UpdateCell(x - 1, y, "joueur");
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

            UpdateCell(x, y, "vide");
            UpdateCell(x + 1, y, "joueur");
            playerPos[0]++;
        }
    }
}

void AddKey() {
    keys++;
    UpdateCell(sizeX + 1, 1, $"Clés : {keys}", true);
    Console.Beep();
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
                _ => labyrinthe[i, j]
            };
            Console.Write(cell);

            if (j == sizeX - 1) {
                Console.Write("░░\n");
            }
        }
    }
    for (int i = 0; i < sizeX * 3 + 2; i++) {
        Console.Write("░");
    }
    Console.WriteLine($"\n\nPosition joueur : {playerPos[0]}, {playerPos[1]}");
    Console.WriteLine("\nUtilise les flèches du clavier pour bouger, ESC pour sortir :");
}

void UpdatePlayerPos() {
    UpdateCell(0, 12, $"Position joueur : {playerPos[0]}, {playerPos[1]}", true);
}
