using System;
using Game.Scripts.Logic.Tables;
using Game.Scripts.Logic.Visitors.Waypoints;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Scripts.Logic.Visitors
{
    public class VisitorMover
    {
        public event Action ReachedTargetDestination;
        public event Action ReachedToLastWaypoint;

        private readonly VisitorAnimatorController _visitorAnimatorController;
        private NavMeshAgent _agent;
        private Waypoint _currentWaypoint;
        private Waypoint[] _myPath;
        private VisitorWaypoints _waypoints;
        private readonly Chair _chair;

        private int _currentIndex;
        private bool _alreadyMove;

        public VisitorMover(VisitorAnimatorController visitorAnimatorController, NavMeshAgent agent,
            VisitorWaypoints waypoints)
        {
            _visitorAnimatorController = visitorAnimatorController;
            _agent = agent;
            _waypoints = waypoints;
            _myPath = _waypoints.Waypoints;
        }
        
        public void Move()
        {
            if (_agent.enabled == false)
                _agent.enabled = true;
            
            if (_alreadyMove)
            {
                if (_agent.remainingDistance < 0.3f && !_agent.pathPending)
                {
                    _alreadyMove = false;
                }
            }
            else
            {
                _alreadyMove = TryMoveToNextPoint();
            }
        }

        public void MoveTo(Transform target, Transform currentTransform)
        {
            if (_agent.enabled == false)
                _agent.enabled = true;
            
            if (_alreadyMove)
            {
                if (_agent.remainingDistance < 0.05f && !_agent.pathPending)
                {
                    _alreadyMove = false;

                    float distance = Vector3.Distance(currentTransform.position, target.position);

                    if (distance < 0.3f)
                    {
                        ReachedTargetDestination?.Invoke();
                    }
                }
            }
            else
            {
                _alreadyMove = true;
                
                _agent.SetDestination(target.position);
                _visitorAnimatorController.PlayWalking();
            }
        }

        private bool TryMoveToNextPoint()
        {
            var waypoint = GetNextWaypointPos();
            
            if (waypoint != null)
            {
                _currentWaypoint = waypoint;

                _agent.SetDestination(_currentWaypoint.transform.position);
                _visitorAnimatorController.PlayWalking();
                
                return true;
            }
            
            return false;
        }

        private Waypoint GetNextWaypointPos()
        {
            if (_myPath.Length == 0) return null;
            Waypoint nextWaypoint = null;

            var nextIndex = _currentIndex + 1;

            if (nextIndex <= _myPath.Length)
            {
                nextWaypoint = _myPath[_currentIndex];

                _currentIndex = nextIndex;
            }
            else
            {
                ReachedToLastWaypoint?.Invoke();
            }

            return nextWaypoint;
        }
    }
}