using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MFPClassic
{
    public class ActivateSpeechOnInput: MonoBehaviour
    {

        public SpeechTriggerControllerScript tracking, target;

        public void Start()
        {
           // input = gameObject.AddComponent<SwitchScript>();
        }

        public void Update()
        {
            if (tracking == null)
            {

                target.StartCoroutine("triggerSpeech");
                MFPEditorUtils.Log("Triggered");
                Destroy(this);
            }
            
        }
    }
}
