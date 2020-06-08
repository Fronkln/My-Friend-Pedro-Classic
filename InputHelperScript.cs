using System;
using Boo.Lang.Runtime;
using I2.Loc;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000060 RID: 96
[Serializable]
public class InputHelperScript : MonoBehaviour
{
    // Token: 0x0600024E RID: 590 RVA: 0x000031C3 File Offset: 0x000013C3
    public virtual void Awake()
    {
        this.player = ReInput.players.GetPlayer(0);
    }

    // Token: 0x0600024F RID: 591 RVA: 0x00036C64 File Offset: 0x00034E64
    public virtual void Start()
    {
        GameObject x = GameObject.Find("Root");
        if (x != null)
        {
            this.root = (RootScript)GameObject.Find("Root").GetComponent(typeof(RootScript));
        }
    }

    // Token: 0x06000250 RID: 592 RVA: 0x000020A7 File Offset: 0x000002A7
    public virtual void Update()
    {
    }

    // Token: 0x06000251 RID: 593 RVA: 0x00036CAC File Offset: 0x00034EAC
    public virtual GameObject GetInputSymbol(string action, bool isKeyboard)
    {
        string inputButtonName = null;
        if (action == "LOOK")
        {
            if (isKeyboard)
            {
                inputButtonName = "Mouse";
            }
            else if (this.GetInputButtonName("Aim Horizontal", false, false).Contains("Right Stick") && this.GetInputButtonName("Aim Horizontal", true, false).Contains("Right Stick") && this.GetInputButtonName("Aim Vertical", false, false).Contains("Right Stick") && this.GetInputButtonName("Aim Vertical", true, false).Contains("Right Stick"))
            {
                inputButtonName = "Right Stick";
            }
            else
            {
                if (!this.GetInputButtonName("Aim Horizontal", false, false).Contains("Left Stick") || !this.GetInputButtonName("Aim Horizontal", true, false).Contains("Left Stick") || !this.GetInputButtonName("Aim Vertical", false, false).Contains("Left Stick") || !this.GetInputButtonName("Aim Vertical", true, false).Contains("Left Stick"))
                {
                    GameObject gameObject = new GameObject();
                    RectTransform rectTransform = (RectTransform)gameObject.AddComponent(typeof(RectTransform));
                    rectTransform.anchorMax = (rectTransform.anchorMin = Vector2.zero);
                    RectTransform rectTransform2 = (RectTransform)this.GetInputSymbolFromButtonName(this.GetInputButtonName("Aim Horizontal", false, isKeyboard), isKeyboard).GetComponent(typeof(RectTransform));
                    RectTransform rectTransform3 = (RectTransform)this.GetInputSymbolFromButtonName(this.GetInputButtonName("Aim Horizontal", true, isKeyboard), isKeyboard).GetComponent(typeof(RectTransform));
                    RectTransform rectTransform4 = (RectTransform)this.GetInputSymbolFromButtonName(this.GetInputButtonName("Aim Vertical", true, isKeyboard), isKeyboard).GetComponent(typeof(RectTransform));
                    RectTransform rectTransform5 = (RectTransform)this.GetInputSymbolFromButtonName(this.GetInputButtonName("Aim Vertical", false, isKeyboard), isKeyboard).GetComponent(typeof(RectTransform));
                    float x = rectTransform2.sizeDelta.x + rectTransform3.sizeDelta.x + rectTransform4.sizeDelta.x + rectTransform5.sizeDelta.x + (float)25;
                    Vector2 sizeDelta = rectTransform.sizeDelta;
                    float num = sizeDelta.x = x;
                    Vector2 vector = rectTransform.sizeDelta = sizeDelta;
                    int num2 = 30;
                    Vector2 sizeDelta2 = rectTransform.sizeDelta;
                    float num3 = sizeDelta2.y = (float)num2;
                    Vector2 vector2 = rectTransform.sizeDelta = sizeDelta2;
                    rectTransform2.SetParent(rectTransform, false);
                    rectTransform3.SetParent(rectTransform, false);
                    rectTransform4.SetParent(rectTransform, false);
                    rectTransform5.SetParent(rectTransform, false);
                    rectTransform2.anchoredPosition = new Vector2(rectTransform2.sizeDelta.x / (float)2 + (float)5, (float)15);
                    rectTransform3.anchoredPosition = new Vector2(rectTransform2.anchoredPosition.x + rectTransform3.sizeDelta.x / (float)2 + rectTransform2.sizeDelta.x / (float)2 + (float)5, (float)15);
                    rectTransform4.anchoredPosition = new Vector2(rectTransform3.anchoredPosition.x + rectTransform4.sizeDelta.x / (float)2 + rectTransform3.sizeDelta.x / (float)2 + (float)5, (float)15);
                    rectTransform5.anchoredPosition = new Vector2(rectTransform4.anchoredPosition.x + rectTransform5.sizeDelta.x / (float)2 + rectTransform4.sizeDelta.x / (float)2 + (float)5, (float)15);
                    return gameObject;
                }
                inputButtonName = "Left Stick";
            }
        }
        else if(action == "PISTOL")
            inputButtonName = this.GetInputButtonName("Pistol", true, isKeyboard);
        else if (action == "SHOOT")
        {
            inputButtonName = this.GetInputButtonName("Fire", true, isKeyboard);
        }
        else if (action == "SHOOT2")
        {
            inputButtonName = this.GetInputButtonName("Fire2", true, isKeyboard);
        }
        else if (action == "INTERACT")
        {
            inputButtonName = this.GetInputButtonName("Interact", true, isKeyboard);
        }
        else if (action == "ACTION")
        {
            inputButtonName = this.GetInputButtonName("Focus", true, isKeyboard);
        }
        else if (action == "RESTART")
        {
            inputButtonName = this.GetInputButtonName("Interact", true, isKeyboard);
        }
        else if (action == "RELOAD")
        {
            inputButtonName = this.GetInputButtonName("Reload", true, isKeyboard);
        }
        else if (action == "LEFT")
        {
            inputButtonName = this.GetInputButtonName("Move", false, isKeyboard);
        }
        else if (action == "RIGHT")
        {
            inputButtonName = this.GetInputButtonName("Move", true, isKeyboard);
        }
        else if (action == "CROUCH")
        {
            inputButtonName = this.GetInputButtonName("Crouch", false, isKeyboard);
        }
        else if (action == "DODGE")
        {
            inputButtonName = this.GetInputButtonName("Dodge", true, isKeyboard);
        }
        else if (action == "MELEE")
        {
            inputButtonName = this.GetInputButtonName("Kick", true, isKeyboard);
        }
        else if (action == "JUMP")
        {
            inputButtonName = this.GetInputButtonName("Jump", true, isKeyboard);
        }
        else if (action == "CHANGEWEAPON")
        {
            inputButtonName = this.GetInputButtonName("Change Weapon", true, isKeyboard);
        }
        else if (action == "MOTOUP")
        {
            inputButtonName = this.GetInputButtonName("MotorcycleY", true, isKeyboard);
        }
        else if (action == "MOTODOWN")
        {
            inputButtonName = this.GetInputButtonName("MotorcycleY", false, isKeyboard);
        }
        else if (action == "MOTOWHEELIE")
        {
            inputButtonName = this.GetInputButtonName("MotorcycleWheelie", true, isKeyboard);
        }
        else if (action == "MOTOJUMP")
        {
            inputButtonName = this.GetInputButtonName("Jump", true, isKeyboard);
        }
        else if (action == "UISPECIAL1")
        {
            inputButtonName = this.GetInputButtonName("UISpecial1", true, isKeyboard);
        }
        else if (action == "UISPECIAL2")
        {
            inputButtonName = this.GetInputButtonName("UISpecial2", true, isKeyboard);
        }
        else if (action == "UISPECIAL3")
        {
            inputButtonName = this.GetInputButtonName("UISpecial3", true, isKeyboard);
        }
        else if (action == "UISPECIAL4")
        {
            inputButtonName = this.GetInputButtonName("UISpecial4", true, isKeyboard);
        }
        else if (action == "UISUBMIT")
        {
            inputButtonName = this.GetInputButtonName("UISubmit", true, isKeyboard);
        }
        else if (action == "UIBACK")
        {
            inputButtonName = this.GetInputButtonName("UIBack", true, isKeyboard);
        }
        return this.GetInputSymbolFromButtonName(inputButtonName, isKeyboard);
    }

