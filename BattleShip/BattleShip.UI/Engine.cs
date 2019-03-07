using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleShip.BLL.GameLogic;
using BattleShip.BLL.Requests;
using BattleShip.BLL.Responses;
using BattleShip.BLL.Ships;

namespace BattleShip.UI {
    class Engine {
        public static string _playerOneName;
        public static string _playerTwoName;
        public static string[] _availableShips;
        private const int NUMBER_OF_ROWS = 11;
        private const int NUMBER_OF_COLS = 11;
        private const char FIRST_ROW_LETTER = 'A';
        private const char LAST_ROW_LETTER = 'J';

        public static void ResetAvailableShips() {
            _availableShips = new string[5];
            _availableShips[0] = "destroyer";
            _availableShips[1] = "submarine";
            _availableShips[2] = "cruiser";
            _availableShips[3] = "battleship";
            _availableShips[4] = "carrier";
        }

        public static void RemoveShipFromAvailable(int index) {
            _availableShips[index] = null;
        }

        public static string[] GetAvailableShips() {
            return _availableShips;
        }

        public bool ChangeTurns(bool isPlayerOneTurn) {
            return !isPlayerOneTurn;
        }

        public enum GridLetters {
            A = 1,
            B,
            C,
            D,
            E,
            F,
            G,
            H,
            I,
            J,
            BAD
        }

        // Sets font color to white and displays the title screen
        public void Start() {
            Console.ForegroundColor = ConsoleColor.White;
            ConsoleUI.DisplaySplashScreen();
        }
        
        // Gets both player's names
        public void GetPlayersNames() {
            _playerOneName = ConsoleUI.GetPlayerName("one");
            _playerOneName = ConvertNameToProperCase(_playerOneName); // Convert the name to proper case

            _playerTwoName = ConsoleUI.GetPlayerName("two");
            _playerTwoName = ConvertNameToProperCase(_playerTwoName);
        }

        // randomly determine which player goes first
        public static bool ChooseWhoGoesFirst() {
            bool isPlayerOneTurn = false;
            int playerToGoFirst;

            Random random = new Random();
            playerToGoFirst = random.Next(1, 3);

            if (playerToGoFirst == 1) {
                isPlayerOneTurn = true;
                ConsoleUI.PrintWhoGoesFirst(_playerOneName);
            }
            else { ConsoleUI.PrintWhoGoesFirst(_playerTwoName); }

            return isPlayerOneTurn;
        }

        // Ask if they want to play again
        public static bool PlayAgain() {
            bool willPlayAgain = false;

            string answer = ConsoleUI.AskUsersToPlayAgain();

            if (answer == "yes") {
                willPlayAgain = true;
            }

            return willPlayAgain;
        }

        // Takes a letter and turns it into an int
        public static int TranslateRow(string rowLetter) {
            int row = 0;

            switch(rowLetter) {
                case "A":
                    row = (int)GridLetters.A;
                    break;
                case "B":
                    row = (int)GridLetters.B;
                    break;
                case "C":
                    row = (int)GridLetters.C;
                    break;
                case "D":
                    row = (int)GridLetters.D;
                    break;
                case "E":
                    row = (int)GridLetters.E;
                    break;
                case "F":
                    row = (int)GridLetters.F;
                    break;
                case "G":
                    row = (int)GridLetters.G;
                    break;
                case "H":
                    row = (int)GridLetters.H;
                    break;
                case "I":
                    row = (int)GridLetters.I;
                    break;
                case "J":
                    row = (int)GridLetters.J;
                    break;
                default:
                    row = (int)GridLetters.BAD;
                    break;
            }

            return row;
        }

        // Takes a column and if not in range, return -1 which will cause coordinate validation to fail
        public static int TranslateColumn(string column) {
            bool isValid = int.TryParse(column, out int col);
            if (!isValid) { col = -1; }
            return col;
        }

        // Checks that given row is a letter between a and j
        private static bool IsValidRowCoordinate(string coordinate) {
            bool isValid = true;

            if (coordinate.Substring(0, 1) != "A" && coordinate.Substring(0, 1) != "B" && coordinate.Substring(0, 1) != "C" && coordinate.Substring(0, 1) != "D" &&
                coordinate.Substring(0, 1) != "E" && coordinate.Substring(0, 1) != "F" && coordinate.Substring(0, 1) != "G" && coordinate.Substring(0, 1) != "H" &&
                coordinate.Substring(0, 1) != "I" && coordinate.Substring(0, 1) != "J") {
                isValid = false;
            }

            return isValid;
        }

