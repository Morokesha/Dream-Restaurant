using Game.Scripts.Common;
using Game.Scripts.Logic.Foods;
using Game.Scripts.Logic.Tables;

namespace Game.Scripts.Logic.Visitors
{
    public interface IClient
    {
        bool Serviced();
        void SetChair(Chair chair);
        void SetFood(Food food);

        FoodType GetFoodType();
    }
}