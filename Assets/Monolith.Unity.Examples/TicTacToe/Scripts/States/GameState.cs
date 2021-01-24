using Monolith.States;
using Monolith.Unity.Examples.TicTacToe.Controllers;
using Monolith.Unity.Examples.TicTacToe.Scenes;
using Monolith.Unity.Extensions;
using Monolith.Unity.Pooling;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Monolith.Unity.Examples.TicTacToe.States
{

    public sealed class GameState : IState
    {
        
        private enum LoadStep
        {
            LoadScene,
            LoadPrefabs,
            Done
        }

        private LoadStep _loadStep;
        private AsyncOperation _sceneLoadOperation;
        private GameSceneReferences _sceneReferences;
        private AsyncOperationHandle<GameObject> _crossPrefabLoadOperation;
        private AsyncOperationHandle<GameObject> _circlePrefabLoadOperation;
        private GameController _gameController;
        
        public GameState(Game game)
        {
            _sceneLoadOperation = SceneManager.LoadSceneAsync("Game", new LoadSceneParameters(LoadSceneMode.Single, LocalPhysicsMode.None));
            _sceneLoadOperation.allowSceneActivation = true;
        }
    
        public bool Load(Game game)
        {
            switch (_loadStep)
            {
                case LoadStep.LoadScene:
                    if (_sceneLoadOperation.isDone)
                    {
                        _sceneReferences = SceneManager.GetActiveScene().GetComponentInRootGameObjects<GameSceneReferences>();
                        _crossPrefabLoadOperation = Addressables.LoadAssetAsync<GameObject>(_sceneReferences.XPrefab);
                        _circlePrefabLoadOperation = Addressables.LoadAssetAsync<GameObject>(_sceneReferences.OPrefab);
                        _loadStep = LoadStep.LoadPrefabs;
                    }
                    break;
                case LoadStep.LoadPrefabs:
                    if (_crossPrefabLoadOperation.IsDone && _circlePrefabLoadOperation.IsDone)
                    {
                        _gameController = new GameController(
                            game.StateManager.Get<MainState>().Mode,
                            _sceneReferences.TurnText,
                            _sceneReferences.GridButtons,
                            _sceneReferences.QuitButton,
                            _sceneReferences.ResetButton,
                            _crossPrefabLoadOperation.Result.GetComponent<PoolObject>(),
                            _circlePrefabLoadOperation.Result.GetComponent<PoolObject>()
                        );

                        _loadStep = LoadStep.Done;
                    }
                    break;
            }
            
            return _loadStep == LoadStep.Done;
        }

        public bool Enter(Game game)
        {
            _gameController.SetButtonsEnabled(true);
            
            return true;
        }

        public void Update(Game game)
        {
            InputSystem.Update();
            
            bool stop = _gameController.Update();
            
            if (stop) game.StateManager.SetTarget<TitleState>();
        }
        
        public bool Exit(Game game)
        {
            _gameController.SetButtonsEnabled(false);
            
            return true;
        }

        public bool Unload(Game game)
        {
            _gameController.Dispose();
            
            Addressables.Release(_crossPrefabLoadOperation);
            Addressables.Release(_circlePrefabLoadOperation);

            _sceneLoadOperation = null;
            _sceneReferences = null;
            _crossPrefabLoadOperation = default;
            _circlePrefabLoadOperation = default;
            
            return true;
        }

    }

}