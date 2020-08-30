using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using HarmonyLib;
using System.Reflection;
using UnityEngine.SceneManagement;
using I2.Loc;
using UnityEngine.UI;
using TMPro;


namespace MFPClassic
{
    public class Hook
    {
        public static bool initialized = false;
        public static bool doLoad = true;

        public static bool doAssetReload = false;

        public static bool devMode
        {
#if DEBUG
            get { return true; }
#elif !DEBUG
            get { return false; }
#endif
        }

        public static bool superDebugMode = false;


        static Harmony _harmony;

        public static void Initialize()
        {
            // PrepareMenu();

       
            MFPEditorUtils.ClearLog();

#if DEBUG
            MFPEditorUtils.InitGUILogging();
#endif
            if (PlatformPlayerPrefs.HasKey("mfpClassicCurrentLevel"))
            {
                MapManager.currentLevel = PlatformPlayerPrefs.GetInt("mfpClassicCurrentLevel");
                MFPEditorUtils.Log("got cur level");
            }

            if (doAssetReload)
            {
                AssetBundle.UnloadAllAssetBundles(true);
                MFPClassicAssets.classicBundle = null;
                MFPClassicAssets.weaponSound = null;
            }

            if (MFPClassicAssets.classicBundle == null)
            {
                MFPClassicAssets.classicBundle = AssetBundle.LoadFromFile(MFPEditorUtils.LoadFile("mfpclassic"));
                MFPClassicAssets.LoadAssets();
            }


            if (!initialized)
            {
                //GameObject.Find("Ground (1)/key_art_assets").GetComponent<MeshRenderer>().sharedMaterial.color = new Color32(111, 75, 45, 255);
                GameObject.Find("Pedro_high_res/pedro_face_big").SetActive(false);

                _harmony = new Harmony("Jhrino.MFPClassic");
                _harmony.PatchAll(Assembly.GetExecutingAssembly());

                //

                MFPEditorUtils.CreateTranslation("instructionsJump", "<JUMP>   - Jump");

                MFPEditorUtils.CreateTranslation("instructionsWeaponWheel", "<CHANGEWEAPON>   - Switch to your knife");
                MFPEditorUtils.CreateTranslation("instructionsShootPistol", "<SHOOT>   - Fire your pistol");
                MFPEditorUtils.CreateTranslation("instructionsPistol", "<PISTOL>   - Switch to your pistol");
                MFPEditorUtils.CreateTranslation("instructionsKick", "<MELEE>   - Kick");
                MFPEditorUtils.CreateTranslation("instructionsFocus", "<ACTION>   - Slow-down time");
                MFPEditorUtils.CreateTranslation("instructionsRoll1", "<CROUCH>   -  Crouch");
                MFPEditorUtils.CreateTranslation("instructionsRoll2", "<LEFT> or <RIGHT>   - Roll");
                MFPEditorUtils.CreateTranslation("instructionsFlip1", "<JUMP>    - Jump");
                MFPEditorUtils.CreateTranslation("instructionsFlip2", "Hold <LEFT> or <RIGHT> mid-air    - Flip");

                MFPEditorUtils.CreateTranslation("instructionsWallJump1", "<JUMP> Jump on the wall");
                MFPEditorUtils.CreateTranslation("instructionsWallJump2", "<JUMP> Jump again");

                MFPEditorUtils.CreateTranslation("mClassicTheme", "MFP Classic");
                MFPEditorUtils.CreateTranslation("mClassicTutorial", "Tutorial");
                MFPEditorUtils.CreateTranslation("mAgren", "DeadToast");


                MFPEditorUtils.CreateTranslation("pedro01_level1_MFPClassic", "Hello.|It's me. Pedro|... Your friend.|Don't worry. I'm wearing my invisibility undies.|You're the only one who can see me.|We're going on an adventure, you and me.|Let's start by learning some basics...");
                MFPEditorUtils.CreateTranslation("pedro01a_level1_MFPClassic", "Try moving over here.");
                MFPEditorUtils.CreateTranslation("pedro01b_level1_MFPClassic", "Jolly good!");

                MFPEditorUtils.CreateTranslation("pedro02_level1_MFPClassic", "Would you look at this...|Somebody placed these boxes RIGHT in our way!|People have no respect these days.|You're going to have to jump over them.");
                MFPEditorUtils.CreateTranslation("pedro03_level1_MFPClassic", "Look!|I bet he's the one who put the boxes in our way.|Let's teach him a lesson!|Shoot him in the head.|Don't worry, he is totally asking for it!");

                /*Shoot the tutorial enemy*/
                MFPEditorUtils.CreateTranslation("pedro04a_level1_MFPClassic", "Nice shooting back there!|You're doing great!|Now. Look here.|You need to do a wall jump off of that wall.|Remember, timing is important!");
                /*Stab the tutorial enemy*/
                MFPEditorUtils.CreateTranslation("pedro04b_level1_MFPClassic", "Nice stabbing back there! Guns are overrated anyway.|You're doing great!|Now. Look here.|You need to do a wall jump off of that wall.|Remember, timing is important!");
                /*Kick the tutorial enemy*/
                MFPEditorUtils.CreateTranslation("pedro04c_level1_MFPClassic", "Nice kicks! Your martial arts training paid off.|You're doing great!|Now. Look here.|You need to do a wall jump off of that wall.|Remember, timing is important!");
                /*Flee the tutorial enemy*/
                MFPEditorUtils.CreateTranslation("pedro04d_level1_MFPClassic", "Are you insane?!|That thug is not your friend!|Get back there and kill him before he alerts others!");

                MFPEditorUtils.CreateTranslation("pedro05_level1_MFPClassic", "Excellent! You're a natural at this.|See that narrow opening under this machine?|Let's try rolling into it!");
                MFPEditorUtils.CreateTranslation("pedro06_level1_MFPClassic", "Wonderful!|There's only one more trick i have to teach you now.|Remember all those pills you were forced to take..?|Well, they seem to have had a nice side effect...|You can slow down your perception of time!|Isn't that cool?!|While slowing down time you can perform stunts in the air.|You'll look like a total bad-ass!");

                MFPEditorUtils.CreateTranslation("pedro01_level2_MFPClassic", "Alright! Are you ready to have some fun?|There are more inconsiderate gangsters below this glass roof.|Let's take them out by surprise!|Stand on top of the glass roof and shoot it.|Slow down time for an extra magnificent entrance!");
            }
            else
                return;

            MFPEditorUtils.Log("Game patched");

            initialized = true;

        }

