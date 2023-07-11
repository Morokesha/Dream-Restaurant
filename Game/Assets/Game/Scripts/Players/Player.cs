using System.Collections.Generic;
using Cinemachine;
using Game.Scripts.Common;
using Game.Scripts.Core.InputServices;
using Game.Scripts.Logic.Foods;
using Game.Scripts.Logic.MoneyManager;
using Game.Scripts.Logic.Tables;
using Game.Scripts.Logic.Visitors;
using UnityEngine;

namespace Game.Scripts.Players
{
    public class Player : MonoBehaviour, ICollector
    {
        [SerializeField] private Transform _foodContainer;
        [SerializeField] private CharacterController _controller;

        private IInputService _inputService;
        private IMoneyManager _moneyManager;
        private PlayerAnimatorController _playerAnimatorController;
        private PlayerMovement _playerMovement;
        
        private CollectableStorage<Food> _collectableStorage;

        private int _weight;
        public void Init(IInputService inputService, IMoneyManager moneyManager)
        {
            _inputService = inputService;
            _moneyManager = moneyManager;

            _playerAnimatorController = GetComponent<PlayerAnimatorController>();
            _playerMovement = new PlayerMovement();
            
            _playerMovement.Init(_inputService, _playerAnimatorController, _controller);

            _collectableStorage = new CollectableStorage<Food>(_foodContainer);
        }

        private void Update()
        {
            _playerMovement.Move(transform,Time.deltaTime);
        }

        public void CollectItem(Food food, FoodType type)
        {
            _collectableStorage.AddItem(food, type);
            
            CheckToFoodOnHand();
        }

        public void AddMoney(int amount)
        {
            _moneyManager.AddMoney(amount);
        }

        public Vector3 GetCollectorPoint()
        {
            Vector3 thisPosition = transform.position;

            return new Vector3(thisPosition.x,_controller.height *0.5f, thisPosition.z);
        }

        private void TryServeWaitingClients(Dictionary<IClient,Chair> waitingClients)
        {
            foreach (KeyValuePair<IClient,Chair> waitingClient in waitingClients)
            {
                IClient client = waitingClient.Key;

                FoodType foodType = client.GetFoodType();

                if (_collectableStorage.HasItemInStack(foodType))
                {
                    if (client.Serviced() == false)
                    {
                        Chair chair = waitingClient.Value;

                        Food food = _collectableStorage.GetCollectableItem(chair.GetFoodPoint(),
                            foodType);
                        
                        client.SetFood(food);
                    }
                }
            }
        }

        private void CheckToFoodOnHand()
        {
            _weight = _collectableStorage.IsEmpty() ? 0 : 1;
            
            _playerAnimatorController.SetWeightLayer(_weight);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Table table))
            {
                if (table.TryGetWaitingClients(out Dictionary<IClient, Chair> waitingClients))
                {
                    TryServeWaitingClients(waitingClients);
                }
            }
        }
    }
}