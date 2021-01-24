using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Monolith.Unity.Examples.TicTacToe.Scenes
{
    
    public sealed class GameSceneReferences : MonoBehaviour
    {

        [FormerlySerializedAs("CrossPrefab")] public AssetReferenceGameObject XPrefab;
        [FormerlySerializedAs("CirclePrefab")] public AssetReferenceGameObject OPrefab;
        [FormerlySerializedAs("CellButtons")] public Button[] GridButtons;
        public Button QuitButton;
        public Button ResetButton;
        public Text TurnText;

    }
    
}