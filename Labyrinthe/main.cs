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
    { 13, 16 },
};

int[] enemyWalkDir;
int[] enemyRebounds;
int reboundLimit = 2;
int exitY = 10;
int neededKeys = 6;
bool userWantsToQuit = false;
bool gameIsFinished = false;
bool keyLock = false;
int[] defaultCursorPos = new int[] { 0, 28 };

string[,] labyrinthe = new string[,] { //Attention, pour l'accéder il faut faire labyrinthe[y, x]
    { "joueur", "vide", "vide", "vide", "vide", "vide", "vide", "vide", "vide", "vide", "║║║",  "vide", "║║║",  "vide", "║║║",  "vide", "vide", "vide", "vide", "vide" },
    { "═══",    "═══",  "═══",  "═══",  "═══",  "vide", "╔═╦",  "═══",  "╦═╗",  "vide", "║║║",  "vide", "╚═╝",  "vide", "║║║",  "vide", "╔═╦",  "═══",  "═══",  "vide" },
    { "vide",   "vide", "vide", "vide", "vide", "vide", "║║║",  "vide", "║║║",  "vide", "║║║",  "vide", "vide", "vide", "║║║",  "vide", "║║║",  "vide", "vide", "vide" },
    { "═══",    "═══",  "═══",  "vide", "╔═╦",  "═══",  "╣║║",  "vide", "║║║",  "vide", "╚═╝",  "vide", "═══",  "═══",  "╩═╝",  "vide", "║║╠",  "═══",  "═══",  "vide" },
    { "vide",   "vide", "vide", "vide", "║║║",  "vide", "║║║",  "vide", "║║║",  "vide", "vide", "vide", "vide", "vide", "vide", "vide", "║║║",  "clé", "vide", "vide" },
    { "╔═╗",    "vide", "╔═╗",  "vide", "╚═╝",  "vide", "║║║",  "vide", "╚═╩",  "═══",  "╦═╗",  "vide", "═══",  "═══",  "╦═╗",  "vide", "║║╠",  "═══",  "═══",  "═══" },
    { "║║║",    "vide", "║║║",  "vide", "vide", "vide", "║║║",  "vide", "vide", "clé",  "║║║",  "vide", "vide", "vide", "║║║",  "vide", "║║║",  "vide", "vide", "vide" },
    { "╚═╝",    "vide", "║║╠",  "═══",  "═══",  "vide", "║║║",  "vide", "╔═╦",  "═══",  "╣║║",  "vide", "╔═╦",  "═══",  "╣║║",  "vide", "║║║",  "vide", "╔═╗",  "vide" },
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

string[,] originalLabyrinthe = Copy2DArray(labyrinthe);
int[,] originalEnemies = Copy2DArray(enemies);

bool[,] needsUpdate = new bool[sizeY, sizeX];


CancellationTokenSource cts;

ConsoleColor color = Console.ForegroundColor;
ConsoleColor bgColor = Console.BackgroundColor;
#endregion

#region Game Loop
Setup();
Update(4);

while (!userWantsToQuit) {
    HandleInput();
}

async Task Update(int refreshRate) {
    while (!gameIsFinished) {
        Render();
        await Task.Delay(refreshRate);
    }
}
#endregion

#region Input
void HandleInput() {
    if (keyLock) return;

    cki = Console.ReadKey();
    ConsoleKey key = cki.Key;
    if (key == ConsoleKey.Escape)
        userWantsToQuit = true;

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

    }

    if (key == ConsoleKey.R) {
        gameIsFinished = true;
        ResetGameplay();
        Setup();
        Update(4);
        return;
    }

    if (key == ConsoleKey.C) {
        cts.Cancel();
        return;
    }
}
#endregion

#region Game Logic

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
void UpdateLabyrinth(int x, int y, string cell, ConsoleColor color, ConsoleColor bgColor) {
    labyrinthe[y, x] = cell;
    needsUpdate[y, x] = true;
}
void ResetGameplay() {
    labyrinthe = Copy2DArray(originalLabyrinthe);
    enemies = Copy2DArray(originalEnemies);
    playerPos = new int[] { 0, 0 };
    keys = 0;
    ResetWalkDirs();
    ResetRebounds();
    cts.Cancel();
}

void CheckEnemyAndDie(int x, int y) {
    if (GetCell(x, y) == "ennemi") {
        PlayDieSound();
        DisplayGameOverAndFinish();
    }
}

