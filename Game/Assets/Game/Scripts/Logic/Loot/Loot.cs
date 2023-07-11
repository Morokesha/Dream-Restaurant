using System;
using System.Collections;
using DG.Tweening;
using Game.Scripts.Players;
using Game.Scripts.StaticData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Logic.Loot
{
    public class Loot : MonoBehaviour
    {
        private LootVisual _lootVisual;

        private LootData _lootData;
        private ICollector _collector;
        private Tween _rotationTween;
        
        private float _radius = 10f;
        private float _speed = 5f;

        private bool _isFollowing = false;

        public void Init(LootData lootData, LootVisual lootVisual)
        {
            _lootData = lootData;
            
            _lootVisual = lootVisual;
            _lootVisual.LootCollected += OnLootCollected;
            
            StartCoroutine(MovementLerpCoroutine(GetPointOnRadius(_radius)));
            RandomRotation();
        }

        private void Update()
        {
            if (_isFollowing)
            {
                transform.position = Vector3.Lerp(transform.position,
                    _collector.GetCollectorPoint(),_speed * Time.deltaTime);
            }
        }

        private Vector3 GetPointOnRadius(float radius)
        {
            Vector2 randPoint = Random.insideUnitCircle.normalized * radius;
            return new Vector3(randPoint.x, 0, randPoint.y);
        }
        
        private void RandomRotation()
        {
            float rotationSpeed = Random.Range(1f, 5f);
            
            Vector3 randomRotate = new Vector3(0f, 360f, 0f);
            _rotationTween = _lootVisual.transform.DORotate(randomRotate, rotationSpeed, 
            RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).
                SetEase(Ease.Linear);
        }

        private void OnLootCollected(ICollector collector)
        {
            _collector.AddMoney(_lootData.RandomAmount());

            _rotationTween.Kill();
            _lootVisual.LootCollected -= OnLootCollected;

            StartCoroutine(DestroyCo());
        }

        private IEnumerator MovementLerpCoroutine(Vector3 targetPosition)
        {
            float _currentTimer = 0;
            float _timer = 1f;

            if (_currentTimer <= _timer)
            {
                _currentTimer += Time.deltaTime;

                transform.position = Vector3.Lerp(transform.position, 
                    targetPosition, _speed * Time.deltaTime);

                yield return null;
            }
        }

        private IEnumerator DestroyCo()
        {
            yield return new WaitForSeconds(.2f);

            Destroy(gameObject);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ICollector collector))
            {
                _collector = collector;
                _isFollowing = true;
            }
        }
    }
}