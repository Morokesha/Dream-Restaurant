using System;
using Game.Scripts.Logic.Visitors;
using UnityEngine;

namespace Game.Scripts.Logic.Tables
{
    public class Chair : MonoBehaviour
    {
        public event Action<IClient, Chair> WaitedOrder;
        public event Action<IClient> ClearedChair;
        public bool IsOccupied => _isOccupied;
        
        [SerializeField] private Transform _foodPoint;

        private IClient _currentClient;
        
        private bool _isOccupied;

        public Transform GetFoodPoint() => _foodPoint;

        public void SetClient(IClient client)
        {
            _currentClient = client;

            _isOccupied = true;
        }

        public void ClientWaitingOrder(IClient client)
        {
            WaitedOrder?.Invoke(client,this);
        }

        public void ClearChair()
        {
            _isOccupied = false;

            ClearedChair?.Invoke(_currentClient);
            _currentClient = null;
        }
    }
}