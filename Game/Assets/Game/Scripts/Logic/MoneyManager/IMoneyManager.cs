using System;
using Game.Scripts.Core;

namespace Game.Scripts.Logic.MoneyManager
{
    public interface IMoneyManager : IService
    {
        event Action<int> MoneyChanged; 

        void AddMoney(int amount);
        void TakeMoney(int price);
        bool HaveMoney(int price);

        int GetCurrentMoney();
    }
}