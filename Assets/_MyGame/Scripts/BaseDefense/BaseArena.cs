using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Curio.Gameplay
{
    public class BaseArena : MonoBehaviour
    {
        [System.Serializable]
        private struct BatterySection
        {
            public Battery[] batteries;
        }

        public NavMeshData navMeshData;
        [SerializeField] private Transform playerSpawnPoint;
        [SerializeField] private Transform[] enemySpawnPoint;
        [SerializeField] private float enemySpawnRange = 1.5f;

        [SerializeField] private BatterySection[] batterySection;

        //[SerializeField] private Battery[] batteries;

        NavMeshDataInstance[] instances = new NavMeshDataInstance[1];

        public Transform PlayerSpawnPoint => playerSpawnPoint;

        private List<Battery> availableBatteries;
        private int totalBatteryDestroyed = 0;
        private int totalBattries;

        private void OnDisable()
        {
            NavMesh.RemoveAllNavMeshData();
        }

        public void InitializeArena(float batteryHealthMultiplier)
        {
            instances[0] = NavMesh.AddNavMeshData(navMeshData);

            availableBatteries = new List<Battery>();

            int batterySectionIndex = Random.Range(0, batterySection.Length);

            for (int i = 0; i < batterySection[batterySectionIndex].batteries.Length; i++)
            {
                batterySection[batterySectionIndex].batteries[i].gameObject.SetActive(true);
                availableBatteries.Add(batterySection[batterySectionIndex].batteries[i]);
                batterySection[batterySectionIndex].batteries[i].InitializeBattery(batteryHealthMultiplier, this);
            }

            totalBattries = availableBatteries.Count;
            BaseDefenseManager.Instance.SetBatteryRemaining(availableBatteries.Count.ToString());
        }

        public Vector3 GetEnemySpawnPosition()
        {
            Vector3 pos = enemySpawnPoint[Random.Range(0,enemySpawnPoint.Length)].position + Random.insideUnitSphere * 1.5f;
            pos.y = 0;

            return pos;
        }

        public void BatteryDestroyed(Battery battery)
        {
            if (availableBatteries.Contains(battery))
            {
                availableBatteries.Remove(battery);
            }

            totalBatteryDestroyed++;
            BaseDefenseManager.Instance.SetBatteryRemaining((totalBattries - totalBatteryDestroyed).ToString());
            if (totalBatteryDestroyed >= totalBattries)
            {
                BaseDefenseManager.Instance.LostBase(battery);
            }
        }

        public IActor GetBatteryTarget()
        {
            IActor target = null;
            if (GameManager.Instance.GameState == GameState.PLAYING)
            {
                int index = Random.Range(0, availableBatteries.Count);

                if (index >= 0)
                    target = availableBatteries[index];
            }

            return target;
        }

    }
}