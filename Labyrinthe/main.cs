#region variable_declarations
ConsoleKeyInfo cki;
string cell;

int sizeX = 20;
int sizeY = 20;
int keys = 0;
int[] playerPos = new int[] { 0, 0 };
int[,] enemies = new int[,] {
    { 1, 7 },
    { 7, 7 },
    { 11, 4 },
    { 13, 15 },
};
int[] enemyWalkDir;
int[] enemyRebounds;
int reboundLimit = 2;
int exitY = 10;
int neededKeys = 6;
bool userWantsToQuit = false;
bool gameIsFinished = false;

string[,] labyrinthe = new string[,] { //Attention, pour l'accéder il faut faire labyrinthe[y, x]
    { "joueur", "vide", "vide", "vide", "vide", "vide", "vide", "vide", "vide", "vide", "║║║",  "vide", "║║║",  "vide", "║║║",  "vide", "vide", "vide", "vide", "vide" },
    { "═══",    "═══",  "═══",  "═══",  "═══",  "vide", "╔═╦",  "═══",  "╦═╗",  "vide", "║║║",  "vide", "╚═╝",  "vide", "║║║",  "vide", "╔═╦",  "═══",  "═══",  "vide" },
    { "vide",   "vide", "vide", "vide", "vide", "vide", "║║║",  "vide", "║║║",  "vide", "║║║",  "vide", "vide", "vide", "║║║",  "vide", "║║║",  "vide", "vide", "vide" },
    { "═══",    "═══",  "═══",  "vide", "╔═╦",  "═══",  "╣║║",  "vide", "║║║",  "vide", "╚═╝",  "vide", "═══",  "═══",  "╩═╝",  "vide", "║║╠",  "═══",  "═══",  "vide" },
    { "vide",   "vide", "vide", "vide", "║║║",  "vide", "║║║",  "vide", "║║║",  "vide", "vide", "vide", "vide", "vide", "vide", "vide", "║║║",  "clé", "vide", "vide" },
    { "╔═╗",    "vide", "╔═╗",  "vide", "╚═╝",  "vide", "║║║",  "vide", "╚═╩",  "═══",  "╦═╗",  "vide", "═══",  "═══",  "╦═╗",  "vide", "║║╠",  "═══",  "═══",  "═══" },
    { "║║║",    "vide", "║║║",  "vide", "vide", "vide", "║║║",  "vide", "vide", "clé",  "║║║",  "vide", "vide", "vide", "║║║",  "vide", "║║║",  "vide", "vide", "vide" },
    { "╚═╝",    "vide", "╣║╠",  "═══",  "═══",  "vide", "║║║",  "vide", "╔═╦",  "═══",  "╣║║",  "vide", "╔═╦",  "═══",  "╣║║",  "vide", "║║║",  "vide", "╔═╗",  "vide" },
    { "vide",   "vide", "║║║",  "vide", "vide", "vide", "║║║",  "vide", "║║║",  "vide", "║║║",  "vide", "║║║",  "vide", "║║║",  "vide", "║║║",  "vide", "║║║",  "vide" },
    { "═══",    "vide", "║║╠",  "═══",  "═══",  "vide", "║║║",  "vide", "║║║",  "vide", "╚═╝",  "vide", "╚═╝",  "vide", "╚═╩",  "═══",  "╩═╝",  "vide", "╚═╩",  "═══" },
    { "clé",    "vide", "║║║",  "vide", "vide", "vide", "║║║",  "vide", "║║║",  "vide", "vide", "vide", "vide", "vide", "vide", "vide", "vide", "vide", "vide", "vide" },
    { "╔═╦",    "═══",  "╣║║",  "vide", "╔═╦",  "═══",  "╩═╝",  "vide", "║║║",  "vide", "═══",  "═══",  "╦═╦",  "═══",  "═══",  "vide", "╔═╗",  "vide", "╔═╦",  "═══" },
    { "║║║",    "vide", "║║║",  "vide", "║║║",  "vide", "vide", "vide", "║║║",  "vide", "vide", "vide", "║║║",  "vide", "vide", "vide", "║║║",  "vide", "║║║",  "vide" },
    { "║║║",    "vide", "╚═╝",  "vide", "╚═╝",  "vide", "╔═╗",  "vide", "║║║",  "vide", "╔═╗",  "vide", "║║╠",  "═══",  "═══",  "═══",  "╩═╝",  "vide", "╚═╝",  "vide" },
    { "║║║",    "vide", "vide", "vide", "vide", "vide", "║║║",  "vide", "║║║",  "vide", "║║║",  "vide", "║║║",  "vide", "vide", "vide", "vide", "vide", "vide", "vide" },
    { "║║╠",    "═══",  "═══",  "vide", "╔═╗",  "vide", "║║╠",  "═══",  "╩═╝",  "vide", "╚═╩",  "═══",  "╣║╠",  "═══",  "═══",  "vide", "═══",  "═══",  "═══",  "═══" },
    { "║║║",    "vide", "vide", "vide", "║║║",  "vide", "║║║",  "vide", "vide", "vide", "vide", "vide", "║║║",  "vide", "vide", "vide", "vide", "vide", "vide", "vide" },
    { "╚═╝",    "vide", "╔═╗",  "vide", "║║╠",  "═══",  "╩═╩",  "═══",  "═══",  "vide", "╔═╗",  "vide", "║║║",  "vide", "╔═╦",  "═══",  "═══",  "vide", "╔═╗",  "vide" },
    { "clé",    "vide", "║║║",  "vide", "║║║",  "clé",  "vide", "vide", "vide", "vide", "║║║",  "vide", "║║║",  "vide", "║║║",  "vide", "vide", "vide", "║║║",  "vide" },
    { "╔═╗",    "vide", "║║║",  "vide", "║║╠",  "═══",  "═══",  "═══",  "═══",  "═══",  "╣║╠",  "═══",  "╣║╠",  "═══",  "╣║║",  "clé", "╔═╗",  "vide", "║║║",  "vide" }
};
string[,] originalLabyrinthe = labyrinthe.Clone() as string[,];
int[,] originalEnemies = enemies.Clone() as int[,];
CancellationTokenSource cts;

