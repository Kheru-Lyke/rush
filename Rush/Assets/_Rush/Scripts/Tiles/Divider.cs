///-----------------------------------------------------------------
/// Author : Gabriel Massé
/// Date : 17/11/2019 10:31
///-----------------------------------------------------------------

using Com.IsartDigital.IsartGDP2Rush3CSetup.Rush;
using System;
using UnityEngine;

namespace Com.DefaultCompany.Rush.Rush.Tiles {
	public class Divider : HasActionOnCube {

        [SerializeField] Animator animator;

        private int _direction = -1;

        private void Start()
        {
            groundAnimator = transform.parent?.GetComponent<Animator>();
        }

        public int Direction
        {
            get
            {
                if (_direction < 0) animator.SetBool("Right", true);
                else animator.SetBool("Right", false);
                return _direction *= -1;
            }
        }

        protected override void ActionOnCube(Cube cube)
        {
            cube.MovementDirection = Quaternion.AngleAxis(Direction * 90, transform.up) * cube.transform.forward;
            cube.SetModeMove();

            animator.SetTrigger("Turn");
        }
    }
}