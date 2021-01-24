using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Monolith.Unity.Examples.TicTacToe.Scenes
{
    
    public sealed class TitleSceneReferences : MonoBehaviour
    {

        [FormerlySerializedAs("PlayButton")] public Button PlayerVsPlayerButton;
        public Button PlayerVsAiButton;
        public Button QuitButton;

    }
    
}