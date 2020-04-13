///-----------------------------------------------------------------
/// Author : Gabriel Massé
/// Date : 06/11/2019 10:19
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.DefaultCompany.Rush.Rush.UI {
	public class RotationTile : MonoBehaviour {
        private Transform mainCamera;
        [SerializeField] private Transform level;

        private Vector3 cameraToLevel;

        private void Start()
        {
            mainCamera = Camera.main.transform;
            UpdateRotation();
        }

        private void Update () {
            UpdateRotation();
        }

        private void UpdateRotation()
        {
            cameraToLevel = level.position - mainCamera.position;
            transform.rotation = Quaternion.FromToRotation(Vector3.ProjectOnPlane(cameraToLevel, level.up), level.forward);
        }
	}
}