void AddKey() {
    keys++;
    DrawMessage(sizeX * 3 + 3, 1, $"Clés : {keys}", color, bgColor);
    PlayGetKeySound();

    if (keys == neededKeys) {
        OpenExit();
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
            CheckPlayerAndKill(x, y - 1);
            enemies[enemyNum, 1]--;
            UpdateLabyrinth(x, y, "vide", color, bgColor);
            UpdateLabyrinth(x, y - 1, "ennemi", ConsoleColor.Red, bgColor);
            break;
        case 1:
            CheckPlayerAndKill(x - 1, y);
            enemies[enemyNum, 0]--;
            UpdateLabyrinth(x, y, "vide", color, bgColor);
            UpdateLabyrinth(x - 1, y, "ennemi", ConsoleColor.Red, bgColor);
            break;
        case 2:
            CheckPlayerAndKill(x, y + 1);
            enemies[enemyNum, 1]++;
            UpdateLabyrinth(x, y, "vide", color, bgColor);
            UpdateLabyrinth(x, y + 1, "ennemi", ConsoleColor.Red, bgColor);
            break;
        case 3:
            CheckPlayerAndKill(x + 1, y);
            enemies[enemyNum, 0]++;
            UpdateLabyrinth(x, y, "vide", color, bgColor);
            UpdateLabyrinth(x + 1, y, "ennemi", ConsoleColor.Red, bgColor);
            break;
    }
    //UpdateCell(0, sizeY + 3 + enemyNum, $"Position ennemi : {enemies[enemyNum, 0]}, {enemies[enemyNum, 1]}", color, bgColor, true);
}

void CheckPlayerAndKill(int x, int y) {
    if (GetCell(x, y) == "joueur") {
        DisplayGameOverAndFinish();
        PlayDieSound();
    }
}

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

async Task MoveLoop(CancellationToken cancellationToken) {
    while (!gameIsFinished && !cancellationToken.IsCancellationRequested) {
        for (int enemyNum = 0; enemyNum <= enemies.GetUpperBound(0); enemyNum++) {
            MoveEnemy(enemyNum);
        }
        PlayStepSound();
        await Task.Delay(700);
    }
}

#endregion

#region PlayerMovement

void MoveDown() {
    int x = playerPos[0];
    int y = playerPos[1];

    if (y + 1 < sizeY) {
        if (IsWalkable(GetCell(x, y + 1))) {
            CheckAndGrabKey(x, y + 1);
            CheckEnemyAndDie(x, y + 1);

            UpdateLabyrinth(x, y, "vide", color, bgColor);
            UpdateLabyrinth(x, y + 1, "joueur", color, bgColor);
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

            UpdateLabyrinth(x, y, "vide", color, bgColor);
            UpdateLabyrinth(x, y - 1, "joueur", color, bgColor);
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

            UpdateLabyrinth(x, y, "vide", color, bgColor);
            UpdateLabyrinth(x - 1, y, "joueur", color, bgColor);
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

            UpdateLabyrinth(x, y, "vide", color, bgColor);
            UpdateLabyrinth(x + 1, y, "joueur", color, bgColor);
            playerPos[0]++;
        }
    } else {
        CheckVictory(x, y);
    }
}

void CheckVictory(int x, int y) {
    if (x == sizeX - 1 && y == exitY && keys >= neededKeys) {
        UpdateLabyrinth(x, y, "vide", color, bgColor);
        DrawMessage(x + 2, y, DecodeCell("joueur"), color, bgColor);
        ReplaceInputMessage();
        PlayVictorySong();
        gameIsFinished = true;
    }
}

#endregion

