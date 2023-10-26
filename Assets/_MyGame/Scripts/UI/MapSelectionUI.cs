using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Curio.Gameplay
{
    public class MapSelectionUI : MonoBehaviour
    {
        [SerializeField] private TogglePanel teamMenuToggle;
        [SerializeField] private Button startFightButton;
        [SerializeField] private TogglePanel togglePanel;
        [SerializeField] private MapButton mapButtonPrefab;
        [SerializeField] private Transform mapButtonContainer;
        [SerializeField] private MapButtonData[] mapButtonDatas;

        private int selectedArena;
        private List<MapButton> mapButtonList;


        private void Start()
        {
            InitializeMapUI();
            startFightButton.onClick.AddListener(FightButtonListner);
        }

        private void InitializeMapUI()
        {
            mapButtonList = new List<MapButton>();
            for (int i = 0; i < mapButtonDatas.Length; i++)
            {
                MapButton mapButton = Instantiate(mapButtonPrefab, mapButtonContainer);
                mapButton.SetMapButton(mapButtonDatas[i]);
                mapButton.onButtonSelected.AddListener(MapButtonSelectedListner);
                mapButtonList.Add(mapButton);
            }

            MapButtonSelectedListner(selectedArena);
        }

        private void MapButtonSelectedListner(int arenaIndex)
        {
            for (int i = 0; i < mapButtonList.Count; i++)
            {
                mapButtonList[i].CheckIfThisButtonIsClicked(arenaIndex);
            }
        }

        private void FightButtonListner()
        {
            togglePanel.ToggleVisibility(false);
            teamMenuToggle.ToggleVisibility(true);
            DeathMatchManager.Instance.InitializeDeathMatchManager();
        }

    }
}