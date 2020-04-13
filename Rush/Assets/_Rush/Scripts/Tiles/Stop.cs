///-----------------------------------------------------------------
/// Author : Gabriel Massé
/// Date : 19/11/2019 15:50
///-----------------------------------------------------------------

using Com.IsartDigital.IsartGDP2Rush3CSetup.Rush;
using UnityEngine;

namespace Com.DefaultCompany.Rush.Rush.Tiles {
	public class Stop : HasActionOnCube {

        private void Start()
        {
            groundAnimator = transform.parent?.GetComponent<Animator>();
        }
        protected override void ActionOnCube(Cube cube)
        {
            if (cube.PauseDelay == 0) cube.SetModeStop(1);
            else
            {
                cube.PauseDelay = 0;
                cube.SetModeMove();
            }
        }
    }
}