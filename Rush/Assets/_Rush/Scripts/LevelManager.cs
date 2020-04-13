///-----------------------------------------------------------------
/// Author : Gabriel Massé
/// Date : 29/10/2019 16:54
///-----------------------------------------------------------------

using Com.DefaultCompany.Rush.Rush.Tiles;
using Com.DefaultCompany.Rush.Rush.UI;
using Com.IsartDigital.IsartGDP2Rush3CSetup.Rush;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.DefaultCompany.Rush.Rush
{
    public class LevelManager : MonoBehaviour
    {
        protected static LevelManager instance;
        public static LevelManager Instance { get { return instance; } }

        protected int cubeSpawned = 0;
        protected int cubeDestroyed = 0;
        protected Action doAction;
        protected int currentLevel = 0;

        [SerializeField] protected LayerMask groundMask;

        [SerializeField] protected GameObject[] levelsPrefabs;
        [SerializeField] protected GameObject levelContainer;
        [SerializeField] protected GameObject tileContainer;
        [SerializeField] protected string[] levelsTiles;

        [Space]


        protected Transform currentTile = null;
        private AudioSource sound;

        protected void Awake()
        {
            if (instance)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
        }

        protected void Start()
        {
            sound = GetComponent<AudioSource>();
            Spawner.Spawned += AddCube;
            Cube.CubeDestroyed += DestroyCube;
            Play.OnPressed += SetModeResolution;
            UITiles.OnSelected += SetModeTilePlacement;

            SetModeWait();
        }

        public void CreateLevel(int levelNumber)
        {

            List<int> tiles = new List<int>();

            foreach (var item in levelsTiles[levelNumber].Split(new Char[] { ',' }))
            {
                tiles.Add(Convert.ToInt32(item));
            }

            currentTile = null;

            UITiles.Instance.StartLevel(tiles.ToArray());
            GameObject.Instantiate(levelsPrefabs[levelNumber]).transform.SetParent(levelContainer.transform);
            currentLevel = levelNumber;
        }

        public void DestroyCurrentLevel()
        {
            for (int i = 0; i < levelContainer.transform.childCount; i++)
            {
                Destroy(levelContainer.transform.GetChild(i).gameObject);
            }

            cubeSpawned = 0;
            cubeDestroyed = 0;
        }

        public void ResetCurrentLevel()
        {
            Ticker.Instance.StopTicking();
            CreateLevel(currentLevel);
        }

        public void BackToReflexion()
        {
            ResetCount();
            Spawner.DestroyAllCubes();
            Ticker.Instance.StopTicking();
            doAction = doActionPlaceTile;
        }

        protected void SetModeWait()
        {
            doAction = doActionVoid;
        }

        protected void doActionVoid()
        {

        }

        protected void SetModeTilePlacement(GameObject tile)
        {
            string valueName = currentTile?.name.Remove(currentTile.name.IndexOf("("));

            if (valueName != tile.name)
            {
                sound.Play();
                if (currentTile != null) GameObject.Destroy(currentTile.gameObject);
                currentTile = GameObject.Instantiate(tile).transform;
                currentTile.GetComponent<Animator>().enabled = true;
                currentTile.localScale = new Vector3(1, 1, 1);
            }

            doAction = doActionPlaceTile;
        }

        protected void doActionPlaceTile()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool raycastHit = Physics.Raycast(ray, out RaycastHit hit, 50f, groundMask);


            if (raycastHit)         // Rayon touche
            {
                GameObject tile = hit.collider.gameObject;
                if (currentTile != null && tile.transform.childCount <= 1)      //Case libre
                {
                    currentTile.position = hit.collider.transform.position + new Vector3(0, 0.5f);
                    if (Input.GetMouseButtonDown(0))
                    {
                        currentTile.SetParent(hit.collider.transform);
                        currentTile.GetComponent<HasActionOnCube>().Place();
                        currentTile = null;
                        UITiles.Instance.tileNumber -= 1;
                    }
                }
                else if (Input.GetMouseButtonDown(0) && hit.transform.GetChild(1).gameObject.layer == LayerMask.NameToLayer("UI"))      //Case occupée par tile placée
                {
                    UITiles.Instance.recoveredTile = hit.transform.GetChild(1).gameObject;      //Récupération de tile
                }
            }
        }

        protected void SetModeResolution()
        {
            if (currentTile != null) GameObject.Destroy(currentTile.gameObject);
            currentTile = null;
            Ticker.Instance.StartTicking();
            doAction = doActionResolution;
        }

        protected void doActionResolution()
        {

        }

        private void ResetCount()
        {
            cubeDestroyed = 0;
            cubeSpawned = 0;
        }


        protected void DestroyCube()
        {
            cubeDestroyed++;

            if (cubeDestroyed == cubeSpawned)
            {
                GameManager.WinLevel();
            }
        }

        protected void AddCube()
        {
            cubeSpawned++;
        }

        protected void Update()
        {
            doAction();
        }

        protected void OnDestroy()
        {
            if (this == instance) instance = null;
        }
    }
}