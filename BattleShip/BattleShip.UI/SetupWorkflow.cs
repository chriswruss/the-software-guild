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
    public class SetupWorkflow {
        public static int shipsPlaced;
        public static string[,] placedShipsGrid;
        private const int MAX_NUM_OF_SHIPS = 5;
        private static ShipPlacement shipPlacement;
        private static string playerName;

        public static Board CreateBoard(int playerNumber) {
            shipsPlaced = 0; 
            GetPlayerName(playerNumber);
            Board board = new Board();
            placedShipsGrid = Engine.CreateGridLabels();
            do {
                ConsoleUI.DrawPlayerShips(placedShipsGrid); // Draws the grid with updated locations for placed ships
                if (shipsPlaced == 0) { Engine.ResetAvailableShips(); } // Create the availableShips array
                PlaceShipRequest placeShipRequest = new PlaceShipRequest();
                placeShipRequest.ShipType = ConsoleUI.GetShipType(playerName);

                do {
                    placeShipRequest.Coordinate = ConsoleUI.GetCoordinate("");
                    placeShipRequest.Direction = ConsoleUI.GetShipDirection();
                    shipPlacement = board.PlaceShip(placeShipRequest);
                    if (shipPlacement == ShipPlacement.NotEnoughSpace) { ConsoleUI.PrintError("Not enough space, try again."); }
                    else if (shipPlacement == ShipPlacement.Overlap) { ConsoleUI.PrintError("That's overlapping another ship, try again."); }
                } while (shipPlacement != ShipPlacement.Ok);

                placedShipsGrid = Engine.AddShipToPlayerBoard(placedShipsGrid); // Adds the ship the player just placed to their board
                shipsPlaced++;

                if (shipsPlaced == MAX_NUM_OF_SHIPS) {
                    Engine.ResetAvailableShips();
                    ConsoleUI.DrawPlayerShips(placedShipsGrid);
                    Console.WriteLine($"{playerName}, this is your board.");
                    ConsoleUI.PressEnterToContinue();
                }
            } while (shipsPlaced < MAX_NUM_OF_SHIPS);

            return board;
        }

        // Gets the correct player name from the Engine to use in writing to the console
        private static void GetPlayerName(int playerNumber) {
            switch (playerNumber) {
                case 1:
                    playerName = Engine._playerOneName;
                    break;
                case 2:
                    playerName = Engine._playerTwoName;
                    break;
            }
        }
    }
}
