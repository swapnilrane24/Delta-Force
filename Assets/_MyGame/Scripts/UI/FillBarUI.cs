using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;

namespace Curio.Gameplay
{
    public class FillBarUI : MonoBehaviour
    {
        [SerializeField] private Image shadowImage;
        [SerializeField] private Image fillImage;

        public void SetFillColor(Color color)
        {
            fillImage.color = color;
        }

        public void SetFillvalue(float value)
        {
            gameObject.SetActive(value > 0);
            fillImage.fillAmount = value;
        }

        public void SetFillvalue(float value, float fillTimer, float fillDelay)
        {
            gameObject.SetActive(value > 0);
            fillImage.DOFillAmount(value, fillTimer).SetDelay(fillDelay);
        }

        public void SetFillvalue(float value, float fillTimer, float fillDelay, Action callback)
        {
            gameObject.SetActive(value > 0);
            fillImage.DOFillAmount(value, fillTimer).SetDelay(fillDelay).OnComplete(() => callback());
        }

        public void SetImages(Sprite image)
        {
            shadowImage.sprite = image;
            fillImage.sprite = image;
        }
    }
}
