using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MFPClassic
{
    public class ClearInstructionsOnTrigger : MonoBehaviour
    {
        public void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                MFPClassicAssets.root.clearInstructionText();
                Destroy(this);
            }
        }
    }
}
