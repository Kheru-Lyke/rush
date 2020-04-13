///-----------------------------------------------------------------
/// Author : Gabriel Massé
/// Date : 19/11/2019 12:01
///-----------------------------------------------------------------

using Com.IsartDigital.IsartGDP2Rush3CSetup.Rush;
using UnityEngine;

namespace Com.DefaultCompany.Rush.Rush.Tiles {
	public class Ground : Tile {
        [SerializeField] public Renderer slime;

        override public void Action(Cube cube)
        {
            base.Action(cube);

            slime.gameObject.SetActive(true);
            slime.material.color = color;

            cube.SetModeMove();
        }
    }
}