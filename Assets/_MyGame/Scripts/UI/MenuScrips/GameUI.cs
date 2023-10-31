using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Curio.Gameplay
{
    public class GameUI : BaseUI
    {
        [SerializeField] private TextMeshProUGUI blueTeamScoreText, redTeamScoreText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI enemyKillCountText, batteryText;
        [SerializeField] private GameObject deathmatchUI, basedefenseUI;
        [SerializeField] private GameObject countDownHolder;
        [SerializeField] private TextMeshProUGUI countDownText;

        private void OnDisable()
        {
            //DeathMatchManager.Instance.onDeathMatchFinishEvent.RemoveListener(() => togglePanel.ToggleVisibilityInstant(false));
            GameManager.Instance.onLevelCompleteEvent.RemoveListener(() => togglePanel.ToggleVisibilityInstant(false));

            GameManager.Instance.onLevelStartEvent.RemoveListener(() =>
            {
                DecideUI();
            });
        }

        protected override void Start()
        {
            base.Start();
            DeathMatchManager.Instance.GameUI = this;
            BaseDefenseManager.Instance.GameUI = this;

            //DeathMatchManager.Instance.onDeathMatchFinishEvent.AddListener(() => togglePanel.ToggleVisibilityInstant(false));
            GameManager.Instance.onLevelCompleteEvent.AddListener(() => togglePanel.ToggleVisibilityInstant(false));

            //BaseDefenseManager.Instance.onBaseDefenseFinishEvent.AddListener(() => togglePanel.ToggleVisibilityInstant(false));

            GameManager.Instance.onLevelStartEvent.AddListener(() =>
            {
                DecideUI();
            });
        }

        private void DecideUI()
        {
            if (GameManager.Instance.GameMode == GameMode.DEFENSE)
            {
                deathmatchUI.SetActive(false);
                basedefenseUI.SetActive(true);
            }
            else if (GameManager.Instance.GameMode == GameMode.DEATHMATCH)
            {
                deathmatchUI.SetActive(true);
                basedefenseUI.SetActive(false);
            }
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

        public void SetEnemyKillText(string value)
        {
            enemyKillCountText.text = value;
        }

        public void SetBatteryRemainingText(string value)
        {
            batteryText.text = value;
        }

        public void ActivateCountDownHolder(bool isActive)
        {
            countDownHolder.SetActive(isActive);
        }

        public void UpdatCountDownText(float value)
        {
            TimeSpan time = TimeSpan.FromSeconds(value); //set the time value
            countDownText.text = time.ToString("ss");   //convert time to Time format
        }


    }
}