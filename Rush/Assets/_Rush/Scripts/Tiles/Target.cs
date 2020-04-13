///-----------------------------------------------------------------
/// Author : Gabriel Massé
/// Date : 05/11/2019 14:20
///-----------------------------------------------------------------

using Com.IsartDigital.IsartGDP2Rush3CSetup.Rush;
using UnityEngine;

namespace Com.DefaultCompany.Rush.Rush.Tiles {
	public class Target : HasActionOnCube {
        [SerializeField] Spawner spawner;

        protected Cube cube;
        private AudioSource sound;

        private void Start()
        {
            groundAnimator = transform.parent.GetComponent<Animator>();
            sound = GetComponent<AudioSource>();
        }

        protected override void ActionOnCube(Cube cube)
        {
            if (spawner.cubes.Contains(cube))
            {
                sound.Play();
                cube.SetModeDestroy();
            }
            else cube.SetModeMove();

        }
    }
}