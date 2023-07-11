using Game.Scripts.Common;
using Game.Scripts.Core.StaticDataService;
using Game.Scripts.Logic.Foods;
using Game.Scripts.Logic.Tables;
using Game.Scripts.Logic.Visitors;
using Game.Scripts.Players;
using Game.Scripts.UI;
using UnityEngine;

namespace Game.Scripts.Core.Factory
{
    public interface IGameFactory : IService

    {
        public Player CreatePlayer(Transform pointSpawn);
        Visitor CreateVisitor(IStaticDataService staticDataService, VisitorType visitorType);
        Table CreateTable(Vector3 spawnPosition);
        public Food CreateFood(IStaticDataService staticDataService, FoodType type, Vector3 position);
        void CreateLoot(IStaticDataService staticDataService, LootType type, Vector3 spawnPosition);

        public UIManager CreateHUD();
    }
}