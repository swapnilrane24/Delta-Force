using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using DG.Tweening;
using Curio.Gameplay;

public class GameAdsManager : MonoBehaviour
{
    private static GameAdsManager instance;

    #region Serialized Variables
    [SerializeField] private TextMeshProUGUI noAdsText;
    [SerializeField] private float interstitialInterval;
    [SerializeField] private bool disableAds;
    [SerializeField] private float startRVDelay;

    public float StartRVDelay => startRVDelay;
    #endregion

    #region Private Variables
    private float _delayInterstitial;
    private bool rewardSuccess = false, midAdSuccess = false;
    private Action<bool> rewardCallback;
    private Action midAdCallback;
    private bool isRewardAdReady;
    #endregion

    #region Getters & Setters
    public bool AdsDisabled => disableAds;
    public static GameAdsManager Instance { get => instance; }
    public bool IsRewardAdReady => isRewardAdReady;
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

        _delayInterstitial = interstitialInterval;
    }

    private void Start()
    {
        LaggedAPIUnity.OnResumeGame += OnResumeGame;
        LaggedAPIUnity.OnPauseGame += OnPauseGame;
        LaggedAPIUnity.onRewardAdReady += RewardAdReady;
        LaggedAPIUnity.onRewardAdSuccess += RewardSuccess;
        LaggedAPIUnity.onRewardAdFailure += Rewardfailed;
    }

    //private void Update()
    //{
    //    if (disableAds == false)
    //    {
    //        if (_delayInterstitial > 0)
    //        {
    //            _delayInterstitial -= Time.deltaTime;
    //        }

    //        if (startRVDelay > 0)
    //        {
    //            startRVDelay -= Time.deltaTime;
    //        }
    //    }
    //}
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
        //AudioListener.volume = 0;
    }

    private void NoAdsPopUp()
    {
        noAdsText.transform.localScale = Vector3.zero;
        noAdsText.gameObject.SetActive(true);
        Sequence noAdSequence = DOTween.Sequence();
        noAdSequence.Append(noAdsText.transform.DOScale(Vector3.one, 0.2f));
        noAdSequence.AppendInterval(0.5f);
        noAdSequence.Append(noAdsText.transform.DOScale(Vector3.zero, 0.2f));
        OnResumeGame();
    }

    public void AdError()
    {
        NoAdsPopUp();
    }

    public void AdFinished()
    {

    }

    private void RewardSuccess()
    {
        OnResumeGame();
        rewardSuccess = true;
        rewardCallback(rewardSuccess);
        isRewardAdReady = false;
    }

    private void Rewardfailed()
    {
        NoAdsPopUp();
        rewardSuccess = false;
        rewardCallback(rewardSuccess);
        isRewardAdReady = false;
    }

    private void MidAdSuccess()
    {
        OnResumeGame();
        midAdSuccess = true;
        midAdCallback();
        _delayInterstitial = interstitialInterval;
    }

    private void MidAdfailed()
    {
        NoAdsPopUp();
        midAdSuccess = false;
        midAdCallback();
        _delayInterstitial = 30;
    }
    #endregion

    #region Public Methods
    public void RewardAdReady()
    {
        isRewardAdReady = true;
    }

    public void CheckRewardAd()
    {
        LaggedAPIUnity.Instance.CheckRewardAd();
    }

    public bool ShowRewardedAds(Action<bool> callback)
    {
        rewardCallback = callback;
        OnPauseGame();
        rewardSuccess = false;
        LaggedAPIUnity.Instance.PlayRewardAd();
        return rewardSuccess;
    }

    public void ShowNormalAd(Action callback)
    {
        //if (_delayInterstitial <= 0)
        //{
            midAdCallback = callback;
            OnPauseGame();
            LaggedAPIUnity.Instance.ShowAd();
            //_delayInterstitial = interstitialInterval;
        //}
        //else
        //{
            //callback();
        //}
    }
    #endregion

    public bool InternetAvailable()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) return false;
        return true;
    }
}