///-----------------------------------------------------------------
/// Author : Gabriel Massé
/// Date : 12/11/2019 17:48
///-----------------------------------------------------------------

using Com.DefaultCompany.Rush.Rush.UI;
using Com.IsartDigital.IsartGDP2Rush3CSetup.Rush;
using UnityEngine;

namespace Com.DefaultCompany.Rush.Rush
{
    public class GameManager : MonoBehaviour
    {

        [SerializeField] private AudioClip win;
        [SerializeField] private AudioClip loose;
        [SerializeField] private AudioClip level;
        [SerializeField] private AudioSource effects;

        protected static GameManager instance;
        public static GameManager Instance { get { return instance; } }

        protected void Awake()
        {
            if (instance)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
        }
        private void Start()
        {
            Cube.CubeDied += FailLevel;
            PlayerCamera.Instance.setModeUI();

            Ticker.Instance.StartTicking();
        }

        private void Update()
        {

        }

        public void CreateLevel(int levelNumber)
        {
            effects.clip = level;
            effects.Play();
            Ticker.Instance.StopTicking();
            DestroyLevel();
            LevelManager.Instance.CreateLevel(levelNumber);
            PlayerCamera.Instance.SetModeGameplay();
            UIManager.Instance.SetPhaseTile();
        }

        public void ResetCurrentLevel()
        {
            effects.clip = level;
            effects.Play();
            PlayerCamera.Instance.SetModeGameplay();
            DestroyLevel();
            LevelManager.Instance.ResetCurrentLevel();
            UIManager.Instance.SetPhaseTile();
        }

        public void DestroyLevel()
        {
            LevelManager.Instance.DestroyCurrentLevel();
            Spawner.DestroyAllCubes();
        }
        public void BackToReflexion()
        {
            PlayerCamera.Instance.SetModeGameplay();
            Spawner.DestroyAllCubes();
            Ticker.Instance.StopTicking();
            LevelManager.Instance.BackToReflexion();
            UIManager.Instance.SetPhaseTile();

            foreach (var item in FindObjectsOfType<Spawner>())
            {
                item.Restart();
            }
        }
        public void BackToLevelSelector()
        {
            Spawner.DestroyAllCubes();
            Ticker.Instance.StopTicking();
            PlayerCamera.Instance.setModeUI();
            UIManager.Instance.SetPhaseLevelSelector();
        }

        static public void WinLevel()
        {
            instance.effects.clip = instance.win;
            instance.effects.Play();
            Ticker.Instance.StopTicking();
            UIManager.Instance.SetPhaseWin();
        }

        public void More()
        {
            UIManager.Instance.SetPhaseMore();
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void Pause()
        {
            UIManager.Instance.SetPhasePause();
            Ticker.Instance.StopTicking();
        }
        public void Resume()
        {
            UIManager.Instance.SetPhaseResolution();
            Ticker.Instance.StartTicking();
        }

        static public void FailLevel()
        {
            instance.effects.clip = instance.loose;
            instance.effects.Play();
            Ticker.Instance.StopTicking();

            foreach (var item in FindObjectsOfType<Cube>())
            {
                item.SetModeVoid();
            }

            PlayerCamera.Instance.SetModeLoose(Cube.deadCube.transform);
            UIManager.Instance.SetPhaseGameOver();
        }
        protected void OnDestroy()
        {
            if (this == instance) instance = null;
        }
    }
}