    // Token: 0x06000252 RID: 594 RVA: 0x000373A0 File Offset: 0x000355A0
    public virtual GameObject GetInputSymbolFromButtonName(string inputButtonName, bool isKeyboard)
    {
        object obj = null;
        if (!isKeyboard)
        {
            string text;
            if (inputButtonName == "Right Stick")
            {
                text = "rstick";
            }
            else if (inputButtonName == "Right Stick Left")
            {
                text = "rstick_left";
            }
            else if (inputButtonName == "Right Stick Right")
            {
                text = "rstick_right";
            }
            else if (inputButtonName == "Right Stick Up")
            {
                text = "rstick_up";
            }
            else if (inputButtonName == "Right Stick Down")
            {
                text = "rstick_down";
            }
            else if (inputButtonName == "Right Stick Button")
            {
                text = "r3";
            }
            else if (inputButtonName == "Left Stick")
            {
                text = "lstick";
            }
            else if (inputButtonName == "Left Stick Left")
            {
                text = "lstick_left";
            }
            else if (inputButtonName == "Left Stick Right")
            {
                text = "lstick_right";
            }
            else if (inputButtonName == "Left Stick Up")
            {
                text = "lstick_up";
            }
            else if (inputButtonName == "Left Stick Down")
            {
                text = "lstick_down";
            }
            else if (inputButtonName == "Left Stick Button")
            {
                text = "l3";
            }
            else if ((this.gamepadStyle <= 1 && inputButtonName == "B") || (this.gamepadStyle > 1 && inputButtonName == "A") || inputButtonName == "Cross")
            {
                text = "b1";
            }
            else if ((this.gamepadStyle <= 1 && inputButtonName == "A") || (this.gamepadStyle > 1 && inputButtonName == "B") || inputButtonName == "Circle")
            {
                text = "b2";
            }
            else if ((this.gamepadStyle <= 1 && inputButtonName == "Y") || (this.gamepadStyle > 1 && inputButtonName == "X") || inputButtonName == "Square")
            {
                text = "b3";
            }
            else if ((this.gamepadStyle <= 1 && inputButtonName == "X") || (this.gamepadStyle > 1 && inputButtonName == "Y") || inputButtonName == "Triangle")
            {
                text = "b4";
            }
            else if (inputButtonName == "D-Pad Left")
            {
                text = "dpad_left";
            }
            else if (inputButtonName == "D-Pad Right")
            {
                text = "dpad_right";
            }
            else if (inputButtonName == "D-Pad Up")
            {
                text = "dpad_up";
            }
            else if (inputButtonName == "D-Pad Down")
            {
                text = "dpad_down";
            }
            else if (inputButtonName == "L" || inputButtonName == "Left Shoulder" || inputButtonName == "L1")
            {
                text = "l1";
            }
            else if (inputButtonName == "ZL" || inputButtonName == "Left Trigger" || inputButtonName == "L2")
            {
                text = "l2";
            }
            else if (inputButtonName == "R" || inputButtonName == "Right Shoulder" || inputButtonName == "R1")
            {
                text = "r1";
            }
            else if (inputButtonName == "ZR" || inputButtonName == "Right Trigger" || inputButtonName == "R2")
            {
                text = "r2";
            }
            else if (inputButtonName == "Plus" || inputButtonName == "Start" || inputButtonName == "Options")
            {
                text = "start";
            }
            else if (inputButtonName == "Minus" || inputButtonName == "Back" || inputButtonName == "Share")
            {
                text = "select";
            }
            else if (inputButtonName == "SL" || inputButtonName == "Left SL" || inputButtonName == "Right SL")
            {
                text = "sl";
            }
            else if (inputButtonName == "SR" || inputButtonName == "Left SR" || inputButtonName == "Right SR")
            {
                text = "sr";
            }
            else if (inputButtonName == "Touchpad Button")
            {
                text = "touchpad";
            }
            else
            {
                text = "NONE";
            }
            if (text != "NONE")
            {
                if (this.gamepadStyle == 0)
                {
                    obj = UnityEngine.Object.Instantiate(Resources.Load("HUD/ButtonPrompts/NX/" + text));
                }
                else if (this.gamepadStyle == 1)
                {
                    obj = UnityEngine.Object.Instantiate(Resources.Load("HUD/ButtonPrompts/NX_Pro/" + text));
                }
                else if (this.gamepadStyle == 2)
                {
                    obj = UnityEngine.Object.Instantiate(Resources.Load("HUD/ButtonPrompts/PS/" + text));
                }
                else
                {
                    obj = UnityEngine.Object.Instantiate(Resources.Load("HUD/ButtonPrompts/XB/" + text));
                }
            }
        }
        if ((isKeyboard || RuntimeServices.EqualityOperator(obj, null)) && inputButtonName != null)
        {
            bool flag = inputButtonName.Contains("Mouse");
            bool flag2 = flag;
            if (flag)
            {
                if (inputButtonName == "Mouse")
                {
                    obj = UnityEngine.Object.Instantiate(Resources.Load("HUD/ButtonPrompts/KBM/Mouse"));
                }
                else if (inputButtonName == "Left Mouse Button")
                {
                    obj = UnityEngine.Object.Instantiate(Resources.Load("HUD/ButtonPrompts/KBM/MouseLeftClick"));
                }
                else if (inputButtonName == "Right Mouse Button")
                {
                    obj = UnityEngine.Object.Instantiate(Resources.Load("HUD/ButtonPrompts/KBM/MouseRightClick"));
                }
                else if (inputButtonName == "Mouse Button 3")
                {
                    obj = UnityEngine.Object.Instantiate(Resources.Load("HUD/ButtonPrompts/KBM/MouseScroll"));
                }
                else if (inputButtonName == "Mouse Wheel Up")
                {
                    obj = UnityEngine.Object.Instantiate(Resources.Load("HUD/ButtonPrompts/KBM/MouseScrollUp"));
                }
                else if (inputButtonName == "Mouse Wheel Down")
                {
                    obj = UnityEngine.Object.Instantiate(Resources.Load("HUD/ButtonPrompts/KBM/MouseScrollDown"));
                }
                else
                {
                    flag = false;
                }
            }
            if (!flag)
            {
                obj = UnityEngine.Object.Instantiate(Resources.Load("HUD/ButtonPrompts/KBM/Key"));
                GameObject gameObject = obj as GameObject;
                RectTransform rectTransform = (RectTransform)gameObject.GetComponent(typeof(RectTransform));
                Text text2 = (Text)rectTransform.Find("KeyText").GetComponent(typeof(Text));
                string text3;
                if (inputButtonName == "  ")
                {
                    text3 = this.GetTranslation("inNoMap");
                }
                else if (inputButtonName.Length > 2 && inputButtonName != "F10" && inputButtonName != "F11" && inputButtonName != "F12" && inputButtonName != "ESC" && !flag2)
                {
                    text3 = this.GetTranslation(inputButtonName);
                }
                else
                {
                    text3 = inputButtonName;
                }
                text2.text = text3;
                if (text3.Length > 3 || LocalizationManager.CurrentLanguageCode == "zh-CN" || LocalizationManager.CurrentLanguageCode == "zh-TW")
                {
                    float x = Mathf.Clamp(text2.preferredWidth + (float)((!(inputButtonName == "Space")) ? 20 : 40), 27.5f, (float)300);
                    Vector2 sizeDelta = rectTransform.sizeDelta;
                    float num = sizeDelta.x = x;
                    Vector2 vector = rectTransform.sizeDelta = sizeDelta;
                }
                if (text3.Length > 1)
                {
                    text2.fontSize = 10;
                }
            }
        }
        return obj as GameObject;
    }

