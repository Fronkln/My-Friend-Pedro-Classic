using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MFPClassic
{
    //used for the camera in boss fight level
    public class SimpleTrackPlayer : MonoBehaviour
    {
        private PlayerScript player;
        private CameraScript camscript;


        void Start()
        {
            camscript = GetComponent<CameraScript>();
            player = PlayerScript.PlayerInstance;

            if (MFPClassicAssets.rootShared.modSideOnCamera)
            {
                FinalBattleController.inst.playerAlreadyHadEnabledSideCam = true;
                return;
            }

            MFPClassicAssets.rootShared.modSideOnCamera = true;
        }

        void LateUpdate()
        {
            if (player.overrideControls) //player is in auto control zone
                camscript.centerCamPosOnPlayer();

            //transform.rotation = startRot;
        }
    }
}
