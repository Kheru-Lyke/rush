///-----------------------------------------------------------------
/// Author : Gabriel Massé
/// Date : 29/10/2019 14:39
///-----------------------------------------------------------------

using Com.IsartDigital.IsartGDP2Rush3CSetup.Rush;
using UnityEngine;

namespace Com.DefaultCompany.Rush.Rush.Tiles
{
    [RequireComponent(typeof(AudioSource))]
    public class Teleporter : HasActionOnCube
    {
        [SerializeField] Teleporter pairedTeleporter;
        public Animator animator;
        public bool activated = false;

        [SerializeField] private AudioSource sound;

        private void Start()
        {
            groundAnimator = transform.parent.GetComponent<Animator>();
            sound = GetComponent<AudioSource>();
        }

        public override void Action(Cube cube)
        {
            animator = GetComponent<Animator>();
            base.Action(cube);
        }

        protected override void ActionOnCube(Cube cube)
        {
            if (!pairedTeleporter.activated)
            {
                cube.nextForcedPosition = pairedTeleporter.transform.position;
                cube.setModeTeleport();

                animator.SetBool("Entry", true);
                animator.SetTrigger("Teleport");
                sound.Play();
                activated = true;

                pairedTeleporter.animator.SetBool("Entry", false);
                pairedTeleporter.animator.SetTrigger("Teleport");
                pairedTeleporter.activated = false;
            }
            else
            {
                cube.SetModeMove();
                pairedTeleporter.activated = false;
            }
        }
    }
}