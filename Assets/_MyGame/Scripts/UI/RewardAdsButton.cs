using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace Curio.Gameplay
{
    public class RewardAdsButton : MonoBehaviour
    {
        public UnityEvent OnRewardEvent;

        [SerializeField] private int rewardAmount;
        [SerializeField] private TextMeshProUGUI rewardText;

        public UnityEvent onRewardButtonClickedEvent;

        [SerializeField] MenuRoot menuRoot;

        public MenuRoot SetMenuRoot { set { menuRoot = value; } } 

        private Button adButton;

        private void OnDisable()
        {
            adButton.onClick.RemoveListener(AdButtonListner);
        }

        private void OnEnable()
        {
            adButton.interactable = false;
            GameAdsManager.Instance.CheckRewardAd();
            SetInteractable(GameAdsManager.Instance.IsRewardAdReady);
        }

        private void Awake()
        {
            adButton = GetComponent<Button>();
        }

        private void Start()
        {
            adButton.onClick.AddListener(AdButtonListner);
            rewardText.text = CurrencyToString.Convert(rewardAmount);
        }

        public void SetRewardAmount(int value)
        {
            rewardAmount = value;
            rewardText.text = CurrencyToString.Convert(rewardAmount);
        }

        protected virtual void AdButtonListner()
        {
            GameAdsManager.Instance.ShowRewardedAds((bool canReward) =>
            {
                if (canReward)
                {
                    adButton.interactable = false;
                    onRewardButtonClickedEvent?.Invoke();

                    if (menuRoot)
                    {
                        menuRoot.CoinCollectAnimUI.Coins_Animation(() =>
                        {
                            RewardCallBack();
                        });
                    }
                    else
                    {
                        RewardCallBack();
                    }

                    //RewardCallBack();
                    //adButton.interactable = false;
                    //onRewardButtonClickedEvent?.Invoke();
                }
            });

            //RewardCallBack();
        }

        protected virtual void RewardCallBack()
        {
            GameManager.Instance.AddMoney(rewardAmount);
            OnRewardEvent?.Invoke();
        }

        public void SetInteractable(bool isInteractable)
        {
            adButton.interactable = isInteractable;
        }
    }
}