        public static void UnlockWeapons()
        {
#if DEBUG
            PlayerScript playerScript = GameObject.FindObjectOfType<PlayerScript>();

            for (int i = 7; i <= 10; i++)
            {
                playerScript.weaponActive[i] = true;
                playerScript.ammoTotal[i] = 999;
                playerScript.secondaryAmmo[i] = 1;

            }
#endif


        }


        [HarmonyPatch(typeof(LevelCompleteScreenScript))]
        [HarmonyPatch("Awake")]
        private class TestPatch
        {
            [HarmonyPostfix]
            static void PatchingEndLevel(LevelCompleteScreenScript __instance)
            {
                GameObject.Find("HUD/Canvas/EndScreen/LevelCompleteAlert/RestartLevelButton").GetComponent<Button>().onClick.AddListener(delegate { MapManager.currentLevel -= 1; });
            }
        }


        [HarmonyPatch(typeof(MainMenuBackgroundScript))]
        [HarmonyPatch("Update")]
        private class IEnumeratorPatchTest
        {
            [HarmonyPostfix]
            static void DebugOptions()
            {
                if (Input.GetKeyDown(KeyCode.U))
                {
                    doAssetReload = true;
                    Hook.Initialize();
                }

#if DEBUG

                if (Input.GetKeyDown(KeyCode.Z))
                    GameObject.FindObjectOfType<OptionsMenuScript>().buildDebugMenu();

                if (Input.GetKeyDown(KeyCode.O))
                    doLoad = (doLoad ? false : true);
                if (Input.GetKeyDown(KeyCode.P))
                    SceneManager.LoadScene(52);
#endif
            }
        }


