using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Curio.Gameplay
{
    public class FleeState : BaseState
    {
        Vector3 fleeDestination;
        float currentWaitBeforeAiSwitchState;

        private void Awake()
        {
            state = StateEnum.FLEE;
        }

        public override void FixedTick()
        {
            
        }

        public override void StateEnter()
        {
            stateMachine.SwitchToNextState(StateEnum.IDLE);

            //currentWaitBeforeAiSwitchState = Time.time + 3f;
            //aiController.StartMovement();
            //if (aiController.Target)
            //{
            //    //aiController.AIPathControl.CalculateFleePath(aiController.Target.transform.position);
            //    //if (TryGetFleePosition(aiController.Target.transform.position, out fleeDestination))
            //    //{
            //    //    //aiController.NavMeshAgent.SetDestination(fleeDestination);
            //    //    aiController.AIPathControl.SetDestination(fleeDestination);
            //    //    aiController.EnemyActor.SetActorWalkAnim(true);
            //    //    aiController.EnemyActor.SetActorDirectionAnim(Vector2.up);
            //    //}
            //    //else
            //    //{
            //    //    stateMachine.SwitchToNextState(StateEnum.IDLE);
            //    //}
            //}
            //else
            //{
            //    stateMachine.SwitchToNextState(StateEnum.IDLE);
            //}
        }

        public override void StateExit()
        {
            
        }

        public override void Tick()
        {
            //if (currentWaitBeforeAiSwitchState < Time.time)
            //{
            //    if (aiController.NavMeshAgent.remainingDistance <= 1f || aiController.Target == null)
            //    //if(aiController.ReachedDistination() || aiController.Target == null)
            //    {
            //        stateMachine.SwitchToNextState(StateEnum.IDLE);
            //    }
            //}
        }

        public bool TryGetFleePosition(Vector3 threatPosition, out Vector3 fleePosition)
        {
            for (int i = 0; i < 10; i++)
            {
                // Calculate the direction away from the threat
                Vector3 fleeDirection = (transform.position - threatPosition).normalized;

                // Randomize the direction slightly (optional but can lead to more varied flee positions)
                fleeDirection = Quaternion.Euler(0, Random.Range(-45f, 45f), 0) * fleeDirection;

                // Calculate the potential flee position
                Vector3 potentialFleePosition = transform.position + fleeDirection * 10;

                // Check if this position is on the NavMesh
                NavMeshHit hit;
                if (NavMesh.SamplePosition(potentialFleePosition, out hit, 10, NavMesh.AllAreas))
                {
                    fleePosition = hit.position;
                    return true;
                }
            }

            fleePosition = Vector3.zero;
            return false;
        }
    }
}