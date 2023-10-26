using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Curio.Gameplay
{
    public class KillInfoUI : MonoBehaviour
    {
        [SerializeField] private Color blueColor, redColor;
        [SerializeField] private TogglePanel togglePanel;
        [SerializeField] private TextMeshProUGUI killerNametext;
        [SerializeField] private TextMeshProUGUI VictimNameText;
        [SerializeField] private float disapperTimer = 3;

        private float currentTime;

        private ObjectPoolGeneric<KillInfoUI> _objectPoolGeneric;

        public void SetKillUIData(KillUIData killUIData)
        {
            killerNametext.text = killUIData.killerName;
            VictimNameText.text = killUIData.victimName;

            killerNametext.color = killUIData.isRedTeam ? redColor : blueColor;
            VictimNameText.color = !killUIData.isRedTeam ? redColor : blueColor;

            currentTime = disapperTimer;
        }

        public void SetVisibility(bool isVisible)
        {
            togglePanel.ToggleVisibilityInstant(isVisible);
        }

        private void Update()
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                if (currentTime <= 0)
                {
                    ReturnItem();
                }
            }
        }

        public void SetObjectPoolGeneric(ObjectPoolGeneric<KillInfoUI> objectPoolGeneric)
        {
            _objectPoolGeneric = objectPoolGeneric;
        }

        public void ReturnItem()
        {
            if (_objectPoolGeneric != null)
                _objectPoolGeneric.ReturnObject(this);
        }
    }
}