        #region Tutorial Alternative Outcome Stabbing
        [HarmonyPatch(typeof(PlayerScript))]
        [HarmonyPatch("stab")]
        private class TutorialAlternativeOutcomeKnife
        {
            [HarmonyPrefix]
            static void StabPre()
            {
                if (MapManager.currentLevel == 1)
                {
                    if (MFPClassicAssets.root.allEnemies.Length != 0)
                    {
                        if (MFPClassicAssets.root.allEnemies[0].GetComponent<EnemyScript>().enabled)
                            MapManager.inst.tutorialEnemyAlive = true;
                    }
                }
            }

            [HarmonyPostfix]
            static void StabPost()
            {
                if (MapManager.currentLevel == 1)
                {
                    MapManager.inst.recentAttackType = "stab";
                    MapManager.inst.Invoke("CheckTutorialStatus", 0.05f);
                }

                //  GameObject.Find("HUD/Canvas/EndScreen/LevelCompleteAlert/RestartLevelButton").GetComponent<Button>().onClick.AddListener(delegate { MapManager.currentLevel -= 1; });
            }
        }
        #endregion


        [HarmonyPatch(typeof(PlayerScript))]
        [HarmonyPatch("updateHealthHUD")]
        private class RootScriptLoadProgress
        {
            [HarmonyPostfix]
            static void WebHudUpdate(PlayerScript __instance)
            {

                float x = Mathf.Clamp01(__instance.health);
                Vector3 localScale = MFPClassicAssets.healthBar2HUDRect.localScale;
                float num = localScale.x = x;
                Vector3 vector = MFPClassicAssets.healthBar2HUDRect.localScale = localScale;

                MFPClassicAssets.healthBar2HUD.color = MFPClassicAssets.hudOrange;
                MFPClassicAssets.slowMoIcon.color = MFPClassicAssets.hudLightOrange;
            }
        }

        [HarmonyPatch(typeof(PlayerScript))]
        [HarmonyPatch("Awake")]
        private class PlayerScriptAwakePatch
        {
            [HarmonyPrefix]
            static void KillObsoleteEntities()
            {
                MFPClassicAssets.healthBar2HUDRect = GameObject.Find("HUD/Canvas/HealthAndSlowMo/HealthBar/HealthBar2/Bar").GetComponent<RectTransform>();

                foreach (InstructionTextScript insScript in GameObject.FindObjectsOfType<InstructionTextScript>())
                {
                    foreach (string st in insScript.textLines)
                        MFPEditorUtils.Log(st);

                    MFPEditorUtils.Log(insScript.reTrigger.ToString());
                    MFPEditorUtils.Log(insScript.forceActivate.ToString());
                    MFPEditorUtils.Log(insScript.triggerWithoutPlayer.ToString());

                    MFPEditorUtils.Log("--------------");

                    foreach (Component comp in insScript.gameObject.GetComponents<Component>())
                    {
                        MFPEditorUtils.Log(comp.ToString() + " " + comp.gameObject.name);
                        ExtensiveLogging.Log(comp);
                    }

                    MFPEditorUtils.Log("--------------");
                }

                MFPEditorUtils.Log("Done");

                if (!doLoad)
                    return;


                MFPEditorUtils.Log(MFPClassicAssets.player.transform.position.x.ToString() + " " + MFPClassicAssets.player.transform.position.y.ToString() + " " + MFPClassicAssets.player.transform.position.z.ToString());

                if (GameObject.FindObjectOfType<AutoControlZoneScript>())
                    GameObject.Destroy(GameObject.FindObjectOfType<AutoControlZoneScript>().gameObject);

                if (GameObject.FindObjectOfType<CameraZoneScript>())
                    GameObject.Destroy(GameObject.FindObjectOfType<CameraZoneScript>().gameObject);

                foreach (PedroScript pedro in GameObject.FindObjectsOfType<PedroScript>())
                {
                    if (MFPClassicAssets.pedroSample == null)
                    {
                        GameObject newPedro = GameObject.Instantiate(pedro.gameObject);
                        MFPClassicAssets.pedroSample = newPedro;
                        MFPClassicAssets.pedroSample.SetActive(false);
                    }
                    GameObject.Destroy(pedro.gameObject);
                }

                #region Knife
                Animator playerAnim = GameObject.Find("Player").GetComponentInChildren<Animator>();

                AnimatorOverrideController knifeOverride = new AnimatorOverrideController(playerAnim.runtimeAnimatorController);
                AnimationClip knifeIdle = MFPClassicAssets.classicBundle.LoadAsset("knife_idle") as AnimationClip;
                AnimationClip knifeStab = MFPClassicAssets.classicBundle.LoadAsset("Punch1") as AnimationClip;

                knifeOverride["FightPose"] = knifeIdle;
                knifeOverride["Punch1"] = knifeStab;

                playerAnim.runtimeAnimatorController = knifeOverride;

                #endregion

                foreach (SpeechTriggerControllerScript speech in Resources.FindObjectsOfTypeAll<SpeechTriggerControllerScript>())
                    GameObject.Destroy(speech.gameObject);
                foreach (Light l in GameObject.FindObjectsOfType<Light>())
                    l.enabled = false;

                GameObject.Find("HorizonBackground_Theme_1/Background Camera").GetComponent<Camera>().backgroundColor = Color.black;

                foreach (Light light in GameObject.FindObjectsOfType<Light>())
                    if (light.transform.parent == null && !light.transform.name.Contains("Pedro"))
                        GameObject.DestroyImmediate(light.gameObject);

                LightmapSettings.lightmaps = new LightmapData[] { };


                RenderSettings.fogEndDistance = 1000;

                DynamicGI.UpdateEnvironment();
            }
        }


