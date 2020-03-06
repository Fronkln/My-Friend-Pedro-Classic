using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MFPClassic
{
    static class Extensions
    {
        public static SwitchScript GetSwitch(this UnityEngine.MonoBehaviour mono)
        {
            return mono.GetComponent<SwitchScript>();
        }
    }
}
