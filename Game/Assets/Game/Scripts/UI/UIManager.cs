using Game.Scripts.Logic.MoneyManager;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private MoneyCounterUI _moneyCounter;

        public void Init(IMoneyManager moneyManager)
        {
            _moneyCounter.Init(moneyManager);
        }
    }
}