using System;
using Game.Scripts.Logic.Animators;
using UnityEngine;

namespace Game.Scripts.Players
{
    public class PlayerAnimatorController : MonoBehaviour, IAnimationStateReader
    {
        public event Action<PlayerAnimatorState> StateEntered;
        public event Action<PlayerAnimatorState> StateExited;

        private Animator _animator;
        
        private const string WorkState = "Work Layer";
        private readonly int _walking = Animator.StringToHash("Walking");

        private int indexLayer;

        private PlayerAnimatorState State { get; set; }

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            indexLayer = _animator.GetLayerIndex(WorkState);
        }
        
        public void PlayWalking(float speed) => _animator.SetFloat(_walking,speed);

        public void SetWeightLayer(int weight) => _animator.SetLayerWeight(indexLayer,weight);

        public void EnteredState(int stateHash)
        {
            State = StateFor(stateHash);
            StateEntered?.Invoke(State);
        }

        public void ExitedState(int stateHash)
        {
            StateExited?.Invoke(StateFor(stateHash));
        }

        private PlayerAnimatorState StateFor(int stateHash)
        {
            PlayerAnimatorState state;

            if (stateHash == _walking)
                state = PlayerAnimatorState.Walking;
            else
                state = PlayerAnimatorState.Idle;
           
            return state;
        }
    }
}