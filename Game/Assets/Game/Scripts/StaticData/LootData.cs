using Game.Scripts.Common;
using Game.Scripts.Logic.Loot;
using UnityEngine;

namespace Game.Scripts.StaticData
{
    [CreateAssetMenu(fileName = "New LootData", menuName = "Loot Data/Loot Config", order = 3)]
    public class LootData : ScriptableObject
    {
        public LootType LootType;
        public Loot LootTemplate;
        public LootVisual Visual => _visual;
        public Color Color => _color;
        
        [SerializeField] private LootVisual _visual;
        [Range(7,11)]
        [SerializeField]private int _minMoney = 9;

        [Range(12, 15)]
        [SerializeField] private int _maxMoney = 13;
        [SerializeField] private Color _color;

        public int RandomAmount()
        {
            return Random.Range(_minMoney, _maxMoney);
        }
    }
}