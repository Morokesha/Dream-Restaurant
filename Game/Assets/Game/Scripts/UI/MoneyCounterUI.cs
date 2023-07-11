using System;
using Game.Scripts.Logic.MoneyManager;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class MoneyCounterUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _moneyText;
        
        private IMoneyManager _moneyManager;
        
        public void Init(IMoneyManager moneyManager)
        {
            _moneyManager = moneyManager;
            
            _moneyManager.MoneyChanged += OnMoneyChanged;
            
            OnMoneyChanged(_moneyManager.GetCurrentMoney());
        }

        private void OnMoneyChanged(int amount)
        {
            _moneyText.SetText(amount.ToString());
        }

        private void OnDisable()
        {
            _moneyManager.MoneyChanged -= OnMoneyChanged;
        }
    }
}