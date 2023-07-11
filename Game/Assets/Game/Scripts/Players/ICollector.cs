using Game.Scripts.Common;
using Game.Scripts.Logic.Foods;
using UnityEngine;

namespace Game.Scripts.Players
{
    public interface ICollector
    {
        void CollectItem(Food food, FoodType type);
        void AddMoney(int amount);

        Vector3 GetCollectorPoint();
    }
}