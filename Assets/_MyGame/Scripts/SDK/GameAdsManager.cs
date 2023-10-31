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

        GameMonetize.OnResumeGame += OnResumeGame;
        GameMonetize.OnPauseGame += OnPauseGame;
    }

    void OnDestroy()
    {
        GameMonetize.OnResumeGame -= OnResumeGame;
        GameMonetize.OnPauseGame -= OnPauseGame;
    }

    #endregion


    #region Private Methods
    public void OnResumeGame()
    {
        Time.timeScale = 1;
        SoundManager.Instance.FinishedAdsUnpauseAudio();

        //if (shownNormalAds)
        //{
            //shownNormalAds = false;
            interstitialCallback();
        //}
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


    public void ShowNormalAd(Action callback)
    {
        interstitialCallback = callback;
        GameMonetize.Instance.ShowAd();
    }

    #endregion

    public bool InternetAvailable()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) return false;
        return true;
    }
}