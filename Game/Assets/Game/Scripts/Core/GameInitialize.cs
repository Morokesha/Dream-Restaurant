using Cinemachine;
using Game.Scripts.Common;
using Game.Scripts.Core.Factory;
using Game.Scripts.Core.FoodCourtService;
using Game.Scripts.Core.InputServices;
using Game.Scripts.Core.StaticDataService;
using Game.Scripts.Logic.Foods;
using Game.Scripts.Logic.Loot;
using Game.Scripts.Logic.MoneyManager;
using Game.Scripts.Logic.Tables;
using Game.Scripts.Logic.Visitors;
using Game.Scripts.Logic.Visitors.VisitorDefaultConfig;
using Game.Scripts.Players;
using Game.Scripts.UI;
using UnityEngine;

namespace Game.Scripts.Core
{
    public class GameInitialize
    {
        private IFoodCourtManagerService _foodCourtManager;
        private IGameFactory _gameFactory;
        private IMoneyManager _moneyManager;
        private IInputService _inputService;
        private IStaticDataService _staticData;
        
        public GameInitialize()
        {
            RegisterServices();
        }

        public void InitLevel()
        {
            InitSpawnersTables();
            InitFoodSpawners();
            InitSpawnerVisitors();
            InitPlayer();
            InitHUD();
        }

        private void RegisterServices()
        {
            RegisterStaticData();

            _gameFactory = new GameFactory();
            _inputService = new InputService();
            _moneyManager = new MoneyManager();
            _foodCourtManager = new FoodCourtManager();
            
            ServiceLocator.Container.Register(_gameFactory);
            ServiceLocator.Container.Register(_moneyManager);
            ServiceLocator.Container.Register(_inputService);
            ServiceLocator.Container.Register(_foodCourtManager);
        }

        private void RegisterStaticData()
        {
            _staticData = new StaticDataService.StaticDataService();
            _staticData.LoadData();
            ServiceLocator.Container.Register(_staticData);
        }

        private void InitHUD()
        {
            UIManager uiManager = _gameFactory.CreateHUD();
            
            uiManager.Init(_moneyManager);
        }
        
        private void InitPlayer()
        {
            var pointSpawn = GameObject.FindWithTag("Player Spawn Point");
            var virtualCamera = Object.FindObjectOfType<CinemachineVirtualCamera>();
            
            Player player = _gameFactory.CreatePlayer(pointSpawn.transform);
            player.Init(_inputService,_moneyManager);
            
            virtualCamera.Follow = player.transform;
        }
        
        private void InitSpawnerVisitors()
        {
            SpawnerVisitors spawnerVisitors = Object.FindObjectOfType<SpawnerVisitors>();

            VisitorDefaultConfig[] defaultConfigs =
            {
                new VisitorDefaultConfig(VisitorType.Normal, 30),
                new VisitorDefaultConfig(VisitorType.Vip, 15)
            };

            var visitorGenerator = new VisitorGenerator();
            visitorGenerator.Init(_staticData, _gameFactory);
            visitorGenerator.CreateVisitor(defaultConfigs);

            LootSpawner lootSpawner = new LootSpawner(_staticData, _gameFactory);
            
            spawnerVisitors.Init(_staticData,visitorGenerator,lootSpawner);
        }
        
        private void InitSpawnersTables()
        {
            SpawnerTables[] spawnerTables = Object.FindObjectsOfType<SpawnerTables>();

            foreach (var spawner in spawnerTables)
                spawner.Init(_gameFactory, _moneyManager, _foodCourtManager);
        }
        
        private void InitFoodSpawners()
        {
            FoodSpawner[] foodSpawners = Object.FindObjectsOfType<FoodSpawner>();

            foreach (var foodSpawner in foodSpawners)
                foodSpawner.Init(_gameFactory,_staticData);
        }
    }
}