        [HarmonyPatch(typeof(PlayerScript))]
        [HarmonyPatch("Update")]
        private class PlayerScriptUpdatePatch
        {
            [HarmonyPostfix]
            static void KnifeFix(PlayerScript __instance)
            {
                if (!__instance.weaponActive[0])
                {
                    __instance.weaponActive[0] = true;
                    GameObject.FindObjectOfType<UIWeaponSelectorScript>().prepareUI();
                }
            }

            [HarmonyPostfix]
            static void NoAssaultGrenades(PlayerScript __instance)
            {
                if (__instance.secondaryAmmo[5] > 0)
                {
                    __instance.secondaryAmmo[5] = 0;
                    __instance.updateAmmoHUD();
                }
            }
        }

        [HarmonyPatch(typeof(PlayerScript))]
        [HarmonyPatch("Start")]
        private class PlayerScriptStartPatch
        {


            [HarmonyPostfix]
            static void PlayerSetup(PlayerScript __instance, ref AudioClip[] ___weaponSound)
            {

                MFPClassicAssets.LoadGunSounds(___weaponSound);

#if DEBUG
                Hook.UnlockWeapons();
#endif

                __instance.weaponSound = MFPClassicAssets.weaponSound;
                __instance.setGunSound();

                #region Web-ify the HUD
                Image healthBar1HUD = GameObject.Find("HUD/Canvas/HealthAndSlowMo/HealthBar/HealthBar1/Bar").GetComponent<Image>();
                Image healthBar3HUD = GameObject.Find("HUD/Canvas/HealthAndSlowMo/HealthBar/HealthBar3/Bar").GetComponent<Image>();

                MFPClassicAssets.healthBar2HUD.transform.parent.GetComponent<Image>().color = MFPClassicAssets.hudLightOrange;

                //sonra bunları transform find yap üşendiğim için copy paste
                GameObject.Find("HUD/Canvas/HealthAndSlowMo/SlowMoIcon/SlowMoBar/Bar").GetComponent<Image>().color = MFPClassicAssets.hudOrange;
                GameObject.Find("HUD/Canvas/HealthAndSlowMo/SlowMoIcon/SlowMoBar").GetComponent<Image>().color = MFPClassicAssets.hudLightOrange;

                GameObject weaponPanel = GameObject.Find("HUD/Canvas/WeaponPanel");

                weaponPanel.transform.Find("WeaponIcon").GetComponent<Image>().color = MFPClassicAssets.hudLightOrange;
                weaponPanel.transform.Find("InfiniteSymbol").GetComponent<Image>().color = MFPClassicAssets.hudLightOrange;
                weaponPanel.transform.Find("AmmoText").GetComponent<TextMeshProUGUI>().color = MFPClassicAssets.hudLightOrange;

                MFPClassicAssets.slowMoIcon = GameObject.Find("HUD/Canvas/HealthAndSlowMo/SlowMoIcon").GetComponent<Image>();

                GameObject.Find("HUD/Canvas/SpeechBubble_Tail").GetComponent<Image>().color = Color.white;

                foreach (Image obj in GameObject.Find("HUD/Canvas/SpeechBubble_Bubble").GetComponentsInChildren<Image>())
                    obj.color = Color.white;

                foreach (Text obj in GameObject.Find("HUD/Canvas/SpeechBubble_Bubble").GetComponentsInChildren<Text>())
                {
                    obj.color = Color.black;
                    obj.font = MFPClassicAssets.preAlphaFont;
                }


                GameObject healthBar = GameObject.Find("HUD/Canvas/HealthAndSlowMo/HealthBar");
                healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 64f);

