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
        private RootScript root;

        public SwitchABMoveScript elevator;

        public SpawnDoorScript[] spawnDoors;

        bool enemiesKilled = false;
        bool enemiesKilledS = false;

        public ElevatorScript()
        {
            switchScript = gameObject.AddComponent<SwitchScript>();
        }

        public void Start()
        {
            root = MFPClassicAssets.root;
        }

        public void saveState()
        {
            enemiesKilledS = enemiesKilled;
        }

        public void loadState()
        {
            enemiesKilled = enemiesKilledS;
        }

        public void LateUpdate()
        {
            if (root.doCheckpointSave)
                saveState();
            if (root.doCheckpointLoad)
                return;
            loadState();
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
            if (spawnDoors != null)
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
