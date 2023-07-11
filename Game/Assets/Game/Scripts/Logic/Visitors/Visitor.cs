using System;
using Game.Scripts.Common;
using Game.Scripts.Logic.Foods;
using Game.Scripts.Logic.Loot;
using Game.Scripts.Logic.Pools;
using Game.Scripts.Logic.Tables;
using Game.Scripts.Logic.Visitors.Waypoints;
using Game.Scripts.StaticData;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Scripts.Logic.Visitors
{
    public enum VisitorState
    {
        None,
        Walking,
        WalkingToChair,
        Waiting,
        Eating
    }

    public class Visitor : PooledObject, IClient, ICoroutineRunner
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private VisitorAnimatorController _visitorAnimator;
        [SerializeField] private VisitorInteractionUI _visitorInteractionUI;

        private VisitorState State;
        
        private VisitorData _visitorData;
        private FoodData _foodData;

        private LootSpawner _lootSpawner;
        private VisitorMover _mover;
        private VisitorInteraction _visitorInteraction;
        private Chair _currentChair;
        private Food _currentFood;
        
        private bool _serviced;

        public void Init(VisitorData visitorData,FoodData foodData,LootSpawner lootSpawner,VisitorWaypoints waypoints,
            Vector3 spawnPos)
        {
            _visitorData = visitorData;
            _foodData = foodData;

            _lootSpawner = lootSpawner;
            
            _mover = new VisitorMover(_visitorAnimator,_agent, waypoints);
            _mover.ReachedTargetDestination += OnReachedTargetDestination;
            _mover.ReachedToLastWaypoint += OnReachedToLastWaypoint;
            
            _visitorInteraction = new VisitorInteraction(this,_agent,
                _visitorAnimator);
            _visitorInteraction.StartedWaitingOrder += OnStartedWaitingOrder;
            _visitorInteraction.InteractionEnded += OnInteractionEnded;
            
            SetDefaultBehaviour(spawnPos);
        }

        private void Update()
        {
            switch (State)
            {
                case VisitorState.None:
                    break;
                case VisitorState.Walking:
                    NormalMovement();
                    break;
                case VisitorState.WalkingToChair:
                    MovementTo();
                    break;
                case VisitorState.Waiting:
                    _visitorInteraction.WaitingOrder(Time.deltaTime);
                    break;
                case VisitorState.Eating:
                    _visitorInteraction.OrderReceived(Time.deltaTime);
                    break;
            }
        }
        
        public bool Serviced()
        {
            return _serviced;
        }

        public void SetChair(Chair chair)
        {
            _currentChair = chair;
            SetState(VisitorState.WalkingToChair);
        }

        public void SetFood(Food food)
        {
            _currentFood = food;

            _serviced = true;
            
            _visitorInteraction.StartEating();
            SetState(VisitorState.Eating);
        }

        public FoodType GetFoodType()
        {
            return _foodData.FoodType;
        }

        private void NormalMovement()
        {
            _mover.Move();
        }

        private void MovementTo()
        {
            _mover.MoveTo(_currentChair.transform,transform);
        }
        
        private void OnReachedTargetDestination()
        {
            SetState(VisitorState.None);
            
            _visitorInteraction.StartInteractive(_currentChair, transform);
        }
        
        private void OnStartedWaitingOrder()
        {
            _visitorInteraction.Reset();
            
            SetState(VisitorState.Waiting);
            
            _currentChair.ClientWaitingOrder(this);
            
            _visitorInteractionUI.ActivateDisplay(_foodData.Sprite);
        }
        
        private void OnInteractionEnded()
        {
            if (_serviced)
            {
                _lootSpawner.CreateLoot(LootSpawnPosition(), _visitorData.GetRandLootAmount(),
                    _visitorData.LootType);
                
                _currentFood.DestroyItem();
            }
            
            _currentChair.ClearChair();
            _visitorInteractionUI.DisableDisplay();

            SetState(VisitorState.Walking);
            
            _serviced = false;
        }
        
        private void OnReachedToLastWaypoint()
        {
            _mover.ReachedTargetDestination -= OnReachedTargetDestination;
            
            _visitorInteraction.StartedWaitingOrder -= OnStartedWaitingOrder;
            _visitorInteraction.InteractionEnded -= OnInteractionEnded;
            
            ReturnToPool();
        }

        private void SetDefaultBehaviour(Vector3 spawnPosition)
        {
            _agent.enabled = false;
            transform.position = spawnPosition;
            _agent.enabled = true;

            _serviced = false;
            _currentChair = null;
            _currentFood = null;

            SetState(VisitorState.Walking);
        }

        private Vector3 LootSpawnPosition()
        {
            float offsetY = _agent.height * 0.5f;
            
            Vector3 spawnLootPosition = new Vector3(transform.position.x,
                transform.position.y + offsetY, transform.position.z);

            return spawnLootPosition;
        }

        private void SetState(VisitorState state) =>
            State = state;
    }
}