using System;
using System.Collections.Generic;
using Game.Scripts.Logic.Visitors;
using UnityEngine;

namespace Game.Scripts.Logic.Tables
{
    public class Table : MonoBehaviour
    {
        [SerializeField] private Chair[] _chairs;
        
        private Dictionary<IClient, Chair> _waitingClients;

        public void Init()
        {
            _waitingClients = new Dictionary<IClient, Chair>();
            
            foreach (var chair in _chairs)
            {
               chair.WaitedOrder += OnWaitedOrder;
               chair.ClearedChair += OnClearedChair;
            }
        }
        
        public Chair[] GetChairs()
        {
            return _chairs;
        }
        
        public bool TryGetWaitingClients(out Dictionary<IClient, Chair> clients)
        {
            if (_waitingClients.Count > 0)
            {
                clients = _waitingClients;
                return true;
            }
            
            clients = _waitingClients;
            return false;
        }
        
        private void OnClearedChair(IClient client)
        {
            _waitingClients.Remove(client);
        }

        private void OnWaitedOrder(IClient client, Chair chair)
        {
            _waitingClients.Add(client, chair);
        }
    }
}