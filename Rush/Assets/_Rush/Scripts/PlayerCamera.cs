///-----------------------------------------------------------------
/// Author : Gabriel Massé
/// Date : 04/11/2019 16:37
///-----------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Com.DefaultCompany.Rush.Rush
{
    public class PlayerCamera : MonoBehaviour
    {
        [Header("Gameplay phase")]
        [SerializeField]
        private float rotationSpeed = 0.25f;
        private const float fixedcameraDistance = 10;
        [SerializeField] private Transform center;

        [Space]
        [SerializeField] private string horizontal;
        [SerializeField] private string vertical;
        [Space]
        [Header("UI phase")]
        [SerializeField, Range(1f, 7f)] private float speed;
        [SerializeField] private Transform[] waypoints;

        private Vector3 oldMousePosition;
        private Vector2 movement;
        private bool usingUI = false;

        private Action doAction;

        private int index;
        private int moveIndex = 1;

        private static PlayerCamera instance;
        public static PlayerCamera Instance { get { return instance; } }

        private void Update()
        {
            doAction();
        }

        private void Awake()
        {
            if (instance)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
        }

        public void setModeUI()
        {
            doAction = doActionUI;
        }

        private void doActionUI()
        {
            Vector3 nextPosition = waypoints[index].position;

            transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);

            if (transform.position == nextPosition)
            {
                index += moveIndex;

                if (index == 0 || index == waypoints.Length - 1)
                {
                    moveIndex *= -1;
                }
            }

            transform.LookAt(center.position, Vector3.up);
        }

        public void SetModeGameplay()
        {
            cameraPos = transform.position - center.position;
            cameraPos = cameraPos.normalized * fixedcameraDistance;
            transform.position = center.position + cameraPos;
            doAction = doActionGameplay;
        }
        private void doActionGameplay()
        {
            if (Input.GetAxis(vertical) != 0 || Input.GetAxis(horizontal) != 0)
            {
                movement = new Vector2(Input.GetAxis(horizontal), Input.GetAxis(vertical));
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (EventSystem.current.IsPointerOverGameObject(-1)) usingUI = true;
                    else usingUI = false;
                    oldMousePosition = Input.mousePosition;
                }

                if (Input.GetMouseButton(0)) movement = new Vector3((oldMousePosition.x - Input.mousePosition.x) / 100,
                                        (oldMousePosition.y - Input.mousePosition.y) / 100);

                if (Input.GetMouseButtonUp(0)) movement = new Vector2();
            }

            if (!usingUI) RotateCamera(movement);
        }

        private void RotateCamera(Vector2 movement)
        {
            if (movement.y < 0 && Vector3.Cross(transform.forward, Vector3.up).magnitude >= 0.3f)
            {
                transform.RotateAround(center.position, transform.right, rotationSpeed * movement.y * Time.deltaTime);
            }
            else if (movement.y > 0 && Vector3.Cross(transform.forward, Vector3.up).magnitude >= 0.3f)
            {
                transform.RotateAround(center.position, transform.right, rotationSpeed * movement.y * Time.deltaTime);
            }

            if (movement.x < 0)
            {
                transform.RotateAround(center.position, transform.up, -rotationSpeed * movement.x * Time.deltaTime);
            }
            else if (movement.x > 0)
            {
                transform.RotateAround(center.position, transform.up, -rotationSpeed * movement.x * Time.deltaTime);
            }


            transform.LookAt(center.position, Vector3.up);
        }
        Vector3 cameraPos; Quaternion goal;
        public void SetModeLoose(Transform cube)
        {
            cameraPos = transform.position - cube.position;
            cameraPos = cameraPos.normalized * 5;
            cameraPos = cube.position + cameraPos;
            goal = Quaternion.LookRotation(cube.position - transform.position, Vector3.up);
            doAction = DoActionLoose;
        }

        private void DoActionLoose()
        {
            Quaternion.RotateTowards(transform.rotation, goal, 5 * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, cameraPos, 5 * Time.deltaTime);
        }
    }
}