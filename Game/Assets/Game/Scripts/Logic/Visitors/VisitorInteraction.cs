using System;
using System.Collections;
using Game.Scripts.Common;
using Game.Scripts.Logic.Animators;
using Game.Scripts.Logic.Tables;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Scripts.Logic.Visitors
{
    public class VisitorInteraction
    {
        public event Action StartedWaitingOrder;
        public event Action InteractionEnded;

        private readonly ICoroutineRunner _runner;
        private readonly NavMeshAgent _agent;
        private readonly VisitorAnimatorController _animatorController;

        private readonly float _timeWaitLooking = 0.025f;
        private readonly float _waitingTimer = 10f;
        private float _currentTimer;
        private readonly float _timerEating = 7f;
        private float _currentEatingTimer;

        private bool _startedInteraction;

        public VisitorInteraction(ICoroutineRunner runner, NavMeshAgent agent,
            VisitorAnimatorController animatorController)
        {
            _runner = runner;
            _agent = agent;
            _animatorController = animatorController;
        }

        public void StartInteractive(Chair chair, Transform visitorTransform)
        {
            _startedInteraction = true;
            
            _agent.enabled = false;
            //adjusting the position on the chair
            visitorTransform.position += new Vector3(0f, 0.4f, 0.09f);
            
            _animatorController.StateExited += OnStateExited;
            _animatorController.PlayStandToSit();
            
            _runner.StartCoroutine(LookAtFood(chair, visitorTransform));
        }

        public void WaitingOrder(float deltaTime)
        {
            _currentTimer += deltaTime;
            if (_currentTimer >= _waitingTimer)
            {
                InteractionEnded?.Invoke();
                
                _currentTimer = 0;
            }
        }

        public void StartEating()
        {
            _animatorController.PlayEating();
        }
        
        public void OrderReceived(float deltaTime)
        {
            _currentEatingTimer += deltaTime;
            if (_currentEatingTimer >= _timerEating)
            {
                InteractionEnded?.Invoke();
                
                _currentEatingTimer = 0;
            }
        }

        private void OnStateExited(VisitorAnimatorState state)
        {
            if (VisitorAnimatorState.StandToSit == state)
            {
                _startedInteraction = false;
                StartWaitingOrder();
            }
        }

        private void StartWaitingOrder()
        {
            _animatorController.PlaySittingIdle();
            StartedWaitingOrder?.Invoke();
        }

        private IEnumerator LookAtFood(Chair chair, Transform visitorTransform)
        {
            while (_startedInteraction)
            {
                Vector3 visitorPosition = visitorTransform.position;
                Vector3 lookPos = chair.GetFoodPoint().position;
                Vector3 targetPos = new Vector3(lookPos.x,visitorPosition.y, lookPos.z);
                
                visitorTransform.LookAt(targetPos);

                yield return new WaitForSeconds(_timeWaitLooking);
            }
        }

        public void Reset() => 
            _animatorController.StateExited -= OnStateExited;
    }
}