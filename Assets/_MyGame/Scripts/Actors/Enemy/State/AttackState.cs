using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    public class AttackState : BaseState
    {
        #region Serialized Variables
        [Header("X is Left, Right movement, Y is Forward, Backward movement")]
        [SerializeField] private Vector2[] movementPatterns;
        [SerializeField] private float waitBeforeMovement = 3;
        [SerializeField] private float moveTime = 3;
        [SerializeField] private float minAttackRange = 2f;
        #endregion

        #region Private Variables
        private float currentAttackRate;
        private int currentPatternIndex = 0;
        #endregion

        private float currentWaitTime;
        private float currentMoveTime;
        private Vector2 currentPattern;

        #region Getters & Setters
        #endregion

        #region Unity Methods
        private void Awake()
        {
            state = StateEnum.ATTACK;
        }
        #endregion

        #region Private Methods
        #endregion

        #region Public Methods
        public override void FixedTick()
        {

        }

        public override void StateEnter()
        {
            currentPatternIndex = 0;
            aiController.StopMovement();
            currentAttackRate = 0;
            transform.LookAt(aiController.Target.ActorTransfrom.position);
            currentWaitTime = waitBeforeMovement;
        }

        public override void StateExit()
        {

        }

        public override void Tick()
        {
            if (aiController.Target != null)
            {
                Vector3 targetPosition = aiController.Target.ActorTransfrom.position;
                targetPosition.y = transform.position.y;

                //we make sure enemy in attack more will keep attacking event if the target is 25% more out of attack range
                if (Vector3.Distance(transform.position, aiController.Target.ActorTransfrom.position) <= aiController.AttackDistance +
                    aiController.AttackDistance * 0.25f)
                {
                    Attack();
                    transform.LookAt(targetPosition);
                }
                else
                {
                    stateMachine.SwitchToNextState(StateEnum.CHASE);
                }
            }
            else
            {
                stateMachine.SwitchToNextState(StateEnum.IDLE);
            }

            if (currentWaitTime > 0)
            {
                currentWaitTime -= Time.deltaTime;
                if (currentWaitTime <= 0)
                {
                    currentMoveTime = moveTime;
                    currentPatternIndex = Random.Range(0, movementPatterns.Length);
                    currentPattern = movementPatterns[currentPatternIndex % movementPatterns.Length];
                }
            }
            else
            {
                if (currentMoveTime > 0)
                {
                    currentMoveTime -= Time.deltaTime;
                    PlayCurrentPattern();
                    if (currentMoveTime <= 0)
                    {
                        currentMoveTime = moveTime;
                        currentPatternIndex = Random.Range(0, movementPatterns.Length);
                        currentPattern = movementPatterns[currentPatternIndex % movementPatterns.Length];
                        //PlayCurrentPattern();
                    }
                }
            }
        }

        public void Attack()
        {
            aiController.SelectedWeapon.TargetDirection = transform.forward;
            aiController.SelectedWeapon.OnUpdate();
            aiController.SelectedWeapon.CheckForUserInput();
        }

        private void PlayCurrentPattern()
        {
            //if(aiController.NavMeshAgent.isStopped) aiController.NavMeshAgent.isStopped = false;

            aiController.EnemyActor.SetActorWalkAnim(true);
            aiController.EnemyActor.SetActorDirectionAnim(currentPattern);
            Vector3 destination = transform.position + new Vector3(currentPattern.x, 0, currentPattern.y);
            transform.Translate(new Vector3(currentPattern.x, 0, currentPattern.y) * aiController.CurrentMoveSpeed * 1.5f * Time.deltaTime);
        }

        #endregion
    }
}