using Game.Scripts.Logic.Tables;
using Game.Scripts.Logic.Visitors;

namespace Game.Scripts.Core.FoodCourtService
{
    public interface IFoodCourtManagerService : IService
    {
        void AddTableToList(Table table);
        void ControlClient(IClient client);
    }
}