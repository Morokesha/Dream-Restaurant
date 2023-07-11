using Cinemachine;
using Game.Scripts.Core.InputServices;
using UnityEngine;

namespace Game.Scripts.Players
{
    public class PlayerMovement
    {
        private const float MinSensitive = 0.001f;
        
        private IInputService _inputService;
        private PlayerAnimatorController _playerAnimatorController;
        private CharacterController _characterController;

        private float _movementSpeed = 5f;

        public void Init(IInputService inputService, PlayerAnimatorController playerAnimatorController, 
            CharacterController characterController)
        {
            _inputService = inputService;
            _playerAnimatorController = playerAnimatorController;
            _characterController = characterController;
        }

        public void Move(Transform playerTransform,float deltaTime)
        {
            Vector3 directionMovement = Vector3.zero;

            if (_inputService.InputAxis().sqrMagnitude > MinSensitive)
            {
                directionMovement = _inputService.InputAxis();

                playerTransform.forward = directionMovement;
            }

            directionMovement += Physics.gravity;

            _characterController.Move(directionMovement * _movementSpeed * deltaTime);

            _playerAnimatorController.PlayWalking(_characterController.velocity.magnitude);
        }
    }
}