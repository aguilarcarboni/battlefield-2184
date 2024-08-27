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
        
    }
    static void InitializeGrid() {
        for (int i = 0; i < GRID_SIZE; i++) {
            for (int j = 0; j < GRID_SIZE; j++) {
                grid[i, j] = 0;
            }
        }
    }
}