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
        private Vector3 finalPos = Vector3.zero;
        private Quaternion startRot;

        void Start()
        {
            player = PlayerScript.PlayerInstance;
            startRot = transform.rotation;

            MFPClassicAssets.rootShared.modSideOnCamera = true;
        }

        void LateUpdate()
        {
            if (player.overrideControls) //player is in auto control zone
                transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);

            //transform.rotation = startRot;
        }
    }
}
