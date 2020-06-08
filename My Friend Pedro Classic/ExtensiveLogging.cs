using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MFPClassic
{
    public static class ExtensiveLogging
    {
        public static void Log(object obj)
        {
            if(obj is AutoControlZoneScript)
            {
                AutoControlZoneScript aczs = obj as AutoControlZoneScript;

                if (aczs.inputSwitch != null)
                    foreach (SwitchScript switchScript in aczs.inputSwitch)
                        MFPEditorUtils.Log("inputSwitch " + switchScript.transform.name);

                if (aczs.forceOffSwitch != null)
                    MFPEditorUtils.Log("forceOffSwitch " + aczs.forceOffSwitch.transform.name);

                MFPEditorUtils.Log("forceInterruptPlayerOverride " + aczs.forceInteruptPlayerOverride.ToString());

                MFPEditorUtils.Log("enableOverrideOnEnter " + aczs.enableOverrideOnEnter.ToString());
                MFPEditorUtils.Log("disableOverrideWhenLeavingZone " + aczs.disableOverrideWhenLeavingZone.ToString());
                MFPEditorUtils.Log("disableOverrideAfterInteract " + aczs.disableOverrideAfterInteract.ToString());
                MFPEditorUtils.Log("disableOverrideAfterTime " + aczs.disableOverrideAfterTime.ToString());
                MFPEditorUtils.Log("disableOverrideTimer " + aczs.disableOverrideTimer.ToString());

                MFPEditorUtils.Log("");

                MFPEditorUtils.Log("reTrigger " + aczs.reTrigger.ToString());
                MFPEditorUtils.Log("triggerDelay " + aczs.triggerDelay.ToString());
                MFPEditorUtils.Log("triggerOnlyOnEnter " + aczs.triggerOnlyOnEnter.ToString());

                MFPEditorUtils.Log("");

                MFPEditorUtils.Log("setXMoveAmount " + aczs.setXMoveAmount.ToString());
                MFPEditorUtils.Log("xMoveAmount " + aczs.xMoveAmount.ToString());
                MFPEditorUtils.Log("crouch " + aczs.crouch.ToString());
                MFPEditorUtils.Log("jump " + aczs.jump.ToString());
                MFPEditorUtils.Log("dodge " + aczs.dodge.ToString());
                MFPEditorUtils.Log("interact " + aczs.interact.ToString());
                MFPEditorUtils.Log("slowMotion " + aczs.slowMotion.ToString());
                MFPEditorUtils.Log("disableSlowMotionOnExit " + aczs.disableSlowMotionOnExit.ToString());
                MFPEditorUtils.Log("reload " + aczs.reload.ToString());
                MFPEditorUtils.Log("fire " + aczs.fire.ToString());
                MFPEditorUtils.Log("secondaryFire " + aczs.secondaryFire.ToString());

                MFPEditorUtils.Log("");

                MFPEditorUtils.Log("setAim " + aczs.setAim.ToString());
                MFPEditorUtils.Log("aimPos " + aczs.aimPos.ToString());
                MFPEditorUtils.Log("secondaryAimPos " + aczs.secondaryAimPos.ToString());

                MFPEditorUtils.Log("");

                MFPEditorUtils.Log("weapon " + aczs.weapon.ToString());
            }
        }
    }
}
