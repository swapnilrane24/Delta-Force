using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    public class FollowTarget : MonoBehaviour
    {
        [SerializeField] private bool canDetachFromParent;
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 followAxis = Vector3.one;

        private void OnEnable()
        {
            if (canDetachFromParent)
                transform.parent = null;
        }

        public void SetTarget(Transform targetValue)
        {
            target = targetValue;
        }

        private void LateUpdate()
        {
            if (target)
            {
                transform.position = new Vector3(target.position.x * followAxis.x, target.position.y * followAxis.y,
                    target.position.z * followAxis.z);
            }
        }
    }
}