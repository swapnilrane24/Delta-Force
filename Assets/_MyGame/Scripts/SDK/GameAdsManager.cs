using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using DG.Tweening;
using Curio.Gameplay;
using GamePix;

public class GameAdsManager : MonoBehaviour
{
    private static GameAdsManager instance;
    public static GameAdsManager Instance { get => instance; }


    #region Serialized Variables
    [SerializeField] private TextMeshProUGUI noAdsText;
    [SerializeField] private bool showAds;
    [Tooltip("Interval is in seconds")]
    [SerializeField] private float interstitialInterval = 20;
    #endregion

    #region Private Variables
    private Action<bool> rewardCallback = null;
    private Action interstitialCallback = null;
    #endregion

    #region Getters & Setters
    #endregion

    #region Unity Methods
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion


    #region Private Methods
    public void OnResumeGame()
    {
        Time.timeScale = 1;
        SoundManager.Instance.FinishedAdsUnpauseAudio();
    }

    public void OnPauseGame()
    {
        Time.timeScale = 0;
        SoundManager.Instance.ShowingAdsPauseAudio();
    }

    private void NoAdsPopUp()
    {
        noAdsText.transform.localScale = Vector3.zero;
        noAdsText.gameObject.SetActive(true);
        Sequence noAdSequence = DOTween.Sequence();
        noAdSequence.Append(noAdsText.transform.DOScale(Vector3.one, 0.2f));
        noAdSequence.AppendInterval(0.5f);
        noAdSequence.Append(noAdsText.transform.DOScale(Vector3.zero, 0.2f));
    }
    #endregion

    #region Public Methods
    public void ShowRewardedAds(Action<bool> action)
    {
        rewardCallback = action;
        Gpx.Ads.RewardAd(OnRewardedVideoSuccess, OnRewardedVideoFailure);
    }

    public void ShowNormalAd(Action callback)
    {
        interstitialCallback = callback;
        Gpx.Ads.InterstitialAd(InterstitialAd);
    }

    private void InterstitialAd()
    {
        interstitialCallback();
        OnResumeGame();
    }

    public void OnRewardedVideoSuccess()
    {
        rewardCallback(true);
        OnResumeGame();
    }

    public void OnRewardedVideoFailure()
    {
        NoAdsPopUp();
        rewardCallback(false);
        OnResumeGame();
    }

    #endregion

    public bool InternetAvailable()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) return false;
        return true;
    }
}