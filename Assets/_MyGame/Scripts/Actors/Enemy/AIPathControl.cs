using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace Curio.Gameplay
{
    [RequireComponent(typeof(AIPath))]
    public class AIPathControl : MonoBehaviour
    {
        [SerializeField] private AIPath aiPath;

        public bool IsStopped => agent.isStopped;

        protected IAstarAI agent;

        private void Awake()
        {
            agent = GetComponent<IAstarAI>();

        }

        public void SetSpeed(float value)
        {
            aiPath.maxSpeed = value;
        }

        public void MoveAI(bool value)
        {
            agent.canMove = value;
        }

        public bool ReachedDestination()
        {
            return agent.reachedEndOfPath && !agent.pathPending;
        }

        public void SetDestination(Vector3 target)
        {
            agent.destination = target;
            agent.SearchPath();
            agent.canMove = true;
        }

        public void CalculateFleePath(Vector3 fleeFromPosition)
        {
            FleePath path = FleePath.Construct(transform.position, fleeFromPosition, 5);
            SetDestination(path.endPoint);
        }


    }
}