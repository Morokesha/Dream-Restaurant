using System;
using System.Collections;
using Game.Scripts.Core.Factory;
using Game.Scripts.Core.FoodCourtService;
using Game.Scripts.Logic.MoneyManager;
using Game.Scripts.Players;
using UnityEngine;

namespace Game.Scripts.Logic.Tables
{
    public class SpawnerTables : MonoBehaviour
    {
        public event Action<int> ChangedCounter;
        public event Action PlayerExited;
        
        [SerializeField] private int _priceForSpawnTable;
        [SerializeField] private bool _isDefault;
        
        public int PriceForSpawnTable => _priceForSpawnTable;
        
        private IFoodCourtManagerService _foodCourtManager;
        private IGameFactory _gameFactory;
        private IMoneyManager _moneyManager;
        
        private readonly float _waitForBuy = 2.5f;
        private readonly float _timeOfDecreaseMoney = 0.05f;
        private bool _enteredSpawner;

        public void Init(IGameFactory gameFactory, IMoneyManager moneyManager,
            IFoodCourtManagerService foodCourtManager)
        {
            _gameFactory = gameFactory;
            _moneyManager = moneyManager;
            _foodCourtManager = foodCourtManager;

            if (_isDefault) SpawnTable();
        }

        private void SpawnTable()
        {
            var table = _gameFactory.CreateTable(transform.position);
            table.Init();
            
            _foodCourtManager.AddTableToList(table);

            gameObject.SetActive(false);
        }

        private IEnumerator CanSpawnTimerCo()
        {
            _enteredSpawner = false;
            yield return new WaitForSeconds(_waitForBuy);
            _enteredSpawner = true;
            StartCoroutine(TimerForBuyTable());
        }

        private IEnumerator TimerForBuyTable()
        {
            var visualCounter = _priceForSpawnTable;

            while (_enteredSpawner)
            {
                if (_moneyManager.HaveMoney(_priceForSpawnTable) && _isDefault == false)
                {
                    visualCounter -= 5;
                    ChangedCounter?.Invoke(visualCounter);

                    if (visualCounter <= 0)
                    {
                        _moneyManager.TakeMoney(_priceForSpawnTable);
                        SpawnTable();
                    }
                }

                yield return new WaitForSeconds(_timeOfDecreaseMoney);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ICollector player)) StartCoroutine(CanSpawnTimerCo());
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out ICollector player))
            {
                _enteredSpawner = false;
                StopAllCoroutines();
                PlayerExited?.Invoke();
            }
        }
    }
}