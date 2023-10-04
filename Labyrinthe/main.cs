using System.Numerics;
using System.Runtime.InteropServices;

ConsoleKeyInfo cki;
int sizeX = 10;
int sizeY = 10;
int[,] labyrinth;
int[] playerPos = new int[] { 0, 0 }; //y, x
string[,] content = new string[,] {
    { "joueur", "║║║", "vide", "vide", "vide", "vide", "vide", "vide", "vide", "vide" },
    { "vide",   "║║║", "vide", "vide", "vide", "vide", "vide", "vide", "vide", "vide" },
    { "vide",   "║║║", "vide", "vide", "vide", "vide", "vide", "vide", "vide", "vide" },
    { "vide",   "╚═╝", "vide", "vide", "vide", "vide", "vide", "vide", "vide", "vide" },
    { "vide",   "vide", "vide", "vide", "vide", "vide", "vide", "vide", "vide", "vide" },
    { "═══",    "══╗", "vide", "vide", "vide", "vide", "vide", "vide", "vide", "vide" },
    { "vide",   "║║║", "vide", "vide", "vide", "vide", "vide", "vide", "vide", "vide" },
    { "vide",   "║║║", "vide", "vide", "vide", "vide", "vide", "vide", "vide", "vide" },
    { "vide",   "║║║", "vide", "vide", "vide", "vide", "vide", "vide", "vide", "vide" },
    { "vide",   "║║║", "vide", "vide", "vide", "vide", "vide", "vide", "vide", "vide" }
};
string cell;

CreateLabyrinth();

do {
    Draw();
    cki = Console.ReadKey();
    HandleInput(cki.Key);
} while (cki.Key != ConsoleKey.Escape);


void CreateLabyrinth() {
    labyrinth = new int[sizeX, sizeY];
}

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
}

void MoveDown() {
    if (playerPos[1] + 1 < sizeY && content[playerPos[1] + 1, playerPos[0]] == "vide") {
        content[playerPos[1], playerPos[0]] = "vide";
        playerPos[1]++;
        content[playerPos[1], playerPos[0]] = "joueur";
    }
}

void MoveUp() {
    if (playerPos[1] - 1 >= 0 && content[playerPos[1] - 1, playerPos[0]] == "vide") {
        content[playerPos[1], playerPos[0]] = "vide";
        playerPos[1]--;
        content[playerPos[1], playerPos[0]] = "joueur";
    }
}

void MoveLeft() {
    if (playerPos[0] - 1 >= 0 && content[playerPos[1], playerPos[0] - 1] == "vide") {
        content[playerPos[1], playerPos[0]] = "vide";
        playerPos[0]--;
        content[playerPos[1], playerPos[0]] = "joueur";
    }
}

void MoveRight() {
    if (playerPos[0] + 1 < sizeX && content[playerPos[1], playerPos[0] + 1] == "vide") {
        content[playerPos[1], playerPos[0]] = "vide";
        playerPos[0]++;
        content[playerPos[1], playerPos[0]] = "joueur";
    }
}

void Draw() {
    Console.Clear();
    for (int i = 0; i < sizeX; i++) {
        for (int j = 0; j < sizeY; j++) {
            cell = content[i, j] switch {
                "joueur" => "░█░",
                "vide" => "░░░",
                _ => content[i, j]
            };
            Console.Write(cell);

            if (j == sizeX - 1) {
                Console.Write("\n");
            }
        }
    }
    Console.WriteLine($"\nPosition joueur : {playerPos[0]}, {playerPos[1]}");
    Console.WriteLine("\nUtilise les flèches du clavier pour bouger, ESC pour sortir :");
}
