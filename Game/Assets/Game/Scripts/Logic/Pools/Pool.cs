using System.Collections.Generic;
using Game.Scripts.Common;

namespace Game.Scripts.Logic.Pools
{
    internal class Pool<T> : IPoolReturn where T : PooledObject
    {
        private readonly Queue<T> _inactiveObjects = new Queue<T>();
        private readonly Dictionary<VisitorType, Queue<T>> _queueVisitorByType;

        public Pool()
        {
            _queueVisitorByType = new Dictionary<VisitorType, Queue<T>>
            {
                [VisitorType.Normal] = new Queue<T>(),
                [VisitorType.Vip] = new Queue<T>()
            };
        }

        public void ReturnToPool(PooledObject obj)
        {
            obj.Disable();
            _inactiveObjects.Enqueue((T) obj);
        }

        public void ReturnToPool(PooledObject obj, VisitorType type)
        {
            obj.Disable();
            _queueVisitorByType[type].Enqueue((T) obj);
        }

        public void Add(T obj)
        {
            obj.AssignPool(this);
        }

        public bool HasInactiveObjects()
        {
            return _inactiveObjects.Count > 0;
        }

        public bool HasInactiveObjects(VisitorType type)
        {
            return _queueVisitorByType[type].Count > 0;
        }

        public T GetInactiveObject()
        {
            var inactiveObject = _inactiveObjects.Dequeue();
            inactiveObject.Enable();

            return inactiveObject;
        }

        public T GetInactiveObject(VisitorType type)
        {
            var inactiveObject = _queueVisitorByType[type].Dequeue();
            inactiveObject.Enable();

            return inactiveObject;
        }
    }
}