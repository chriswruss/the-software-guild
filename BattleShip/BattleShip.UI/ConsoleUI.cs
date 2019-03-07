using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleShip.BLL.GameLogic;
using BattleShip.BLL.Requests;
using BattleShip.BLL.Responses;
using BattleShip.BLL.Ships;

namespace BattleShip.UI {
    public class ConsoleUI {
        // The following four vars are used to store corresponding data locally so it can be drawn to the board
        public static string uShipDirection; 
        public static string uShipType; 
        public static int uShipTypeSize; 
        public static Coordinate uShipCoordinate; 

        private enum ShipSize {
            Destroyer = 2,
            Cruiser,
            Submarine = 3,
            Battleship,
            Carrier
        }

        public static void DisplaySplashScreen() {
            Console.WriteLine(@"    
                                    ____  ___  ______________    ___________ __  __________ 
                                   / __ )/   |/_  __/_  __/ /   / ____/ ___// / / /  _/ __ \
                                  / __  / /| | / /   / / / /   / __/  \__ \/ /_/ // // /_/ /
                                 / /_/ / ___ |/ /   / / / /___/ /___ ___/ / __  // // ____/ 
                                /_____/_/  |_/_/   /_/ /_____/_____//____/_/ /_/___/_/      

                                                         |__
                                                         |\/
                                                         ---
                                                         / | [
                                                  !      | |||
                                                _/|     _/|-++'
                                            +  +--|    |--|--|_ |-
                                         { /|__|  |/\__|  |--- |||__/
                                        +---------------___[}-_===_.'____                 /\
                                    ____`-' ||___-{]_| _[}-  |     |_[___\==--            \/   _
                     __..._____--==/___]_|__|_____________________________[___\==--____,------' .7
                    |                                                                     BB-61/
                     \_________________________________________________________________________|

");
            Console.WriteLine("Press enter to play!");
            Console.ReadLine();
            Console.Clear();
        }

        public static string GetPlayerName(string playerNumber) {
            string playerName;
            bool isValid = false;

            do {
                Console.Write($"Enter player {playerNumber}'s name: ");
                playerName = Console.ReadLine();
                
                isValid = !string.IsNullOrWhiteSpace(playerName); //if it's null or whitespace it will return true, flip it to set isValid to false

                if (!isValid) { PrintError("Player name cannot be empty or full of spaces."); }
            } while (!isValid);

            return playerName;
        }
        
        public static void PrintError(string message) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
            PressEnterToContinue();
        }

        public static void PrintWhoGoesFirst(string playerName) {
            Console.WriteLine($"{playerName} will go first!");       
        }

        public static void PrintPlayerToPlaceShips(string playerName) {
            Console.WriteLine($"{playerName}, please place your ships.");
        }

        public static string AskUsersToPlayAgain() {
            string input;
            bool isValid;

            do {
                Console.Write("Play again? <yes> <no> : ");
                input = Console.ReadLine();
                input = input.ToLower(); //using ToLower here because we need the format to be all lower case when the method is called
                isValid = input == "yes" || input == "no";
                if (!isValid) {
                    PrintError("Please enter \"yes\" or \"no\"...");
                }
            } while (!isValid);

            return input;
        }

        public static Coordinate GetCoordinate(string playerName) {
            Coordinate coordinate;
            bool isValid;

            do {
                // row and col are initialized at a value that can never be returned by the translator
                int row = -1;
                int col = -1;
                isValid = true; //set to true, will only be set to false if something goes wrong

                //playerName will be passed in as empty if they are placing a ship, it will actually pass a name if they are firing
                //when placing ships, they know it's their turn. When firing, it will print their name so it's clear whose turn it is
                if (playerName != "") { Console.Write($"{playerName}, enter a coordinate to fire at: "); }
                else { Console.Write("Enter a coordinate: "); }
                string input = Console.ReadLine();
                input = input.ToUpper();

                // Check if user entered an empty or full of spaces coordinate. If input is empty, the substrings used when translating rows and cols will throw exceptions
                if (string.IsNullOrWhiteSpace(input)) { isValid = false; }
                else {
                    row = Engine.TranslateRow(input.Substring(0, 1));
                    if (input.Length == 2) { col = Engine.TranslateColumn(input.Substring(1, 1)); } // Checks the col of a two character coordinate
                    else if (input.Length == 3) { col = Engine.TranslateColumn(input.Substring(1, 2)); } // Checks the col of a three character coordinate
                    else { isValid = false; } // Input wasn't a two or three character coordinate
                }
                
                coordinate = new Coordinate(row, col);

                // If isValid didn't get changed to false at any point, check if the coordinate is valid
                if (isValid) { isValid = Engine.IsValidCoordinate(input); } 

                if (!isValid) { PrintError("Invalid coordinate. Usage example \"a1\"."); }
            } while (!isValid);

            uShipCoordinate = coordinate; // Storing a copy of it for use when drawing ships to the board during setup
            return coordinate;
        }

        // Prints the setup board while adding ships
        public static void DrawPlayerShips(string[,] grid) {
            Console.Clear();
            for (int row = 0; row < grid.GetLength(0); row++) {
                for (int col = 0; col < grid.GetLength(1); col++) {
                    Console.Write(grid[row, col].PadLeft(3).PadRight(5) + "|");
                }
                Console.WriteLine();
                Console.WriteLine("".PadLeft(66, '-'));
            }
        }

        // Prints the playing board that shows shot history
        public static void DrawShotsFiredHistory(string[,] grid) {
            Console.Clear();
            for (int row = 0; row < grid.GetLength(0); row++) {
                for (int column = 0; column < grid.GetLength(1); column++) {
                    // If the index contains an H, color it red
                    if (column != 0 && grid[row, column] == "H") {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(grid[row, column].PadLeft(3).PadRight(5));
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("|");
                    }
                    // If the index contains an M, color it yellow
                    else if (grid[row, column] == "M") {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write(grid[row, column].PadLeft(3).PadRight(5));
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("|");
                    }
                    // Else, print an empty space
                    else { Console.Write(grid[row, column].PadLeft(3).PadRight(5) + "|"); }
                }
                Console.WriteLine();
                Console.WriteLine("".PadLeft(66, '-'));
            }
        }

        public static ShipDirection GetShipDirection() {
            ShipDirection direction = new ShipDirection();
            bool isValid;
            string input;

            // Get a string direction, check if it's a valid direction and assign if it is
            do {
                isValid = true;
                Console.Write("Enter a direction (up, down, left, right) : ");
                input = Console.ReadLine();
                input.ToLower(); //Converting to lower so it can easily be check in the following switch
                switch (input) {
                    case "up":
                        direction = ShipDirection.Up;
                        break;
                    case "down":
                        direction = ShipDirection.Down;
                        break;
                    case "left":
                        direction = ShipDirection.Left;
                        break;
                    case "right":
                        direction = ShipDirection.Right;
                        break;
                    default:
                        isValid = false;
                        PrintError("Invalid direction.");
                        break;
                }
            } while (!isValid);

            uShipDirection = input; //Storing a local copy of the direction so it can be drawn to the board
            return direction;
        }

        public static ShipType GetShipType(string playerName) {
            string input;
            ShipType shipType = new ShipType();
            string[] availableShips = Engine.GetAvailableShips();
            int index = -1;
            bool isValid;

            do {
                isValid = true;
                Console.Write($"{playerName}, you have the following ships left to place:");
                for (int i = 0; i < availableShips.Length; i++) {
                    if (availableShips[i] != null) {
                        Console.Write(" " + availableShips[i]);
                    }
                }
                Console.WriteLine();
                Console.Write("Enter a ship type: ");
                input = Console.ReadLine();
                input = input.ToLower();

                // If they entered a valid ship type, get the index of that ship so it can be check if it has already been placed
                switch (input) {
                    case "destroyer":
                        index = 0;
                        break;
                    case "submarine":
                        index = 1;
                        break;
                    case "cruiser":
                        index = 2;
                        break;
                    case "battleship":
                        index = 3;
                        break;
                    case "carrier":
                        index = 4;
                        break;
                    default:
                        isValid = false;
                        PrintError("Invalid ship type, check spelling.");
                        break;
                }

                //Index is initialized at -1, so this won't evaluate to true unless index is changed to a valid index above
                if(index >= 0 && availableShips[index] == null) {
                    PrintError("You already placed that ship.");
                    isValid = false;
                }

             } while (!isValid);

            // Post validation, do all the necessary stuff. 
            // Doing it post validation so new objects and method calls aren't being done wastefully
            switch (input) {
                case "destroyer":
                    shipType = ShipType.Destroyer;
                    Engine.RemoveShipFromAvailable((int)ShipType.Destroyer);
                    uShipTypeSize = (int)ShipSize.Destroyer;
                    break;
                case "submarine":
                    shipType = ShipType.Submarine;
                    Engine.RemoveShipFromAvailable((int)ShipType.Submarine);
                    uShipTypeSize = (int)ShipSize.Submarine;
                    break;
                case "cruiser":
                    shipType = ShipType.Cruiser;
                    Engine.RemoveShipFromAvailable((int)ShipType.Cruiser);
                    uShipTypeSize = (int)ShipSize.Cruiser;
                    break;
                case "battleship":
                    shipType = ShipType.Battleship;
                    Engine.RemoveShipFromAvailable((int)ShipType.Battleship);
                    uShipTypeSize = (int)ShipSize.Battleship;
                    break;
                case "carrier":
                    shipType = ShipType.Carrier;
                    Engine.RemoveShipFromAvailable((int)ShipType.Carrier);
                    uShipTypeSize = (int)ShipSize.Carrier;
                    break;
            }

            uShipType = input; // Storing a local copy of it so it can be drawn to the board whne placing ships
            return shipType;
        }

        // Prints a message to the screen saying if that shot missed, hit, etc
        public static void PrintFireShotStatus(FireShotResponse fireShotResponse) {

            switch (fireShotResponse.ShotStatus.ToString()) {
                case "Invalid":
                    PrintError("SOMEHOW AN INVALID COORDINATE GOT THROUGH");
                    break;
                case "Duplicate":
                    PrintError("You've already fired at that location.");
                    break;
                case "Miss":
                    Console.WriteLine("You missed!");
                    break;
                case "Hit":
                    Console.WriteLine("You hit a ship!");
                    break;
                case "HitAndSunk":
                    Console.WriteLine("You sunk a ship!");
                    break;
                case "Victory":
                    Console.WriteLine("You win!");
                    break;
            }
        }

        // Tells the user to press enter to continue
        public static void PressEnterToContinue() {
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
        }

    } //End of ConsoleUI Class
} //End of Namespace
