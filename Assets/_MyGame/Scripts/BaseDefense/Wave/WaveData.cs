using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    [System.Serializable]
    public class WaveData 
    {
        public float initialDelay;
        public int enemyCount;
        public float spawnRate;
        public float healthMultiplier;
        public float damageMultiplier;
    }
}