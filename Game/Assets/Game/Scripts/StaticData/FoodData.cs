using Game.Scripts.Common;
using Game.Scripts.Logic.Foods;
using UnityEngine;

namespace Game.Scripts.StaticData
{
    [CreateAssetMenu(fileName = "New FoodData", menuName = "Food Data/Food Config", order = 2)]
    public class FoodData : ScriptableObject
    {
        [SerializeField] private FoodType _foodType;
        [SerializeField] private Food _foodTemplate;
        [SerializeField] private Sprite _sprite;
        
        public FoodType FoodType => _foodType;
        public Food Template => _foodTemplate;
        public Sprite Sprite=> _sprite;
    }
}