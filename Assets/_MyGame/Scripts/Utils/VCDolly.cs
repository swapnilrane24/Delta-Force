using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

namespace Curio.Gameplay
{
    public class VCDolly : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera vCam;
        [SerializeField] private CinemachineSmoothPath dollyTrack;
        [SerializeField] private CinemachineDollyCart dollyCart;

        public UnityEvent virtualCamReachedTrackEndEvent;

        bool canFollowPath = true;

        public void ActivateDollyCamera()
        {
            dollyCart.m_Position = 0;
            canFollowPath = true;
        }

        private void Update()
        {
            if (canFollowPath)
            {
                if (Mathf.Abs(dollyCart.m_Position - dollyTrack.PathLength) <= 0.5f)
                {
                    canFollowPath = false;
                    virtualCamReachedTrackEndEvent?.Invoke();
                }
            }
        }
    }
}