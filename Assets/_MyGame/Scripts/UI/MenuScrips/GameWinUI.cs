using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Curio.Gameplay
{
    public class GameWinUI : BaseUI
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Button rewardButton;
        [SerializeField] private TextMeshProUGUI rewardText;

        protected override void Start()
        {
            base.Start();

            BaseDefenseManager.Instance.onBaseDefenseWinEvent.AddListener(() =>
            {
                ActivatePanel();
            });

            continueButton.onClick.AddListener(ContinueButtonListner);
            rewardButton.onClick.AddListener(RewardButtonListner);
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
            //GameAdsManager.Instance.ShowRewardedAds((bool canReward) =>
            //{
            //    if (canReward)
            //    {
            //        continueButton.interactable = false;
            //        rewardButton.interactable = false;

            //        menuRoot.CoinCollectAnimUI.Coins_Animation(() =>
            //        {
            //            GameManager.Instance.AddMoney(GameManager.Instance.RoundEarning * 2);
            //            WaitExtension.Wait(this, 0.2f, () => GameManager.Instance.LoadLevel());
            //        });
            //    }
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