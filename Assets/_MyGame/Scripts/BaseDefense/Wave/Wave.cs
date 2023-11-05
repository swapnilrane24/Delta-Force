using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    [CreateAssetMenu(fileName = "Wave", menuName = "Curio/Wave")]
    public class Wave : ScriptableObject
    {
        public float batteryLifeMultiplier = 1f;
        public WaveData[] waveDatas;
    }
}