        // Checks that given column is a number between 1 and 10
        private static bool IsValidColumnCoordinate(string coordinate) {
            bool isValid = true;

            if (coordinate.Length > 3) {
                isValid = false;
            }
            else if (coordinate.Length < 2) {
                isValid = false;
            }
            else if (coordinate.Length == 2) {
                if (coordinate.Substring(1, 1) != "1" && coordinate.Substring(1, 1) != "2" && coordinate.Substring(1, 1) != "3" && coordinate.Substring(1, 1) != "4"
                    && coordinate.Substring(1, 1) != "5" && coordinate.Substring(1, 1) != "6" && coordinate.Substring(1, 1) != "7" && coordinate.Substring(1, 1) != "8"
                    && coordinate.Substring(1, 1) != "9") {
                    isValid = false;
                }
            }
            else {
                if (coordinate.Substring(1, 2) != "10") {
                    isValid = false;
                }
            }

            return isValid;
        }

        // Calls IsValidRowCoordinate and IsValidColumnCoordinate and returns whether both are true
        public static bool IsValidCoordinate(string coordinate) {
            return IsValidColumnCoordinate(coordinate) && IsValidRowCoordinate(coordinate);
        }

        // Takes a pre validated direction and draws the ship's abbreviation to the ship placement board
        public static string[,] AddShipToPlayerBoard(string[,] grid) {
            string[,] newGrid = grid;
            int row = ConsoleUI.uShipCoordinate.XCoordinate;
            int col = ConsoleUI.uShipCoordinate.YCoordinate;

            switch (ConsoleUI.uShipDirection) {
                case "down":
                    for (int i = row; i < row + ConsoleUI.uShipTypeSize; i++) {
                        newGrid[i, col] = ChooseAbbreviation(ConsoleUI.uShipType);
                    }
                    break;
                case "up":
                    for (int i = row; i > row - ConsoleUI.uShipTypeSize; i--) {
                        newGrid[i, col] = ChooseAbbreviation(ConsoleUI.uShipType);
                    }
                    break;
                case "right":
                    for (int i = col; i < col + ConsoleUI.uShipTypeSize; i++) {
                        newGrid[row, i] = ChooseAbbreviation(ConsoleUI.uShipType);
                    }
                    break;
                case "left":
                    for (int i = col; i > col - ConsoleUI.uShipTypeSize; i--) {
                        newGrid[row, i] = ChooseAbbreviation(ConsoleUI.uShipType);
                    }
                    break;
            }

            return newGrid;
        }

        // This method is called in AddShipToPlayerBoard, and it takes a full name for a ship and returns a 3 letter abbreviation
        private static string ChooseAbbreviation(string shipType) {
            string abr = "";
            switch (shipType) {
                case "destroyer":
                    abr = "DST".PadLeft(4);
                    break;
                case "submarine":
                    abr = "SUB".PadLeft(4);
                    break;
                case "cruiser":
                    abr = "CSR".PadLeft(4);
                    break;
                case "battleship":
                    abr = "BTL".PadLeft(4);
                    break;
                case "carrier":
                    abr = "CAR".PadLeft(4);
                    break;
            }

            return abr;
        }

        public static string ConvertNameToProperCase(string playerName) {
            TextInfo textInfo = new CultureInfo("en-us", false).TextInfo;
            return textInfo.ToTitleCase(playerName.ToLower());
        }

        // Adds labels (A - J, 1 - 10) to grid
        public static string[,] CreateGridLabels() {
            int index = 1;
            string[,] grid;
            grid = new string[NUMBER_OF_ROWS, NUMBER_OF_COLS];
            grid[0, 0] = " ";
            for (int col = 1; col < NUMBER_OF_COLS; col++) {
                grid[0, col] = col.ToString();
            }
            for (char row = FIRST_ROW_LETTER; row <= LAST_ROW_LETTER; row++) {
                grid[index, 0] = row.ToString();
                index++;
            }
            for (int row = 1; row < NUMBER_OF_ROWS; row++) {
                for (int col = 1; col < NUMBER_OF_COLS; col++) {
                    grid[row, col] = " ";
                }
            }

            return grid;
        }
    }
}
