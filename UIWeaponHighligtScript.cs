// Decompiled with JetBrains decompiler
// Type: UIWeaponHighlightScript
// Assembly: Assembly-UnityScript, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C6D02802-CAF3-4F5F-B720-0E01D2380B81
// Assembly location: F:\steamapps2\steamapps\common\My Friend Pedro\My Friend Pedro - Blood Bullets Bananas_Data\Managed\Assembly-UnityScript.dll

using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UIWeaponHighlightScript : MonoBehaviour
{
    private RootScript root;
    public GameObject weaponObj;
    private WeaponPickupScript weaponObjScript;
    private Image uiImg;
    private Transform mainPlayer;
    private RectTransform swirl;
    private RectTransform iconTransform;
    private Image icon;
    private Color iconStartColor;
    private float timer;
    private Camera mainCamera;
    private Vector2 startIconTransformSizeDelta;

    public virtual void Start()
    {
        if (!((UnityEngine.Object)this.root == (UnityEngine.Object)null))
            return;
        this.root = RootScript.RootScriptInstance;
        this.mainCamera = Camera.main;
        this.mainPlayer = !((UnityEngine.Object)PlayerScript.PlayerInstance == (UnityEngine.Object)null) ? PlayerScript.PlayerInstance.transform : GameObject.Find("Player").transform;
        this.swirl = (RectTransform)this.transform.Find("Swirl").GetComponent(typeof(RectTransform));
        this.iconTransform = (RectTransform)this.transform.Find("Icon").GetComponent(typeof(RectTransform));
        this.icon = (Image)this.iconTransform.GetComponent(typeof(Image));
        icon.preserveAspect = true;
        this.iconStartColor = this.icon.color;
        this.startIconTransformSizeDelta = this.iconTransform.sizeDelta;
        this.uiImg = (Image)this.swirl.GetComponent(typeof(Image));
    }

    public virtual void doSetup(bool doFade)
    {
        if ((UnityEngine.Object)this.root == (UnityEngine.Object)null)
            this.Start();
        if ((UnityEngine.Object)this.weaponObj == (UnityEngine.Object)null)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.weaponObjScript = (WeaponPickupScript)this.weaponObj.GetComponent(typeof(WeaponPickupScript));
            this.icon.color = this.iconStartColor;
            this.uiImg.color = this.iconStartColor;
            if (doFade)
            {
                int num1 = 0;
                Color color1 = this.uiImg.color;
                double num2 = (double)(color1.a = (float)num1);
                Color color2 = this.uiImg.color = color1;
                float num3 = -1.5f;
                Color color3 = this.icon.color;
                double num4 = (double)(color3.a = num3);
                Color color4 = this.icon.color = color3;
            }
            this.timer = 0.0f;
            this.icon.transform.localScale = Vector3.one;
            int num5 = 0;
            Quaternion localRotation = this.transform.localRotation;
            Vector3 eulerAngles = localRotation.eulerAngles;
            double num6 = (double)(eulerAngles.z = (float)num5);
            Vector3 vector3 = localRotation.eulerAngles = eulerAngles;
            Quaternion quaternion = this.transform.localRotation = localRotation;
            this.iconTransform.sizeDelta = this.startIconTransformSizeDelta;
            if (this.weaponObjScript.isMedkit)
            {
                this.icon.sprite = this.root.healthIcon;
                this.iconTransform.sizeDelta *= 0.5f;
            }
            else
                this.icon.sprite = this.root.weaponIcons[(int)this.weaponObjScript.weapon];
        }
    }

    public virtual void Update()
    {
        if ((UnityEngine.Object)this.weaponObj == (UnityEngine.Object)null || !this.weaponObj.activeInHierarchy || (!this.weaponObjScript.enabled || this.root.trailerMode))
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            float num1 = (float)(1.5 - (double)Mathf.Clamp(Vector3.Distance(this.weaponObj.transform.position, this.mainPlayer.position), 2f, 10f) / 10.0);
            if (!this.weaponObjScript.pickedUp)
            {
                this.timer += this.root.timescale * (num1 * 0.05f);
                bool ammoFull = this.weaponObjScript.ammoFull;
                float num2 = this.root.Damp(!ammoFull ? 1.1f : 0.6f, this.uiImg.color.a, 0.01f);
                Color color1 = this.uiImg.color;
                double num3 = (double)(color1.a = num2);
                Color color2 = this.uiImg.color = color1;
                float num4 = this.root.Damp((float)((!ammoFull ? 0.899999976158142 : 0.800000011920929) - (double)this.weaponObjScript.uiShake * 20.0), this.icon.color.a, 0.015f);
                Color color3 = this.icon.color;
                double num5 = (double)(color3.a = num4);
                Color color4 = this.icon.color = color3;
                if (ammoFull)
                {
                    this.icon.color = new Color(Color.gray.r, Color.gray.g, Color.gray.b, this.icon.color.a);
                    this.uiImg.color = Color.Lerp(new Color(Color.gray.r, Color.gray.g, Color.gray.b, this.uiImg.color.a), new Color(this.iconStartColor.r, this.iconStartColor.g, this.iconStartColor.b, this.uiImg.color.a), 0.75f);
                    num1 = Mathf.Clamp(num1, 0.5f, 0.7f);
                }
                else
                {
                    this.icon.color = new Color(this.iconStartColor.r, this.iconStartColor.g, this.iconStartColor.b, this.icon.color.a);
                    this.uiImg.color = new Color(this.iconStartColor.r, this.iconStartColor.g, this.iconStartColor.b, this.uiImg.color.a);
                }
                this.iconTransform.localScale = this.iconTransform.localScale + ((!ammoFull ? 1f : 0.8f) * (Vector3.one - Vector3.one * num1 * 0.3f + Vector3.one * Mathf.Abs(Mathf.Sin(this.timer)) * 0.2f) - this.icon.transform.localScale) * Mathf.Clamp01(0.1f * this.root.timescale);
                float num6 = Mathf.Sin(this.timer * 0.3f + 1f) * 5f;
                Quaternion localRotation1 = this.iconTransform.localRotation;
                Vector3 eulerAngles1 = localRotation1.eulerAngles;
                double num7 = (double)(eulerAngles1.z = num6);
                Vector3 vector3_1 = localRotation1.eulerAngles = eulerAngles1;
                Quaternion quaternion1 = this.iconTransform.localRotation = localRotation1;
                float num8 = Mathf.Cos(this.timer * 0.4f) * 3f;
                Vector2 anchoredPosition1 = this.iconTransform.anchoredPosition;
                double num9 = (double)(anchoredPosition1.y = num8);
                Vector2 vector2_1 = this.iconTransform.anchoredPosition = anchoredPosition1;
                float num10 = Mathf.Sin(Time.time * 25f) * this.weaponObjScript.uiShake * 15f;
                Vector2 anchoredPosition2 = this.iconTransform.anchoredPosition;
                double num11 = (double)(anchoredPosition2.x = num10);
                Vector2 vector2_2 = this.iconTransform.anchoredPosition = anchoredPosition2;
                float num12 = this.swirl.localRotation.eulerAngles.z + (float)((!ammoFull ? 1.0 : 0.25) * (6.0 * (double)num1)) * this.root.timescale;
                Quaternion localRotation2 = this.swirl.localRotation;
                Vector3 eulerAngles2 = localRotation2.eulerAngles;
                double num13 = (double)(eulerAngles2.z = num12);
                Vector3 vector3_2 = localRotation2.eulerAngles = eulerAngles2;
                Quaternion quaternion2 = this.swirl.localRotation = localRotation2;
                this.transform.position = this.mainCamera.WorldToScreenPoint(this.weaponObj.transform.position);
                this.transform.localScale = Vector3.one * (float)((double)num1 * 0.400000005960464 + 0.150000005960464);
            }
            else
            {
                float num2 = this.root.Damp(0.0f, this.uiImg.color.a, 0.3f);
                Color color1 = this.uiImg.color;
                double num3 = (double)(color1.a = num2);
                Color color2 = this.uiImg.color = color1;
                float num4 = this.root.Damp(0.0f, this.icon.color.a, 0.4f);
                Color color3 = this.icon.color;
                double num5 = (double)(color3.a = num4);
                Color color4 = this.icon.color = color3;
                this.icon.transform.localScale = this.icon.transform.localScale * Mathf.Pow(0.9f, this.root.timescale);
                float num6 = this.transform.localRotation.eulerAngles.z + 16f * this.root.timescale;
                Quaternion localRotation = this.transform.localRotation;
                Vector3 eulerAngles = localRotation.eulerAngles;
                double num7 = (double)(eulerAngles.z = num6);
                Vector3 vector3 = localRotation.eulerAngles = eulerAngles;
                Quaternion quaternion = this.transform.localRotation = localRotation;
            }
        }
    }

    public virtual void Main()
    {
    }
}
