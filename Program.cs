using System;

static class Program {

    // Constants
    static bool development = true;

    // Initialize grid
    const int GRID_SIZE = 10;
    static int[,] grid = new int[GRID_SIZE, GRID_SIZE];

    // Initialize game
    const int NUM_SHIPS = 3;
    static int shipsRemaining = NUM_SHIPS;

    const int MAX_GUESSES = 20;
    static int guessesLeft = MAX_GUESSES;

    // Initialize ships
    static int[] shipSizes = { 2, 3, 4 };

    // Game class methods
    static void Main(string[] args) {

        InitializeGrid();
        PlaceShips(grid);
        
        while (shipsRemaining > 0 && guessesLeft > 0) {
            Console.WriteLine("Guesses left: " + guessesLeft + '\n');
            PrintGrid(grid);
            int[] guess = GetGuess();
            guessesLeft--;
            ProcessGuess(grid, guess[0], guess[1]);
        }
        
        if (guessesLeft == 0) {
            Console.WriteLine("You've run out of guesses! Better luck next time!");
        }
        else {
            Console.WriteLine("Congratulations! You've sunk all the ships!");
        }
    }
    static void PlaceShips(int[,] grid) {
        Random random = new Random();
        
        for (int i = 0; i < NUM_SHIPS; i++) {
            bool placed = false;
            while (!placed) {

                // Place ship in random location
                int row = random.Next(GRID_SIZE);
                int col = random.Next(GRID_SIZE);
                bool horizontal = random.Next(2) == 0;
                
                // Do I need to pass grid? How to handle as class?
                if (CanPlaceShip(grid, row, col, shipSizes[i], horizontal)) {
                    PlaceShip(grid, row, col, shipSizes[i], horizontal);
                    placed = true;
                }
            }
        }
    }
    static int[] GetGuess() {
        int row, col;
        do {
            Console.Write("Enter row (0-9): ");
            row = int.Parse(Console.ReadLine());
            Console.Write("Enter column (0-9): ");
            col = int.Parse(Console.ReadLine());
        } while (row < 0 || row >= GRID_SIZE || col < 0 || col >= GRID_SIZE);
        
        return new int[] { row, col };
    }
    static void ProcessGuess(int[,] grid, int row, int col) {
        if (grid[row, col] > 0) {
            Console.WriteLine("Hit!");
            grid[row, col] = -grid[row, col];
            if (IsShipSunk(grid, grid[row, col])) {
                Console.WriteLine($"You've sunk a ship of size {-grid[row, col]}!");
                shipsRemaining--;
            }
        }
        else if (grid[row, col] == 0) {
            Console.WriteLine("Miss!");
            grid[row, col] = -9;
        }
        else {
            Console.WriteLine("You've already guessed this location!");
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
    static void InitializeGrid() {
        for (int i = 0; i < GRID_SIZE; i++) {
            for (int j = 0; j < GRID_SIZE; j++) {
                grid[i, j] = 0;
            }
        }
    }
    static void PrintGrid(int[,] grid) {
        Console.WriteLine("  0 1 2 3 4 5 6 7 8 9");
        for (int i = 0; i < GRID_SIZE; i++) {
            Console.Write(i + " ");
            for (int j = 0; j < GRID_SIZE; j++)
            {
                if (grid[i, j] < 0) {
                    Console.Write("X ");
                }
                else if (grid[i, j] > 0 && development) {
                    Console.Write(grid[i, j] + " ");
                }
                else if (grid[i, j] == -9) {
                    Console.Write("O ");
                }
                else {
                    Console.Write(". ");
                }
            }
            Console.WriteLine();
        }
    }

    // Ship class methods
    static void PlaceShip(int[,] grid, int row, int col, int size, bool horizontal) {
        if (horizontal) {
            for (int i = 0; i < size; i++) {
                grid[row, col + i] = size;
            }
        }
        else {
            for (int i = 0; i < size; i++) {
                grid[row + i, col] = size;
            }
        }
    }
    static bool CanPlaceShip(int[,] grid, int row, int col, int size, bool horizontal) {
        if (horizontal) {
            if (col + size > GRID_SIZE) return false;
            for (int i = 0; i < size; i++) {
                if (grid[row, col + i] != 0) return false;
            }
        }
        else {
            if (row + size > GRID_SIZE) return false;
            for (int i = 0; i < size; i++) {
                if (grid[row + i, col] != 0) return false;
            }
        }
        return true;
    }
    
}