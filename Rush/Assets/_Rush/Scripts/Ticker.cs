///-----------------------------------------------------------------
/// Author : Gabriel Massé
/// Date : 29/10/2019 15:49
///-----------------------------------------------------------------

using Com.DefaultCompany.Rush.Rush.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Com.DefaultCompany.Rush.Rush {
	public class Ticker : MonoBehaviour {
		protected static Ticker instance;
		public static Ticker Instance { get { return instance; } }

        public float remainingTime { get; private set; }

        protected float baseSpeed = 0.9f;
        protected float speed = 0.9f;
        protected float elapsedTime = 0f;
        protected float durationBetweenTicks = 1f;

        public Action Timer;
        public Action DecorTick;


        protected Action Call;

        protected void Awake(){
			if (instance){
				Destroy(gameObject);
				return;
			}
			
			instance = this;
            Call = CallDecor;

		}

        private void CallBoth()
        {
            Timer();
            DecorTick();
        }
        private void CallDecor()
        {
            DecorTick();
        }

        public void StartTicking()
        {
            elapsedTime = 0f;
            Call = CallBoth;
        }

        public void StopTicking()
        {
            Call = CallDecor;
        }

        public void SetSpeed(Slider slider)
        {
            speed = baseSpeed * slider.value;
        }

        static protected void VoidFunction() {}

        protected void Tick()
        {
            if (elapsedTime > durationBetweenTicks)
            {
                elapsedTime = 0f;
                Call();
            }

            remainingTime = durationBetweenTicks - elapsedTime;
            elapsedTime += Time.deltaTime * speed;
            
        }

        protected void Update () {
            Tick();
        }
		
		protected void OnDestroy(){
			if (this == instance) instance = null;
		}
	}
}