ConsoleColor color = Console.ForegroundColor;
ConsoleColor bgColor = Console.BackgroundColor;
#endregion

Setup();

while (!userWantsToQuit) {
    cki = Console.ReadKey();
    if (cki.Key == ConsoleKey.Escape) userWantsToQuit = true;
    HandleInput(cki.Key);
}

#region Labyrinthe
void Setup() {
    Console.Clear();
    ResetWalkDirs();
    ResetRebounds();

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

    UpdateCell(sizeX, exitY, "▓▓", ConsoleColor.DarkRed, ConsoleColor.Black, true);
    Console.WriteLine($"\n\nPosition joueur : {playerPos[0]}, {playerPos[1]}");
    //UpdateCell(0, sizeY + 3, $"Position ennemi : {enemies[0, 0]}, {enemies[0, 1]}", color, bgColor, true);
    UpdateCell(0, sizeY + 3 + enemies.GetUpperBound(0) + 1, $"Utilise les flèches du clavier pour bouger, ESC pour sortir :", color, bgColor, true);
    cts = new CancellationTokenSource();
    MoveLoop(cts.Token);
    gameIsFinished = false;
}

string DecodeCell(string cell) => cell switch {
    "joueur" => " █ ",
    "vide" => "   ",
    "clé" => " ⌐ ",
    "ennemi" => " Ö ",
    _ => cell
};

void UpdateCell(int x, int y, string cell, ConsoleColor color, ConsoleColor bgColor, bool isUI = false) {
    (int cursorLeft, int cursorTop) = Console.GetCursorPosition();
    ConsoleColor originalBG = Console.BackgroundColor;
    ConsoleColor originalFG = Console.ForegroundColor;

    Console.BackgroundColor = bgColor;
    Console.ForegroundColor = color;

    Console.SetCursorPosition(x * 3, y);
    string cellContent = DecodeCell(cell);
    Console.Write(cellContent);
    Console.SetCursorPosition(cursorLeft, cursorTop);

    if (!isUI)
        labyrinthe[y, x] = cell;

    Console.BackgroundColor = originalBG;
    Console.ForegroundColor = originalFG;
}

