using UnityEngine;

namespace Game.Scripts.Core.InputServices
{
    public class InputService : IInputService
    {
        public Vector3 InputAxis()
        {
            return new Vector3(Joystick.Instance.Horizontal,0f,Joystick.Instance.Vertical);
        }
    }
}