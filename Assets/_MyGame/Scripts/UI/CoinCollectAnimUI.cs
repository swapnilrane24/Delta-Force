using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace Curio.Gameplay
{
    public class CoinCollectAnimUI : MonoBehaviour
    {
        [SerializeField] private SoundPlay soundPlay;
        [SerializeField] private RectTransform mainCoinIcon;
        [SerializeField] private RectTransform[] coins;

        public void Coins_Animation()
        {
            soundPlay.Play();
            //play vibration
            for (int i = 0; i < coins.Length; i++)
            {
                float t = i / (float)coins.Length;

                float x_pos = Mathf.Cos(t * 360f * Mathf.Deg2Rad);
                float y_pos = Mathf.Sin(t * 360f * Mathf.Deg2Rad);

                Vector2 pos = new Vector2(x_pos, y_pos) * Random.Range(50f, 150f);
                pos += coins[i].anchoredPosition;

                coins[i].gameObject.SetActive(true);
                Sequence seq = DOTween.Sequence();

                GameObject coinObj = coins[i].gameObject;

                seq.Append(coins[i].DOAnchorPos(pos, Random.Range(0.5f, 1f)).SetEase(Ease.OutBack))
                .Append(coins[i].DOLocalMove(mainCoinIcon.transform.localPosition, Random.Range(.5f, 1f)).SetEase(Ease.OutBack))
                .Join(coins[i].DOScale(Vector3.one * 0.25f, 1f))
                .OnComplete(() =>
                {
                    coinObj.SetActive(false);
                });

            }
        }

        public void Coins_Animation(Action callback)
        {
            soundPlay.Play();
            //play vibration
            int coinAnimationDoneCount = 0; //used just to make sure scene is loaded only on last coin animation
            for (int i = 0; i < coins.Length; i++)
            {
                float t = i / (float)coins.Length;

                float x_pos = Mathf.Cos(t * 360f * Mathf.Deg2Rad);
                float y_pos = Mathf.Sin(t * 360f * Mathf.Deg2Rad);

                Vector2 pos = new Vector2(x_pos, y_pos) * Random.Range(50f, 150f);
                pos += coins[i].anchoredPosition;

                coins[i].gameObject.SetActive(true);
                Sequence seq = DOTween.Sequence();

                GameObject coinObj = coins[i].gameObject;

                seq.Append(coins[i].DOAnchorPos(pos, Random.Range(0.5f, 1f)).SetEase(Ease.OutBack))
                .Append(coins[i].DOLocalMove(mainCoinIcon.transform.localPosition, Random.Range(.5f, 1f)).SetEase(Ease.OutBack))
                .Join(coins[i].DOScale(Vector3.one * 0.25f, 1f))
                .OnComplete(() =>
                {
                    coinAnimationDoneCount++;
                    coinObj.SetActive(false);
                    if (coinAnimationDoneCount >= coins.Length)
                    {
                        //if (adBoost == false)
                        //    GameManager.Instance.AddMoney(GameManager.Instance.RoundEarning);
                        //else
                        //    GameManager.Instance.AddMoney(totalRewardEarning);

                        callback();

                        //LevelManager.instance.LevelCompleted();
                        //GameManager.Instance.GameState = GameState.PLAYING;
                        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    }
                });

            }
        }

    }
}