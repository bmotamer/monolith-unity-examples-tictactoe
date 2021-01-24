using Monolith.Unity.Examples.TicTacToe.Models;
using Monolith.Unity.Pooling;
using UnityEngine;

namespace Monolith.Unity.Examples.TicTacToe.Views
{
    
    public sealed class GridObject : PoolObject
    {

        [SerializeField] private GridObjectType _type;

        public GridObjectType Type => _type;
        
        protected override void OnInstantiate()
        {
        }

        protected override void OnSpawn()
        {
        }

        protected override void OnRespawn()
        {
        }

        protected override void OnDespawn()
        {
        }

        protected override void OnDispose()
        {
        }
        
    }
    
}