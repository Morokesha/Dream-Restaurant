using System;

namespace Game.Scripts.Logic.MoneyManager
{
    public class MoneyManager : IMoneyManager
    {
        public event Action<int> MoneyChanged;

        private int _currentBalanceMoney = 20;

        public void AddMoney(int amount)
        {
            _currentBalanceMoney += amount;
            
            MoneyChanged?.Invoke(_currentBalanceMoney);
        }

        public void TakeMoney(int price)
        {
            if (_currentBalanceMoney < price) return;
            
            _currentBalanceMoney -= price;
            
            MoneyChanged?.Invoke(_currentBalanceMoney);
        }

        public bool HaveMoney(int price)
        {
            if (_currentBalanceMoney < price) return false;
            
            return true;
        }

        public int GetCurrentMoney() => _currentBalanceMoney;
    }
}