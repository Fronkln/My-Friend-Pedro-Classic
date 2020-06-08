// Decompiled with JetBrains decompiler
// Type: WeaponPickupScript
// Assembly: Assembly-UnityScript, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C6D02802-CAF3-4F5F-B720-0E01D2380B81
// Assembly location: F:\steamapps2\steamapps\common\My Friend Pedro\My Friend Pedro - Blood Bullets Bananas_Data\Managed\Assembly-UnityScript.dll

using Boo.Lang.Runtime;
using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class WeaponPickupScript : MonoBehaviour
{
    public bool isMedkit;
    public float weapon;
    public bool pickedUp;
    private Transform mainPlayer;
    private Transform mainPlayerHand;
    private RootScript root;
    private RootSharedScript rootShared;
    private float targetYRot;
    private bool onFloor;
    private float pickUpTimer;
    private Vector3 pickUpPos;
    private Quaternion pickUpRot;
    private LayerMask layerMask;
    private GameObject weaponUIIndicator;
    private UIWeaponHighlightScript weaponUIIndicatorScript;
    [NonSerialized]
    private static GameObject[] weaponUIIndicatorList;
    [NonSerialized]
    private static int curWeaponUIIndicatorInstance;
    private Rigidbody rBody;
    private PlayerScript playerScript;
    private bool playerInRangeDoOnce;
    [HideInInspector]
    public bool motorcycle;
    private float motorcycleYSpeed;
    [HideInInspector]
    public float uiShake;
    [HideInInspector]
    public bool ammoFull;
    private Vector3 startDropPos;
    private float ranOffsetNr;
    private bool skyfallDoOnce;
    private bool pickedUpS;
    private bool onFloorS;
    private float pickUpTimerS;
    private bool playerInRangeDoOnceS;
    private float uiShakeS;
    private Vector3 startDropPosS;
    private bool skyfallDoOnceS;
    private bool haveSaved;

    public WeaponPickupScript()
    {
        this.weapon = 1f;
    }

    public virtual void saveState()
    {
        this.pickedUpS = this.pickedUp;
        this.onFloorS = this.onFloor;
        this.pickUpTimerS = this.pickUpTimer;
        this.playerInRangeDoOnceS = this.playerInRangeDoOnce;
        this.uiShakeS = this.uiShake;
        this.startDropPosS = this.startDropPos;
        this.skyfallDoOnceS = this.skyfallDoOnce;
        this.haveSaved = true;
    }

    public virtual void loadState()
    {
        this.pickedUp = this.pickedUpS;
        this.onFloor = this.onFloorS;
        this.pickUpTimer = this.pickUpTimerS;
        this.playerInRangeDoOnce = this.playerInRangeDoOnceS;
        this.uiShake = this.uiShakeS;
        this.startDropPos = this.startDropPosS;
        this.skyfallDoOnce = this.skyfallDoOnceS;
    }

    public virtual void LateUpdate()
    {
        if (this.root.doCheckpointSave)
            this.saveState();
        if (!this.root.doCheckpointLoad)
            return;
        if (!this.haveSaved)
            UnityEngine.Object.Destroy((UnityEngine.Object)this.gameObject);
        else
            this.loadState();
    }

    public virtual void Start()
    {
        this.mainPlayer = PlayerScript.PlayerInstance.transform;
        this.mainPlayerHand = this.mainPlayer.Find("PlayerGraphics/Armature/Center/LowerBack/UpperBack/Shoulder_R/UpperArm_R/LowerArm_R/Hand_R");
        this.root = RootScript.RootScriptInstance;
        this.rootShared = RootSharedScript.Instance;
        this.targetYRot = (double)this.transform.localRotation.eulerAngles.y >= 90.0 ? 90f : 270f;
        this.layerMask = (LayerMask)98560;
        this.rBody = (Rigidbody)this.gameObject.GetComponent(typeof(Rigidbody));
        this.playerScript = (PlayerScript)this.mainPlayer.GetComponent(typeof(PlayerScript));
        if (this.motorcycle)
        {
            RigidBodySlowMotion component = (RigidBodySlowMotion)this.GetComponent(typeof(RigidBodySlowMotion));
            if ((UnityEngine.Object)component != (UnityEngine.Object)null)
                component.enabled = false;
            this.motorcycleYSpeed = 0.3f;
        }
        this.ranOffsetNr = UnityEngine.Random.value * 5f;
    }

    public virtual void Awake()
    {
        this.CreateUIIndicatorPool();
    }

    public virtual void OnDisable()
    {
        if (!((UnityEngine.Object)this.weaponUIIndicator != (UnityEngine.Object)null))
            return;
        this.weaponUIIndicator.SetActive(false);
    }

    public virtual void OnEnable()
    {
        if (!this.haveSaved)
        {
            this.pickedUp = false;
            this.onFloor = false;
            this.pickUpTimer = 0.0f;
        }
        this.startDropPos = this.transform.position;
        this.FindWeaponUIIndicator();
        this.weaponUIIndicatorScript.weaponObj = this.gameObject;
        this.weaponUIIndicatorScript.doSetup(true);
        this.weaponUIIndicator.SetActive(true);
    }

    public virtual void OnBecameVisible()
    {
        if (!this.onFloor || !((UnityEngine.Object)this.weaponUIIndicatorScript == (UnityEngine.Object)null) && !((UnityEngine.Object)this.weaponUIIndicatorScript.weaponObj != (UnityEngine.Object)this.gameObject))
            return;
        this.FindWeaponUIIndicator();
        this.weaponUIIndicatorScript.weaponObj = this.gameObject;
        this.weaponUIIndicatorScript.doSetup(false);
        this.weaponUIIndicator.SetActive(true);
    }

    public virtual void doSetup()
    {
        if (!((UnityEngine.Object)this.weaponUIIndicatorScript != (UnityEngine.Object)null))
            return;
        this.weaponUIIndicatorScript.doSetup(true);
    }

    public virtual void Update()
    {
        if (!this.pickedUp && (double)this.root.setGravity.y > 0.0)
        {
            this.onFloor = true;
            if (this.playerScript.skyfall)
            {
                if (!this.skyfallDoOnce)
                {
                    this.startDropPos.y -= (float)UnityEngine.Random.Range(2, 5);
                    this.skyfallDoOnce = true;
                }
                if ((double)this.startDropPos.y > 4.0)
                    this.startDropPos.y += 0.1f * this.root.timescale;
                else
                    this.startDropPos.y += 0.025f * this.root.timescale;
                if ((double)this.startDropPos.x < -7.0)
                    this.startDropPos.x = this.root.Damp(-7f, this.startDropPos.x, 0.05f);
                else if ((double)this.startDropPos.x > 7.0)
                    this.startDropPos.x = this.root.Damp(7f, this.startDropPos.x, 0.05f);
            }
        }
        if (!this.isMedkit)
        {
            int num1 = (double)this.playerScript.ammoTotal[(int)this.weapon] >= (double)this.root.ammoMax[(int)this.weapon] ? 1 : 0;
            if (num1 != 0)
            {
                int num2 = (double)this.weapon == 5.0 ? 1 : 0;
                if (num2 != 0)
                    num2 = (double)this.playerScript.secondaryAmmo[5] < 5.0 ? 1 : 0;
                num1 = num2 == 0 ? 1 : 0;
            }
            this.ammoFull = num1 != 0;
        }
        else
            this.ammoFull = (double)this.playerScript.health >= 1.0;
        if (this.rootShared.modInfiniteAmmo)
        {
            this.weaponUIIndicator.SetActive(false);
            ((Collider)this.gameObject.GetComponent(typeof(Collider))).enabled = true;
            ((Rigidbody)this.gameObject.GetComponent(typeof(Rigidbody))).isKinematic = false;
            ((Behaviour)this.GetComponent(typeof(WeaponPickupScript))).enabled = false;
        }
        else
        {
            float num1 = Vector2.Distance((Vector2)this.transform.position, (Vector2)(!this.onFloor ? this.mainPlayerHand.position : this.mainPlayer.position));
            if (this.pickedUp)
            {
                this.pickUpTimer += 0.1f * this.root.timescale;
                this.transform.position = Vector3.Lerp(this.pickUpPos, this.mainPlayerHand.position, this.pickUpTimer);
                this.transform.rotation = Quaternion.Slerp(this.pickUpRot, this.mainPlayerHand.rotation * Quaternion.Euler(0.0f, 90f, 0.0f), this.pickUpTimer);
                if ((double)Vector2.Distance((Vector2)this.transform.position, (Vector2)this.mainPlayerHand.position) >= 0.300000011920929)
                    return;
                if (this.isMedkit)
                {
                    float health = this.playerScript.health;
                    this.playerScript.health = this.root.difficultyMode != 2 ? Mathf.Clamp01(this.playerScript.health + 0.3f) : 1f;
                    this.playerScript.healthPackEffect = 1f;
                    this.root.doPickUpNotification(999, Mathf.Round((this.playerScript.health - health) * 100f), true);
                    this.playerScript.playerAudioSource.clip = this.playerScript.healthPickUpSound;
                    this.playerScript.playerAudioSource.volume = UnityEngine.Random.Range(0.85f, 1f);
                    this.playerScript.playerAudioSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
                    this.playerScript.playerAudioSource.Play();
                }
                else
                    this.playerScript.pickedUpWeapon(this.weapon);
                this.root.rumble(0, 0.2f, 0.2f);
                this.root.rumble(1, 0.2f, 0.2f);
                this.gameObject.SetActive(false);
            }
            else
            {
                if (this.motorcycle)
                {
                    int num2 = this.ammoFull || (double)this.transform.position.x < -23.0 ? -16 : -4;
                    Vector3 velocity1 = this.rBody.velocity;
                    double num3 = (double)(velocity1.x = (float)num2);
                    Vector3 vector3_1 = this.rBody.velocity = velocity1;
                    int num4 = 0;
                    Vector3 velocity2 = this.rBody.velocity;
                    double num5 = (double)(velocity2.z = (float)num4);
                    Vector3 vector3_2 = this.rBody.velocity = velocity2;
                    int num6 = 0;
                    Vector3 velocity3 = this.rBody.velocity;
                    double num7 = (double)(velocity3.y = (float)num6);
                    Vector3 vector3_3 = this.rBody.velocity = velocity3;
                    this.motorcycleYSpeed -= 0.01f * this.root.timescale;
                    float num8 = this.transform.position.y + this.motorcycleYSpeed * this.root.timescale;
                    Vector3 position1 = this.transform.position;
                    double num9 = (double)(position1.y = num8);
                    Vector3 vector3_4 = this.transform.position = position1;
                    if ((double)this.transform.position.y < 0.100000001490116)
                    {
                        float num10 = 0.1f;
                        Vector3 position2 = this.transform.position;
                        double num11 = (double)(position2.y = num10);
                        Vector3 vector3_5 = this.transform.position = position2;
                        this.motorcycleYSpeed *= -0.7f;
                    }
                    float num12 = this.root.Damp(this.mainPlayer.position.z, this.transform.position.z, 0.005f);
                    Vector3 position3 = this.transform.position;
                    double num13 = (double)(position3.z = num12);
                    Vector3 vector3_6 = this.transform.position = position3;
                }
                if (!this.onFloor && (double)this.rBody.velocity.magnitude < 0.100000001490116)
                    this.onFloor = true;
                this.uiShake = Mathf.Clamp01(this.uiShake - 0.005f * this.root.timescale);
                if ((double)num1 < (!this.onFloor ? 1.5 : 3.0))
                {
                    if (!Physics.Linecast(this.transform.position, this.mainPlayerHand.position, (int)this.layerMask))
                    {
                        if (this.isMedkit && this.ammoFull)
                        {
                            if (!this.playerInRangeDoOnce)
                            {
                                if ((double)this.playerScript.health >= 1.0)
                                {
                                    this.root.showHintHealthFull = 120f;
                                    this.uiShake = 0.3f;
                                }
                                if (this.onFloor)
                                {
                                    this.rBody.isKinematic = false;
                                    float num2 = this.rBody.velocity.y + 4f;
                                    Vector3 velocity = this.rBody.velocity;
                                    double num3 = (double)(velocity.y = num2);
                                    Vector3 vector3_1 = this.rBody.velocity = velocity;
                                    float num4 = this.rBody.angularVelocity.z + (float)UnityEngine.Random.Range(-5, 5);
                                    Vector3 angularVelocity = this.rBody.angularVelocity;
                                    double num5 = (double)(angularVelocity.z = num4);
                                    Vector3 vector3_2 = this.rBody.angularVelocity = angularVelocity;
                                }
                                if (!this.playerScript.playerAudioSource.isPlaying)
                                {
                                    this.playerScript.playerAudioSource.clip = this.playerScript.itemPickUpFullSound;
                                    this.playerScript.playerAudioSource.volume = UnityEngine.Random.Range(0.85f, 1f);
                                    this.playerScript.playerAudioSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
                                    this.playerScript.playerAudioSource.Play();
                                }
                            }
                        }
                        else if (!this.isMedkit && this.ammoFull)
                        {
                            if (!this.playerInRangeDoOnce)
                            {
                                this.root.showHintAmmoFull = 120f;
                                this.uiShake = 0.3f;
                                if (this.onFloor)
                                {
                                    this.rBody.isKinematic = false;
                                    float num2 = this.rBody.velocity.y + 4f;
                                    Vector3 velocity = this.rBody.velocity;
                                    double num3 = (double)(velocity.y = num2);
                                    Vector3 vector3_1 = this.rBody.velocity = velocity;
                                    float num4 = this.rBody.angularVelocity.z + (float)UnityEngine.Random.Range(-5, 5);
                                    Vector3 angularVelocity = this.rBody.angularVelocity;
                                    double num5 = (double)(angularVelocity.z = num4);
                                    Vector3 vector3_2 = this.rBody.angularVelocity = angularVelocity;
                                }
                                if (!this.playerScript.playerAudioSource.isPlaying)
                                {
                                    this.playerScript.playerAudioSource.clip = this.playerScript.itemPickUpFullSound;
                                    this.playerScript.playerAudioSource.volume = UnityEngine.Random.Range(0.85f, 1f);
                                    this.playerScript.playerAudioSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
                                    this.playerScript.playerAudioSource.Play();
                                }
                            }
                        }
                        else
                        {
                            this.pickedUp = true;
                            this.uiShake = 0.0f;
                            ((Collider)this.gameObject.GetComponent(typeof(Collider))).enabled = false;
                            ((Rigidbody)this.gameObject.GetComponent(typeof(Rigidbody))).isKinematic = true;
                            this.pickUpPos = this.transform.position;
                            this.pickUpRot = this.transform.rotation;
                        }
                    }
                    if (this.playerInRangeDoOnce)
                        return;
                    this.playerInRangeDoOnce = true;
                }
                else
                {
                    if (!this.playerInRangeDoOnce)
                        return;
                    this.playerInRangeDoOnce = false;
                }
            }
        }
    }

    public virtual void FixedUpdate()
    {
        if (this.pickedUp || (double)this.root.setGravity.y <= 0.0)
            return;
        int num1 = 0;
        Vector3 velocity1 = this.rBody.velocity;
        double num2 = (double)(velocity1.z = (float)num1);
        Vector3 vector3_1 = this.rBody.velocity = velocity1;
        float num3 = this.rBody.velocity.x + this.root.DampAddFixed(this.startDropPos.x, this.transform.position.x, 0.6f);
        Vector3 velocity2 = this.rBody.velocity;
        double num4 = (double)(velocity2.x = num3);
        Vector3 vector3_2 = this.rBody.velocity = velocity2;
        float num5 = this.rBody.velocity.y + this.root.DampAddFixed(this.startDropPos.y, this.transform.position.y, 0.6f);
        Vector3 velocity3 = this.rBody.velocity;
        double num6 = (double)(velocity3.y = num5);
        Vector3 vector3_3 = this.rBody.velocity = velocity3;
        this.rBody.velocity *= Mathf.Pow(0.9f, this.root.fixedTimescale);
        this.rBody.velocity += (Vector3)(new Vector2(Mathf.Cos(Time.time + this.ranOffsetNr) + Mathf.Sin(Time.time * 1.3f + this.ranOffsetNr), Mathf.Sin(Time.time + this.ranOffsetNr) + Mathf.Cos(Time.time * 1.43f + this.ranOffsetNr)) * 0.4f * this.root.fixedTimescale);
        this.rBody.angularVelocity += this.rBody.velocity * 0.05f * Mathf.Sin(Time.time * 0.3f + this.ranOffsetNr) * this.root.fixedTimescale;
    }

    public virtual void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer != 8)
            return;
        this.onFloor = true;
    }

    public virtual void FindWeaponUIIndicator()
    {
        WeaponPickupScript.curWeaponUIIndicatorInstance = (int)Mathf.Repeat((float)(WeaponPickupScript.curWeaponUIIndicatorInstance + 1), (float)Extensions.get_length((System.Array)WeaponPickupScript.weaponUIIndicatorList));
        this.weaponUIIndicator = WeaponPickupScript.weaponUIIndicatorList[WeaponPickupScript.curWeaponUIIndicatorInstance];
        this.weaponUIIndicatorScript = (UIWeaponHighlightScript)this.weaponUIIndicator.GetComponent(typeof(UIWeaponHighlightScript));
    }

    public virtual void CreateUIIndicatorPool()
    {
        int length = 30;
        if (!RuntimeServices.op_Equality((System.Array)WeaponPickupScript.weaponUIIndicatorList, (System.Array)null) && Extensions.get_length((System.Array)WeaponPickupScript.weaponUIIndicatorList) >= length && !((UnityEngine.Object)WeaponPickupScript.weaponUIIndicatorList[0] == (UnityEngine.Object)null))
            return;
        WeaponPickupScript.weaponUIIndicatorList = new GameObject[length];
        for (int index = 0; index < Extensions.get_length((System.Array)WeaponPickupScript.weaponUIIndicatorList); ++index)
        {
            WeaponPickupScript.weaponUIIndicatorList[index] = UnityEngine.Object.Instantiate<GameObject>((GameObject)Resources.Load("HUD/WeaponHighlight", typeof(GameObject)));
            WeaponPickupScript.weaponUIIndicatorList[index].transform.SetParent(GameObject.Find("HUD/Canvas/WeaponHighlightHolder").transform);
            WeaponPickupScript.weaponUIIndicatorList[index].transform.SetAsFirstSibling();
            WeaponPickupScript.weaponUIIndicatorList[index].SetActive(false);
        }
    }

    public virtual void Main()
    {
    }
}