    // Token: 0x06000253 RID: 595 RVA: 0x000031D6 File Offset: 0x000013D6
    public virtual string GetTranslation(string id)
    {
        return LocalizationManager.GetTranslation(id, true, 0, true, false, null, null);
    }

    // Token: 0x06000254 RID: 596 RVA: 0x00037BF4 File Offset: 0x00035DF4
    public virtual Controller GetControllerToUse(bool isKeyboard, bool getMouse)
    {
        Controller controller;
        if (!getMouse)
        {
            controller = this.player.controllers.GetLastActiveController((!isKeyboard) ? ControllerType.Joystick : ControllerType.Keyboard);
            if (RuntimeServices.EqualityOperator(controller, null))
            {
                controller = this.player.controllers.GetController((!isKeyboard) ? ControllerType.Joystick : ControllerType.Keyboard, 0);
            }
        }
        else
        {
            controller = this.player.controllers.GetLastActiveController(ControllerType.Mouse);
            if (RuntimeServices.EqualityOperator(controller, null))
            {
                controller = this.player.controllers.GetController(ControllerType.Mouse, 0);
            }
        }
        if (!isKeyboard && !RuntimeServices.EqualityOperator(controller, null))
        {
            if (controller.name.Contains("Nintendo"))
            {
                if (controller.name.Contains("Pro"))
                {
                    this.gamepadStyle = 1;
                }
                else
                {
                    this.gamepadStyle = 0;
                }
            }
            else if (controller.name.Contains("Sony"))
            {
                this.gamepadStyle = 2;
            }
            else
            {
                this.gamepadStyle = 3;
            }
        }
        return controller;
    }

