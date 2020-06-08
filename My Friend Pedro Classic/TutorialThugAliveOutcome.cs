using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MFPClassic
{
    public class TutorialThugAliveOutcome : MonoBehaviour
    {

        public SpeechTriggerScript target;
        private EnemyScript tutorialNPC;



        public void Update()
        {
            if(MFPClassicAssets.root.allEnemies.Length != 0)
            {
                if (tutorialNPC == null)
                    tutorialNPC = MFPClassicAssets.root.allEnemies[0].GetComponent<EnemyScript>();

                if(!tutorialNPC.enabled)
                    Destroy(this.gameObject);
            }
        }

        /*
        public void OnTriggerEnter(Collider collision)
        {
            if (collision.transform.tag == "Player" && target != null)
            {
                if(MFPClassicAssets.root.allEnemies[0].GetComponent<EnemyScript>().enabled)
                target.locStringId = "pedro04d_level1_MFPClassic";
            }
        }
        */
    }
}
