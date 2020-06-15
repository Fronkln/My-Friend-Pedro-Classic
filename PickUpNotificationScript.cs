// Decompiled with JetBrains decompiler
// Type: PickUpNotificationScript
// Assembly: Assembly-UnityScript, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C6D02802-CAF3-4F5F-B720-0E01D2380B81
// Assembly location: F:\steamapps2\steamapps\common\My Friend Pedro\My Friend Pedro - Blood Bullets Bananas_Data\Managed\Assembly-UnityScript.dll

using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PickUpNotificationScript : MonoBehaviour
{
    private RootScript root;
    private float timer;
    public float yPosOffset;
    private RectTransform theRectTransform;
    private Vector2 startPos;
    private Image uiImage;
    private Image bulletImage;
    private Image plusImage;
    private Vector2 startDeltaSize;

    public virtual void LateUpdate()
    {
        if (!this.root.doCheckpointLoad)
            return;
        this.gameObject.SetActive(false);
    }

    public virtual void Start()
    {
        this.root = (RootScript)GameObject.Find("Root").GetComponent(typeof(RootScript));
        this.theRectTransform = (RectTransform)this.gameObject.GetComponent(typeof(RectTransform));
        this.uiImage = (Image)this.GetComponent(typeof(Image));
        this.bulletImage = (Image)this.transform.Find("Ammo").GetComponent(typeof(Image));
        this.plusImage = (Image)this.transform.Find("Plus").GetComponent(typeof(Image));
        this.startDeltaSize = this.theRectTransform.sizeDelta;
        uiImage.preserveAspect = true;
        this.gameObject.SetActive(false);


    }

    public virtual void doSetup(bool isWeapon, Sprite iconToUse)
    {
        this.startPos = this.theRectTransform.anchoredPosition;
        int num1 = 0;
        Color color1 = this.plusImage.color;
        double num2 = (double)(color1.a = (float)num1);
        Color color2 = this.plusImage.color = color1;
        int num3 = num1;
        Color color3 = this.bulletImage.color;
        double num4 = (double)(color3.a = (float)num3);
        Color color4 = this.bulletImage.color = color3;
        int num5 = num3;
        Color color5 = this.uiImage.color;
        double num6 = (double)(color5.a = (float)num5);
        Color color6 = this.uiImage.color = color5;
        float num7 = this.theRectTransform.anchoredPosition.x - 50f;
        Vector2 anchoredPosition = this.theRectTransform.anchoredPosition;
        double num8 = (double)(anchoredPosition.x = num7);
        Vector2 vector2 = this.theRectTransform.anchoredPosition = anchoredPosition;
        this.timer = 0.0f;
        this.uiImage.sprite = iconToUse;
        this.theRectTransform.sizeDelta = this.startDeltaSize;
        if (!isWeapon)
            this.theRectTransform.sizeDelta *= 0.6f;
        this.bulletImage.enabled = isWeapon;
        this.yPosOffset = 0.0f;
    }

    public virtual void Update()
    {
        this.timer += this.root.timescale;
        float num1 = this.root.Damp(this.startPos.y + this.yPosOffset, this.theRectTransform.anchoredPosition.y, 0.3f);
        Vector2 anchoredPosition1 = this.theRectTransform.anchoredPosition;
        double num2 = (double)(anchoredPosition1.y = num1);
        Vector2 vector2_1 = this.theRectTransform.anchoredPosition = anchoredPosition1;
        float num3 = this.root.Damp(this.startPos.x, this.theRectTransform.anchoredPosition.x, 0.3f);
        Vector2 anchoredPosition2 = this.theRectTransform.anchoredPosition;
        double num4 = (double)(anchoredPosition2.x = num3);
        Vector2 vector2_2 = this.theRectTransform.anchoredPosition = anchoredPosition2;
        if ((double)this.timer > 180.0)
        {
            float num5 = this.plusImage.color.a - 0.02f * this.root.timescale;
            Color color1 = this.plusImage.color;
            double num6 = (double)(color1.a = num5);
            Color color2 = this.plusImage.color = color1;
            float num7 = num5;
            Color color3 = this.bulletImage.color;
            double num8 = (double)(color3.a = num7);
            Color color4 = this.bulletImage.color = color3;
            float num9 = num7;
            Color color5 = this.uiImage.color;
            double num10 = (double)(color5.a = num9);
            Color color6 = this.uiImage.color = color5;
            if ((double)this.uiImage.color.a <= 0.0)
                this.gameObject.SetActive(false);
        }
        else
        {
            float num5 = this.root.Damp(0.8f, this.uiImage.color.a, 0.3f);
            Color color1 = this.plusImage.color;
            double num6 = (double)(color1.a = num5);
            Color color2 = this.plusImage.color = color1;
            float num7 = num5;
            Color color3 = this.bulletImage.color;
            double num8 = (double)(color3.a = num7);
            Color color4 = this.bulletImage.color = color3;
            float num9 = num7;
            Color color5 = this.uiImage.color;
            double num10 = (double)(color5.a = num9);
            Color color6 = this.uiImage.color = color5;
        }
        if (!this.root.dead)
            return;
        this.gameObject.SetActive(false);
    }

    public virtual void Main()
    {
    }
}
