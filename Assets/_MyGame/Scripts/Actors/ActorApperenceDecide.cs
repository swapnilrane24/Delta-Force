using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Curio.Gameplay
{
    public class ActorApperenceDecide : MonoBehaviour
    {
        [SerializeField] private bool isPlayer;
        [System.Serializable]
        public class ActorAperenceDetails
        {
            
            public SkinnedMeshRenderer body;
            public SkinnedMeshRenderer head;
        }

        public Material redTeamMat, blueTeamMat;
        public Material[] randomMatList;
        public MeshRenderer backpack;
        public ActorAperenceDetails[] actorAperenceList;


        //[Button("SelectApperence")]
        public void SelectApperence(int teamID)
        {
            for (int i = 0; i < actorAperenceList.Length; i++)
            {
                actorAperenceList[i].body.gameObject.SetActive(false);
                actorAperenceList[i].head.gameObject.SetActive(false);
            }

            int randomBodyIndex = Random.Range(0, actorAperenceList.Length);
            actorAperenceList[randomBodyIndex].body.gameObject.SetActive(true);

            int randomHeadIndex = Random.Range(0, actorAperenceList.Length);
            actorAperenceList[randomHeadIndex].head.gameObject.SetActive(true);
            if (GameManager.Instance.GameMode == GameMode.DEATHMATCH)
            {
                //0 id is for red team
                actorAperenceList[randomBodyIndex].body.material = teamID == 0 ? redTeamMat : blueTeamMat;
                actorAperenceList[randomHeadIndex].head.material = teamID == 0 ? redTeamMat : blueTeamMat;
            }
            else if (GameManager.Instance.GameMode == GameMode.DEFENSE)
            {
                int randomIndex = 0;
                if (BaseDefenseManager.Instance)
                {
                    randomIndex = BaseDefenseManager.Instance.CurrentWaveIndex;
                    if (randomIndex >= randomMatList.Length)
                        randomIndex = randomMatList.Length - 1;
                }

                if (isPlayer)
                {
                    actorAperenceList[randomBodyIndex].body.material = blueTeamMat;
                    actorAperenceList[randomHeadIndex].head.material = blueTeamMat;
                }
                else
                {
                    actorAperenceList[randomBodyIndex].body.material = randomMatList[randomIndex];
                    actorAperenceList[randomHeadIndex].head.material = randomMatList[randomIndex];
                }
            }
        }



    }
}