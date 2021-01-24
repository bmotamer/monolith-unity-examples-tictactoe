using Monolith.States;
using Monolith.Unity.Examples.TicTacToe.States;

namespace Monolith.Unity.Examples.TicTacToe
{

    public sealed class TicTacToe : UnityGame
    {
        
        public TicTacToe(IEventListener eventListener) : base(eventListener)
        {
        }

        protected override StateTreeStatic CreateStateTree()
        {
            var stateTree = new StateTreeDynamic();

            stateTree.Add<MainState>();
            stateTree.Add<MainState, TitleState>();
            stateTree.Add<MainState, GameState>();
            
            stateTree.SetDefault<TitleState>();

            return stateTree.AsStatic();
        }

    }

}