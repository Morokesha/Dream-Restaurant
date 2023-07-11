using Game.Scripts.Common;

namespace Game.Scripts.Logic.Visitors.VisitorDefaultConfig
{
    public class VisitorDefaultConfig
    {
        public VisitorDefaultConfig(VisitorType type, int defaultAmount)
        {
            VisitorType = type;
            DefaultAmount = defaultAmount;
        }

        public int DefaultAmount { get; }

        public VisitorType VisitorType { get; }
    }
}