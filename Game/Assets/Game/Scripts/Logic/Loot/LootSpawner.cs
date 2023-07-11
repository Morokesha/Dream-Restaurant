using System.Numerics;
using Game.Scripts.Common;
using Game.Scripts.Core.Factory;
using Game.Scripts.Core.StaticDataService;
using Vector3 = UnityEngine.Vector3;

namespace Game.Scripts.Logic.Loot
{
    public class LootSpawner
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IGameFactory _gameFactory;

        public LootSpawner(IStaticDataService staticDataService, IGameFactory gameFactory)
        {
            _staticDataService = staticDataService;
            _gameFactory = gameFactory;
        }

        public void CreateLoot(Vector3 spawnPosition, int amountLoot,LootType type)
        {
            for (int i = 0; i < amountLoot; i++)
            {
                _gameFactory.CreateLoot(_staticDataService, type,spawnPosition);
            }
        }
    }
}