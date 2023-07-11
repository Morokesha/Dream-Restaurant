using UnityEngine;

namespace Game.Scripts.Core
{
    public class GameBootstrapper : MonoBehaviour
    {
        private GameInitialize _gameInitialize;

        private void Awake()
        {
            _gameInitialize = new GameInitialize();
            
            _gameInitialize.InitLevel();
            DontDestroyOnLoad(gameObject);
        }
    }
}