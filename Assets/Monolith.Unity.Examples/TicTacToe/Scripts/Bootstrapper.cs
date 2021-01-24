using UnityEngine;

namespace Monolith.Unity.Examples.TicTacToe
{

    public static class Bootstrapper
    {

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Run() => Unity.Bootstrapper.Run<TicTacToe>();

    }

}