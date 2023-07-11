using System.Collections.Generic;
using Game.Scripts.Logic.Tables;
using Game.Scripts.Logic.Visitors;
using UnityEngine;

namespace Game.Scripts.Core.FoodCourtService
{
    public class FoodCourtManager : IFoodCourtManagerService
    {
        private readonly List<Table> _currentTables = new List<Table>();
        private readonly List<IClient> _listClients = new List<IClient>();

        public void AddTableToList(Table table)
        {
            _currentTables.Add(table);
        }

        public void ControlClient(IClient client)
        {
            if (!_listClients.Contains(client))
            {
                _listClients.Add(client);

                CheckFreeChair(client);
            }
            else
                _listClients.Remove(client);
        }

        private void CheckFreeChair(IClient client)
        {
            foreach (Table table in _currentTables)
            {
                foreach (Chair chair in table.GetChairs())
                {
                    if (chair.IsOccupied == false)
                    {
                        chair.SetClient(client);
                        
                        client.SetChair(chair);

                        return;
                    }
                }
            }
        }
    }
}