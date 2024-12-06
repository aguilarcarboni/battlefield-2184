using System;

static class Functional {

    static bool development = true;
    static int currentPlayer = 1;

    const int GRID_SIZE = 10;
    static int[,] player1Grid = new int[GRID_SIZE, GRID_SIZE];
    static int[,] player2Grid = new int[GRID_SIZE, GRID_SIZE];

    const int NUM_SHIPS = 3;
    static int player1ShipsRemaining = NUM_SHIPS;
    static int player2ShipsRemaining = NUM_SHIPS;

    const int MAX_GUESSES = 40;
    static int guessesLeft = MAX_GUESSES;


    // Define the shapes of the ships for both players
    static int[][,] shipShapes = new int[][,] {
        new int[,] { {1, 1},
                     {0, 0} },
        
        new int[,] { {0, 1, 0},
                     {0, 1, 1},
                     {1, 0, 0} },
        
        new int[,] { {1, 1, 1, 1},
                     {0, 0, 1, 0} }
    };

    static void MatrixWrite(string text, bool newLine = true) {
        Console.ForegroundColor = ConsoleColor.Green;
        foreach (char c in text) {
            Console.Write(c);
            System.Threading.Thread.Sleep(10);
        }
        if (newLine) Console.WriteLine();
        Console.ResetColor();
    }

    // Change Main to RunGame and make it public
    public static void RunGame() {
        MatrixWrite("Welcome to Battleship!");
        MatrixWrite("Press Enter to continue...");
        Console.ReadLine();

        InitializeGrid(player1Grid);
        InitializeGrid(player2Grid);
        
        PlaceShips(player1Grid);
        PlaceShips(player2Grid);

        while (player1ShipsRemaining > 0 && player2ShipsRemaining > 0 && guessesLeft > 0) {
            Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
            MatrixWrite($"Player {currentPlayer}'s turn");
            MatrixWrite("Guesses left: " + guessesLeft + '\n');
            
            PrintGrid(currentPlayer == 1 ? player2Grid : player1Grid);
            int[] guess = GetGuess();
            guessesLeft--;
            
            if (currentPlayer == 1) {
                ProcessGuess(player2Grid, guess[0], guess[1], ref player2ShipsRemaining);
            } else {
                ProcessGuess(player1Grid, guess[0], guess[1], ref player1ShipsRemaining);
            }
            
            currentPlayer = (currentPlayer == 1) ? 2 : 1; // Switch players
        }
        
        // Game over logic
        if (guessesLeft == 0) {
            MatrixWrite("Game over - You've run out of guesses!\n");
        } else if (player2ShipsRemaining == 0) {
            MatrixWrite("Congratulations! Player 1 wins!");
        } else {
            MatrixWrite("Congratulations! Player 2 wins!");
        }

        System.Threading.Thread.Sleep(5000);
        
        Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
        development = true;
        Console.WriteLine("Final Boards:");
        Console.WriteLine("Player 1's board:");
        PrintGrid(player1Grid);
        System.Threading.Thread.Sleep(5000);

        Console.WriteLine("\nPlayer 2's board:");
        PrintGrid(player2Grid);
        System.Threading.Thread.Sleep(5000);
    }
    static void PlaceShips(int[,] grid) {
        Random random = new Random();
        
        for (int i = 0; i < NUM_SHIPS; i++) {
            bool placed = false;
            int[,] currentShape = shipShapes[i];
            
            while (!placed) {
                int row = random.Next(GRID_SIZE);
                int col = random.Next(GRID_SIZE);
                int rotation = random.Next(4); // 0-3 for rotations
                
                int[,] rotatedShape = RotateShape(currentShape, rotation);
                
                if (CanPlaceShip(grid, row, col, rotatedShape)) {
                    PlaceShip(grid, row, col, rotatedShape);
                    placed = true;
                }
            }
        }
    }
    static int[] GetGuess() {
        int row, col;
        do {
            MatrixWrite("Enter row (0-9): ", false);
            row = int.Parse(Console.ReadLine());
            MatrixWrite("Enter column (0-9): ", false);
            col = int.Parse(Console.ReadLine());
        } while (row < 0 || row >= GRID_SIZE || col < 0 || col >= GRID_SIZE);
        
        return new int[] { row, col };
    }
    static void ProcessGuess(int[,] grid, int row, int col, ref int shipsRemaining) {
        if (grid[row, col] > 0) {
            MatrixWrite("Hit!");
            grid[row, col] = -grid[row, col];
            if (IsShipSunk(grid, grid[row, col])) {
                MatrixWrite($"You've sunk a ship of size {-grid[row, col]}!");
                shipsRemaining--;
            }
        }
        else if (grid[row, col] == 0) {
            MatrixWrite("Miss!");
            grid[row, col] = -9;
        }
        else {
            MatrixWrite("You've already guessed this location!");
        }
    }
    static bool IsShipSunk(int[,] grid, int shipSize) {
        for (int i = 0; i < GRID_SIZE; i++) {
            for (int j = 0; j < GRID_SIZE; j++) {
                if (grid[i, j] == -shipSize) {
                    return false;
                }
            }
        }
        return true;
    }