#region Visuals
void Setup() {
    Console.Clear();
    ResetWalkDirs();
    ResetRebounds();

    for (int y = 0; y < sizeY; y++) {
        for (int x = 0; x < sizeX; x++) {
            cell = DecodeCell(labyrinthe[y, x]);

            Console.ForegroundColor = GetRenderColor(labyrinthe[y, x]);

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

    DrawMessage(sizeX * 3, exitY, "▓▓", ConsoleColor.DarkRed, ConsoleColor.Black);
    DrawMessage(0, sizeY + 3 + enemies.GetUpperBound(0) + 1, $"Utilise les flèches du clavier pour bouger, ESC pour sortir :", color, bgColor);
    cts = new CancellationTokenSource();
    gameIsFinished = false;
    MoveLoop(cts.Token);
}

string DecodeCell(string cell) => cell switch {
    "joueur" => " █ ",
    "vide" => "   ",
    "clé" => " ⌐ ",
    "ennemi" => " Ö ",
    _ => cell
};

void Render() {
    ConsoleColor originalFG = Console.ForegroundColor;
    for (int y = 0; y < sizeY; y++) {
        for (int x = 0; x < sizeX; x++) {
            if (needsUpdate[y, x]) {
                Console.CursorVisible = false;
                Console.SetCursorPosition(x * 3, y);
                cell = labyrinthe[y, x];
                string cellContent = DecodeCell(cell);
                ConsoleColor cellColor = GetRenderColor(cell);
                Console.ForegroundColor = cellColor;
                Console.Write(cellContent);
                Console.ForegroundColor = originalFG;
            }
        }
    }
    Console.CursorVisible = true;
    Console.SetCursorPosition(defaultCursorPos[0], defaultCursorPos[1]);
}

ConsoleColor GetRenderColor(string cell) => cell switch {
    "joueur" => ConsoleColor.Blue,
    "clé" => ConsoleColor.Yellow,
    "ennemi" => ConsoleColor.Red,
    _ => ConsoleColor.White
};

void DrawMessage(int col, int row, string message, ConsoleColor color, ConsoleColor bgColor) {
    (int cursorLeft, int cursorTop) = Console.GetCursorPosition();
    ConsoleColor originalBG = Console.BackgroundColor;
    ConsoleColor originalFG = Console.ForegroundColor;

    Console.BackgroundColor = bgColor;
    Console.ForegroundColor = color;

    Console.SetCursorPosition(col, row);
    Console.Write(message);
    Console.SetCursorPosition(cursorLeft, cursorTop);

    Console.BackgroundColor = originalBG;
    Console.ForegroundColor = originalFG;
}

void UpdatePlayerPos() {
    DrawMessage(0, sizeY + 2, $"Position joueur : {playerPos[0]}, {playerPos[1]}", color, bgColor);
}

void ReplaceInputMessage() {
    DrawMessage(0, sizeY + 5, "Félicitations et merci d'avoir joué ! Appuyez sur R pour recommencer !", ConsoleColor.Magenta, bgColor);
}

void DisplayGameOverAndFinish() {
    gameIsFinished = true;

    LineTypeWriter(sizeX * 3 / 4, 3, "╔═╗ ╔═╗ ╔╦╗ ╔══  ╔═╗ ╦ ╦ ╔══ ╔══╗", ConsoleColor.Red, 5, 0, true);
    LineTypeWriter(sizeX * 3 / 4, 4, "║ ╦ ╠═╣ ║ ║ ╠═   ║ ║ ║ ║ ╠═  ╠═╦╝", ConsoleColor.Red, 5, 700, true);
    LineTypeWriter(sizeX * 3 / 4, 5, "╚═╝ ╩ ╩ ╩ ╩ ╚══  ╚═╝  ╚╝ ╚══ ╩ ╩═", ConsoleColor.Red, 5, 1400, true);
    LineTypeWriter(sizeX * 3 / 4, 6, "  Appuyez sur R pour recommencer ", ConsoleColor.Red, 5, 2100);

    Console.SetCursorPosition(defaultCursorPos[0], defaultCursorPos[1]);
}

void OpenExit() {
    DrawMessage(sizeX * 3, exitY, "▓▓", ConsoleColor.Yellow, ConsoleColor.Black);
    PlayOpenDoorSound();

    DrawMessage(sizeX * 3, exitY, "     -> Exit", color, bgColor);
}

async Task LineTypeWriter(int col, int row, string userString, ConsoleColor color, int delay, int startDelay = 0, bool keepKeylock = false) {
    await Task.Delay(startDelay);
    keyLock = true;

    for (int i = 0; i < userString.Length; i++) {
        (int cursorLeft, int cursorTop) = Console.GetCursorPosition();
        ConsoleColor originalFG = Console.ForegroundColor;
        Console.SetCursorPosition(col, row);
        Console.ForegroundColor = color;
        Console.Write(userString[i]);
        col++;
        Console.SetCursorPosition(cursorLeft, cursorTop);
        Console.ForegroundColor = originalFG;
        await Task.Delay(delay);
    }
    if (keepKeylock == false) {
        keyLock = false;
    }
}

#endregion

#region Sound
async Task PlayGetKeySound() {
    Console.Beep(800, 25);
    Console.Beep(1400, 100);
}

void PlayOpenDoorSound() {
    for (int i = 0; i < 15; i++) {
        Console.Beep(300, 25);
    }
}

async Task PlayStepSound() {
    Console.Beep(160, 25);
}

async Task PlayDieSound() {
    Console.Beep(350, 250);
    Console.Beep(300, 500);
}

void PlayVictorySong() {
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

#region Outils
T[,] Copy2DArray<T>(T[,] originalArray) {
    T[,] copiedArray = new T[originalArray.GetLength(0), originalArray.GetLength(1)];

    for (int i = 0; i < originalArray.GetLength(0); i++) {
        for (int j = 0; j < originalArray.GetLength(1); j++) {
            copiedArray[i, j] = originalArray[i, j];
        }
    }

    return copiedArray;
}
#endregion