                GameObject healthBarIcon = GameObject.Find("HUD/Canvas/HealthAndSlowMo/HealthIcon");
                healthBarIcon.GetComponent<Image>().sprite = MFPClassicAssets.hudHealthSquare;


                GameObject healthBarAnchor = new GameObject();
                healthBarAnchor.AddComponent<RectTransform>();
                healthBarAnchor.transform.parent = GameObject.Find("HUD/Canvas/WeaponPanel").transform;

                healthBarAnchor.GetComponent<RectTransform>().localPosition = new Vector3(50, 250, 0);


                healthBar.transform.parent = healthBarAnchor.transform;
                healthBar.transform.localPosition = Vector3.zero;


                GameObject healthBarIconAnchor = new GameObject();

                healthBarIconAnchor.AddComponent<RectTransform>();
                healthBarIconAnchor.transform.parent = GameObject.Find("HUD/Canvas/WeaponPanel").transform;

                healthBarIconAnchor.GetComponent<RectTransform>().localPosition = new Vector3(50, 430, 0);
                healthBarIcon.transform.parent = healthBarIconAnchor.transform;
                healthBarIcon.transform.localPosition = Vector3.zero;

                healthBar.GetComponent<RectTransform>().Rotate(0, 0, 90);


                // healthBar.GetComponent<RectTransform>().transform.position = new Vector3(1230, 0);
                // healthBar.GetComponent<RectTransform>().localPosition += Vector3.right * 10;
                #endregion

                if (!doLoad)
                    return;

                MapCleaner.Clean();

                new GameObject().AddComponent<MapManager>();

            }
        }


        [HarmonyPatch(typeof(LevelChangerScript))]
        [HarmonyPatch("doTheThing")]
        private class LevelChangerFinishPatch
        {
            [HarmonyPostfix]
            static void NextLevel()
            {
                MapManager.currentLevel++;
            }
        }


