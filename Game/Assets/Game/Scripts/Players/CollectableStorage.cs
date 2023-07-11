using System.Collections.Generic;
using Game.Scripts.Common;
using Game.Scripts.Logic.Foods;
using UnityEngine;

namespace Game.Scripts.Players
{
    internal class CollectableStorage<T> where T : ICollectableItem
    {
        private readonly Transform _container;
        private Vector3 _collectablePosition;
        
        private readonly Stack<T> _foodStack = new Stack<T>();
        private readonly Dictionary<FoodType, Stack<T>> _stackFoodByType;

        public CollectableStorage(Transform container)
        {
            _container = container;
            _collectablePosition = Vector3.zero;

            _stackFoodByType = new Dictionary<FoodType, Stack<T>>
            {
                [FoodType.Toast] = new Stack<T>(),
                [FoodType.Cake] = new Stack<T>()
            };
        }

        public bool HasItemInStack(FoodType type)
        {
            return _stackFoodByType[type].Count > 0;
        }
        
        public void AddItem(T collectableItem, FoodType type)
        {
            AttachToParent(collectableItem);
            
            _stackFoodByType[type].Push(collectableItem);
        }
        
        public T GetCollectableItem(Transform newContainer,FoodType type)
        {
            ICollectableItem collectableItem = _stackFoodByType[type].Pop();
            
            ChangeParentContainer(collectableItem, newContainer);
            return (T) collectableItem;
        }

        public bool IsEmpty()
        {
            foreach (var item in _stackFoodByType)
            {
                if (item.Value.Count > 0)
                    return false;
            }

            return true;
        }

        private void AttachToParent(ICollectableItem collectableItem)
        {
            collectableItem.GetCollectableTransform().SetParent(_container);
            
            IncreaseCollectionPosition(collectableItem.GetHeightItem());
            
            collectableItem.MoveTo(_collectablePosition);
        }

        private void ChangeParentContainer(ICollectableItem collectableItem,Transform container)
        {
            collectableItem.GetCollectableTransform().SetParent(container);
            
            DecreaseCollectionPosition(collectableItem.GetHeightItem());
            
            collectableItem.MoveTo(Vector3.zero);
        }

        private void IncreaseCollectionPosition(float collectableHeightItem)
        {
            _collectablePosition += new Vector3(0f,collectableHeightItem,0f);
        }

        private void DecreaseCollectionPosition(float collectableHeightItem)
        {
            Vector3 newCollectablePosition = Vector3.zero;

            foreach (Transform childItem in _container)
            {
                childItem.transform.localPosition = newCollectablePosition;
                
                newCollectablePosition += new Vector3(0f, collectableHeightItem, 0f);
            }

            newCollectablePosition -= new Vector3(0f, collectableHeightItem, 0f);

            _collectablePosition = newCollectablePosition;
        }
    }
}