using System;
using System.Collections;
using Game.Scripts.Common;
using Game.Scripts.Core.StaticDataService;
using Game.Scripts.Logic.Loot;
using Game.Scripts.Logic.Visitors.Waypoints;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Logic.Visitors
{
    public class SpawnerVisitors : MonoBehaviour
    {
        [SerializeField] private VisitorWaypoints _waypoints;

        private IStaticDataService _staticDataService;
        private LootSpawner _lootSpawner;
        private VisitorGenerator _generator;
        private Visitor _visitor;

        private VisitorType _visitorType;
        private FoodType _foodType;

        private int _randomCountVisitor;
        private int _activationChanceVip = 6;
        
        private float _reloading;
        private float _timerSpawn;

        public void Init(IStaticDataService staticDataService, 
            VisitorGenerator visitorGenerator, LootSpawner lootSpawner)
        {
            _staticDataService = staticDataService;
            _lootSpawner = lootSpawner;
            _generator = visitorGenerator;
            
            StartCoroutine(ActivateVisitor());
        }

        private IEnumerator ActivateVisitor()
        {
            while (true)
            {
                _timerSpawn = Random.Range(20f,40f);

                StartCoroutine(ReloadingSpawnVisitor());
                yield return new WaitForSeconds(_timerSpawn);
            }
        }

        private IEnumerator ReloadingSpawnVisitor()
        {
            _randomCountVisitor = Random.Range(2, 8);
            
            for (var i = 0; i < _randomCountVisitor; i++)
            {
                _visitorType = GetVisitorType();
                _foodType = GetRandomFood();

                _visitor = _generator.GetInactiveVisitor(_visitorType);
                _visitor.Init(_staticDataService.GetVisitorData(_visitorType),
                    _staticDataService.GetFoodData(_foodType),_lootSpawner,_waypoints,transform.position);

                _reloading = Random.Range(2f, 6f);
                
                yield return new WaitForSeconds(_reloading);
            }
        }

        private VisitorType GetVisitorType()
        {
            if (GetaActivationChance() > _activationChanceVip)
            {
                return VisitorType.Normal;
            }

            return VisitorType.Vip;
        }

        private FoodType GetRandomFood()
        { 
            Array lenghtFoodType = Enum.GetValues(typeof(FoodType)); 
            FoodType randomFood = (FoodType) Random.Range(0,lenghtFoodType.Length);

            return randomFood;
        }

        private int GetaActivationChance()
        {
            int activationChance = Random.Range(0, 100);
            
            return activationChance;
        }
    }
}