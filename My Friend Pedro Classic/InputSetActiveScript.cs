using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MFPClassic
{
    public class InputSetActiveScript : MonoBehaviour
    {
        public SwitchScript inputSwitch;
        private SwitchScript switchScript;

        public GameObject[] activatableObjects;

        bool doOnce = false;


        public InputSetActiveScript()
        {
            switchScript = gameObject.AddComponent<SwitchScript>();
        }

        public void Start()
        {
            if (activatableObjects.Length != 0)
            {
                for (int i = 0; i < activatableObjects.Length; i++)
                    if(activatableObjects[i] != null)
                    activatableObjects[i].SetActive(false);
            }
        }

        public void Update()
        {
            if (!doOnce)
                if (inputSwitch.output >= 1)
                {
                    doOnce = true;

                    for(int i = 0; i < activatableObjects.Length; i++)
                        activatableObjects[i].SetActive(true);

                    switchScript.output = 1;
                }
        }

    }
}
