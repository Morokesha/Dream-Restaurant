using Game.Scripts.Common;
using UnityEngine;

namespace Game.Scripts.Logic.Pools
{
    public class PooledObject : MonoBehaviour
    {
        private IPoolReturn _pool;

        public void AssignPool(IPoolReturn pool)
        {
            _pool = pool;
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        protected void ReturnToPool()
        {
            _pool.ReturnToPool(this);
        }

        protected void ReturnToPool(VisitorType type)
        {
            _pool.ReturnToPool(this, type);
        }
    }
}