string GetCell(int x, int y) {
    return labyrinthe[y, x];
}

Boolean IsWalkable(string cellType) {
    if (cellType == "vide" || cellType == "clé" || cellType == "joueur" || cellType == "ennemi")
        return true;
    return false;
}

void CheckAndGrabKey(int x, int y) {
    if (GetCell(x, y) == "clé") {
        AddKey();
    }
}
#endregion
#region EnemyMovement
void MoveEnemy(int enemyNum) {
    int x = enemies[enemyNum, 0];
    int y = enemies[enemyNum, 1];

    //Check possible moves
    bool[] nwse = new bool[] {
            (y - 1 >= 0) && IsWalkable(GetCell(x, y - 1)) && GetCell(x, y - 1) != "clé",
            (x - 1 >= 0) && IsWalkable(GetCell(x - 1, y)) && GetCell(x - 1, y) != "clé",
            (y + 1 < sizeY) && IsWalkable(GetCell(x, y + 1)) && GetCell(x, y + 1) != "clé",
            (x + 1 < sizeX) && IsWalkable(GetCell(x + 1, y)) && GetCell(x + 1, y) != "clé"
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
    if (possibleMoves == 1) { //Si on est dans un coin sans sortie
        enemyRebounds[enemyNum]++;
    }

    int chosenDirection;
    bool IsGoodDir = Array.Exists(directions, checkDir => checkDir == enemyWalkDir[enemyNum]); //Vous m'excuserez !

    if (enemyRebounds[enemyNum] < reboundLimit && possibleMoves < 3 && IsGoodDir) {
        chosenDirection = enemyWalkDir[enemyNum];
    } else {
        Random rand = new Random();
        chosenDirection = directions[rand.Next(0, possibleMoves)];
        enemyRebounds[enemyNum] = 0;
    }

    enemyWalkDir[enemyNum] = chosenDirection;

    switch (chosenDirection) {
        case 0:
            if (GetCell(x, y - 1) == "joueur") DisplayGameOverAndFinish();
            enemies[enemyNum, 1]--;
            UpdateCell(x, y, "vide", color, bgColor);
            UpdateCell(x, y - 1, "ennemi", ConsoleColor.Red, bgColor);
            break;
        case 1:
            if (GetCell(x - 1, y) == "joueur") DisplayGameOverAndFinish();
            enemies[enemyNum, 0]--;
            UpdateCell(x, y, "vide", color, bgColor);
            UpdateCell(x - 1, y, "ennemi", ConsoleColor.Red, bgColor);
            break;
        case 2:
            if (GetCell(x, y + 1) == "joueur") DisplayGameOverAndFinish();
            enemies[enemyNum, 1]++;
            UpdateCell(x, y, "vide", color, bgColor);
            UpdateCell(x, y + 1, "ennemi", ConsoleColor.Red, bgColor);
            break;
        case 3:
            if (GetCell(x + 1, y) == "joueur")  DisplayGameOverAndFinish();
            enemies[enemyNum, 0]++;
            UpdateCell(x, y, "vide", color, bgColor);
            UpdateCell(x + 1, y, "ennemi", ConsoleColor.Red, bgColor);
            break;
    }
    //UpdateCell(0, sizeY + 3 + enemyNum, $"Position ennemi : {enemies[enemyNum, 0]}, {enemies[enemyNum, 1]}", color, bgColor, true);
}
async Task MoveLoop(CancellationToken cancellationToken) {
    while (!gameIsFinished && !cancellationToken.IsCancellationRequested) {
        for (int enemyNum = 0; enemyNum <= enemies.GetUpperBound(0); enemyNum++) {
            MoveEnemy(enemyNum);
        }
        await Task.Delay(700);
    }
    ResetGameplay();
}
#endregion
#region PlayerMovement
void HandleInput(ConsoleKey key) {
    if (!gameIsFinished) {
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

    if (key == ConsoleKey.R) {
        ResetGameplay();
        Setup();
        return;
    }

    if (key == ConsoleKey.C) {
        cts.Cancel();
        return;
    }
}

void MoveDown() {
    int x = playerPos[0];
    int y = playerPos[1];

    if (y + 1 < sizeY) {
        if (IsWalkable(GetCell(x, y + 1))) {
            CheckAndGrabKey(x, y + 1);
            CheckEnemyAndDie(x, y + 1);

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
        if (IsWalkable(GetCell(x, y - 1))) {
            CheckAndGrabKey(x, y - 1);
            CheckEnemyAndDie(x, y - 1);

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
        if (IsWalkable(GetCell(x - 1, y))) {
            CheckAndGrabKey(x - 1, y);
            CheckEnemyAndDie(x - 1, y);

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
       if (IsWalkable(GetCell(x + 1, y))) {
            CheckAndGrabKey(x + 1, y);
            CheckEnemyAndDie(x + 1, y);

            UpdateCell(x, y, "vide", color, bgColor);
            UpdateCell(x + 1, y, "joueur", color, bgColor);
            playerPos[0]++;
        }
    } else if (x == sizeX - 1 && y == exitY && keys == neededKeys) { //Victory
        UpdateCell(x, y, "vide", color, bgColor);
        UpdateCell(x + 2, y, "joueur", color, bgColor, true);
        ReplaceInputMessage();
        PlayVictorySong();
        gameIsFinished = true;
    }
}
#endregion
#region Gameplay
void ResetWalkDirs() {
    enemyWalkDir = new int[enemies.GetUpperBound(0) + 1];
    for (int enemyNum = 0; enemyNum <= enemies.GetUpperBound(0); enemyNum++) {
        enemyWalkDir[enemyNum] = -1;
    }
}
void ResetRebounds() {
    enemyRebounds = new int[enemies.GetUpperBound(0) + 1];
    for (int enemyNum = 0; enemyNum <= enemies.GetUpperBound(0); enemyNum++) {
        enemyRebounds[enemyNum] = 0;
    }
}

void ResetGameplay() {
    labyrinthe = originalLabyrinthe;
    enemies = originalEnemies;
    playerPos = new int[] { 0, 0 };
    ResetWalkDirs();
    ResetRebounds();
    cts.Cancel();
}

void CheckEnemyAndDie(int x, int y) {
    if (GetCell(x, y) == "ennemi") {
        DisplayGameOverAndFinish();
    }
}

void AddKey() {
    keys++;
    UpdateCell(sizeX + 1, 1, $"Clés : {keys}", color, bgColor, true);
    PlayGetKeySound();

    if (keys == neededKeys) {
        OpenExit();
    }
}

void OpenExit() {
    UpdateCell(sizeX, exitY, "▓▓", ConsoleColor.Yellow, ConsoleColor.Black, true);
    PlayOpenDoorSound();

    UpdateCell(sizeX, exitY, "     -> Exit", color, bgColor, true);
}
#endregion
#region UI
void UpdatePlayerPos() {
    UpdateCell(0, sizeY + 2, $"Position joueur : {playerPos[0]}, {playerPos[1]}", color, bgColor, true);
}

void ReplaceInputMessage() {
    UpdateCell(0, sizeY + 5, "Félicitations et merci d'avoir joué ! Appuyez sur R pour recommencer !", ConsoleColor.Magenta, bgColor, true);
}

void DisplayGameOverAndFinish() {
    gameIsFinished = true;
    UpdateCell(sizeX / 4, 3, "╔═╗ ╔═╗ ╔╦╗ ╔══  ╔═╗ ╦ ╦ ╔══ ╔══╗", ConsoleColor.Red, bgColor, true);
    UpdateCell(sizeX / 4, 4, "║ ╦ ╠═╣ ║ ║ ╠═   ║ ║ ║ ║ ╠═  ╠═╦╝", ConsoleColor.Red, bgColor, true);
    UpdateCell(sizeX / 4, 5, "╚═╝ ╩ ╩ ╩ ╩ ╚══  ╚═╝  ╚╝ ╚══ ╩ ╩═", ConsoleColor.Red, bgColor, true);
    UpdateCell(sizeX / 4, 6, "  Appuyez sur R pour recommencer ", ConsoleColor.Red, bgColor, true);
}
#endregion
#region Sound
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
#endregion