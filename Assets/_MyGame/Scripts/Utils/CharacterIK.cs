using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    [RequireComponent(typeof(Animator))]
    public class CharacterIK : MonoBehaviour
    {
        protected Animator animator;

        public bool ikActive = false;
        public Transform _leftHandTarget = null;
        public Transform _rightHandTarget = null;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void OnAnimatorIK()
        {
            if (animator)
            {
                if (ikActive)
                {
                    if (_leftHandTarget != null)
                    {
                        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                        animator.SetIKPosition(AvatarIKGoal.LeftHand, _leftHandTarget.position);
                        animator.SetIKRotation(AvatarIKGoal.LeftHand, _leftHandTarget.rotation);
                    }

                    if (_rightHandTarget != null)
                    {
                        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                        animator.SetIKPosition(AvatarIKGoal.RightHand, _rightHandTarget.position);
                        animator.SetIKRotation(AvatarIKGoal.RightHand, _rightHandTarget.rotation);
                    }
                }
                else
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);

                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);

                    //animator.SetLookAtWeight(0);
                }
            }
        }

        public void SetIK(Transform leftHandTarget)
        {
            _leftHandTarget = leftHandTarget;
            ikActive = true;
        }

        public void ActivateIK(bool value)
        {
            ikActive = value;

            //animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, value == true ? 1 : 0);
            //animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, value == true ? 1 : 0);

            //animator.SetIKPositionWeight(AvatarIKGoal.RightHand, value == true ? 1 : 0);
            //animator.SetIKRotationWeight(AvatarIKGoal.RightHand, value == true ? 1 : 0);
        }

    }
}