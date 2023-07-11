using Game.Scripts.Common;
using Game.Scripts.Logic.Foods;
using Game.Scripts.Logic.Visitors;
using Game.Scripts.StaticData;

namespace Game.Scripts.Core.StaticDataService
{
    public interface IStaticDataService : IService
    {
        void LoadData();
        public VisitorData GetVisitorData(VisitorType type);
        public FoodData GetFoodData(FoodType type);
        public LootData GetLootData(LootType type);
    }
}