    // Grid class methods
    static void InitializeGrid(int[,] grid) {
        for (int i = 0; i < GRID_SIZE; i++) {
            for (int j = 0; j < GRID_SIZE; j++) {
                grid[i, j] = 0;
            }
        }
    }
    static void PrintGrid(int[,] grid) {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("  0 1 2 3 4 5 6 7 8 9");
        for (int i = 0; i < GRID_SIZE; i++) {
            Console.Write(i + " ");
            for (int j = 0; j < GRID_SIZE; j++) {
                if (grid[i, j] == -9) {
                    Console.Write("O ");
                }
                else if (grid[i, j] < 0) {
                    if (development) {
                        Console.Write(grid[i, j] + " ");
                    }
                    else {
                        Console.Write("X ");
                    }
                }
                else if (grid[i, j] > 0 && development) {
                    Console.Write(grid[i, j] + " ");
                }
                else {
                    Console.Write(". ");
                }
                System.Threading.Thread.Sleep(5);
            }
            Console.WriteLine();
        }
        Console.ResetColor();
    }

    // Ship class methods
    static void PlaceShip(int[,] grid, int row, int col, int[,] shape) {
        int shipSize = CountShipCells(shape);
        int rows = shape.GetLength(0);
        int cols = shape.GetLength(1);
        
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                if (shape[i, j] == 1) {
                    grid[row + i, col + j] = shipSize;
                }
            }
        }
    }
    static bool CanPlaceShip(int[,] grid, int row, int col, int[,] shape) {
        int rows = shape.GetLength(0);
        int cols = shape.GetLength(1);
        
        // Check if ship fits within grid bounds
        if (row + rows > GRID_SIZE || col + cols > GRID_SIZE) {
            return false;
        }
        
        // Check if space is clear
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                if (shape[i, j] == 1) {
                    // Check surrounding cells (including diagonals)
                    for (int di = -1; di <= 1; di++) {
                        for (int dj = -1; dj <= 1; dj++) {
                            int newRow = row + i + di;
                            int newCol = col + j + dj;
                            
                            if (newRow >= 0 && newRow < GRID_SIZE && 
                                newCol >= 0 && newCol < GRID_SIZE && 
                                grid[newRow, newCol] != 0) {
                                return false;
                            }
                        }
                    }
                }
            }
        }
        return true;
    }

    // New method to rotate shapes
    static int[,] RotateShape(int[,] shape, int rotations) {
        int[,] result = shape;
        for (int r = 0; r < rotations; r++) {
            result = RotateShapeOnce(result);
        }
        return result;
    }

    static int[,] RotateShapeOnce(int[,] shape) {
        int rows = shape.GetLength(0);
        int cols = shape.GetLength(1);
        int[,] rotated = new int[cols, rows];
        
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                rotated[j, rows - 1 - i] = shape[i, j];
            }
        }
        return rotated;
    }

    // New helper method to count ship cells
    static int CountShipCells(int[,] shape) {
        int count = 0;
        int rows = shape.GetLength(0);
        int cols = shape.GetLength(1);
        
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                if (shape[i, j] == 1) count++;
            }
        }
        return count;
    }
 
}