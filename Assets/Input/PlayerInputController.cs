using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Input = UnityEngine.Input;

namespace Curio.Gameplay
{
    public class PlayerInputController : MonoBehaviour
    {
        public Vector2 movement;
        public Vector2 look;
        public bool fire;

        [SerializeField] private PlayerCharacterController character;

        private CharacterInputs inputs = new CharacterInputs();

        public void MovementInput(InputAction.CallbackContext callbackContext)
        {
            movement = callbackContext.ReadValue<Vector2>();
        }

        public void LookInput(InputAction.CallbackContext callbackContext)
        {
            look = callbackContext.ReadValue<Vector2>();
        }

        public void FireInput(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.started)
            {
                fire = true;
            }
            else if (callbackContext.canceled)
            {
                fire = false;
            }
        }



        private void Update()
        {
            if (GameManager.Instance.GameState == GameState.PLAYING)
            {
                inputs.moveVector = movement;
                inputs.lookVector = look;

                inputs.fire = fire;

                character.SetInputs(inputs);
            }
        }

    }
}