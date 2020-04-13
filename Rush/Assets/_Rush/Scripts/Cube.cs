///-----------------------------------------------------------------
/// Author : Gabriel Massé
/// Date : 25/10/2019 11:29
///-----------------------------------------------------------------

using Com.DefaultCompany.Rush.Rush;
using Com.DefaultCompany.Rush.Rush.Tiles;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.IsartDigital.IsartGDP2Rush3CSetup.Rush
{
    [RequireComponent(typeof(Animator))]
    public class Cube : MonoBehaviour
    {
        protected Vector3 fromPosition;
        protected Vector3 toPosition;


        protected Quaternion movementRotation;

        protected float rotationOffsetY = 0f;

        protected float cubeSide = 1f;
        protected float cubeFaceDiagonal = 0f;

        protected float raycastDistance = 0f;
        protected float raycastOffsetDistance = 0.4f;
        protected Vector3 down;
        protected RaycastHit hit;

        public Color color;

        public Animator animator;
        static protected List<Animator> allAnimators = new List<Animator>();
        static protected float animatorSpeed
        {
            get { return _animSpeed; }
            set
            {
                _animSpeed = value;

                foreach (var item in allAnimators)
                {
                    item.SetFloat("Speed", value);
                }
            }
        }


        protected Action SetMode;
        protected Action DoAction;

        static public Action CubeDestroyed;
        static public Action CubeDied;

        protected int elapsedTicks = 0;
        protected Vector3 temporaryDirection;
        public Vector3 nextForcedPosition;
        protected bool toDestroy;

        public static Cube deadCube;

        protected AudioSource sound;
        [SerializeField] AudioClip roll;
        [SerializeField] AudioClip bump;

        protected void Start()
        {
            color = GetComponentInChildren<Renderer>().material.color;
            animator = GetComponent<Animator>();
            sound = GetComponent<AudioSource>();
            allAnimators.Add(animator);

            Ticker.Instance.Timer += Tick;
            raycastDistance = (cubeSide / 2) + raycastOffsetDistance;

            cubeFaceDiagonal = Mathf.Sqrt(2) * cubeSide;
            rotationOffsetY = cubeFaceDiagonal / 2 - cubeSide / 2;

            MovementDirection = transform.forward;
            movementRotation = Quaternion.AngleAxis(90f, transform.right);

            toPosition = transform.position;

            setModeSpawn();
            DoAction = DoActionVoid;
        }


        protected void setModeSpawn()
        {
            animator.SetTrigger("Spawn");
        }

        //              UPDATES

        virtual protected void Tick()
        {
            CheckCollisions();

            if (DoAction != DoActionFall && DoAction != doActionWait) elapsedTicks = 0;
            DoAction();
        }

        public void UpdatePosition()
        {
            transform.position = toPosition;
            transform.rotation = Quaternion.LookRotation(MovementDirection, transform.up);

            //CheckCollisions();
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Cube>()) onDeath();
        }

        protected void onDeath()
        {
            animator.SetTrigger("Loose");
            deadCube = this;
            CubeDied();
        }

        protected void CheckCollisions()
        {
            down = Vector3.down;

            if (Physics.Raycast(toPosition, down, out hit, raycastDistance, groundLayer))
            {
                GameObject hitObject = hit.collider.gameObject;
                hitObject.GetComponentInParent<Tile>()?.Action(this);
            }
            else
            {
                SetModeFall();
            }

        }

        public void CheckWall()
        {
            Vector3 previousDirection = MovementDirection;

            if (Physics.Raycast(transform.position + new Vector3(0,0.4f), previousDirection, out hit, raycastDistance))
            {
                MovementDirection = Quaternion.AngleAxis(90, Vector3.up) * previousDirection;
                InitNextForcedMovement(transform.position);
                transform.rotation = Quaternion.LookRotation(previousDirection, Vector3.up);
                SetModeBump();
            }
            else ActuallySetMove();
        }

        protected void SetModeBump()
        {
            DoAction = DoActionBump;
        }

        protected void DoActionBump()
        {
            sound.volume = 1;
            sound.clip = bump;
            sound.Play();
            animator.SetTrigger("Bump");
        }



        //              DESTROY
        public void SetModeDestroy()
        {
            animator.SetTrigger("Destroy");
            GetComponent<Collider>().enabled = false;
        }

        protected void DoActionDestroy()
        {
            allAnimators.Remove(animator);
            CubeDestroyed();
            Destroy(this.gameObject);
        }


        protected void OnDestroy()
        {
            if (Ticker.Instance != null) Ticker.Instance.Timer -= Tick;
        }


        //              MOVE
        protected Vector3 _direction;
        public Vector3 MovementDirection
        {
            get { return _direction; }
            set
            {
                transform.rotation = Quaternion.LookRotation(value, Vector3.up);
                _direction = value;
            }
        }

        protected void InitNextMovement()
        {
            fromPosition = toPosition;
            toPosition = fromPosition + MovementDirection;
        }

        static public void SetSpeed(Slider slider)
        {
            animatorSpeed = slider.value;
        }

        public void SetModeMove()
        {
            CheckWall();
        }

        protected void ActuallySetMove()
        {
            InitNextMovement();
            DoAction = DoActionMove;
        }

        protected void DoActionMove()
        {
            sound.volume = 0.5f;
            sound.clip = roll;
            sound.Play();
            animator.SetTrigger("Move");
        }

        //              FALL
        protected void InitNextFallingMovement()
        {
            fromPosition = transform.position;
            toPosition = fromPosition + Vector3.down;
        }

        protected void SetModeFall()
        {
            InitNextFallingMovement();
            DoAction = DoActionFall;
        }

        protected void DoActionFall()
        {
            if (elapsedTicks >= 4) onDeath();
            animator.SetTrigger("Fall");
            elapsedTicks++;
        }


        //              STOP
        protected int _pauseDelay = 0;

        public int PauseDelay
        {
            get { return _pauseDelay; }
            set
            {
                _pauseDelay = value;
                elapsedTicks = 0;
            }
        }

        public void SetModeStop(int duration)
        {
            PauseDelay = duration;
            toPosition = transform.position;
            DoAction = doActionWait;
        }

        protected void doActionWait()
        {
            animator.SetTrigger("Wait");
            elapsedTicks++;
        }

        public void SetModeVoid()
        {
            DoAction = DoActionVoid;
            animator.speed = 0;
        }
        protected void DoActionVoid()
        {
        }

        //              FORCED MOVEMENT

        protected void InitNextForcedMovement(Vector3 nextPos)
        {
            fromPosition = transform.position;
            toPosition = nextPos;
        }

        //              conveyor
        public void setModeConvey()
        {
            nextForcedPosition += Vector3.up * 0.5f;
            InitNextForcedMovement(nextForcedPosition);
            transform.LookAt(nextForcedPosition);
            DoAction = DoActionConvey;
        }

        protected void DoActionConvey()
        {
            animator.SetTrigger("Convey");
        }



        //              teleporter


        protected bool justMoved;
        protected static float _animSpeed = 1;
        [SerializeField] protected LayerMask groundLayer;

        public void setModeTeleport()
        {
            nextForcedPosition += Vector3.up * 0.5f;
            InitNextForcedMovement(nextForcedPosition);
            DoAction = DoActionTeleport;
        }

        protected void DoActionTeleport()
        {
            animator.SetTrigger("Teleport");
        }
    }
}