    // Token: 0x06000255 RID: 597 RVA: 0x00037D08 File Offset: 0x00035F08
    public virtual ActionElementMap GetActionElementMapToUse(Controller controller, string theAction, bool positiveAxis)
    {
        if (!RuntimeServices.EqualityOperator(controller, null))
        {
            ControllerMap map = this.player.controllers.maps.GetMap(controller.type, controller.id, "Default", "Default");
            ActionElementMap[] elementMapsWithAction = map.GetElementMapsWithAction(theAction);
            int i = 0;
            ActionElementMap[] array = elementMapsWithAction;
            int length = array.Length;
            while (i < length)
            {
                if (array[i].axisContribution == ((!positiveAxis) ? Pole.Negative : Pole.Positive))
                {
                    return array[i];
                }
                i++;
            }
            map = this.player.controllers.maps.GetMap(controller.type, controller.id, "Motorcycle", "Default");
            if (!RuntimeServices.EqualityOperator(map, null))
            {
                elementMapsWithAction = map.GetElementMapsWithAction(theAction);
                int j = 0;
                ActionElementMap[] array2 = elementMapsWithAction;
                int length2 = array2.Length;
                while (j < length2)
                {
                    if (array2[j].axisContribution == ((!positiveAxis) ? Pole.Negative : Pole.Positive))
                    {
                        return array2[j];
                    }
                    j++;
                }
            }
            map = this.player.controllers.maps.GetMap(controller.type, controller.id, "UI Nav", "Default");
            if (!RuntimeServices.EqualityOperator(map, null))
            {
                elementMapsWithAction = map.GetElementMapsWithAction(theAction);
                int k = 0;
                ActionElementMap[] array3 = elementMapsWithAction;
                int length3 = array3.Length;
                while (k < length3)
                {
                    if (array3[k].axisContribution == ((!positiveAxis) ? Pole.Negative : Pole.Positive))
                    {
                        return array3[k];
                    }
                    k++;
                }
            }
        }
        return null;
    }

