using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MFPClassic
{
    public class EnemyWaveInput : MonoBehaviour
    {
        public SwitchScript switchScript;
        public SpawnDoorScript[] spawnDoors;


        private bool doOnce = false;

        public EnemyWaveInput()
        {
            switchScript = gameObject.AddComponent<SwitchScript>();
            switchScript.output = -1;
        }

        public void Update()
        {

            int nrOfFinishedDoors = 0;

            if (!doOnce)
            {
                for (int i = 0; i < spawnDoors.Length; i++)
                {
                    if (spawnDoors[0].nrOfEnemies <= 0)
                        nrOfFinishedDoors++;
                }

                if (nrOfFinishedDoors == spawnDoors.Length)
                {
                    switchScript.output = 1;
                    doOnce = true;
                }
            }
        }

    }
}
