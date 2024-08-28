using System;

static class Program {
    const int GRID_SIZE = 10;
    const int NUM_SHIPS = 3;
    static int[,] grid = new int[GRID_SIZE, GRID_SIZE];
    static int[] shipSizes = { 2, 3, 4 };
    static int shipsRemaining = NUM_SHIPS;

    static bool development = true;

    // Main
    static void Main(string[] args) {

        InitializeGrid();
        PlaceShips(grid);

        PrintGrid(grid);
    }

    // Grid
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

    // Ship
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