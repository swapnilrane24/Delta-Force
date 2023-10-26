using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    public class LookAtCamera : MonoBehaviour
    {

        private void LateUpdate()
        {
            Vector3 cameraPos = new Vector3(Camera.main.transform.position.x, transform.position.y,
                Camera.main.transform.position.z);
            transform.LookAt(cameraPos);
        }

    }
}