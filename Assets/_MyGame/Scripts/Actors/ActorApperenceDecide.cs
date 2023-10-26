using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Curio.Gameplay
{
    public class ActorApperenceDecide : MonoBehaviour
    {
        [System.Serializable]
        public class ActorAperenceDetails
        {
            
            public SkinnedMeshRenderer body;
            public SkinnedMeshRenderer head;
        }

        public Material redTeamMat, blueTeamMat;
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

            //0 id is for red team
            actorAperenceList[randomBodyIndex].body.material = teamID == 0 ? redTeamMat : blueTeamMat;

            int randomHeadIndex = Random.Range(0, actorAperenceList.Length);
            actorAperenceList[randomHeadIndex].head.gameObject.SetActive(true);
            actorAperenceList[randomHeadIndex].head.material = teamID == 0 ? redTeamMat : blueTeamMat;
        }



    }
}