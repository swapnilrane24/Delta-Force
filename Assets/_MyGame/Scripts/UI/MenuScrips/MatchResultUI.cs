using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Curio.Gameplay
{
    public class MatchResultUI : BaseUI
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Button rewardButton;
        [SerializeField] private TextMeshProUGUI rewardText;
        [SerializeField] private TextMeshProUGUI blueTeamScoreText, redTeamScoreText;
        [SerializeField] private TextMeshProUGUI WinnerTeamNameText;

        protected override void Start()
        {
            base.Start();
            DeathMatchManager.Instance.MatchResultUI = this;
            continueButton.onClick.AddListener(ContinueButtonListner);
            rewardButton.onClick.AddListener(RewardButtonListner);
        }

        public void SetScore(int blueTeamScore, int redTeamScore)
        {
            blueTeamScoreText.text = "" + blueTeamScore;
            redTeamScoreText.text = "" + redTeamScore;

            if (blueTeamScore > redTeamScore)
            {
                WinnerTeamNameText.text = "TEAM BLUE WINS";
            }
            else if (blueTeamScore < redTeamScore)
            {
                WinnerTeamNameText.text = "TEAM RED WINS";
            }
            else if (blueTeamScore == redTeamScore)
            {
                WinnerTeamNameText.text = "MATCH IS DRAW";
            }

        }

        public void ContinueButtonListner()
        {
            continueButton.interactable = false;
            rewardButton.interactable = false;

            menuRoot.CoinCollectAnimUI.Coins_Animation(() =>
            {
                GameManager.Instance.AddMoney(GameManager.Instance.RoundEarning);
                WaitExtension.Wait(this, 0.2f, () => GameManager.Instance.LoadLevel());
            });
        }

        public void RewardButtonListner()
        {
            GameAdsManager.Instance.ShowRewardedAds((bool canReward) =>
            {
                if (canReward)
                {
                    continueButton.interactable = false;
                    rewardButton.interactable = false;

                    menuRoot.CoinCollectAnimUI.Coins_Animation(() =>
                    {
                        GameManager.Instance.AddMoney(GameManager.Instance.RoundEarning * 2);
                        WaitExtension.Wait(this, 0.2f, () => GameManager.Instance.LoadLevel());
                    });
                }
            });

            //menuRoot.CoinCollectAnimUI.Coins_Animation(() =>
            //{
            //    GameManager.Instance.AddMoney(GameManager.Instance.RoundEarning * 2);
            //    WaitExtension.Wait(this, 0.2f, () => GameManager.Instance.LoadLevel());
            //});

        }

        public override void ActivatePanel()
        {
            base.ActivatePanel();
            GameManager.Instance.IncreaseRoundEarning(200);
            rewardText.text = CurrencyToString.Convert(GameManager.Instance.RoundEarning);
            Cursor.lockState = CursorLockMode.None;
            menuRoot.TotalCurrencyUI.gameObject.SetActive(true);
        }

    }
}