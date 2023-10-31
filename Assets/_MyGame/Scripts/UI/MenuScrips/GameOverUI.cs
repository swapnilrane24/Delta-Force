using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Curio.Gameplay
{
    public class GameOverUI : BaseUI
    {
        [SerializeField] private Button continueButton;

        protected override void Start()
        {
            base.Start();
            BaseDefenseManager.Instance.onBaseDefenseFailedEvent.AddListener(() =>
            {
                ActivatePanel();
            });

            continueButton.onClick.AddListener(ContinueButtonListner);
            
        }

        public void ContinueButtonListner()
        {
            continueButton.interactable = false;

            GameManager.Instance.LoadLevel();
        }

        public override void ActivatePanel()
        {
            base.ActivatePanel();
            Cursor.lockState = CursorLockMode.None;
            if (GameAdsManager.Instance.RewardAdReady())
            {
                GameAdsManager.Instance.ShowNormalAd(() => Debug.Log("Normal"));
            }
        }

    }
}