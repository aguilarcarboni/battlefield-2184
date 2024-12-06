using System;

static class Program {
    
    static void Main(string[] args) {

        Console.WriteLine("Choose game mode:");
        Console.WriteLine("1. Functional");
        Console.WriteLine("2. Object-Oriented");
        
        string? choice = Console.ReadLine();
        
        switch (choice) {
            case "1":
                Functional.RunGame();
                break;
            case "2":
                OOP.Game.RunGame();
                break;
            default:
                Console.WriteLine("Invalid choice!");
                break;
        }
    }
}