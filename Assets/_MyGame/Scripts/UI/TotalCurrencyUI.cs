using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Curio.Gameplay
{
    public class TotalCurrencyUI : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI totalCurrencyText;

        private void Start()
        {
            totalCurrencyText.text = CurrencyToString.Convert(GameManager.Instance.TotalMoney);
            GameManager.Instance.TotalMoney.AddListener(() =>
            {
                totalCurrencyText.text = CurrencyToString.Convert(GameManager.Instance.TotalMoney);
            });
        }

    }
}