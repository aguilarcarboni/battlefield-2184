using System;

static class Program {
    const int GRID_SIZE = 10;
    const int NUM_SHIPS = 3;
    static int[,] grid = new int[GRID_SIZE, GRID_SIZE];
    static int[] shipSizes = { 2, 3, 4 };
    static int shipsRemaining = NUM_SHIPS;

    // Main
    static void Main(string[] args) {

        InitializeGrid();
        PrintGrid();
        
    }
    static void InitializeGrid() {
        for (int i = 0; i < GRID_SIZE; i++) {
            for (int j = 0; j < GRID_SIZE; j++) {
                grid[i, j] = 0;
            }
        }
    }
    static void PrintGrid() {
        Console.WriteLine("  0 1 2 3 4 5 6 7 8 9");
        for (int i = 0; i < GRID_SIZE; i++) {
            Console.Write(i + " ");
            for (int j = 0; j < GRID_SIZE; j++)
            {
                if (grid[i, j] < 0) {
                    Console.Write("X ");
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
}