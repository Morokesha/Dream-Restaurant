using System;
using Game.Scripts.Players;
using UnityEngine;

namespace Game.Scripts.Logic.Loot
{
    public class LootVisual : MonoBehaviour
    {
        public event Action<ICollector> LootCollected;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ICollector collector))
            {
                LootCollected?.Invoke(collector);
            }
        }
    }
}