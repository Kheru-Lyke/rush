///-----------------------------------------------------------------
/// Author : Gabriel Massé
/// Date : 19/11/2019 15:10
///-----------------------------------------------------------------

using Com.IsartDigital.IsartGDP2Rush3CSetup.Rush;
using UnityEngine;

namespace Com.DefaultCompany.Rush.Rush.Tiles {
	public class HasActionOnCube : Tile {
        protected virtual void ActionOnCube(Cube cube)
        {
        }

        public void Place()
        {
            groundAnimator = transform.parent.GetComponent<Animator>();
        }

        private void Start()
        {
            groundAnimator =  transform.parent?.GetComponent<Animator>();
        }

        public override void Action(Cube cube)
        {
            base.Action(cube);

            GetComponent<Renderer>().material.color = color;
            Renderer slime = GetComponentInParent<Ground>().slime;
            slime.gameObject.SetActive(true);
            slime.material.color = color;

            ActionOnCube(cube);
        }
    }
}