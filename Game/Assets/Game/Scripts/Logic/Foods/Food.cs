using System.Collections;
using Game.Scripts.Common;
using UnityEngine;
using DG.Tweening;

namespace Game.Scripts.Logic.Foods
{
    public class Food : MonoBehaviour, ICollectableItem
    {
        private FoodType _eatType;

        private Sequence _jumpSequence;

        [SerializeField] private BoxCollider _collider;

        private readonly float _speedJump = 2.8f;

        public void Init(FoodType type)
        {
            _eatType = type;
        }

        public void MoveTo(Vector3 endPosition)
        {
            Jumping(endPosition);
            //StartCoroutine(MovementToCo(targetPosition));
        }

        public float GetHeightItem()
        {
            return _collider.bounds.size.y;
        }

        public Transform GetCollectableTransform()
        {
            return transform;
        }

        public void DestroyItem()
        {
            Destroy(gameObject);
        }

        private void Jumping(Vector3 endPosition)
        {
            _jumpSequence = transform.DOLocalJump(endPosition,
                1,
                1,
                0.4f).SetEase(Ease.InOutSine);
        }

        /*private IEnumerator MovementToCo(Vector3 targetPosition)
        {
            float currentTimer = 0f;
            float timer = 1f;
            while (currentTimer <= timer)
            {
                currentTimer += Time.deltaTime;
                
                transform.localPosition = Vector3.Lerp(transform.localPosition,targetPosition,
                    _speedJump * Time.deltaTime);
                
                yield return null;
            }
        }*/
    }
}