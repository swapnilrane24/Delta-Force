using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using UnityEngine.Windows;
using UnityEngine.InputSystem;
using MoreMountains.Feedbacks;

namespace Curio.Gameplay
{
    [System.Serializable]
    public struct CharacterInputs
    {
        public Vector2 moveVector;
        public Vector2 lookVector;
        public bool fire;
    }

    public class PlayerCharacterController : MonoBehaviour, ICharacterController
    {
        //[SerializeField] private float mouseSentivity = 0.5f;
        //[SerializeField] private PlayerActor playerActor;

        [SerializeField] private MMF_Player shootFeedback;
        [SerializeField] private KinematicCharacterMotor _characterMotor;
        [Header("Stable Movement")]
        [SerializeField] private float _maxStableMoveSpeed = 10f;
        [SerializeField] private float _stableMovementSharpness = 15;
        [SerializeField] private float _orientationSharpness = 10;

        [Header("Air Movement")]
        [SerializeField] private float _maxAirMoveSpeed = 10f;
        [SerializeField] private float _airAccelerationSpeed = 5f;
        [SerializeField] private float _drag = 0.1f;

        [Header("Misc")]
        public Vector3 _gravity = new Vector3(0, -30f, 0);

        private PlayerActor playerActor;
        //private Transform aimTarget;

        private Vector3 _moveInputVector;
        private Vector3 _lookInputVector;
        private Quaternion lookAngle;
        private Vector2 currentAnimationBlendVector;
        private Vector2 animationVelocity;
        private float animationSmoothTime = 0.1f;

        // cinemachine
        private float _cinemachineTargetYaw;

        private Vector3 forward, right;

        public void InitializePlayerController(PlayerActor actor)
        {
            playerActor = actor;

            Cursor.lockState = CursorLockMode.Locked;
            _characterMotor.CharacterController = this;
            //aimTarget = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
            //aimTarget.localScale = Vector3.one * 0.5f;
            //aimTarget.GetComponent<Collider>().enabled = false;

            _cinemachineTargetYaw = transform.rotation.eulerAngles.y;
        }

        public void SetInputs(CharacterInputs inputs)
        {
            if (playerActor.IsAlive == false || GameManager.Instance.GameState != GameState.PLAYING) return;

            forward = Camera.main.transform.forward.normalized;
            right = Camera.main.transform.right.normalized;

            Vector2 moveClamped = Vector2.ClampMagnitude(inputs.moveVector, 1);

            currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, moveClamped, ref animationVelocity, animationSmoothTime);

            _moveInputVector = forward * currentAnimationBlendVector.y + right * currentAnimationBlendVector.x;

            CameraRotation(inputs.lookVector);

            playerActor.SetActorDirectionAnim(currentAnimationBlendVector);
            //playerActor.SetActorDirectionAnim(inputs.moveVector);
            playerActor.SetActorWalkAnim(_moveInputVector.sqrMagnitude > 0.1f ? true : false);

            if (playerActor.SelectedWeapon)
            {
                playerActor.SelectedWeapon.OnUpdate();
                if (inputs.fire)
                {
                    shootFeedback.PlayFeedbacks();
                    playerActor.SelectedWeapon.TargetDirection = transform.forward;
                    playerActor.SelectedWeapon.CheckForUserInput();
                }
                else if (inputs.fire == false)
                {
                    playerActor.SelectedWeapon.ResetWeaponRate();
                }
            }
            

            playerActor.SetActorShootAnim(inputs.fire);
            //SetPlayerGrounded(_characterMotor.GroundingStatus.IsStableOnGround);
        }

        private void CameraRotation(Vector2 lookInput)
        {
            // if there is an input and camera position is not fixed
            if (lookInput.sqrMagnitude >= 0.01f)
            {
                _cinemachineTargetYaw += lookInput.x * GameManager.Instance.mouseSensitivity;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);

            lookAngle = Quaternion.Euler(0f, _cinemachineTargetYaw, 0.0f);
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        public void SetPosition(Vector3 position)
        {
            _characterMotor.SetPosition(position);
        }

        public void SetRotation(Quaternion rotation)
        {
            _characterMotor.SetRotation(rotation);
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            if (playerActor.IsAlive == false) return;

            if (_orientationSharpness > 0f)
            {
                currentRotation = lookAngle;
            }

        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (playerActor.IsAlive == false) return;

            Vector3 targetMovementVelocity = Vector3.zero;
            if (_characterMotor.GroundingStatus.IsStableOnGround)
            {
                // Reorient source velocity on current ground slope (this is because we don't want our smoothing to cause any velocity losses in slope changes)
                currentVelocity = _characterMotor.GetDirectionTangentToSurface(currentVelocity, _characterMotor.GroundingStatus.GroundNormal) * currentVelocity.magnitude;

                // Calculate target velocity
                Vector3 inputRight = Vector3.Cross(_moveInputVector, _characterMotor.CharacterUp);
                Vector3 reorientedInput = Vector3.Cross(_characterMotor.GroundingStatus.GroundNormal, inputRight).normalized * _moveInputVector.magnitude;
                targetMovementVelocity = reorientedInput * _maxStableMoveSpeed;

                // Smooth movement Velocity
                currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1 - Mathf.Exp(-_stableMovementSharpness * deltaTime));
            }
            else
            {
                // Add move input
                if (_moveInputVector.sqrMagnitude > 0f)
                {
                    targetMovementVelocity = _moveInputVector * _maxAirMoveSpeed;

                    // Prevent climbing on un-stable slopes with air movement
                    if (_characterMotor.GroundingStatus.FoundAnyGround)
                    {
                        Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(_characterMotor.CharacterUp, _characterMotor.GroundingStatus.GroundNormal), _characterMotor.CharacterUp).normalized;
                        targetMovementVelocity = Vector3.ProjectOnPlane(targetMovementVelocity, perpenticularObstructionNormal);
                    }

                    Vector3 velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - currentVelocity, _gravity);
                    currentVelocity += velocityDiff * _airAccelerationSpeed * deltaTime;
                }

                // Gravity
                currentVelocity += _gravity * deltaTime;

                // Drag
                currentVelocity *= (1f / (1f + (_drag * deltaTime)));
            }
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
            //throw new System.NotImplementedException();
        }

        public void PostGroundingUpdate(float deltaTime)
        {
            //throw new System.NotImplementedException();
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
            //throw new System.NotImplementedException();
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            //throw new System.NotImplementedException();
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            //throw new System.NotImplementedException();
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
            //throw new System.NotImplementedException();
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
            //throw new System.NotImplementedException();
        }
    }
}