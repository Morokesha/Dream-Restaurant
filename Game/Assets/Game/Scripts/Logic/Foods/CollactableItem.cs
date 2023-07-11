using UnityEngine;

namespace Game.Scripts.Logic.Foods
{
    public interface ICollectableItem
    {
        public void MoveTo(Vector3 endPosition);
        public float GetHeightItem();
        public Transform GetCollectableTransform();

        public void DestroyItem();
    }
}