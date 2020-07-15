using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MFPClassic
{
    public class FinalBattleController : MonoBehaviour
    {
        public static FinalBattleController inst;

        public SwitchScript startSwitch;
        public Victor victorBoss;

        public Canvas bossfightUI;
        private Text bossText;
        public RectTransform creditsRoot, youRect, winRect;

        public static Vector3 playerPosHack;

        private bool startBattleDoOnce = false;
        public bool battleStarted = false;

        private int explosions = 18;


        public bool playerAlreadyHadEnabledSideCam = false;

        private Transform bgCamera;

        IEnumerator WaitPlayerWings()
        {
            yield return new WaitForSeconds(2);
            MFPClassicAssets.player.gameObject.AddComponent<PlayerWings>();
        }

        IEnumerator DeathExplosions()
        {
            RootScript root = RootScript.RootScriptInstance;

            while(explosions != 0)
            {
                root.explode(victorBoss.transform.position, 6, 1, Vector3.zero, "Yellow", true, true);
                explosions--;

                yield return new WaitForSeconds(0.2f);
            }


            yield return new WaitForSeconds(5);

            GameObject.FindObjectOfType<CameraScript>().enabled = false;
            root.explode(PlayerScript.PlayerInstance.transform.position, 12, 1, Vector3.zero, "Yellow", false, true);
            PlayerScript.PlayerInstance.gameObject.SetActive(false);

            yield return new WaitForSeconds(3);

            creditsRoot.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            winRect.gameObject.SetActive(true);

            yield return new WaitForSeconds(2);

            float targetSec = 20;
            float curSec = 0;

            while(curSec <= targetSec)
            {
                curSec += Time.deltaTime;
                creditsRoot.anchoredPosition += new Vector2(0, (Time.deltaTime * 64) * root.timescale);

                yield return null;
            }


            yield return new WaitForSeconds(3);
            MFPClassicAssets.rootShared.modSideOnCamera = false;
            GameObject.FindObjectOfType<LevelChangerScript>().doTheThing();
            creditsRoot.gameObject.SetActive(false);
        }

        public void Awake()
        {
            inst = this;
            victorBoss = GameObject.FindObjectOfType<Victor>();
            bossfightUI = GameObject.Find("MFPLevel/BossfightUI").GetComponent<Canvas>();
            bossfightUI.enabled = false;


            creditsRoot = bossfightUI.transform.Find("CreditsRoot").GetComponent<RectTransform>();
            bossText = bossfightUI.transform.Find("Text").GetComponent<Text>();
            youRect = creditsRoot.Find("YOU").GetComponent<RectTransform>();
            winRect = creditsRoot.Find("WIN").GetComponent<RectTransform>();

            creditsRoot.gameObject.SetActive(false);
            winRect.gameObject.SetActive(false);

            bgCamera = GameObject.Find("HorizonBackground_Theme_1/Background Camera").transform;
            bgCamera.GetComponent<Camera>().farClipPlane = 300;
            bgCamera.GetComponent<Camera>().enabled = false;
            bgCamera.transform.position += new Vector3(0,60,0);

            RenderSettings.skybox = MFPClassicAssets.classicBundle.LoadAsset("nightSky") as Material;
            //   GameObject.FindObjectOfType<CameraScript>().GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;

            if (PlayerScript.PlayerInstance.health < 0.5f)
                PlayerScript.PlayerInstance.health = 0.5f;
        }


        public void OnVictorDeath()
        {
            victorBoss.victorAnimator.enabled = false;
            victorBoss.health = 0;
            victorBoss.StopAllCoroutines();
            victorBoss.enabled = false;
            bossText.enabled = false;
            victorBoss.healthBar.transform.parent.gameObject.SetActive(false);


            StartCoroutine(DeathExplosions());
            PlayerScript.PlayerInstance.overrideControls = true;

          /*  GameObject blendZone = new GameObject();
            BoxCollider blendZoneColl = blendZone.AddComponent<BoxCollider>();
            blendZoneColl.size = new Vector3(999, 999, 999);
            blendZoneColl.isTrigger = true;

            CameraZoneScript camZoneScript = blendZone.AddComponent<CameraZoneScript>();
            camZoneScript.focusPos = new Vector3(555,5555,5555);*/

                
        }


        public void OnDestroy()
        {
            if (!playerAlreadyHadEnabledSideCam)
                MFPClassicAssets.rootShared.modSideOnCamera = false;

            if (playerAlreadyHadEnabledSideCam && !MFPClassicAssets.rootShared.modSideOnCamera)
                MFPClassicAssets.rootShared.modSideOnCamera = true;
        }

        public void Update()
        {
            MFPClassicAssets.player.propellerHat = true;

            if (victorBoss.health <= 0)
            {
                victorBoss.transform.Translate((-victorBoss.transform.up * Time.deltaTime * 4) * RootScript.RootScriptInstance.timescale);
            }


            if (startSwitch.output == 1)
            {
                if (!startBattleDoOnce)
                {
                    startBattleDoOnce = true;
                    victorBoss.victorAnimator.Play("movetoScene", 0, 0);
                }
            }
            else
                    if (MFPClassicAssets.player.dodging)
                MFPClassicAssets.player.nrOfDodgeSpins = 0;
        }

        public void LateUpdate()
        {

            if (!startBattleDoOnce)
                bgCamera.transform.Translate((Vector3.up * Time.deltaTime * 12) * RootScript.RootScriptInstance.timescale);

            if(MFPClassicAssets.root.doCheckpointSave)
                playerPosHack = MFPClassicAssets.player.transform.position;
            if (MFPClassicAssets.root.doCheckpointLoad)
            {
                //PlayerWings does some stupid shit to player's saved position on checkpoint so a simple workaround

                if (!MFPClassicAssets.player.GetComponent<PlayerWings>())
                    MFPClassicAssets.player.gameObject.AddComponent<PlayerWings>().checkpointFix = true;

                MFPClassicAssets.player.transform.position = playerPosHack;
            }
        }
    }
}
