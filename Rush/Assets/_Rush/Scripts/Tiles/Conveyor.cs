///-----------------------------------------------------------------
/// Author : Gabriel Massé
/// Date : 27/11/2019 11:01
///-----------------------------------------------------------------

using Com.IsartDigital.IsartGDP2Rush3CSetup.Rush;
using UnityEngine;

namespace Com.DefaultCompany.Rush.Rush.Tiles {
	public class Conveyor : HasActionOnCube {

        [SerializeField] private Animator animator;
        [SerializeField] private AudioSource sound;

        private void Start()
        {
            sound = GetComponent<AudioSource>();
        }

        public override void Action(Cube cube)
        {
            color = cube.color;

            GetComponent<Renderer>().material.color = color;
            Renderer slime = GetComponentInParent<Ground>().slime;
            slime.gameObject.SetActive(true);
            slime.material.color = color;

            ActionOnCube(cube);
        }

        protected override void ActionOnCube(Cube cube)
        {
            cube.nextForcedPosition = transform.position + transform.forward;
            sound.Play();
            cube.setModeConvey();

            animator.SetTrigger("Active");
        }
    }
}