    // Token: 0x06000256 RID: 598 RVA: 0x00037EB4 File Offset: 0x000360B4
    public virtual string GetInputButtonName(string theAction, bool positiveAxis, bool isKeyboard)
    {
        Controller controllerToUse = this.GetControllerToUse(isKeyboard, false);
        string result;
        if (!RuntimeServices.EqualityOperator(controllerToUse, null))
        {
            ActionElementMap actionElementMapToUse = this.GetActionElementMapToUse(controllerToUse, theAction, positiveAxis);
            if (RuntimeServices.EqualityOperator(actionElementMapToUse, null))
            {
                if (isKeyboard)
                {
                    controllerToUse = this.GetControllerToUse(isKeyboard, true);
                    if (RuntimeServices.EqualityOperator(controllerToUse, null))
                    {
                        result = this.GetTranslation("inMissing");
                    }
                    else
                    {
                        actionElementMapToUse = this.GetActionElementMapToUse(controllerToUse, theAction, positiveAxis);
                        result = ((!RuntimeServices.EqualityOperator(actionElementMapToUse, null)) ? actionElementMapToUse.elementIdentifierName : "  ");
                    }
                }
                else
                {
                    result = "  ";
                }
            }
            else
            {
                result = actionElementMapToUse.elementIdentifierName;
            }
        }
        else
        {
            result = ((!isKeyboard) ? this.GetTranslation("inNoPad") : this.GetTranslation("inNoKeyb"));
        }
        return result;
    }

