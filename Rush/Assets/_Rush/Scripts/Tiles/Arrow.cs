///-----------------------------------------------------------------
/// Author : Gabriel Massé
/// Date : 19/11/2019 12:23
///-----------------------------------------------------------------

using Com.IsartDigital.IsartGDP2Rush3CSetup.Rush;
using UnityEngine;

namespace Com.DefaultCompany.Rush.Rush.Tiles {
	public class Arrow : HasActionOnCube {

        

        protected override void ActionOnCube(Cube cube)
        {
            cube.transform.rotation = Quaternion.LookRotation(transform.forward, transform.up);
            cube.MovementDirection = transform.forward;
            cube.SetModeMove();
        }
    }
}