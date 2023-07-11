using System;
using Game.Scripts.Logic.Animators;
using UnityEngine;

namespace Game.Scripts.Logic.Visitors
{
    [RequireComponent(typeof(Animator))]
    public class VisitorAnimatorController : MonoBehaviour, IAnimationStateReader
    {
        public event Action<VisitorAnimatorState> StateEntered;
        public event Action<VisitorAnimatorState> StateExited;

        [SerializeField] private Animator _animator;

        private readonly int _eating = Animator.StringToHash("Eating");
        private readonly int _sitting = Animator.StringToHash("Sitting");
        private readonly int _walking = Animator.StringToHash("Walking");
        private readonly int _idle = Animator.StringToHash("Idle");

        private VisitorAnimatorState State { get; set; }

        public void PlaySittingIdle() => _animator.SetTrigger(_idle);
        public void PlayEating() => _animator.SetTrigger(_eating);
        public void PlayWalking() => _animator.SetTrigger(_walking);
        public void PlayStandToSit() => _animator.SetTrigger(_sitting);

        public void EnteredState(int stateHash)
        {
            State = StateFor(stateHash);
            StateEntered?.Invoke(State);
        }

        public void ExitedState(int stateHash)
        {
            StateExited?.Invoke(StateFor(stateHash));
        }

        private VisitorAnimatorState StateFor(int stateHash)
        {
            VisitorAnimatorState state;
            
            if (stateHash == _walking)
                state = VisitorAnimatorState.Walking;
            else if (stateHash == _sitting)
                state = VisitorAnimatorState.StandToSit;
            else if (stateHash == _idle)
                state = VisitorAnimatorState.SittingIdle;
            else if (stateHash == _eating)
                state = VisitorAnimatorState.Eating;
            else
                state = VisitorAnimatorState.None;

            return state;
        }
    }
}