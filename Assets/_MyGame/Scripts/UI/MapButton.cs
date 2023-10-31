using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Curio.Gameplay
{
    [System.Serializable]
    public struct MapButtonData
    {
        public string mapName;
        public Sprite mapIcon;
        public int arenaIndex;
        public int levelToUnlock;
    }

    public class MapButton : MonoBehaviour
    {
        [SerializeField] private Button mapButton;
        [SerializeField] private TextMeshProUGUI mapNameText;
        [SerializeField] private Image mapIconImage;
        [SerializeField] private Image selectedImage;
        [SerializeField] private GameObject lockHolder;

        public UnityEvent<int> onButtonSelected;

        private int arenaIndex;

        public void SetMapButton(MapButtonData mapButtonData)
        {
            arenaIndex = mapButtonData.arenaIndex;
            mapNameText.text = mapButtonData.mapName;
            mapIconImage.sprite = mapButtonData.mapIcon;
            mapButton.onClick.AddListener(MapButtonListner);
            mapButton.interactable = (BaseDefenseManager.Instance.BaseDefenseLevel + 1) >= mapButtonData.levelToUnlock;
            lockHolder.SetActive(!mapButton.interactable);
            if (mapButton.interactable == false)
            {
                mapNameText.text = "Unlock Lvl " + mapButtonData.levelToUnlock;
            }
        }

        private void MapButtonListner()
        {
            DeathMatchManager.Instance.SelectedArenaIndex(arenaIndex);
            selectedImage.enabled = true;
            onButtonSelected?.Invoke(arenaIndex);
        }

        public void CheckIfThisButtonIsClicked(int selectedArenaIndex)
        {
            selectedImage.enabled = arenaIndex == selectedArenaIndex;
        }
    }
}