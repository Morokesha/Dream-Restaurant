using UnityEngine;

namespace Game.Scripts.Logic.Visitors.Waypoints
{
    public class VisitorWaypoints : MonoBehaviour
    {
        public Waypoint[] Waypoints;

        private int _currentIndexPoint;

        public Waypoint GetNextWaypointPos()
        {
            if (Waypoints.Length == 0)
                return null;

            Waypoint nextWaypoint = null;

            var nextIndex = _currentIndexPoint + 1;

            if (nextIndex <= Waypoints.Length)
            {
                nextWaypoint = Waypoints[_currentIndexPoint];

                _currentIndexPoint = nextIndex;

                if (_currentIndexPoint > Waypoints.Length) return null;
            }

            return nextWaypoint;
        }
    }
}