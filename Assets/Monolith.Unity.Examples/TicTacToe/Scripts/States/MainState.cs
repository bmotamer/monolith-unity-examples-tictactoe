using System;
using Monolith.States;
using Monolith.Unity.Examples.TicTacToe.Models;
using UnityEngine;

namespace Monolith.Unity.Examples.TicTacToe.States
{

    public sealed class MainState : IState
    {
        
        public GameMode Mode;

        public MainState(Game game)
        {
        }

        public bool Load(Game game)
        {
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