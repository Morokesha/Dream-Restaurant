using UnityEngine;

namespace Game.Scripts.Core.InputServices
{
    public interface IInputService : IService
    {
        Vector3 InputAxis();
    }
}