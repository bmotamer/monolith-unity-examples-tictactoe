using Monolith.States;
using Monolith.Unity.Examples.TicTacToe.Models;
using Monolith.Unity.Examples.TicTacToe.Scenes;
using Monolith.Unity.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Monolith.Unity.Examples.TicTacToe.States
{

    public sealed class TitleState : IState
    {

        private AsyncOperation _sceneLoadOperation;
        private TitleSceneReferences _sceneReferences;
        private Button _buttonClicked;
        
        public TitleState(Game game)
        {
            _sceneLoadOperation = SceneManager.LoadSceneAsync("Title", new LoadSceneParameters(LoadSceneMode.Single, LocalPhysicsMode.None));
            _sceneLoadOperation.allowSceneActivation = true;
        }
    
        public bool Load(Game game)
        {
            bool isDone = _sceneLoadOperation.isDone; 
            
            if (isDone) _sceneReferences = SceneManager.GetActiveScene().GetComponentInRootGameObjects<TitleSceneReferences>();
            
            return isDone;
        }

        public bool Enter(Game game)
        {
            _sceneReferences.PlayerVsPlayerButton.onClick.AddListener(() => _buttonClicked = _sceneReferences.PlayerVsPlayerButton);
            _sceneReferences.PlayerVsAiButton.onClick.AddListener(() => _buttonClicked = _sceneReferences.PlayerVsAiButton);
            _sceneReferences.QuitButton.onClick.AddListener(() => _buttonClicked = _sceneReferences.QuitButton);

            return true;
        }

        public void Update(Game game)
        {
            InputSystem.Update();
            
            if (_buttonClicked)
            {
                if (_buttonClicked == _sceneReferences.PlayerVsPlayerButton)
                {
                    game.StateManager.Get<MainState>().Mode = GameMode.PlayerVsPlayer;
                    game.StateManager.SetTarget<GameState>();    
                }
                else if (_buttonClicked == _sceneReferences.PlayerVsAiButton)
                {
                    game.StateManager.Get<MainState>().Mode = GameMode.PlayerVsAi;
                    game.StateManager.SetTarget<GameState>();
                }
                else if (_buttonClicked == _sceneReferences.QuitButton)
                {
                    Application.Quit();
                }
            }
            
            _buttonClicked = null;
        }

        public bool Exit(Game game)
        {
            _sceneReferences.PlayerVsPlayerButton.onClick.RemoveAllListeners();
            _sceneReferences.PlayerVsAiButton.onClick.RemoveAllListeners();
            _sceneReferences.QuitButton.onClick.RemoveAllListeners();

            return true;
        }

        public bool Unload(Game game)
        {
            _sceneReferences = null;
            _sceneLoadOperation = null;
            _buttonClicked = null;
            
            return true;
        }

    }

}