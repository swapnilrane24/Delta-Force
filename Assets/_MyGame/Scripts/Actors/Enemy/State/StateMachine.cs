using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Curio.Gameplay
{
    public class StateMachine : MonoBehaviour
    {
        #region Serialized Variables
        [SerializeField] private StateEnum startState;
        #endregion

        #region Private Variables
        private Enemy _aiController;
        private List<BaseState> availableStates = new List<BaseState>();
        private BaseState currentState;
        private bool initialized;
        #endregion

        public Type CurrentStateType { get => currentState?.GetType(); }
        public event Action<BaseState> OnStateChanged;

        #region Getters & Setters
        public BaseState CurrentState { get => currentState; }
        #endregion

        #region Unity Methods
        public void Initialize(Enemy aIController)
        {
            _aiController = aIController;
            foreach (BaseState state in GetComponents<BaseState>())
            {
                if (!availableStates.Contains(state))
                {
                    state.Controller = _aiController;
                    state.StateMachine = this;
                    availableStates.Add(state);
                }
            }

            initialized = true;
        }

        // Update is called once per frame
        public void OnUpdate()
        {
            if (initialized)
            {
                if (currentState == null)
                {
                    SwitchToNextState(startState);
                }

                currentState?.Tick();
            }
        }

        public void OnFixedUpdate()
        {
            //if (GameManager.gameStatus != GameStatus.Playing) return;

            currentState?.FixedTick();
        }
        #endregion

        #region Private Methods
        public void SwitchToNextState(StateEnum stateEnum)
        {
            currentState?.StateExit();
            bool stateChanged = false;
            for (int i = 0; i < availableStates.Count; i++)
            {
                if (availableStates[i].State == stateEnum)
                {
                    currentState = availableStates[i];
                    //_aiController.OnStateChange();
                    currentState.StateEnter();
                    stateChanged = true;
                    break;
                }
            }

            if (stateChanged)
            {
                OnStateChanged?.Invoke(CurrentState);
            }
            else
            {
                //Debug.LogWarning("Requested State not Present:-" + stateEnum.ToString());
                SwitchToNextState(StateEnum.IDLE);
            }
        }
        #endregion

        #region Public Methods
        #endregion
    }
}