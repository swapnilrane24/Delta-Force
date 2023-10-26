using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Curio.Gameplay
{
    public class KillUIData
    {
        public string killerName;
        public string victimName;
        public bool isRedTeam;

        public KillUIData(string killerNameValue, string victimNameValue, bool isRedTeamValue)
        {
            killerName = killerNameValue;
            victimName = victimNameValue;
            isRedTeam = isRedTeamValue;
        }

    }

    public class KillDataUI : MonoBehaviour
    {
        [SerializeField] private KillInfoUI killUpdateUIPrefab;
        [SerializeField] private Transform container;

        private ObjectPoolGeneric<KillInfoUI> killInfoPool;

        private void Start()
        {
            killInfoPool = new ObjectPoolGeneric<KillInfoUI>(SpawnKillInfo, ActivateKillInfo, DeactivateKillInfo);
            DeathMatchManager.Instance.KillDataUI = this;
        }

        public void PopulateData(KillUIData killUIData)
        {
            KillInfoUI killInfoUI = killInfoPool.GetObject();
            killInfoUI.transform.SetParent(container);
            killInfoUI.SetKillUIData(killUIData);
            //killInfoUI.gameObject.SetActive(true);
            killInfoUI.SetVisibility(true);
        }

        public KillInfoUI SpawnKillInfo()
        {
            KillInfoUI killInfo = Instantiate(killUpdateUIPrefab, transform);
            killInfo.SetObjectPoolGeneric(killInfoPool);
            return killInfo;
        }

        public void ActivateKillInfo(KillInfoUI killInfoUI)
        {
            killInfoUI.SetVisibility(true);
            //killInfoUI.gameObject.SetActive(true);
        }

        public void DeactivateKillInfo(KillInfoUI killInfoUI)
        {
            killInfoUI.transform.SetParent(transform);// transform;
            killInfoUI.SetVisibility(false);
        }

    }
}