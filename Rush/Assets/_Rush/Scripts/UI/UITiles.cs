///-----------------------------------------------------------------
/// Author : Gabriel Massé
/// Date : 11/11/2019 14:29
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Com.DefaultCompany.Rush.Rush.UI {
    [RequireComponent(typeof(ToggleGroup))]
    public class UITiles : MonoBehaviour
    {
        private static UITiles instance;
        public static UITiles Instance { get { return instance; } }

        private ToggleGroup tiles;
        public delegate void OnTileSelected(GameObject tile);

        public static OnTileSelected OnSelected;

        private Dictionary<GameObject, int> tilesAllowed;
        [SerializeField] GameObject[] allTiles;

        public GameObject recoveredTile
        {
            get { return null; }
            set
            {
                string valueName = value.name.Remove(value.name.IndexOf("("));
                foreach (var tileUI in tilesAllowed.Keys)
                {
                    foreach (var item in tileUI.GetComponentsInChildren<MeshRenderer>(true))
                    {
                        if (item.gameObject.name == valueName)
                        {
                            lastTile = tileUI;
                            tileNumber += 1;
                            GameObject.Destroy(value);
                            OnSelected(value);
                            return;
                        }
                    }
                }
                return;
            }
        }
        private GameObject lastTile;
        public int tileNumber
        {
            get
            {
                if (tilesAllowed.TryGetValue(lastTile, out int number)) return number;
                return 0;
            }
            set
            {
                if (tilesAllowed.TryGetValue(lastTile, out int number))
                {
                    tilesAllowed[lastTile] = value;
                    if (value <= 0) lastTile.gameObject.SetActive(false);
                    else lastTile.gameObject.SetActive(true);
                    lastTile.GetComponentInChildren<Text>().text = value.ToString();
                }
            }
        }

        private void Awake()
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
            tiles = GetComponent<ToggleGroup>();
            tiles.SetAllTogglesOff();
        }

        public void StartLevel(int[] tilesNumbers)
        {
            lastTile = null;
            tilesAllowed = new Dictionary<GameObject, int>();

            for (int i = 0; i < tilesNumbers.Length; i++)
            {
                tilesAllowed.Add(allTiles[i], tilesNumbers[i]);

                foreach (var tile in GetComponentsInChildren<Toggle>(true))
                {
                    if (tile.gameObject == allTiles[i])
                    {
                        if (tilesNumbers[i] <= 0) tile.gameObject.SetActive(false);
                        else
                        {
                            tile.gameObject.SetActive(true);
                            tile.GetComponentInChildren<Text>().text = tilesNumbers[i].ToString();
                        }
                    }
                    else continue;
                }
            }
        }

        private void Update()
        {
            if (tiles.AnyTogglesOn())
            {
               lastTile = tiles.ActiveToggles().FirstOrDefault().gameObject;
               OnSelected(lastTile.GetComponentInChildren<MeshRenderer>().gameObject);
            }
        }
        private void OnDestroy()
        {
            if (this == instance) instance = null;
        }
    }
}