///-----------------------------------------------------------------
/// Author : Gabriel Massé
/// Date : 08/11/2019 11:56
///-----------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Com.DefaultCompany.Rush.Rush.UI {
    [RequireComponent(typeof(Button))]
	public class Play : MonoBehaviour {
        private Button playBtn;

        static public Action OnPressed;
	
		private void Start () {
            playBtn = GetComponent<Button>();

            playBtn.onClick.AddListener(delegate
            {
                DoActionPlay();
            });
        }

        private void DoActionPlay()
        {
            OnPressed();
        }

        private void Update () {
			
		}
	}
}