        [HarmonyPatch(typeof(LevelChangerScript))]
        [HarmonyPatch("Start")]
        private class LevelChangerStartPatch
        {
            [HarmonyPostfix]
            static void SetClassic(LevelChangerScript __instance)
            {
                if (GameObject.Find("MFPLevel"))
                    return;

                MFPClassicAssets.root = GameObject.FindObjectOfType<RootScript>();
                MFPClassicAssets.rootShared = GameObject.FindObjectOfType<RootSharedScript>();

                GameObject gameObject = GameObject.Find("Player/PlayerGraphics/TorsorBlackLongSleeve");

                GameObject.Find("Player/PlayerGraphics/Armature/Center/LowerBack/UpperBack/Shoulder_L/UpperArm_L/LowerArm_L/Hand_L/hand_01_L").GetComponent<SkinnedMeshRenderer>().material.color = Color.black;
                GameObject.Find("Player/PlayerGraphics/Armature/Center/LowerBack/UpperBack/Shoulder_R/UpperArm_R/LowerArm_R/Hand_R/hand_01").GetComponent<SkinnedMeshRenderer>().material.color = Color.black;

                #region Beta Looks
                if (Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault((GameObject g) => g.name == "Head01" && g.transform.root.name == "Player") != null)
                {
                    GameObject betaHead = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault((GameObject g) => g.name == "Head01" && g.transform.root.name == "Player");
                    betaHead.SetActive(true);

                    betaHead.GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = true;
                    betaHead.GetComponent<SkinnedMeshRenderer>().materials = new Material[] { MFPClassicAssets.character_head_test_bald };
                }
                else
                    MFPEditorUtils.Log("Head01 not present!");

                GameObject betaLegs = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault((GameObject g) => g.name == "Legs01" && g.transform.root.name == "Player");
                betaLegs.SetActive(true);


                betaLegs.GetComponent<SkinnedMeshRenderer>().sharedMaterial.mainTexture = MFPClassicAssets.classicBundle.LoadAsset("legs01_2") as Texture;


                GameObject defaultRenderer = GameObject.Find("Player/PlayerGraphics/TorsorBlackLongSleeve");


                GameObject torsor = new GameObject();
                SkinnedMeshRenderer torsorRenderer = torsor.AddComponent<SkinnedMeshRenderer>();
                torsorRenderer.sharedMesh = MFPClassicAssets.classicBundle.LoadAsset("TorsoLongCoatAndHoodie") as Mesh;
                torsorRenderer.sharedMaterial = MFPClassicAssets.classicBundle.LoadAsset("torsor_long_coat_and_hoodie") as Material;
                //   torsorRenderer.sharedMaterial.mainTexture = MFPClassicAssets.classicBundle.LoadAsset("torsor_long_coat_and_hoodie_tex") as Texture;
                // torsorRenderer.sharedMaterial.SetTexture("_BumpMap", MFPClassicAssets.classicBundle.LoadAsset("torsor_long_coat_and_hoodie_normal") as Texture);

                torsorRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
                torsorRenderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;

                MFPEditorUtils.Log("Torso material loaded");

                torsor.transform.parent = defaultRenderer.transform;
                torsor.transform.position = defaultRenderer.transform.position;

                SkinnedMeshRenderer originalRenderer = defaultRenderer.GetComponent<SkinnedMeshRenderer>();


                Transform[] replacementBones = new Transform[25];

                for (int i = 0; i < 23; i++)
                    replacementBones[i] = originalRenderer.bones[i];

                replacementBones[23] = replacementBones[0];
                replacementBones[24] = replacementBones[0];

                torsorRenderer.bones = replacementBones;
                torsorRenderer.rootBone = originalRenderer.rootBone;
                torsorRenderer.probeAnchor = originalRenderer.probeAnchor;


                torsorRenderer.updateWhenOffscreen = true;

                originalRenderer.enabled = false;
                #endregion


                foreach (SkinnedMeshRenderer rend in MFPClassicAssets.player.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    rend.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
                    rend.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
                }


                __instance.inputSwitch = new SwitchScript[] { __instance.gameObject.AddComponent<SwitchScript>() };

            }
        }


        [HarmonyPatch(typeof(EnemyScript))]
        [HarmonyPatch("Start")]
        private class EnemyStartPatch
        {
            [HarmonyPostfix]
            static void FixProbes(EnemyScript __instance)
            {
                foreach (SkinnedMeshRenderer rend in __instance.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    rend.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
                    rend.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
                }
            }
        }

        [HarmonyPatch(typeof(RootScript))]
        [HarmonyPatch("Start")]
        private class RootStartPatch
        {
            [HarmonyPostfix]
            static void Testing(RootScript __instance)
            {
                __instance.weaponIcons[5] = MFPClassicAssets.m4Sprite;
            }
        }


        [HarmonyPatch(typeof(RootScript))]
        [HarmonyPatch("Update")]
        private class RootUpdatePatch
        {
            [HarmonyPostfix]
            static void Testing(RootScript __instance)
            {
                __instance.levelToLoad = 6;
            }
        }

    }
}
