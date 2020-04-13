///-----------------------------------------------------------------
/// Author : Gabriel Massé
/// Date : 29/10/2019 15:42
///-----------------------------------------------------------------

using Com.IsartDigital.IsartGDP2Rush3CSetup.Rush;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.DefaultCompany.Rush.Rush
{
    [RequireComponent(typeof(ParticleSystem))]
    public class Spawner : MonoBehaviour {

        [SerializeField] GameObject cubePrefab;

        [SerializeField] private int spawnFrequency = 1;
        [SerializeField] private int spawnDelay = 0;
        [SerializeField] private int nCubes = 5;

        private int nTicksElapsed = 0;
        private int nCubeSpawned = 0;
        private Action DoAction;
        private Color cubeColor;

        static public Action Spawned;
        static public List<Spawner> spawners = new List<Spawner>();
        public List<Cube> cubes = new List<Cube>();

        private AudioSource sound;

        private void Start()
        {
            Restart(); 
            Ticker.Instance.Timer += DoAction;
            sound = GetComponent<AudioSource>();
            spawners.Add(this);
        }
        
        public void Restart()
        {
            DoAction = DoActionStart;

            cubeColor = GetComponent<Renderer>().material.color;
            GetComponent<ParticleSystem>().Play(true);
            var particle = GetComponent<ParticleSystem>().main;
            particle.startColor = cubeColor;
        }

        static public void DestroyAllCubes()
        {

            foreach (var spawner in spawners)
            {
                spawner.DestroyCubes();
            }
            
        }

        virtual protected void DestroyCubes()
        {
            Cube item;
            for (int i = cubes.Count - 1; i >= 0; i--)
            {
                item = cubes[i];
                if (item != null) GameObject.Destroy(item.gameObject);
            }

            cubes = new List<Cube>();
            nTicksElapsed = 0;
            nCubeSpawned = 0;
        }

        private void DoActionStart()
        {
            GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            nTicksElapsed ++;

            if (nTicksElapsed >= spawnDelay)
            {
                nTicksElapsed = spawnFrequency;
                SetModeSpawn();
            }
        }

        private void SetModeSpawn()
        {
            Ticker.Instance.Timer -= DoAction;
            DoAction = DoActionSpawn;
            Ticker.Instance.Timer += DoAction;
        }

        private void DoActionSpawn()
        {
            if (nTicksElapsed >= spawnFrequency)
            {
                cubes.Add(Spawn());
                nTicksElapsed = 0;
            }

            nTicksElapsed++;
        }

        private Cube Spawn()
        {
            Cube cube = Instantiate(cubePrefab).GetComponent<Cube>();
            cube.transform.position = transform.position + new Vector3(0, 0.5f, 0);
            cube.transform.forward = transform.forward;

            foreach (var item in cube.GetComponentsInChildren<Renderer>())
            {
                item.material.SetColor("_Color", cubeColor);
            } 

            nCubeSpawned++;
            if (nCubeSpawned >= nCubes) SetModeVoid();

            Spawned();
            sound.Play();

            return cube;
        }

        private void SetModeVoid()
        {
            Ticker.Instance.Timer -= DoAction;
            DoAction = DoActionVoid;
            Ticker.Instance.Timer += DoAction;
        }

        private void DoActionVoid() {}

        private void OnDestroy()
        {
            if (Ticker.Instance != null) Ticker.Instance.Timer -= DoAction;
        }
    }
}