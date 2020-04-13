///-----------------------------------------------------------------
/// Author : Gabriel Massé
/// Date : 05/11/2019 19:13
///-----------------------------------------------------------------

using Com.IsartDigital.IsartGDP2Rush3CSetup.Rush;
using UnityEngine;

namespace Com.DefaultCompany.Rush.Rush.Tiles {
    [RequireComponent (typeof(Animator))]
	public class Tile : MonoBehaviour
    {
        protected Color color;
        protected Animator groundAnimator;

        private void Start()
        {
            groundAnimator = GetComponentInParent<Animator>();
        }

        public virtual void Action(Cube cube)
        {
            color = cube.color;
            groundAnimator.SetTrigger("Cube");
        }

    }
}