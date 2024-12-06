using System;

static class OOP {
    public class Ship {
        public int[,] Shape { get; private set; }
        public int Size { get; private set; }

        public Ship(int[,] shape) {
            Shape = shape;
            Size = CountCells(shape);
        }

        private int CountCells(int[,] shape) {
            int count = 0;
            for (int i = 0; i < shape.GetLength(0); i++) {
                for (int j = 0; j < shape.GetLength(1); j++) {
                    if (shape[i, j] == 1) count++;
                }
            }
            return count;
        }

        public int[,] Rotate(int rotations) {
            int[,] result = Shape;
            for (int r = 0; r < rotations; r++) {
                result = RotateOnce(result);
            }
            return result;
        }

        private int[,] RotateOnce(int[,] shape) {
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
    }

    public class Grid {
        private const int GRID_SIZE = 10;
        private int[,] board;
        public int ShipsRemaining { get; private set; }

        public Grid() {
            board = new int[GRID_SIZE, GRID_SIZE];
            InitializeGrid();
        }

        private void InitializeGrid() {
            for (int i = 0; i < GRID_SIZE; i++) {
                for (int j = 0; j < GRID_SIZE; j++) {
                    board[i, j] = 0;
                }
            }
        }

        public void Print(bool development) {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  0 1 2 3 4 5 6 7 8 9");
            for (int i = 0; i < GRID_SIZE; i++) {
                Console.Write(i + " ");
                for (int j = 0; j < GRID_SIZE; j++) {
                    if (board[i, j] == -9) {
                        Console.Write("O ");
                    }
                    else if (board[i, j] < 0) {
                        if (development) {
                            Console.Write(board[i, j] + " ");
                        }
                        else {
                            Console.Write("X ");
                        }
                    }
                    else if (board[i, j] > 0 && development) {
                        Console.Write(board[i, j] + " ");
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

        public bool CanPlaceShip(int row, int col, int[,] shape) {
            int rows = shape.GetLength(0);
            int cols = shape.GetLength(1);
            
            if (row + rows > GRID_SIZE || col + cols > GRID_SIZE) {
                return false;
            }
            
            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < cols; j++) {
                    if (shape[i, j] == 1) {
                        for (int di = -1; di <= 1; di++) {
                            for (int dj = -1; dj <= 1; dj++) {
                                int newRow = row + i + di;
                                int newCol = col + j + dj;
                                
                                if (newRow >= 0 && newRow < GRID_SIZE && 
                                    newCol >= 0 && newCol < GRID_SIZE && 
                                    board[newRow, newCol] != 0) {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void PlaceShip(int row, int col, Ship ship, int[,] rotatedShape) {
            int rows = rotatedShape.GetLength(0);
            int cols = rotatedShape.GetLength(1);
            
            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < cols; j++) {
                    if (rotatedShape[i, j] == 1) {
                        board[row + i, col + j] = ship.Size;
                    }
                }
            }
            ShipsRemaining++;
        }

        public (bool isHit, bool isSunk) ProcessGuess(int row, int col) {
            if (board[row, col] > 0) {
                int shipSize = board[row, col];
                board[row, col] = -shipSize;
                bool isSunk = IsShipSunk(shipSize);
                if (isSunk) {
                    ShipsRemaining--;
                }
                return (true, isSunk);
            }
            else if (board[row, col] == 0) {
                board[row, col] = -9;
                return (false, false);
            }
            return (false, false);
        }

        private bool IsShipSunk(int shipSize) {
            for (int i = 0; i < GRID_SIZE; i++) {
                for (int j = 0; j < GRID_SIZE; j++) {
                    if (board[i, j] == shipSize) {
                        return false;
                    }
                }
            }
            return true;
        }
    }

    public class Player {
        public string Name { get; private set; }
        public Grid Grid { get; private set; }
        
        public Player(string name) {
            Name = name;
            Grid = new Grid();
        }
    }

    public class Game {
        private Player player1;
        private Player player2;
        private Player currentPlayer;
        private int guessesLeft;
        private bool development;
        private const int MAX_GUESSES = 40;
        private static readonly Ship[] ships;

        static Game() {
            ships = new Ship[] {
                new Ship(new int[,] { {1, 1}, {0, 0} }),
                new Ship(new int[,] { {0, 1, 0}, {0, 1, 1}, {1, 0, 0} }),
                new Ship(new int[,] { {1, 1, 1, 1}, {0, 0, 1, 0} })
            };
        }

        public Game() {
            player1 = new Player("Player 1");
            player2 = new Player("Player 2");
            currentPlayer = player1;
            guessesLeft = MAX_GUESSES;
            development = true;
        }

        public static void RunGame() {
            Game game = new Game();
            game.Start();
        }

        private void Start() {
            MatrixWrite("Welcome to Battleship!");
            MatrixWrite("Press Enter to continue...");
            Console.ReadLine();

            PlaceShipsRandomly(player1.Grid);
            PlaceShipsRandomly(player2.Grid);

            while (player1.Grid.ShipsRemaining > 0 && player2.Grid.ShipsRemaining > 0 && guessesLeft > 0) {
                PlayTurn();
            }

            EndGame();
        }

        private void PlaceShipsRandomly(Grid grid) {
            Random random = new Random();
            
            foreach (Ship ship in ships) {
                bool placed = false;
                while (!placed) {
                    int row = random.Next(10);
                    int col = random.Next(10);
                    int rotation = random.Next(4);
                    
                    int[,] rotatedShape = ship.Rotate(rotation);
                    
                    if (grid.CanPlaceShip(row, col, rotatedShape)) {
                        grid.PlaceShip(row, col, ship, rotatedShape);
                        placed = true;
                    }
                }
            }
        }

        private void PlayTurn() {
            Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
            MatrixWrite($"{currentPlayer.Name}'s turn");
            MatrixWrite("Guesses left: " + guessesLeft + '\n');
            
            Grid targetGrid = (currentPlayer == player1) ? player2.Grid : player1.Grid;
            targetGrid.Print(development);
            
            int[] guess = GetGuess();
            guessesLeft--;
            
            var (isHit, isSunk) = targetGrid.ProcessGuess(guess[0], guess[1]);
            
            if (isHit) {
                Console.WriteLine("Hit!");
                if (isSunk) {
                    Console.WriteLine("You've sunk a ship!");
                }
            } else {
                Console.WriteLine("Miss!");
            }
            
            currentPlayer = (currentPlayer == player1) ? player2 : player1;
        }

        private int[] GetGuess() {
            int row, col;
            do {
                MatrixWrite("Enter row (0-9): ", false);
                row = int.Parse(Console.ReadLine() ?? "0");
                MatrixWrite("Enter column (0-9): ", false);
                col = int.Parse(Console.ReadLine() ?? "0");
            } while (row < 0 || row >= 10 || col < 0 || col >= 10);
            
            return new int[] { row, col };
        }

        private void EndGame() {
            if (guessesLeft == 0) {
                Console.WriteLine("Game over - You've run out of guesses!\n");
            } else if (player2.Grid.ShipsRemaining == 0) {
                Console.WriteLine($"Congratulations! {player1.Name} wins!");
            } else {
                Console.WriteLine($"Congratulations! {player2.Name} wins!");
            }

            System.Threading.Thread.Sleep(1000);
            
            Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
            development = true;
            Console.WriteLine("Final Boards:");
            Console.WriteLine($"{player1.Name}'s board:");
            player1.Grid.Print(development);
            System.Threading.Thread.Sleep(5000);

            Console.WriteLine($"\n{player2.Name}'s board:");
            player2.Grid.Print(development);
            System.Threading.Thread.Sleep(5000);
        }

        private void MatrixWrite(string text, bool newLine = true) {
            Console.ForegroundColor = ConsoleColor.Green;
            foreach (char c in text) {
                Console.Write(c);
                System.Threading.Thread.Sleep(10);
            }
            if (newLine) Console.WriteLine();
            Console.ResetColor();
        }
    }
}