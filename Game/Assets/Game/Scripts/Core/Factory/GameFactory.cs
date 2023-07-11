using Game.Scripts.Common;
using Game.Scripts.Core.StaticDataService;
using Game.Scripts.Logic.Foods;
using Game.Scripts.Logic.Loot;
using Game.Scripts.Logic.Tables;
using Game.Scripts.Logic.Visitors;
using Game.Scripts.Players;
using Game.Scripts.StaticData;
using Game.Scripts.UI;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Scripts.Core.Factory
{
    public class GameFactory : IGameFactory

    {
        public Player CreatePlayer(Transform pointSpawn)
        {
            var playerTemplate = Resources.Load<Player>("Player/Player Template");

            return Object.Instantiate(playerTemplate, pointSpawn.position, Quaternion.identity);
        }
        
        public Visitor CreateVisitor(IStaticDataService staticDataService,
            VisitorType visitorType)
        {
            VisitorData visitorData= staticDataService.GetVisitorData(visitorType);
            
            Visitor visitor = Object.Instantiate(visitorData.VisitorTemplate);
            
            Object.Instantiate(visitorData.GetRandomVisual(), visitor.transform, true);
            
            return visitor;
        }
        
        public Table CreateTable(Vector3 spawnPosition)
        {
            var tableTemplate = Resources.Load<Table>("Tables/Table");

            return Object.Instantiate(tableTemplate, spawnPosition, quaternion.identity);
        }
        
        public Food CreateFood(IStaticDataService staticDataService,FoodType type,Vector3 position)
        {
            FoodData foodData = staticDataService.GetFoodData(type);

            Food foodTemplate = Object.Instantiate(foodData.Template, position, Quaternion.identity);
            
            return foodTemplate;
        }
        
        public void CreateLoot(IStaticDataService staticDataService, LootType type, Vector3 spawnPosition)
        {
            LootData lootData = staticDataService.GetLootData(type);

            Loot loot = Object.Instantiate(lootData.LootTemplate,spawnPosition,Quaternion.identity);
            
            LootVisual lootVisual = Object.Instantiate(lootData.Visual, loot.transform);
            
            loot.Init(lootData,lootVisual);
        }

        public UIManager CreateHUD()
        {
            var uiManager = Resources.Load<UIManager>("HUD/HUD");

            return Object.Instantiate(uiManager);
        }
    }
}