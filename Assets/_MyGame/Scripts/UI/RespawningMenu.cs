using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Curio.Gameplay
{
    public class RespawningMenu : MonoBehaviour
    {
        [SerializeField] private TogglePanel togglePanel;
        [SerializeField] private TextMeshProUGUI countText;

        private float _respawnTimer;

        private void Start()
        {
            DeathMatchManager.Instance.RespawningMenu = this;
        }

        public void ActivateRespawnScreen(float respawnTimer)
        {
            _respawnTimer = respawnTimer;
            togglePanel.ToggleVisibilityInstant(true);
        }

        private void Update()
        {
            if (_respawnTimer > 0)
            {
                _respawnTimer -= Time.deltaTime;

                countText.text = "" + Mathf.FloorToInt(_respawnTimer);

                if (_respawnTimer <= 0)
                {
                    togglePanel.ToggleVisibilityInstant(false);
                    DeathMatchManager.Instance.RespawnPlayer();
                }
            }
        }


    }
}