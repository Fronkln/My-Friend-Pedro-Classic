using System;
using UnityEngine;

namespace MFPClassic
{
    public class VictorAnimEvents : MonoBehaviour
    {
        Victor vic;
        AudioClip footStep;

        int doSteps = 5;


        public void Awake()
        {
            vic = GetComponent<Victor>();
            footStep = MFPClassicAssets.classicBundle.LoadAsset("boss_footstep") as AudioClip;
        }

        public void Step()
        {
            MFPEditorUtils.Log("Step");

            GameObject.FindObjectOfType<CameraScript>().bigScreenShake = 0.5f;
            vic.audioSource.PlayOneShot(footStep);
            //MFPClassicAssets.root.rumble(1, 1000, 1f);
        }

        public void EndStep()
        {
            doSteps--;

            if (doSteps != 0)
                vic.victorAnimator.Play("movetoScene", 0, 0);
            else
            {
                FinalBattleController.inst.bossfightUI.enabled = true;
                FinalBattleController.inst.battleStarted = true;
            }
        }
    }
}
