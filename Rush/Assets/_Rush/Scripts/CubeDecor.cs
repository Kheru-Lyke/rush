///-----------------------------------------------------------------
/// Author : Gabriel Massé
/// Date : 02/12/2019 12:44
///-----------------------------------------------------------------

using Com.IsartDigital.IsartGDP2Rush3CSetup.Rush;
using UnityEngine;

namespace Com.DefaultCompany.Rush.Rush {
	public class CubeDecor : Cube {

        new private void Start()
        {

            color = GetComponentInChildren<Renderer>().material.color;
            animator = GetComponent<Animator>();
            sound = GetComponent<AudioSource>();
            allAnimators.Add(animator);

            Ticker.Instance.DecorTick += Tick;
            raycastDistance = (cubeSide / 2) + raycastOffsetDistance;

            cubeFaceDiagonal = Mathf.Sqrt(2) * cubeSide;
            rotationOffsetY = cubeFaceDiagonal / 2 - cubeSide / 2;

            MovementDirection = transform.forward;
            movementRotation = Quaternion.AngleAxis(90f, transform.right);

            toPosition = transform.position;

            setModeSpawn();
            DoAction = DoActionVoid;
        }
    }
}