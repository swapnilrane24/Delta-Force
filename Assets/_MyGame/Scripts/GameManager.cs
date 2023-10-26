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

    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance => instance;

        private GameState gameState = GameState.NONE;

        public GameState GameState => gameState;

        //[SerializeField] private int totalLevels;
        [SerializeField] private IntVariable totalMoney;
        [Range(0.1f, 5f)]
        public float mouseSensitivity = 1;

        private int roundEarning;

        public UnityEvent onLevelCompleteEvent;
        public UnityEvent onDeathMatchStartEvent;

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

        public void DeathMatchStarted()
        {
            roundEarning = 0;
            gameState = GameState.PLAYING;
            onDeathMatchStartEvent?.Invoke();
        }

        public void DeathMatchFinish()
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