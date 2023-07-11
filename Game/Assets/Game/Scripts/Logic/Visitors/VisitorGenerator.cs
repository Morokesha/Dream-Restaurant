using Game.Scripts.Common;
using Game.Scripts.Core.Factory;
using Game.Scripts.Core.StaticDataService;
using Game.Scripts.Logic.Pools;

namespace Game.Scripts.Logic.Visitors
{
    public class VisitorGenerator
    {
        private IGameFactory _gameFactory;

        private Pool<Visitor> _pool;
        private IStaticDataService _staticData;

        public void Init(IStaticDataService dataService, IGameFactory gameFactory)
        {
            _staticData = dataService;
            _gameFactory = gameFactory;
            _pool = new Pool<Visitor>();
        }

        public void CreateVisitor(VisitorDefaultConfig.VisitorDefaultConfig[] defaultConfigs)
        {
            foreach (var defaultConfig in defaultConfigs)
                for (var i = 0; i < defaultConfig.DefaultAmount; i++)
                    GenerateVisitor(defaultConfig.VisitorType);
        }

        private void GenerateVisitor(VisitorType visitorType)
        {
            Visitor visitor = _gameFactory.CreateVisitor(_staticData, visitorType);

            _pool.Add(visitor);
            _pool.ReturnToPool(visitor, visitorType);
        }

        public Visitor GetInactiveVisitor(VisitorType type)
        {
            return _pool.GetInactiveObject(type);
        }
    }
}