///-----------------------------------------------------------------
/// Author : Gabriel Massé
/// Date : 28/11/2019 13:38
///-----------------------------------------------------------------

using System;
using UnityEngine;

namespace Com.DefaultCompany.Rush.Rush {
	public class Multicolor : MonoBehaviour {

        [SerializeField] protected Color[] colors;
        protected int index = 0;

        protected Renderer[] renderers;
        private int elapsedTick;

        protected void Start () {
            Ticker.Instance.Timer += ChangeColor;
            renderers = GetComponentsInChildren<Renderer>();

            foreach (var item in renderers)
            {
                item.material.color = colors[index];
            }
		}

        protected void ChangeColor()
        {
            if (elapsedTick >= 1)
            {

                if (index >= colors.Length - 1) index = 0;
                else index++;

                foreach (var item in renderers)
                {
                    item.material.color = colors[index];
                }
                elapsedTick = 0;
            }

            elapsedTick++;
        }
    }
}