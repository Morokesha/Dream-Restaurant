using Game.Scripts.Common;
using Game.Scripts.Core.Factory;
using Game.Scripts.Core.StaticDataService;
using Game.Scripts.Players;
using UnityEngine;

namespace Game.Scripts.Logic.Foods
{
    public class FoodSpawner : MonoBehaviour
    {
        [SerializeField] private FoodSpawnUI _foodSpawnUI;
        [SerializeField] private Transform _spawnTransform;
        [SerializeField] private FoodType _foodType;

        private IGameFactory _gameFactory;
        private IStaticDataService _staticDataService;
        private ICollector _collector;
        private Food _currentFood;

        private float _currentTimer;
        private readonly float _timeForSpawn = 2.0f;

        private bool _isActivated;

        public void Init(IGameFactory gameFactory, IStaticDataService staticDataService)
        {
            _gameFactory = gameFactory;
            _staticDataService = staticDataService;
        }
        

        private void Update()
        {
            if (_isActivated == true)
            {
                SpawnFood();
                float scale = _currentTimer / _timeForSpawn;
                _foodSpawnUI.DrawSpawnFood(scale);
            }
        }

        private void ControlDisplay(bool active)
        {
            if (active == false)
            {
                _currentTimer = 0f;
            }

            _foodSpawnUI.ActivateDisplay(_staticDataService.GetFoodData(_foodType).Sprite,active);
        }

        private void SpawnFood()
        {
            _currentTimer += Time.deltaTime;

            if (_currentTimer >= _timeForSpawn)
            {
                _currentTimer = 0;

                _currentFood = _gameFactory.CreateFood(_staticDataService, _foodType,
                    _spawnTransform.position);
                _currentFood.Init(_foodType);
                
                _collector.CollectItem(_currentFood, _foodType);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ICollector collector))
            {
                _collector = collector;
                
                _isActivated = true;
                ControlDisplay(_isActivated);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out ICollector collector))
            {
                _isActivated = false;
                ControlDisplay(_isActivated);
            }
        }
    }
}