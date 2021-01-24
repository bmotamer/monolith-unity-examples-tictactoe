using System;
using Monolith.Unity.Examples.TicTacToe.Models;
using Monolith.Unity.Pooling;
using UnityEngine;
using UnityEngine.UI;

namespace Monolith.Unity.Examples.TicTacToe.Views
{

    public sealed class GameView : IDisposable
    {

        private readonly Text _turnText;
        private readonly Button[] _gridButtons;
        private readonly Button _quitButton;
        private readonly Button _resetButton;
        private Button _buttonClicked;
        private readonly Pool _xPool;
        private readonly Pool _oPool;

        public GameView(GameTurn turn, GridObjectType turnObjectType, Text turnText, Button[] gridButtons, Button quitButton, Button resetButton, PoolObject xPrefab, PoolObject oPrefab)
        {
            _turnText = turnText;
            _gridButtons = gridButtons;
            _quitButton = quitButton;
            _resetButton = resetButton;
            
            int poolSize = Mathf.CeilToInt(_gridButtons.Length / 2.0F);

            _xPool = new Pool(xPrefab);
            _xPool.Instantiate(poolSize);

            _oPool = new Pool(oPrefab);
            _oPool.Instantiate(poolSize);

            Reset(turn, turnObjectType);
        }

        public void Reset(GameTurn turn, GridObjectType turnObjectType)
        {
            while (_xPool.OldestActive) _xPool.OldestActive.Despawn();
            while (_oPool.OldestActive) _oPool.OldestActive.Despawn();

            SetTurn(turn, turnObjectType);
        }

        public void SetButtonsEnabled(bool enabled)
        {
            if (enabled)
            {
                foreach (Button gridButton in _gridButtons)
                {
                    gridButton.onClick.AddListener(() => _buttonClicked = gridButton);
                }
                
                _quitButton.onClick.AddListener(() => _buttonClicked = _quitButton);
                _resetButton.onClick.AddListener(() => _buttonClicked = _resetButton);
            }
            else
            {
                foreach (Button gridButton in _gridButtons)
                {
                    gridButton.onClick.RemoveAllListeners();
                }
                
                _quitButton.onClick.RemoveAllListeners();
                _resetButton.onClick.RemoveAllListeners();
            }

            _buttonClicked = null;
        }

        public void PlaceAt(int x, int y, GridObjectType objectType)
        {
            Pool poolToUse;

            switch (objectType)
            {
                case GridObjectType.X:
                    poolToUse = _xPool;
                    break;
                case GridObjectType.O:
                    poolToUse = _oPool;
                    break;
                default:
                    poolToUse = null;
                    break;
            }
                    
            PoolObject objectPlaced = poolToUse.Spawn();

            objectPlaced.transform.SetParent(_gridButtons[x + y * 3].transform, false);
        }

        public GameButton GetLastButtonClick(out int x, out int y)
        {
            GameButton button = GameButton.None;
            
            x = 0;
            y = 0;
            
            if (_buttonClicked)
            {
                if (_buttonClicked == _quitButton)
                {
                    button = GameButton.Quit;
                }
                else if (_buttonClicked == _resetButton)
                {
                    button = GameButton.Reset;
                }
                else
                {
                    int gridButtonIndex = Array.IndexOf(_gridButtons, _buttonClicked);

                    if (gridButtonIndex != -1)
                    {
                        button = GameButton.Grid;
                    
                        x = gridButtonIndex % 3;
                        y = gridButtonIndex / 3;
                    }
                }
            }

            return button;
        }
        
        public void ClearLastButtonClick() => _buttonClicked = null;

        public void Dispose()
        {
            _xPool.Dispose();
            _oPool.Dispose();
        }

        public Color GetColor(GridObjectType turnObjectType)
        {
            Color color;
            
            switch (turnObjectType)
            {
                case GridObjectType.X:
                    color = Color.red;
                    break;
                case GridObjectType.O:
                    color = Color.blue;
                    break;
                default:
                    color = Color.white;
                    break;
            }

            return color;
        }

        public void SetTurn(GameTurn turn, GridObjectType turnObjectType)
        {
            switch (turn)
            {
                case GameTurn.None:
                    _turnText.text = string.Empty;
                    break;
                case GameTurn.Player1:
                    _turnText.text = "Player 1's turn";
                    break;
                case GameTurn.Player2:
                    _turnText.text = "Player 2's turn";
                    break;
                case GameTurn.Ai:
                    _turnText.text = "AI's turn";
                    break;
                default:
                    throw new NotImplementedException();
            }

            _turnText.color = GetColor(turnObjectType);
        }

        public void SetWinner(GameTurn winner, GridObjectType winnerObjectType)
        {
            switch (winner)
            {
                case GameTurn.None:
                    _turnText.text = "Draw";
                    break;
                case GameTurn.Player1:
                    _turnText.text = "Player 1 won!";
                    break;
                case GameTurn.Player2:
                    _turnText.text = "Player 2 won!";
                    break;
                case GameTurn.Ai:
                    _turnText.text = "AI won!";
                    break;
                default:
                    throw new NotImplementedException();
            }

            _turnText.color = GetColor(winnerObjectType);
        }
    }

}