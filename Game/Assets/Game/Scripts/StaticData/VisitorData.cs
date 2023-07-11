using System.Collections.Generic;
using Game.Scripts.Common;
using Game.Scripts.Logic.Visitors;
using UnityEngine;
namespace Game.Scripts.StaticData
{
    [CreateAssetMenu(fileName = "New VisitorData", menuName = "Visitor Data/Visitor Data", order = 1)]
    public class VisitorData : ScriptableObject
    {
        public Visitor VisitorTemplate;
        public VisitorType VisitorType;
        public LootType LootType;
        
        [SerializeField] private List<GameObject> Visual;
        [Range(1,3)]
        [SerializeField] private int _minLootAmount = 2;

        [Range(4, 6)]
        [SerializeField] private int _maxLootAmount = 5;

        public int GetRandLootAmount()
        {
            return Random.Range(_minLootAmount, _maxLootAmount);
        }
        public GameObject GetRandomVisual() => Visual[Random.Range(0, Visual.Count)];
    }
}