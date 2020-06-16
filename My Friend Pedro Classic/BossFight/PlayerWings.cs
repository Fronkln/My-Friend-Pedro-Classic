using System;
using UnityEngine;
using Rewired;



namespace MFPClassic
{
    public class PlayerWings : MonoBehaviour
    {
        private RootScript root;
        private PlayerScript playerScript;
        private Player playerInput;

        // private AudioSource theSound;
        private float propellerAngle;
        // private Quaternion propellerStartRotation;
        private Transform upperLegR;
        private Transform lowerLegR;
        private Transform upperLegL;
        private Transform lowerLegL;
        private float propellerSpeed;

        public bool flying = false;
        private bool flyingOverride = false;

        public bool checkpointFix = false;

        public virtual void Start()
        {
            MFPEditorUtils.Log("PlayerWings Awake");

            root = (RootScript)GameObject.Find("Root").GetComponent(typeof(RootScript));
            playerScript = (PlayerScript)GameObject.Find("Player").GetComponent(typeof(PlayerScript));
            playerInput = ReInput.players.GetPlayer(0);


            gameObject.AddComponent<BoxCollider>();

            // theSound = (AudioSource)GetComponent(typeof(AudioSource));
            transform.parent = GameObject.Find("Player/PlayerGraphics/Armature/Center/LowerBack/UpperBack/Neck/Head").transform;
            transform.localPosition = new Vector3(-0.226f, 0.0f, 0.007f);
            //   GameObject.Find("Player/PlayerGraphics/Hair").SetActive(false);
            //     propeller = transform.Find("FlyHelmet_Propeller");
            //     umbrella = propeller.Find("FlyHelmet_Umbrella");
            //   propellerStartRotation = propeller.localRotation;
            playerScript.propellerHat = true;
            upperLegR = GameObject.Find("Player/PlayerGraphics/Armature/Center/Hip_R/UpperLeg_R").transform;
            lowerLegR = GameObject.Find("Player/PlayerGraphics/Armature/Center/Hip_R/UpperLeg_R/LowerLeg_R").transform;
            upperLegL = GameObject.Find("Player/PlayerGraphics/Armature/Center/Hip_L/UpperLeg_L").transform;
            lowerLegL = GameObject.Find("Player/PlayerGraphics/Armature/Center/Hip_L/UpperLeg_L/LowerLeg_L").transform;

            if (checkpointFix)
                playerScript.transform.position = FinalBattleController.playerPosHack;
        }

        public virtual void Update()
        {
            flying = playerInput.GetButton("Jump");
            playerScript.propellerHat = true;

            //  umbrellaOpenAmount = Mathf.Clamp01(umbrellaOpenAmount + (float)((!playerScript.kCrouch ? playerScript.ySpeed * -0.200000002980232 : 0.0) - 0.100000001490116) * root.timescale);
            //  umbrella.localScale = Vector3.Lerp(new Vector3(0.15f, 0.15f, 1.9f), Vector3.one, umbrellaOpenAmount);
            propellerSpeed = 0.0f;
            //       if (playerScript.dodging)
            //   playerScript.nrOfDodgeSpins = 0;
            if (!playerScript.onGround)
            {
                if (!playerScript.kCrouch)
                {
                    playerScript.targetRotation *= Mathf.Pow(playerScript.ySpeed >= 0.0 ? 0.8f : 0.7f, root.timescale);
                    playerScript.justWallJumped = false;
                }
                //propeller.rotation = root.DampSlerp(Quaternion.LookRotation(Vector3.up, transform.forward), propeller.rotation, 0.2f);
            }
            //    else
            //propeller.localRotation = root.DampSlerp(propellerStartRotation, propeller.localRotation, 0.1f);
            float propellerAngle = this.propellerAngle;
            // Quaternion localRotation = propeller.localRotation;
            // Vector3 eulerAngles = localRotation.eulerAngles;
            // double num = (eulerAngles.z = propellerAngle);
            // Vector3 vector3 = localRotation.eulerAngles = eulerAngles;
            // Quaternion quaternion = propeller.localRotation = localRotation;
            playerScript.dontLockTowall = true;
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<AutoControlZoneScript>())
                if (other.GetComponent<AutoControlZoneScript>().jump)
                flyingOverride = true;
        }

        public virtual void OnTriggerStay(Collider other)
        {

            if (other.GetComponent<AutoControlZoneScript>())
                if (other.GetComponent<AutoControlZoneScript>().setAim)
                    other.GetComponent<AutoControlZoneScript>().aimPos = new Vector3(15, playerScript.transform.position.y - 5, 0);
        }

        public virtual void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<AutoControlZoneScript>())
                if (other.GetComponent<AutoControlZoneScript>().jump)
                {
                    Destroy(GetComponent<BoxCollider>());
                    flyingOverride = false;
                }
        }

        

        public virtual void LateUpdate()
        {
            float num = (!playerScript.faceRight ? -1f : 1f) * -Mathf.Clamp(playerScript.ySpeed / 10f, -1f, 1f);
            upperLegR.localRotation *= Quaternion.Euler(0.0f, (float)(-playerScript.xSpeed * 3.5) * num, 0.0f);
            lowerLegR.localRotation *= Quaternion.Euler(0.0f, playerScript.xSpeed * 0.5f * num, 0.0f);
            upperLegL.localRotation *= Quaternion.Euler(0.0f, (float)(-playerScript.xSpeed * 2.0) * num, 0.0f);
            lowerLegL.localRotation *= Quaternion.Euler(0.0f, (float)(-playerScript.xSpeed * 3.5) * num, 0.0f);
        }

        public virtual void FixedUpdate()
        {
            propellerSpeed = 0.0f;
            if (flying || flyingOverride)
            {
                if (playerScript.ySpeed < 7.0)
                    playerScript.ySpeed += root.fixedTimescale * playerScript.speedModifier;
                propellerSpeed += -20f * root.fixedTimescale;
            }
            else
                propellerSpeed += -5f * root.fixedTimescale;
            if (!playerScript.onGround)
            {
                if (playerScript.kXDir != 0.0)
                    playerScript.xSpeed = root.DampFixed(playerScript.kXDir * 14f * playerScript.speedModifier, playerScript.xSpeed, 0.04f * Mathf.Clamp01(Mathf.Abs(playerScript.xSpeed) / 7f));
                propellerSpeed -= Mathf.Abs(playerScript.xSpeed * 0.5f) * root.fixedTimescale;
            }
            if (!playerScript.kCrouch && !playerScript.onGround && (playerScript.ySpeed < 0.0 && playerScript.ySpeed < -3.5))
                playerScript.ySpeed = 5;
            //    theSound.volume = root.DampFixed((float)((!playerScript.dodging ? (!playerScript.onGround ? 0.699999988079071 : 0.400000005960464) : 0.899999976158142) + propellerSpeed * 0.00999999977648258), theSound.volume, 0.1f);
            //  theSound.pitch = root.DampFixed((float)(Mathf.Abs(propellerSpeed) / 25.0 + 0.200000002980232), theSound.pitch, 0.3f);
            propellerAngle += propellerSpeed * root.fixedTimescale;
        }

        public virtual void Main()
        {
        }
    }
}
