using System;
using Monolith.Unity.Examples.TicTacToe.Models;
using Monolith.Unity.Examples.TicTacToe.Views;
using Monolith.Unity.Pooling;
using UnityEngine.UI;

namespace Monolith.Unity.Examples.TicTacToe.Controllers
{

    public sealed class GameController : IDisposable
    {

        private readonly GameModel _model;
        private readonly GameView _view;

        public GameController(GameMode mode, Text turnText, Button[] gridButtons, Button quitButton, Button resetButton, PoolObject xPrefab, PoolObject oPrefab)
        {
            _model = new GameModel(mode, 42069);
            _view = new GameView(_model.Turn, _model.GetTurnObject(_model.Turn), turnText, gridButtons, quitButton, resetButton, xPrefab, oPrefab);
        }

        public void SetButtonsEnabled(bool enabled) => _view.SetButtonsEnabled(enabled);

        public bool Update()
        {
            var stop = false;
            
            bool grid = false;
            var gridX = 0;
            var gridY = 0;

            switch (_model.Turn)
            {
                case GameTurn.None:
                case GameTurn.Player1:
                case GameTurn.Player2:
                    switch (_view.GetLastButtonClick(out gridX, out gridY))
                    {
                        case GameButton.Grid:
                            grid = _model.Turn != GameTurn.None;
                            break;
                        case GameButton.Quit:
                            stop = true;
                            break;
                        case GameButton.Reset:
                            _model.Reset(42069);
                            _view.Reset(_model.Turn, _model.GetTurnObject(_model.Turn));
                            break;
                    }

                    break;
                case GameTurn.Ai:
                    grid = _model.GetRandomSpot(42069, out gridX, out gridY);
                    break;
            }
            
            _view.ClearLastButtonClick();

            if (grid)
            {
                GridObjectType turnObjectType = _model.GetTurnObject(_model.Turn);
                GamePlaceResult result = _model.PlaceObjectAt(gridX, gridY);

                switch (result)
                {
                    case GamePlaceResult.Fail:
                        break;
                    case GamePlaceResult.Success:
                        _view.PlaceAt(gridX, gridY, turnObjectType);
                        _view.SetTurn(_model.Turn, _model.GetTurnObject(_model.Turn));
                        break;
                    case GamePlaceResult.End:
                        _view.PlaceAt(gridX, gridY, turnObjectType);
                        _view.SetWinner(_model.Winner, _model.GetTurnObject(_model.Winner));
                        break;
                }
            }

            return stop;
        }

        public void Dispose() => _view.Dispose();

    }

}