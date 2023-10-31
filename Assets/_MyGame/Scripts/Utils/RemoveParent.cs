using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    public class RemoveParent : MonoBehaviour
    {
        private void Start()
        {
            transform.SetParent(null);
        }
    }
}