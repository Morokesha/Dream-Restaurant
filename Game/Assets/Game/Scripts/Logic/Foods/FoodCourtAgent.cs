using Game.Scripts.Core;
using Game.Scripts.Core.FoodCourtService;
using Game.Scripts.Logic.Visitors;
using UnityEngine;

namespace Game.Scripts.Logic.Foods
{
    public class FoodCourtAgent : MonoBehaviour
    {
        private IFoodCourtManagerService _manager;

        private void Start()
        {
            _manager = ServiceLocator.Container.GetSingle<IFoodCourtManagerService>();
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IClient client))
            { 
                _manager.ControlClient(client);
            }
        }
    }
}