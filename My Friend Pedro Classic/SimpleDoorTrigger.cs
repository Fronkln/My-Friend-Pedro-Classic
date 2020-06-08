using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MFPClassic
{
    public class SimpleDoorTrigger : MonoBehaviour
    {
        public SpawnDoorScript[] spawnDoors;
        private RootScript root;


        public bool doOnce = true;
        bool doOnceS = true;


        public void Start()
        {
            root = MFPClassicAssets.root;
        }

        public void saveState()
        {
            doOnceS = doOnce;
        }

        public void loadState()
        {
            doOnce = doOnceS;
        }

        public void LateUpdate()
        {
            if (root.doCheckpointSave)
                saveState();
            if (root.doCheckpointLoad)
                return;
            loadState();
        }


        public void OnTriggerEnter(Collider coll)
        {
            MFPEditorUtils.Log(doOnce.ToString());
            if (doOnce)
            {
                if (coll.transform.name == "Player")
                {
                    for (int i = 0; i < spawnDoors.Length; i++)
                        spawnDoors[i].isTriggered = true;

                    doOnce = false;
                    MFPEditorUtils.Log("Triggered...");
                }
            }
        }
    }
}
