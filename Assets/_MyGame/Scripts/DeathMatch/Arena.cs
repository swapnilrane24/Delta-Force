using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Curio.Gameplay
{
    public class Arena : MonoBehaviour
    {
        [SerializeField] private TeamManager redTeamManager, blueTeamManager;
        [SerializeField] private VCDolly[] virtualDollyCams;

        public NavMeshData navMeshData;
       

        private int activeCameraIndex;

        NavMeshDataInstance[] instances = new NavMeshDataInstance[1];

        public TeamManager RedTeamManager => redTeamManager;
        public TeamManager BlueTeamManager => blueTeamManager;

        private void Start()
        {
            for (int i = 0; i < virtualDollyCams.Length; i++)
            {
                virtualDollyCams[i].gameObject.SetActive(false);
            }

            virtualDollyCams[activeCameraIndex].gameObject.SetActive(true);
            virtualDollyCams[activeCameraIndex].ActivateDollyCamera();
            virtualDollyCams[activeCameraIndex].virtualCamReachedTrackEndEvent.AddListener(virtualCamReachedTrackEndEventListner);

            GameManager.Instance.onLevelStartEvent.AddListener(DisableAllCameras);
        }

        public void InitializeArena()
        {
            instances[0] = NavMesh.AddNavMeshData(navMeshData);
        }

        private void OnDisable()
        {
            NavMesh.RemoveAllNavMeshData();
            GameManager.Instance.onLevelStartEvent.RemoveListener(DisableAllCameras);
        }

        private void SwitchCamera()
        {
            virtualDollyCams[activeCameraIndex].gameObject.SetActive(false);

            activeCameraIndex++;

            activeCameraIndex = activeCameraIndex % virtualDollyCams.Length;

            virtualDollyCams[activeCameraIndex].gameObject.SetActive(true);
            virtualDollyCams[activeCameraIndex].ActivateDollyCamera();
            virtualDollyCams[activeCameraIndex].virtualCamReachedTrackEndEvent.AddListener(virtualCamReachedTrackEndEventListner);
        }

        private void virtualCamReachedTrackEndEventListner()
        {
            virtualDollyCams[activeCameraIndex].virtualCamReachedTrackEndEvent.RemoveListener(virtualCamReachedTrackEndEventListner);
            SwitchCamera();
        }

        private void DisableAllCameras()
        {
            for (int i = 0; i < virtualDollyCams.Length; i++)
            {
                virtualDollyCams[i].gameObject.SetActive(false);
            }
        }

    }
}