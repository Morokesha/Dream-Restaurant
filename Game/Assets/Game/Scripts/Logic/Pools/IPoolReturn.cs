using Game.Scripts.Common;

namespace Game.Scripts.Logic.Pools
{
    public interface IPoolReturn
    {
        void ReturnToPool(PooledObject pooledObject);
        void ReturnToPool(PooledObject pooledObject, VisitorType type);
    }
}