    // Token: 0x06000257 RID: 599 RVA: 0x00037F8C File Offset: 0x0003618C
    public virtual bool checkForMissingInput(bool isKeyboard)
    {
        bool result = default(bool);
        if (!isKeyboard)
        {
            if (this.GetInputButtonName("Aim Horizontal", false, isKeyboard) == "  ")
            {
                result = true;
            }
            else if (this.GetInputButtonName("Aim Horizontal", true, isKeyboard) == "  ")
            {
                result = true;
            }
            else if (this.GetInputButtonName("Aim Vertical", true, isKeyboard) == "  ")
            {
                result = true;
            }
            else if (this.GetInputButtonName("Aim Vertical", false, isKeyboard) == "  ")
            {
                result = true;
            }
        }
        if (this.GetInputButtonName("Move", false, isKeyboard) == "  ")
        {
            result = true;
        }
        else if (this.GetInputButtonName("Move", true, isKeyboard) == "  ")
        {
            result = true;
        }
        else if (this.GetInputButtonName("Jump", true, isKeyboard) == "  ")
        {
            result = true;
        }
        else if (this.GetInputButtonName("Crouch", false, isKeyboard) == "  ")
        {
            result = true;
        }
        else if (this.GetInputButtonName("Dodge", true, isKeyboard) == "  ")
        {
            result = true;
        }
        else if (this.GetInputButtonName("Interact", true, isKeyboard) == "  ")
        {
            result = true;
        }
        else if (this.GetInputButtonName("Focus", true, isKeyboard) == "  ")
        {
            result = true;
        }
        else if (this.GetInputButtonName("Kick", true, isKeyboard) == "  ")
        {
            result = true;
        }
        else if (this.GetInputButtonName("Fire", true, isKeyboard) == "  ")
        {
            result = true;
        }
        else if (this.GetInputButtonName("Fire2", true, isKeyboard) == "  ")
        {
            result = true;
        }
        else if (this.GetInputButtonName("Reload", true, isKeyboard) == "  ")
        {
            result = true;
        }
        else if (this.GetInputButtonName("Change Weapon", true, isKeyboard) == "  ")
        {
            result = true;
        }
        else if (this.GetInputButtonName("Scroll Weapon", true, isKeyboard) == "  ")
        {
            result = true;
        }
        else if (this.GetInputButtonName("Scroll Weapon", false, isKeyboard) == "  ")
        {
            result = true;
        }
        else if (this.GetInputButtonName("MotorcycleY", true, isKeyboard) == "  ")
        {
            result = true;
        }
        else if (this.GetInputButtonName("MotorcycleY", false, isKeyboard) == "  ")
        {
            result = true;
        }
        else if (this.GetInputButtonName("MotorcycleWheelie", true, isKeyboard) == "  ")
        {
            result = true;
        }
        return result;
    }

    // Token: 0x06000258 RID: 600 RVA: 0x000020A7 File Offset: 0x000002A7
    public virtual void Main()
    {
    }

    // Token: 0x0400071F RID: 1823
    private RootScript root;

    // Token: 0x04000720 RID: 1824
    private Player player;

    // Token: 0x04000721 RID: 1825
    private int gamepadStyle;
}
