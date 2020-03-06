using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MFPClassic
{
    public class CustomZPusher : MonoBehaviour
    {
        public float pushIntensity = 3;

        void OnTriggerStay(Collider coll)
        {
            if (coll.transform.name == "Player")
                coll.gameObject.GetComponent<PlayerScript>().zPushBackPublic = pushIntensity;
        }
    }
}
