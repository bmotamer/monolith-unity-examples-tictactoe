using System;
using System.Collections.Generic;

namespace Monolith.Unity.Examples.TicTacToe.Models
{

    public sealed class GameModel
    {

        public GameMode Mode;
        public GridObjectType[,] Grid;
        public GameTurn Turn;

        public GameModel(GameMode mode, int seed)
        {
            Mode = mode;
            Grid = new GridObjectType[3, 3];
            
            Reset(seed);
        }

        public void Reset(int seed)
        {
            for (var x = 0; x < Grid.GetLength(0); ++x)
            {
                for (var y = 0; y < Grid.GetLength(1); ++y)
                {
                    Grid[x, y] = GridObjectType.Empty;
                }
            }
            
            Turn = GetFirstTurn(seed);
        }

        public GameTurn GetFirstTurn(int seed)
        {
            GameTurn turn;
            
            var random = new Random(seed);

            if (random.Next(1) == 0)
            {
                turn = GameTurn.Player1;
            }
            else
            {
                if (Mode == GameMode.PlayerVsPlayer)
                {
                    turn = GameTurn.Player2;    
                }
                else
                {
                    turn = GameTurn.Ai;
                }
            }

            return turn;
        }

        public GridObjectType GetTurnObject(GameTurn current)
        {
            GridObjectType objectType;
            
            switch (current)
            {
                case GameTurn.Player1:
                    objectType = GridObjectType.X;
                    break;
                case GameTurn.Player2:
                case GameTurn.Ai:
                    objectType = GridObjectType.O;
                    break;
                default:
                    objectType = GridObjectType.Empty;
                    break;
            }

            return objectType;
        }

        public GameTurn GetObjectTurn(GridObjectType objectType)
        {
            GameTurn turn;
            
            switch (objectType)
            {
                case GridObjectType.X:
                    turn = GameTurn.Player1;
                    break;
                case GridObjectType.O:
                    if (Mode == GameMode.PlayerVsPlayer)
                    {
                        turn = GameTurn.Player2;    
                    }
                    else
                    {
                        turn = GameTurn.Ai;
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            return turn;
        }

        public GameTurn GetNextTurn(GameTurn current)
        {
            GameTurn next;
            
            switch (current)
            {
                case GameTurn.Player1:
                    if (Mode == GameMode.PlayerVsPlayer)
                    {
                        next = GameTurn.Player2;    
                    }
                    else
                    {
                        next = GameTurn.Ai;
                    }
                    break;
                case GameTurn.Player2:
                case GameTurn.Ai:
                    next = GameTurn.Player1;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return next;
        }

        public GameTurn Winner
        {
            get
            {
                GameTurn winner;
            
                // Horizontal checks
                if ((Grid[0, 0] != GridObjectType.Empty) && (Grid[0, 0] == Grid[1, 0]) && (Grid[1, 0] == Grid[2, 0]))
                {
                    winner = GetObjectTurn(Grid[0, 0]);
                }
                else if ((Grid[0, 1] != GridObjectType.Empty) && (Grid[0, 1] == Grid[1, 1]) && (Grid[1, 1] == Grid[2, 1]))
                {
                    winner = GetObjectTurn(Grid[0, 1]);
                }
                else if ((Grid[0, 2] != GridObjectType.Empty) && (Grid[0, 2] == Grid[1, 2]) && (Grid[1, 2] == Grid[2, 2]))
                {
                    winner = GetObjectTurn(Grid[0, 2]);
                }
                // Vertical checks
                else if ((Grid[0, 0] != GridObjectType.Empty) && (Grid[0, 0] == Grid[0, 1]) && (Grid[0, 1] == Grid[0, 2]))
                {
                    winner = GetObjectTurn(Grid[0, 0]);
                }
                else if ((Grid[1, 0] != GridObjectType.Empty) && (Grid[1, 0] == Grid[1, 1]) && (Grid[1, 1] == Grid[1, 2]))
                {
                    winner = GetObjectTurn(Grid[1, 0]);
                }
                else if ((Grid[2, 0] != GridObjectType.Empty) && (Grid[2, 0] == Grid[2, 1]) && (Grid[2, 1] == Grid[2, 2]))
                {
                    winner = GetObjectTurn(Grid[2, 0]);
                }
                // Diagonal checks
                else if ((Grid[0, 0] != GridObjectType.Empty) && (Grid[0, 0] == Grid[1, 1]) && (Grid[1, 1] == Grid[2, 2]))
                {
                    winner = GetObjectTurn(Grid[0, 0]);
                }
                else if ((Grid[0, 2] != GridObjectType.Empty) && (Grid[0, 2] == Grid[1, 1]) && (Grid[1, 1] == Grid[2, 0]))
                {
                    winner = GetObjectTurn(Grid[0, 2]);
                }
                else
                {
                    winner = GameTurn.None;
                }

                return winner;
            }
        }

        public bool IsFull
        {
            get
            {
                bool result = true;
                
                foreach (GridObjectType objectType in Grid)
                {
                    result &= objectType != GridObjectType.Empty;

                    if (!result) break;
                }

                return result;
            }
        }
        
        public GamePlaceResult PlaceObjectAt(int x, int y)
        {
            GamePlaceResult result;
            
            if (Grid[x, y] == GridObjectType.Empty)
            {
                Grid[x, y] =  GetTurnObject(Turn);

                if ((Turn == Winner) || IsFull)
                {
                    Turn = GameTurn.None;
                    
                    result = GamePlaceResult.End;
                }
                else
                {
                    Turn = GetNextTurn(Turn);

                    result = GamePlaceResult.Success;
                }
            }
            else
            {
                result = GamePlaceResult.Fail;
            }

            return result;
        }

        public bool GetRandomSpot(int seed, out int gridX, out int gridY)
        {
            var available = new List<(int, int)>(Grid.GetLength(0) * Grid.GetLength(1));
            
            for (var x = 0; x < Grid.GetLength(0); ++x)
            {
                for (var y = 0; y < Grid.GetLength(1); ++y)
                {
                    if (Grid[x, y] == GridObjectType.Empty) available.Add((x, y));
                }
            }
            
            bool success = available.Count > 0;

            if (success)
            {
                var random = new Random(seed);

                (int x, int y) = available[random.Next(available.Count)];

                gridX = x;
                gridY = y;
            }
            else
            {
                gridX = 0;
                gridY = 0;
            }

            return success;
        }

    }

}