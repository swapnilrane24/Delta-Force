using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Curio.Gameplay
{
    public abstract class BaseUI : MonoBehaviour
    {
        [SerializeField] private MenuEnum menuEnum;
        [SerializeField] protected TogglePanel togglePanel;
        [SerializeField] protected Button backbutton;

        protected MenuRoot menuRoot;

        public MenuRoot SetMenuRoot { set => menuRoot = value; }

        public MenuEnum MenuEnum => menuEnum;

        protected virtual void Start()
        {
            if (backbutton)
                backbutton.onClick.AddListener(BackButton);
        }

        public virtual void ActivatePanel()
        {
            togglePanel.ToggleVisibility(true);
        }

        public virtual void DeactivatePanel()
        {
            togglePanel.ToggleVisibility(false);
        }

        protected virtual void BackButton()
        {
            DeactivatePanel();
        }
    }
}