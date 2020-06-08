﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MFPClassic
{
    public class FinalBattleController : MonoBehaviour
    {
        public SwitchScript startSwitch;
        public Victor victorBoss;


        public static Vector3 playerPosHack;

        private bool startBattleDoOnce = false;

        IEnumerator WaitPlayerWings()
        {
            yield return new WaitForSeconds(2);
            MFPClassicAssets.player.gameObject.AddComponent<PlayerWings>();
        }

        public void Awake()
        {
            victorBoss = GameObject.FindObjectOfType<Victor>();
        }

        public void Update()
        {
            MFPClassicAssets.player.propellerHat = true;

            if (startSwitch.output == 1)
            {
                if (!startBattleDoOnce)
                {
                    startBattleDoOnce = true;
                    victorBoss.victorAnimator.Play("movetoScene", 0, 0);
                }
            }
            else
                    if (MFPClassicAssets.player.dodging)
                MFPClassicAssets.player.nrOfDodgeSpins = 0;
        }

        public void LateUpdate()
        {
            if(MFPClassicAssets.root.doCheckpointSave)
                playerPosHack = MFPClassicAssets.player.transform.position;
            if (MFPClassicAssets.root.doCheckpointLoad)
            {
                //PlayerWings does some stupid shit to player's saved position on checkpoint so a simple workaround

                if (!MFPClassicAssets.player.GetComponent<PlayerWings>())
                    MFPClassicAssets.player.gameObject.AddComponent<PlayerWings>().checkpointFix = true;

                MFPClassicAssets.player.transform.position = playerPosHack;
            }
        }
    }
}
