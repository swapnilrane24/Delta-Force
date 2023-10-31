using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Curio.Gameplay
{

    public enum GameState
    {
        NONE,
        PLAYING,
        COMPLETE
    }

    public enum GameMode
    {
        DEFENSE,
        DEATHMATCH
    }

    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance => instance;

        private GameState gameState = GameState.NONE;
        private GameMode gameMode = GameMode.DEFENSE;

        public GameState GameState => gameState;
        public GameMode GameMode { get { return gameMode; } set { gameMode = value; } }

        //[SerializeField] private int totalLevels;
        [SerializeField] private IntVariable totalMoney;
        [Range(0.1f, 5f)]
        public float mouseSensitivity = 1;
        [Range(0.1f, 1f)]
        private float soundVolume = 1;

        public float GetSoundVolumn => soundVolume;

        private int roundEarning;

        public UnityEvent onLevelCompleteEvent;
        public UnityEvent onLevelStartEvent;
        public UnityEvent<float> soundVolumnUpdateEvent;

        [HideInInspector] public UnityEvent roundEarningIncreaseEvent;
        public int RoundEarning => roundEarning;
        public IntVariable TotalMoney => totalMoney;

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
            totalMoney.Value = ES3.Load<int>("TotalMoney", 0);
        }

        public void UpdateSoundValumn(float value)
        {
            soundVolume = value;
            soundVolumnUpdateEvent?.Invoke(value);
        }

        public void LevelStarted()
        {
            roundEarning = 0;
            gameState = GameState.PLAYING;
            onLevelStartEvent?.Invoke();
        }

        public void LevelFinish()
        {
            gameState = GameState.COMPLETE;
            onLevelCompleteEvent?.Invoke();
        }

        public void AddMoney(int amount)
        {
            totalMoney.Value += amount;
            ES3.Save<int>("TotalMoney", totalMoney.Value);
        }

        public void RemoveMoney(int amount)
        {
            totalMoney.Value -= amount;
            ES3.Save<int>("TotalMoney", totalMoney.Value);
        }

        public void IncreaseRoundEarning(int amount)
        {
            roundEarning += amount;
            roundEarningIncreaseEvent?.Invoke();
        }

        public void LoadLevel()
        {
            totalMoney.RemoveAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }
}