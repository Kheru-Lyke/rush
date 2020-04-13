///-----------------------------------------------------------------
/// Author : Gabriel Massé
/// Date : 06/11/2019 15:50
///-----------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Com.DefaultCompany.Rush.Rush.UI {
	public class UIManager : MonoBehaviour {
		private static UIManager instance;
		public static UIManager Instance { get { return instance; } }

        [SerializeField] private CanvasGroup tileUI;
        [SerializeField] private CanvasGroup resolutionUI;
        [SerializeField] private CanvasGroup titleCardUI;
        [SerializeField] private CanvasGroup levelSelectorUI;
        [SerializeField] private CanvasGroup gameOverUI;
        [SerializeField] private CanvasGroup winUI;
        [SerializeField] private CanvasGroup pauseUI;
        [SerializeField] private CanvasGroup moreUI;
		
		private void Awake(){
			if (instance){
				Destroy(gameObject);
				return;
			}
			
			instance = this;
		}
		
		private void Start () {
            Play.OnPressed += SetPhaseResolution;

            DesactivateAll();
            ActivateUI(titleCardUI);
        }

        private void ActivateUI(CanvasGroup UI)
        {
            DesactivateAll();
            UI.gameObject.SetActive(true);
        }
        
        private void DesactivateAll()
        {
            resolutionUI.gameObject.SetActive(false);
            levelSelectorUI.gameObject.SetActive(false);
            titleCardUI.gameObject.SetActive(false);
            tileUI.gameObject.SetActive(false);
            gameOverUI.gameObject.SetActive(false);
            winUI.gameObject.SetActive(false);
            pauseUI.gameObject.SetActive(false);
            moreUI.gameObject.SetActive(false);
        }

        public void SetPhaseResolution()
        {
            ActivateUI(resolutionUI);
        }
        public void SetPhaseMore()
        {
            ActivateUI(moreUI);
        }
        public void SetPhaseTile()
        {
            ActivateUI(tileUI);
        }
        public void SetPhaseLevelSelector()
        {
            ActivateUI(levelSelectorUI);
        }
        public void SetPhaseGameOver()
        {
            ActivateUI(gameOverUI);
        }
        public void SetPhaseWin()
        {
            ActivateUI(winUI);
        }
        public void SetPhasePause()
        {
            ActivateUI(pauseUI);
        }

        private void OnDestroy(){
			if (this == instance) instance = null;
		}
	}
}