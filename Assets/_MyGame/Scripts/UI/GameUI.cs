using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Curio.Gameplay
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TogglePanel togglePanel;
        [SerializeField] private TextMeshProUGUI blueTeamScoreText, redTeamScoreText;
        [SerializeField] private TextMeshProUGUI timerText;

        private void OnDisable()
        {
            DeathMatchManager.Instance.onDeathMatchFinishEvent.RemoveListener(() => togglePanel.ToggleVisibilityInstant(false));
        }

        private void Start()
        {
            DeathMatchManager.Instance.GameUI = this;

            DeathMatchManager.Instance.onDeathMatchFinishEvent.AddListener(() => togglePanel.ToggleVisibilityInstant(false));
        }

        public void SetTimer(float value)
        {
            TimeSpan time = TimeSpan.FromSeconds(value); //set the time value
            timerText.text = time.ToString("mm':'ss");   //convert time to Time format
        }

        public void SetScore(int blueTeamScore, int redTeamScore)
        {
            blueTeamScoreText.text = "" + blueTeamScore;
            redTeamScoreText.text = "" + redTeamScore;

        }

        public void ToggleVisibility(bool visible)
        {
            togglePanel.ToggleVisibility(visible);
        }


    }
}