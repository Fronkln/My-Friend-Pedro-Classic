// Decompiled with JetBrains decompiler
// Type: UINewWeaponScript
// Assembly: Assembly-UnityScript, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C6D02802-CAF3-4F5F-B720-0E01D2380B81
// Assembly location: F:\steamapps2\steamapps\common\My Friend Pedro\My Friend Pedro - Blood Bullets Bananas_Data\Managed\Assembly-UnityScript.dll

using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UINewWeaponScript : MonoBehaviour
{
    private RootScript root;
    private RectTransform bg;
    private Image bgImg;
    private RectTransform lCurve;
    private Image lCurveImg;
    private RectTransform rCurve;
    private Image rCurveImg;
    private RectTransform lLine;
    private RectTransform rLine;
    private Image bgDetailImg;
    private Image weaponIconImg;
    private float lineGrowAmount;
    public float animTimer;
    public int weaponNr;

    public virtual void Start()
    {
        this.root = (RootScript)GameObject.Find("Root").GetComponent(typeof(RootScript));
        this.bg = (RectTransform)this.transform.Find("Background").GetComponent(typeof(RectTransform));
        this.bgImg = (Image)this.bg.GetComponent(typeof(Image));
        this.lCurve = (RectTransform)this.transform.Find("Background/LeftCurveLine").GetComponent(typeof(RectTransform));
        this.lCurveImg = (Image)this.lCurve.GetComponent(typeof(Image));
        this.rCurve = (RectTransform)this.transform.Find("Background/RightCurveLine").GetComponent(typeof(RectTransform));
        this.rCurveImg = (Image)this.rCurve.GetComponent(typeof(Image));
        this.lLine = (RectTransform)this.transform.Find("Background/LeftLine").GetComponent(typeof(RectTransform));
        this.rLine = (RectTransform)this.transform.Find("Background/RightLine").GetComponent(typeof(RectTransform));
        this.bgDetailImg = (Image)this.transform.Find("BackgroundDetail").GetComponent(typeof(Image));
        this.weaponIconImg = (Image)this.transform.Find("Background/Mask/WeaponIcon").GetComponent(typeof(Image));
        weaponIconImg.preserveAspect = true;
        this.Update();
        this.OnEnable();
    }

    public virtual void OnEnable()
    {
        this.animTimer = 0.0f;
        this.lineGrowAmount = 0.0f;
    }

    public virtual void Update()
    {
        if ((double)this.animTimer == 0.0)
            this.weaponIconImg.sprite = this.root.weaponIcons[this.weaponNr];
        this.animTimer += this.root.timescale;
        if ((double)this.animTimer < 360.0)
        {
            this.lineGrowAmount = this.root.Damp(1f, this.lineGrowAmount, 0.1f);
        }
        else
        {
            this.lineGrowAmount = this.root.Damp(-0.3f, this.lineGrowAmount, 0.1f);
            if ((double)this.lineGrowAmount <= 0.0)
                this.gameObject.SetActive(false);
        }
        float num1 = Mathf.Clamp(this.lineGrowAmount - 0.7f, 0.0f, 1f) * 3.333333f;
        this.lCurveImg.fillAmount = 0.4f * num1;
        this.rCurveImg.fillAmount = 0.4f * num1;
        float num2 = 110f * this.lineGrowAmount;
        Vector2 sizeDelta1 = this.bg.sizeDelta;
        double num3 = (double)(sizeDelta1.x = num2);
        Vector2 vector2_1 = this.bg.sizeDelta = sizeDelta1;
        float num4 = Mathf.Clamp01(this.lineGrowAmount - 0.1f) / 0.9f;
        Color color1 = this.bgImg.color;
        double num5 = (double)(color1.a = num4);
        Color color2 = this.bgImg.color = color1;
        float num6 = Mathf.Clamp01(this.lineGrowAmount - 0.3f) / 0.7f;
        Color color3 = this.bgDetailImg.color;
        double num7 = (double)(color3.a = num6);
        Color color4 = this.bgDetailImg.color = color3;
        float num8 = num6;
        Color color5 = this.weaponIconImg.color;
        double num9 = (double)(color5.a = num8);
        Color color6 = this.weaponIconImg.color = color5;
        if ((double)this.lineGrowAmount < 0.699999988079071)
        {
            float num10 = (float)(45.0 - (double)Mathf.Abs((float)(0.5 - (double)this.lineGrowAmount / 0.699999988079071)) * 90.0 + (double)this.lineGrowAmount / 0.699999988079071 * 6.0);
            Vector2 sizeDelta2 = this.rLine.sizeDelta;
            double num11 = (double)(sizeDelta2.y = num10);
            Vector2 vector2_2 = this.rLine.sizeDelta = sizeDelta2;
            float num12 = num10;
            Vector2 sizeDelta3 = this.lLine.sizeDelta;
            double num13 = (double)(sizeDelta3.y = num12);
            Vector2 vector2_3 = this.lLine.sizeDelta = sizeDelta3;
            int num14 = 0;
            Vector3 localPosition1 = this.rLine.localPosition;
            double num15 = (double)(localPosition1.x = (float)num14);
            Vector3 vector3_1 = this.rLine.localPosition = localPosition1;
            int num16 = num14;
            Vector3 localPosition2 = this.lLine.localPosition;
            double num17 = (double)(localPosition2.x = (float)num16);
            Vector3 vector3_2 = this.lLine.localPosition = localPosition2;
            int num18 = 0;
            Quaternion rotation1 = this.rLine.rotation;
            Vector3 eulerAngles1 = rotation1.eulerAngles;
            double num19 = (double)(eulerAngles1.z = (float)num18);
            Vector3 vector3_3 = rotation1.eulerAngles = eulerAngles1;
            Quaternion quaternion1 = this.rLine.rotation = rotation1;
            int num20 = num18;
            Quaternion rotation2 = this.lLine.rotation;
            Vector3 eulerAngles2 = rotation2.eulerAngles;
            double num21 = (double)(eulerAngles2.z = (float)num20);
            Vector3 vector3_4 = rotation2.eulerAngles = eulerAngles2;
            Quaternion quaternion2 = this.lLine.rotation = rotation2;
            if ((double)this.lineGrowAmount > 0.349999994039536)
            {
                float num22 = (float)(((double)this.lineGrowAmount - 0.349999994039536) / 0.349999994039536 * -35.5);
                Vector3 localPosition3 = this.lLine.localPosition;
                double num23 = (double)(localPosition3.y = num22);
                Vector3 vector3_5 = this.lLine.localPosition = localPosition3;
                float num24 = (float)(((double)this.lineGrowAmount - 0.349999994039536) / 0.349999994039536 * 35.5);
                Vector3 localPosition4 = this.rLine.localPosition;
                double num25 = (double)(localPosition4.y = num24);
                Vector3 vector3_6 = this.rLine.localPosition = localPosition4;
            }
            else
            {
                int num22 = 0;
                Vector3 localPosition3 = this.rLine.localPosition;
                double num23 = (double)(localPosition3.y = (float)num22);
                Vector3 vector3_5 = this.rLine.localPosition = localPosition3;
                int num24 = num22;
                Vector3 localPosition4 = this.lLine.localPosition;
                double num25 = (double)(localPosition4.y = (float)num24);
                Vector3 vector3_6 = this.lLine.localPosition = localPosition4;
            }
        }
        else
        {
            int num10 = 6;
            Vector2 sizeDelta2 = this.rLine.sizeDelta;
            double num11 = (double)(sizeDelta2.y = (float)num10);
            Vector2 vector2_2 = this.rLine.sizeDelta = sizeDelta2;
            int num12 = num10;
            Vector2 sizeDelta3 = this.lLine.sizeDelta;
            double num13 = (double)(sizeDelta3.y = (float)num12);
            Vector2 vector2_3 = this.lLine.sizeDelta = sizeDelta3;
            float num14 = (float)(((double)this.lCurveImg.fillAmount * 360.0 + 90.0) * (Math.PI / 180.0));
            float num15 = (float)-((double)num14 * 57.2957801818848) + 90f;
            Quaternion rotation1 = this.lLine.rotation;
            Vector3 eulerAngles1 = rotation1.eulerAngles;
            double num16 = (double)(eulerAngles1.z = num15);
            Vector3 vector3_1 = rotation1.eulerAngles = eulerAngles1;
            Quaternion quaternion1 = this.lLine.rotation = rotation1;
            float num17 = (float)-((double)num14 * 57.2957801818848) + 90f;
            Quaternion rotation2 = this.rLine.rotation;
            Vector3 eulerAngles2 = rotation2.eulerAngles;
            double num18 = (double)(eulerAngles2.z = num17);
            Vector3 vector3_2 = rotation2.eulerAngles = eulerAngles2;
            Quaternion quaternion2 = this.rLine.rotation = rotation2;
            this.lLine.anchoredPosition = this.lCurve.anchoredPosition + new Vector2(Mathf.Cos(-num14), Mathf.Sin(-num14)) * 35.5f;
            this.rLine.anchoredPosition = this.rCurve.anchoredPosition + new Vector2(Mathf.Cos(-num14), Mathf.Sin(-num14)) * -35.5f;
        }
        if (!this.root.doCheckpointLoad && !this.root.dead)
            return;
        this.animTimer = 0.0f;
        this.lineGrowAmount = 0.0f;
        this.gameObject.SetActive(false);
    }

    public virtual void Main()
    {
    }
}
