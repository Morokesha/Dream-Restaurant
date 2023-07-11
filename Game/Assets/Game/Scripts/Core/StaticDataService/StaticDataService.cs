using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Common;
using Game.Scripts.Logic.Loot;
using Game.Scripts.StaticData;
using UnityEngine;

namespace Game.Scripts.Core.StaticDataService
{
    public class StaticDataService : IStaticDataService
    {
        private const string _visitorPath = "Visitor/Visitor Config/";
        private const string _foodPath = "Food/Food Data/";
        private const string _lootPath = "Loot/Loot Config/";
    
        private Dictionary<VisitorType, VisitorData> _visitorConfigsByType;
        private Dictionary<FoodType, FoodData> _foodDataByType;
        private Dictionary<LootType, LootData> _lootDataByType;

        private VisitorData _visitorData;
        private FoodData _foodData;
        private LootData _lootData;

        public void LoadData()
        {
            _visitorConfigsByType =
                Resources.LoadAll<VisitorData>(_visitorPath).ToDictionary(
                    item => item.VisitorType, item => item);
            
            _foodDataByType =
                Resources.LoadAll<FoodData>(_foodPath).ToDictionary(
                    item => item.FoodType, item => item);
            
            _lootDataByType =
                Resources.LoadAll<LootData>(_lootPath).ToDictionary(
                    item => item.LootType, item => item);
        }

        public VisitorData GetVisitorData(VisitorType type) =>
            _visitorConfigsByType.TryGetValue(type, out var visitorConfig)
                ? visitorConfig
                : null;
        
        public FoodData GetFoodData(FoodType type) =>
            _foodDataByType.TryGetValue(type, out FoodData foodData)
                ? foodData
                : null;
        
        public LootData GetLootData(LootType type) =>
            _lootDataByType.TryGetValue(type, out LootData lootData)
                ? lootData
                : null;
    }
}