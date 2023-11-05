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
    private bool rewardSuccess = false;

    private float _delayInterstitial;
    private bool rewardAdsReady, rewardAdsShown;
    private bool shownNormalAds;
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

    private void Start()
    {
        GameDistribution.OnResumeGame += OnResumeGame;
        GameDistribution.OnPauseGame += OnPauseGame;
        GameDistribution.OnPreloadRewardedVideo += OnPreloadRewardedVideo;
        GameDistribution.OnRewardedVideoSuccess += OnRewardedVideoSuccess;
        GameDistribution.OnRewardedVideoFailure += OnRewardedVideoFailure;
        GameDistribution.OnRewardGame += OnRewardGame;

        _delayInterstitial = interstitialInterval;

        GameDistribution.Instance.ShowAd();
        //GameDistribution.Instance.PreloadRewardedAd();
    }

    //private void Update()
    //{
    //    if (showAds)
    //    {
    //        if (_delayInterstitial > 0f)
    //        {
    //            _delayInterstitial -= Time.deltaTime;

    //            if (_delayInterstitial <= 0)
    //            {
    //                //GameDistribution.Instance.ShowAd();
    //                _delayInterstitial = interstitialInterval;
    //            }
    //        }
    //    }
    //}
    #endregion


    #region Private Methods
    public void OnResumeGame()
    {
        Time.timeScale = 1;
        SoundManager.Instance.FinishedAdsUnpauseAudio();
        //if (PlayerPrefs.GetInt("Sound") == 1)
        //    AudioListener.volume = 1;

        if (shownNormalAds)
        {
            shownNormalAds = false;
            interstitialCallback();
        }
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
    }
    #endregion

    #region Public Methods
    public void OnPreloadRewardedVideo(int loaded)
    {
        // Feedback about preloading ad after called GameDistribution.Instance.PreloadRewardedAd
        // 0: SDK couldn't preload ad
        // 1: SDK preloaded ad

        rewardAdsReady = loaded == 0 ? false : true;
    }

    public void PreloadRewardedAd()
    {
        GameDistribution.Instance.PreloadRewardedAd();
    }

    public bool RewardAdReady()
    {
        //#if UNITY_EDITOR
        //return true;
        //#else
        //
        PreloadRewardedAd();
        //if(!rewardAdsReady)
        //   NoAdsPopUp();
        return rewardAdsReady;
        //#endif
    }

    public void ShowRewardedAds(Action<bool> action)
    {
        OnPauseGame();
        GameDistribution.Instance.ShowRewardedAd();
        rewardCallback = action;
    }

    public void ShowNormalAd(Action callback)
    {
        //if (_delayInterstitial <= 0)
        //{
        //Time.timeScale = 0;
        //AudioListener.volume = 0;
        OnPauseGame();
            shownNormalAds = true;
            GameDistribution.Instance.ShowAd();
            interstitialCallback = callback;
            _delayInterstitial = interstitialInterval;
        //}
        //else
        //{
            //callback();
        //}
    }

    public void OnRewardedVideoSuccess()
    {
        rewardAdsShown = true;
        OnResumeGame();

        //rewardCallback(true);
    }

    public void OnRewardedVideoFailure()
    {
        NoAdsPopUp();

        rewardAdsShown = false;
        OnResumeGame();

        rewardCallback(false);

        //GameDistribution.Instance.PreloadRewardedAd();
    }

    public void OnRewardGame()
    {
        // REWARD PLAYER HERE
        OnResumeGame();
        rewardCallback(true);
    }
    #endregion

    public bool InternetAvailable()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) return false;
        return true;
    }
}