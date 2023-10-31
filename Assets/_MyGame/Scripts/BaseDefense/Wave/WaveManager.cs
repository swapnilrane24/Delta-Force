using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    [System.Serializable]
    public class WaveManager
    {
        private BaseDefenseManager _baseDefenseManager;
        private WaveData currentWave;
        private int enemyKillCount;

        private int enemySpawned;
        private float delayTime;
        private bool canStartSpawning;

        public bool waveComplete => enemyKillCount >= currentWave.enemyCount;

        public void SetBaseDefenseManager(BaseDefenseManager baseDefenseManager)
        {
            _baseDefenseManager = baseDefenseManager;
        }

        public void SetWaveData(WaveData waveData)
        {
            currentWave = waveData;
            enemySpawned = 0;
            enemyKillCount = 0;
            delayTime = currentWave.initialDelay;
            canStartSpawning = true;
        }

        public void OnUpdate()
        {
            if (currentWave == null || canStartSpawning == false) return;

            if (enemySpawned < currentWave.enemyCount)
            {
                delayTime -= Time.deltaTime;
                if (delayTime <= 0)
                {
                    delayTime = currentWave.spawnRate;
                    IActor enemy = _baseDefenseManager.GetEnemy();
                    enemy.OnDeadEvent.AddListener(EnemyKilled);
                    enemySpawned++;
                }
            }
        }

        public void EnemyKilled(IActor enemy)
        {
            enemy.OnDeadEvent.RemoveListener(EnemyKilled);
            enemyKillCount++;
            if (enemyKillCount >= currentWave.enemyCount)
            {
                canStartSpawning = false;
                _baseDefenseManager.WaveComplete();
            }
        }
    }
}