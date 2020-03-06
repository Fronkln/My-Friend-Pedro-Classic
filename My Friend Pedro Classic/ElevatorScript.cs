using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MFPClassic
{
    public class ElevatorScript : MonoBehaviour
    {
        public SwitchScript inputSwitch;

        private SwitchScript switchScript;
        public SwitchABMoveScript elevator;

        public SpawnDoorScript[] spawnDoors;

        bool enemiesKilled = false;

        public ElevatorScript()
        {
            switchScript = gameObject.AddComponent<SwitchScript>();
        }


        public void OnTriggerStay(Collider coll)
        {
            if (enemiesKilled)
                if (coll.name == "Player")
                {
                    if (inputSwitch.output == 1)
                        switchScript.output = -1;
                }
        }

        public void Update()
        {
            if(spawnDoors != null)
            {
                if (!enemiesKilled)
                {
                    int validation = 0;

                    foreach (SpawnDoorScript door in spawnDoors)
                        if (door.nrOfEnemies <= 0)
                            validation++;

                    if (validation == spawnDoors.Length)
                    {
                        enemiesKilled = true;
                        MFPEditorUtils.Log("Elevator coming down...");

                        switchScript.output = 1;
                    }
                }

            }

        }
    }
}
