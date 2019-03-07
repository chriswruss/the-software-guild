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
    public class GameWorkflow {
        Engine _engine;
        private bool _isPlayerOneTurn;
        static Board playerOneBoard;
        static Board playerTwoBoard;
        private static string[,] _playerOneFiredShotsGrid;
        private static string[,] _playerTwoFiredShotsGrid;
        private static FireShotResponse fireShotResponse;
        private static Coordinate _fireShotCoordinate;

        public void PlayGame() {
            StartEngine();
            _engine.GetPlayersNames();

            do {
                CreateBoards();
                LabelGrids();
                _isPlayerOneTurn = Engine.ChooseWhoGoesFirst();
                ConsoleUI.PressEnterToContinue();
                do {
                    if (_isPlayerOneTurn) { TakeTurn((int)Players.PlayerOne, playerTwoBoard, _playerOneFiredShotsGrid, Engine._playerOneName); }
                    else { TakeTurn((int)Players.PlayerTwo, playerOneBoard, _playerTwoFiredShotsGrid, Engine._playerTwoName); }
                    _isPlayerOneTurn = _engine.ChangeTurns(_isPlayerOneTurn);
                } while (fireShotResponse.ShotStatus.ToString() != "Victory");
            } while (Engine.PlayAgain());
        }

        private void StartEngine() {
            _engine = new Engine();
            _engine.Start();
        }

        private enum Players {
            PlayerOne = 1,
            PlayerTwo
        }

        // Adds an H or an M to the board 
        private static string[,] AddShotToPlayerGrid(FireShotResponse fireShotResponse, string[,] playerGrid, Coordinate coordinate) {
            if (fireShotResponse.ShotStatus.ToString() == "Hit" || fireShotResponse.ShotStatus.ToString() == "HitAndSunk") { playerGrid[coordinate.XCoordinate, coordinate.YCoordinate] = "H"; }
            if (fireShotResponse.ShotStatus.ToString() == "Miss") { playerGrid[coordinate.XCoordinate, coordinate.YCoordinate] = "M"; }

            return playerGrid;
        }

        // Code that gets executed to start a player's turn
        private static void TakeTurn(int playerNumber, Board opponentBoard, string[,] firedShotsGrid, string playerName) {
            ConsoleUI.DrawShotsFiredHistory(firedShotsGrid); // Draws the shot history grid to the console
            do {
                _fireShotCoordinate = ConsoleUI.GetCoordinate(playerName);
                fireShotResponse = opponentBoard.FireShot(_fireShotCoordinate);
                ConsoleUI.PrintFireShotStatus(fireShotResponse);
            } while (fireShotResponse.ShotStatus.ToString() == "Invalid" || fireShotResponse.ShotStatus.ToString() == "Duplicate");

            // Add the shot to the grid
            if (playerNumber == 1) { _playerOneFiredShotsGrid = AddShotToPlayerGrid(fireShotResponse, _playerOneFiredShotsGrid, _fireShotCoordinate); }
            if (playerNumber == 2) { _playerTwoFiredShotsGrid = AddShotToPlayerGrid(fireShotResponse, _playerTwoFiredShotsGrid, _fireShotCoordinate); }

            ConsoleUI.PressEnterToContinue();
        }

        // Creates both player's boards
        private static void CreateBoards() {
            playerOneBoard = SetupWorkflow.CreateBoard((int)Players.PlayerOne);
            playerTwoBoard = SetupWorkflow.CreateBoard((int)Players.PlayerTwo);
        }

        // Gives the grids labels i.e. A - J and 1 - 10
        private static void LabelGrids() {
            _playerOneFiredShotsGrid = Engine.CreateGridLabels();
            _playerTwoFiredShotsGrid = Engine.CreateGridLabels();
        }
    }
}
