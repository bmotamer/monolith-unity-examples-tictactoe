using System;
using Monolith.States;
using Monolith.Unity.Examples.TicTacToe.Models;
using UnityEngine;

namespace Monolith.Unity.Examples.TicTacToe.States
{

    public sealed class MainState : IState
    {

        private const string PlayerPrefsKeyWinCount = "";

        private int _winCount;
        
        public MainState(Game game)
        {
        }
        
        public int WinCount
        {
            get => _winCount;
            set
            {
                if (_winCount != value)
                {
                    PlayerPrefs.SetInt(PlayerPrefsKeyWinCount, value);
                    _winCount = value;
                }
            }
        }

        public GameMode Mode;
    
        public bool Load(Game game)
        {
            _winCount = Math.Max(0, PlayerPrefs.GetInt(PlayerPrefsKeyWinCount, 0));
            
            return true;
        }

        public bool Enter(Game game)
        {
            throw new InvalidOperationException();
        }

        public void Update(Game game)
        {
        }

        public bool Exit(Game game)
        {
            return true;
        }

        public bool Unload(Game game)
        {
            return true;
        }

    }

}