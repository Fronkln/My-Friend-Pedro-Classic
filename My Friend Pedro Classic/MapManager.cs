using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;
using Rewired;

namespace MFPClassic
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager inst;

        public static int currentLevel = 1;
        private int optimalPixelLightAmount = 5;
        public bool isBossLevel = false;

        public List<EnemyScript> enemies = new List<EnemyScript>();

        public PostProcessingBehaviour postProcess;

        public Player playerInput;

        #region Tutorial Alternative Outcomes


        public string recentAttackType = "gun";
        public SpeechTriggerScript postKillSpeech;

        public bool tutorialEnemyAlive = false; //stab ve kicklerken tek kareliğine true yap


        public void CheckTutorialStatus()
        {
            if (tutorialEnemyAlive && MFPClassicAssets.root.allEnemies.Length != 0)
            {
                if (!MFPClassicAssets.root.allEnemies[0].GetComponent<EnemyScript>().enabled)
                {
                    switch (recentAttackType)
                    {
                        default:
                            break;
                        case "kick":
                            MFPEditorUtils.Log("Tutorial enemy got kicked to death.");
                            postKillSpeech.locStringId = "pedro04c_level1_MFPClassic";
                            break;
                        case "stab":
                            MFPEditorUtils.Log("Tutorial enemy got stabbed to death.");
                            postKillSpeech.locStringId = "pedro04b_level1_MFPClassic";
                            break;
                    }

                    MFPClassicAssets.root.clearInstructionText();
                }

                if (recentAttackType != "")
                    recentAttackType = "";


            }
        }

        #endregion
        IEnumerator WaitLevelLoad()
        {

            yield return new WaitForSeconds(0.1f);

            MFPEditorUtils.Log("Level loaded");

            GameObject.Find("Main Camera").GetComponent<CameraScript>().centerCamPosOnPlayer();

            if (!GameObject.Find("MFPLevel"))
                Application.Quit();

            if (GameObject.Find("MFPLevel/ZPushers") != null)
            {
                MFPEditorUtils.Log("Starting pusher preparation");

                foreach (Transform obj in GameObject.Find("MFPLevel/ZPushers").GetComponentsInChildren<Transform>())
                {
                    if (obj.transform.name != "ZPushers")
                        obj.gameObject.AddComponent<CustomZPusher>().pushIntensity = float.Parse(obj.transform.name.Replace("Player Z-Pusher=", ""), System.Globalization.CultureInfo.InvariantCulture);
                }
            }

            if (GameObject.Find("MFPLevel/OpenableDoors") != null)
            {
                foreach (Transform obj in GameObject.Find("MFPLevel/OpenableDoors").GetComponentsInChildren<Transform>())
                {
                    if (obj.transform.name != "OpenableDoors")
                    {
                        string[] args = obj.name.Split('_');
                        RuntimeEntities.CreateOpenableDoor(obj.position, args[1].ToLower().Equals("true"), float.Parse(args[2]));
                    }

                       //-- obj.gameObject.AddComponent<CustomZPusher>().pushIntensity = float.Parse(obj.transform.name.Replace("Player Z-Pusher=", ""), System.Globalization.CultureInfo.InvariantCulture);
                }
            }

            if (GameObject.Find("MFPLevel/DoorSpawns") != null)
                foreach (Transform obj in GameObject.Find("MFPLevel/DoorSpawns").GetComponentsInChildren<Transform>())
                    if (obj.transform.name.Contains("visualdoor"))
                        RuntimeEntities.CreateEnemyDoor(obj.transform.position, 0, true);

            if (GameObject.Find("MFPLevel/BreakableGlass") != null)
                foreach (Transform glass in GameObject.Find("MFPLevel/BreakableGlass").GetComponentsInChildren<Transform>())
                    if (glass.name != "BreakableGlass")
                    {
                        if (glass.name.Contains("glass_piece"))
                        {
                            glass.gameObject.AddComponent<GlassPieceScript>();
                            glass.gameObject.AddComponent<RigidBodySlowMotion>();
                        }

                        if (glass.name == "glass_window_large")
                        {
                            glass.gameObject.AddComponent<GlassWindowScript>().glassSounds = new AudioClip[] { MFPClassicAssets.weaponSound[0], MFPClassicAssets.weaponSound[0] };

                            if (glass.transform.parent.name.Contains("easybreak"))
                                glass.gameObject.GetComponent<GlassWindowScript>().easyBreak = true;
                        }
                    }


            if (GameObject.Find("MFPLevel/Medkits") != null)
                foreach (Transform obj in GameObject.Find("MFPLevel/Medkits").GetComponentsInChildren<Transform>())
                {
                    if (obj.name == "Medkits") //wtf???
                        continue;

                    RuntimeEntities.SpawnMedkit(obj.position);
                }


            foreach (Transform obj in GameObject.Find("MFPLevel/Enemies").GetComponentsInChildren<Transform>())
            {
                if (obj.name == "Enemies") //wtf???
                    continue;

                GameObject newEnemy = GameObject.Instantiate(MFPClassicAssets.enemySample);
                newEnemy.transform.position = obj.transform.position;
                newEnemy.SetActive(true);

                MFPEditorUtils.Log("Enemy spawned from spawnpoint:" + obj.name);

                enemies.Add(newEnemy.GetComponent<EnemyScript>());

                if (obj.name.Split(' ').Length > 1)
                    newEnemy.GetComponent<EnemyScript>().weapon = int.Parse(obj.name.Split(' ')[1]);


                newEnemy.name = obj.name;
            }

            MFPEditorUtils.Log("Enemies spawned");

            PrepareLevelEntities();

            yield return null;
        }


        void PreparePostProcess()
        {
            postProcess.profile.colorGrading.enabled = false;

            VignetteModel.Settings vignet = postProcess.profile.vignette.settings;
            vignet.center = new Vector2(0.5f, 0.5f);
            vignet.intensity = 0.45f;
            vignet.smoothness = 0.3f;
            vignet.color = Color.black;

            BloomModel.Settings bloomSettings = postProcess.profile.bloom.settings;
            bloomSettings.bloom.intensity = 1f;
            bloomSettings.bloom.radius = 6.06f;
            bloomSettings.bloom.softKnee = 0.654f;


            postProcess.profile.vignette.settings = vignet;
            postProcess.profile.bloom.settings = bloomSettings;


            //VERY QUESTIONABLE CHANGE BUT WHAT CAN YOU DO
            if (QualitySettings.pixelLightCount < optimalPixelLightAmount)
                QualitySettings.pixelLightCount = optimalPixelLightAmount;
        }

        void Awake()
        {
            if (MFPClassicAssets.rootShared.levelLoadedFromLevelSelectScreen)
                MFPClassicAssets.player.health = 1f;


            inst = this;

            playerInput = ReInput.players.GetPlayer(0);

            MFPClassicAssets.root.nrOfEnemiesTotal = 0;
            MFPClassicAssets.root.dontAllowCheckpointSave = true;

            MFPEditorUtils.Log("MapManager awake, will load level ID: " + currentLevel.ToString());
            //     GameObject.Find("Main Camera").GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>().enabled = false;

            postProcess = GameObject.Find("Main Camera").GetComponent<PostProcessingBehaviour>();
            PreparePostProcess();

            // SceneManager.LoadScene(MFPClassicAssets.classicLevelsBundle.GetAllScenePaths()[0], LoadSceneMode.Additive);

            //cutscene for bossfight not done yet
            if (currentLevel == 7) currentLevel = 8;

            GameObject map = GameObject.Instantiate(MFPClassicAssets.classicBundle.LoadAsset("MFPLevel" + currentLevel.ToString()) as GameObject);
            map.name = "MFPLevel";

            if (currentLevel != 8)
                map.transform.position = new Vector3(9999, 9999, map.transform.position.z);

#if !DEBUG
            Instantiate(MFPClassicAssets.classicBundle.LoadAsset("alphaUI") as GameObject);
#endif

            RenderSettings.ambientIntensity = 0;
            RenderSettings.ambientEquatorColor = Color.black;
            RenderSettings.ambientGroundColor = Color.black;
            RenderSettings.ambientLight = Color.black;
            RenderSettings.ambientSkyColor = Color.black;
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;

            RenderSettings.skybox.color = Color.black;

            RenderSettings.skybox = null;
          


            MFPClassicAssets.player.transform.position = new Vector3(9999, 9999, 0);
            MFPClassicAssets.player.ySpeed = (float)1;

            MFPClassicAssets.player.transform.position = GameObject.Find("MFPLevel/player_spawn").transform.position;

            MFPEditorUtils.Log(map.name);

            StartCoroutine(WaitLevelLoad());
        }



        void Update()
        {


            if (RootScript.RootScriptInstance.paused)
                if (postProcess.profile.colorGrading.enabled || QualitySettings.pixelLightCount < optimalPixelLightAmount)
                    PreparePostProcess();


            if (currentLevel == 1)
            {
                if (MFPClassicAssets.root.allEnemies.Length != 0)
                {
                    if (MFPClassicAssets.player.kPunchPublic)
                    {
                        if (MFPClassicAssets.root.allEnemies[0].GetComponent<EnemyScript>().enabled)
                        {
                            recentAttackType = "kick";
                            tutorialEnemyAlive = true;
                            Invoke("CheckTutorialStatus", 0.1f);
                        }
                    }

                    if (playerInput.GetButtonDown("Fire"))
                        MFPClassicAssets.root.clearInstructionText();
                }
            }

#if DEBUG
            if (Input.GetKeyDown(KeyCode.P))
            {
                UnityEngine.PostProcessing.PostProcessingBehaviour behaviour = GameObject.Find("Main Camera").GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>();

                if (behaviour.profile.colorGrading.enabled)
                    behaviour.profile.colorGrading.enabled = false;
                else
                    behaviour.profile.colorGrading.enabled = true;
            }

            if (Input.GetKeyDown(KeyCode.O))
                foreach (Light l in GameObject.Find("MFPLevel").GetComponentsInChildren<Light>())
                    l.range -= 10;

            if (Input.GetKeyDown(KeyCode.I))
                GameObject.FindObjectOfType<LevelChangerScript>().doTheThing();
#endif
        }



        void PrepareLevelEntities()
        {
            if (GameObject.Find("MFPLevel/ActionMusicTrigger"))
                GameObject.Find("MFPLevel/ActionMusicTrigger").AddComponent<CustomMusic>();
            else
                new GameObject().AddComponent<CustomMusic>();

            LevelChangerScript lScript = GameObject.Find("MFPLevel/level_end").AddComponent<LevelChangerScript>();

            MFPEditorUtils.FixPlayerLoadout();

            switch (currentLevel)
            {
                case 1:

                    MFPClassicAssets.player.health = 1f;

                    PlayerScript playerScript = MFPClassicAssets.player;

                    lScript.dontShowEndScreen = true;

                    SpawnDoorScript door1 = RuntimeEntities.CreateEnemyDoor(GameObject.Find("MFPLevel/Door1").transform.position, 1);
                    door1.GetComponent<BoxCollider>().enabled = false;


                    for (int i = 1; i <= 4; i++)
                        GameObject.Find("MFPLevel/SpeechTriggers/InstructionStopper" + i.ToString()).AddComponent<ClearInstructionsOnTrigger>();

                    GameObject.Find("Car_1").AddComponent<SwitchScript>();
                    SwitchABMoveScript backgroundCar1_lv1 = GameObject.Find("Car_1").AddComponent<SwitchABMoveScript>();
                    backgroundCar1_lv1.inputSwitch = new SwitchScript[] { };

                    backgroundCar1_lv1.noInputSwitchAnimPause = 166;
                    backgroundCar1_lv1.animLengthInFrames = 180;
                    backgroundCar1_lv1.returnAnimLengthInFrames = 0.1f;
                    backgroundCar1_lv1.movePos = new Vector3(60, 0, 0);
                    backgroundCar1_lv1.acceleration = 0.1f;

                    GameObject.Find("Car_2").AddComponent<SwitchScript>();
                    SwitchABMoveScript backgroundCar1_lv2 = GameObject.Find("Car_2").AddComponent<SwitchABMoveScript>();
                    backgroundCar1_lv2.inputSwitch = new SwitchScript[] { };

                    backgroundCar1_lv2.noInputSwitchAnimPause = 100;
                    backgroundCar1_lv2.animLengthInFrames = 180;
                    backgroundCar1_lv2.returnAnimLengthInFrames = 0.1f;
                    backgroundCar1_lv2.movePos = new Vector3(-60, 0, 0);
                    backgroundCar1_lv2.acceleration = 0.1f;


                    InstructionTextScript instructions1 = RuntimeEntities.CreateInstructionText(new GameObject(), new string[] { "in1", "in2" }, false, false, true);
                    InstructionTextScript instructions2 = RuntimeEntities.CreateInstructionText(new GameObject(), new string[] { "instructionsJump" }, false, false, true);
                    InstructionTextScript instructions3 = RuntimeEntities.CreateInstructionText(new GameObject(), new string[] { "instructionsShootPistol", "instructionsWeaponWheel", "instructionsKick" }, false, false, true);
                    InstructionTextScript instructions4 = RuntimeEntities.CreateInstructionText(new GameObject(), new string[] { "instructionsWallJump1", "instructionsWallJump2" }, false, false, true);
                    InstructionTextScript instructions5 = RuntimeEntities.CreateInstructionText(new GameObject(), new string[] { "instructionsRoll" }, false, false, true);
                    InstructionTextScript instructions6 = RuntimeEntities.CreateInstructionText(new GameObject(), new string[] { "instructionsFocus", "instructionsFlip" }, false, false, true);

                    GameObject speech_Level1Trigger1 = GameObject.Find("MFPLevel/SpeechTriggers/trigger1");
                    GameObject speech_Level1Trigger2 = GameObject.Find("MFPLevel/SpeechTriggers/trigger2");
                    GameObject speech_Level1Trigger3 = GameObject.Find("MFPLevel/SpeechTriggers/trigger3");
                    GameObject speech_Level1Trigger4 = GameObject.Find("MFPLevel/SpeechTriggers/trigger4");
                    GameObject speech_Level1Trigger5 = GameObject.Find("MFPLevel/SpeechTriggers/trigger5");
                    GameObject speech_Level1Trigger6 = GameObject.Find("MFPLevel/SpeechTriggers/trigger6");

                    GameObject instruction2CompletedSpeech = GameObject.Find("MFPLevel/SpeechTriggers/trigger1_1");

                    GameObject tutorialThugAliveSpeech = GameObject.Find("MFPLevel/SpeechTriggers/thugIsAliveTrigger");

                    GameObject speech_Level1Pedro = RuntimeEntities.SpawnPedro(GameObject.Find("MFPLevel/Pedro").transform.position);
                    GameObject speech_Level1Pedro2 = RuntimeEntities.SpawnPedro(GameObject.Find("MFPLevel/Pedro2").transform.position);
                    GameObject speech_Level1Pedro3 = RuntimeEntities.SpawnPedro(GameObject.Find("MFPLevel/Pedro3").transform.position);
                    GameObject speech_Level1Pedro4 = RuntimeEntities.SpawnPedro(GameObject.Find("MFPLevel/Pedro4").transform.position);
                    // GameObject speech_Level1Pedro4_5 = RuntimeEntities.SpawnPedro(GameObject.Find("MFPLevel/Pedro4_5").transform.position);
                    GameObject speech_Level1Pedro5 = RuntimeEntities.SpawnPedro(GameObject.Find("MFPLevel/Pedro5").transform.position);
                    GameObject speech_Level1Pedro6 = RuntimeEntities.SpawnPedro(GameObject.Find("MFPLevel/Pedro6").transform.position);

                    GameObject instruction2CompletedPedro = RuntimeEntities.SpawnPedro(GameObject.Find("MFPLevel/Pedro_a").transform.position);

                    SpeechTriggerControllerScript speechScript1_lv1 = RuntimeEntities.GenerateSpeechScript(speech_Level1Trigger1, speech_Level1Pedro.transform, "Pedro", "pedro01_level1_MFPClassic", MFPClassicAssets.pedroSounds[UnityEngine.Random.Range(0, MFPClassicAssets.pedroSounds.Length)], true, speech_Level1Pedro);
                    speech_Level1Trigger1.GetComponent<SpeechTriggerControllerScript>().triggerWithoutPlayer = true;

                    speechScript1_lv1.triggerInstructionTextOnFinish = instructions1;


                    SpeechTriggerControllerScript speechScript1_1_level1 = RuntimeEntities.GenerateSpeechScript(new GameObject(), speech_Level1Pedro.transform, "Pedro", "pedro01a_level1_MFPClassic", MFPClassicAssets.pedroSounds[UnityEngine.Random.Range(0, MFPClassicAssets.pedroSounds.Length)], false, speech_Level1Pedro);
                    SpeechTriggerControllerScript speechScript1_2_level1 = RuntimeEntities.GenerateSpeechScript(instruction2CompletedSpeech, instruction2CompletedPedro.transform, "Pedro", "pedro01b_level1_MFPClassic", MFPClassicAssets.pedroSounds[UnityEngine.Random.Range(0, MFPClassicAssets.pedroSounds.Length)], false, instruction2CompletedPedro);
                    speechScript1_2_level1.clearInstructionTextOnStart = true;

                    ActivateSpeechOnInput tutorialTalker1 = new GameObject().AddComponent<ActivateSpeechOnInput>();
                    tutorialTalker1.tracking = speechScript1_lv1;
                    tutorialTalker1.target = speechScript1_1_level1;

                    SpeechTriggerControllerScript speechScript2_lv1 = RuntimeEntities.GenerateSpeechScript(speech_Level1Trigger2, speech_Level1Pedro2.transform, "Pedro", "pedro02_level1_MFPClassic", MFPClassicAssets.pedroSounds[UnityEngine.Random.Range(0, MFPClassicAssets.pedroSounds.Length)], true, speech_Level1Pedro2);
                    speechScript2_lv1.clearInstructionTextOnStart = true;
                    speechScript2_lv1.triggerInstructionTextOnFinish = instructions2;

                    GameObject.Find("MFPLevel/SpeechTriggers/SpeechStopper1").AddComponent<SpeechStopperScript>();

                    SpeechTriggerControllerScript speechScript3_lv1 = RuntimeEntities.GenerateSpeechScript(speech_Level1Trigger3, speech_Level1Pedro3.transform, "Pedro", "pedro03_level1_MFPClassic", MFPClassicAssets.pedroSounds[UnityEngine.Random.Range(0, MFPClassicAssets.pedroSounds.Length)], true, speech_Level1Pedro3);

                    speechScript3_lv1.triggerInstructionTextOnFinish = instructions3;

                    door1.inputSwitch = new SwitchScript[] { speechScript3_lv1.GetComponent<SwitchScript>() };
                    door1.standStill = true;

                    postKillSpeech = RuntimeEntities.GenerateSpeechScript(speech_Level1Trigger4, speech_Level1Pedro4.transform, "Pedro", "pedro04a_level1_MFPClassic", MFPClassicAssets.pedroSounds[UnityEngine.Random.Range(0, MFPClassicAssets.pedroSounds.Length)], true, speech_Level1Pedro4).GetComponent<SpeechTriggerScript>();
                    postKillSpeech.GetComponent<SpeechTriggerControllerScript>().triggerInstructionTextOnFinish = instructions4;
                    GameObject.Find("MFPLevel/SpeechTriggers/thugIsAliveTrigger").AddComponent<TutorialThugAliveOutcome>();

                    SpeechTriggerControllerScript dontKillThugSpeech = RuntimeEntities.GenerateSpeechScript(tutorialThugAliveSpeech, speech_Level1Pedro4.transform, "Pedro", "pedro04d_level1_MFPClassic", MFPClassicAssets.pedroSounds[UnityEngine.Random.Range(0, MFPClassicAssets.pedroSounds.Length)], false, speech_Level1Pedro4);
                    dontKillThugSpeech.GetComponent<SpeechTriggerScript>().clickToContinueDontFreeze = false;

                    RuntimeEntities.GenerateSpeechScript(speech_Level1Trigger5, speech_Level1Pedro5.transform, "Pedro", "pedro05_level1_MFPClassic", MFPClassicAssets.pedroSounds[UnityEngine.Random.Range(0, MFPClassicAssets.pedroSounds.Length)], true, speech_Level1Pedro5).triggerInstructionTextOnFinish = instructions5;
                    RuntimeEntities.GenerateSpeechScript(speech_Level1Trigger6, speech_Level1Pedro6.transform, "Pedro", "pedro06_level1_MFPClassic", MFPClassicAssets.pedroSounds[UnityEngine.Random.Range(0, MFPClassicAssets.pedroSounds.Length)], true, speech_Level1Pedro6).triggerInstructionTextOnFinish = instructions6;
                    break;

                case 2:

                    GameObject.FindObjectOfType<LevelChangerScript>().dontShowEndScreen = false;

                    GameObject speech_Level2Trigger1 = GameObject.Find("MFPLevel/SpeechTriggers/trigger1");
                    GameObject speech_Level2Pedro1 = RuntimeEntities.SpawnPedro(GameObject.Find("MFPLevel/Pedro").transform.position);

                    // MFPClassicAssets.root.nrOfEnemiesTotal = enemies.Count / 2 - 6;

                    enemies[0].standStill = true;

                    enemies[1].playerDetectionRadius /= 1.5f;
                    enemies[1].standStill = true;

                    enemies[3].standStill = true;
                    enemies[3].faceRight = false;

                    enemies[4].playerDetectionRadius /= 1.5f;

                    enemies[7].standStill = true;
                    enemies[8].standStill = true;

                    enemies[9].faceRight = false;

                    enemies[11].standStill = true;
                    enemies[11].faceRight = false;
                    enemies[11].playerDetectionRadius *= 1.2f;
                    enemies[11].standStillInHuntMode = true;

                    enemies[11].weapon = 6;
                    enemies[11].disableWeaponPickup = true;

                    RuntimeEntities.CreateChairEnemy(GameObject.Find("MFPLevel/WoodenChair/WoodenChair_0 (3)"), enemies[2], false);
                    RuntimeEntities.CreateChairEnemy(GameObject.Find("MFPLevel/WoodenChair/WoodenChair_0 (1)"), enemies[4], false);

                    RuntimeEntities.GenerateSpeechScript(speech_Level2Trigger1, speech_Level2Pedro1.transform, "Pedro", "pedro01_level2_MFPClassic", MFPClassicAssets.pedroSounds[UnityEngine.Random.Range(0, MFPClassicAssets.pedroSounds.Length)], true, speech_Level2Pedro1);
                    break;


                case 3:

                    SpawnDoorScript door1_level3 = RuntimeEntities.CreateEnemyDoor(GameObject.Find("MFPLevel/DoorSpawns/Door1").transform.position, 5);
                    SpawnDoorScript door2_level3 = RuntimeEntities.CreateEnemyDoor(GameObject.Find("MFPLevel/DoorSpawns/Door2").transform.position, 5);
                    SpawnDoorScript door3_level3 = RuntimeEntities.CreateEnemyDoor(GameObject.Find("MFPLevel/DoorSpawns/Door3").transform.position, 1);
                    SpawnDoorScript door4_level3 = RuntimeEntities.CreateEnemyDoor(GameObject.Find("MFPLevel/DoorSpawns/Door4").transform.position, 1);
                    SpawnDoorScript door5_level3 = RuntimeEntities.CreateEnemyDoor(GameObject.Find("MFPLevel/DoorSpawns/Door5").transform.position, 1);
                    SpawnDoorScript door6_level3 = RuntimeEntities.CreateEnemyDoor(GameObject.Find("MFPLevel/DoorSpawns/Door6").transform.position, 1);
                    SpawnDoorScript door7_level3 = RuntimeEntities.CreateEnemyDoor(GameObject.Find("MFPLevel/DoorSpawns/Door7").transform.position, 1);
                    SpawnDoorScript door8_level3 = RuntimeEntities.CreateEnemyDoor(GameObject.Find("MFPLevel/DoorSpawns/Door8").transform.position, 1);
                    SpawnDoorScript door9_level3 = RuntimeEntities.CreateEnemyDoor(GameObject.Find("MFPLevel/DoorSpawns/Door9").transform.position, 1);
                    SpawnDoorScript door10_level3 = RuntimeEntities.CreateEnemyDoor(GameObject.Find("MFPLevel/DoorSpawns/Door10").transform.position, 2);
                    SpawnDoorScript door11_level3 = RuntimeEntities.CreateEnemyDoor(GameObject.Find("MFPLevel/DoorSpawns/Door11").transform.position, 2);

                    door1_level3.attackAfterDoorSpawn = true;

                    door2_level3.inputSwitch = new SwitchScript[] { door1_level3.GetSwitch() };
                    door2_level3.attackAfterDoorSpawn = true;
                    door3_level3.attackAfterDoorSpawn = true;
                    door4_level3.attackAfterDoorSpawn = true;
                    door5_level3.attackAfterDoorSpawn = true;
                    door6_level3.attackAfterDoorSpawn = true;
                    door7_level3.attackAfterDoorSpawn = true;
                    door8_level3.attackAfterDoorSpawn = true;
                    door9_level3.attackAfterDoorSpawn = true;
                    door10_level3.attackAfterDoorSpawn = true;
                    door11_level3.attackAfterDoorSpawn = true;

                    ElevatorScript elevatorTrigger = GameObject.Find("MFPLevel/ElevatorEntranceTrigger").gameObject.AddComponent<ElevatorScript>();
                    elevatorTrigger.spawnDoors = new SpawnDoorScript[] { door1_level3, door2_level3 };

                    SwitchABMoveScript elevator = GameObject.Find("MFPLevel/Elevator").AddComponent<SwitchABMoveScript>();

                    elevator.gameObject.AddComponent<SwitchScript>();

                    elevator.outputOnReachedEnd = true;
                    elevator.gameObject.transform.parent = null;


                    elevator.inputSwitch = new SwitchScript[] { elevatorTrigger.GetSwitch() };
                    elevatorTrigger.inputSwitch = elevator.GetSwitch();

                    elevator.movePos = new Vector3(0, -11.54f, 0);

                    elevator.moveSpeed = 0.005552662f;
                    elevator.acceleration = 0.1f;

                    elevator.animLengthInFrames = 180;



                    SimpleDoorTrigger doorTrig = GameObject.Find("MFPLevel/DoorTrigger").AddComponent<SimpleDoorTrigger>();
                    doorTrig.spawnDoors = new SpawnDoorScript[] { door10_level3, door11_level3 };

                    enemies[0].standStill = true;
                    enemies[0].standStillInHuntMode = true;
                    enemies[0].weapon = 1;
                    enemies[0].faceRight = false;

                    enemies[1].standStill = true;
                    enemies[1].faceRight = false;

                    enemies[2].weapon = 3;
                    enemies[4].weapon = 3;
                    enemies[5].weapon = 3;

                    enemies[8].weapon = 3;
                    enemies[9].weapon = 6;

                    enemies[10].weapon = 3;

                    enemies[12].standStill = true;
                    enemies[12].faceRight = false;

                    enemies[14].faceRight = true;
                    enemies[15].faceRight = false;

                    SimpleTriggerZoneSwitchScript endElevatorTrigger = GameObject.Find("MFPLevel/EndElevatorTrigger").AddComponent<SimpleTriggerZoneSwitchScript>();
                    endElevatorTrigger.gameObject.AddComponent<SwitchScript>();
                    endElevatorTrigger.enableOnEnter = true;



                    SwitchABMoveScript endElevator = GameObject.Find("MFPLevel/EndElevator").AddComponent<SwitchABMoveScript>();
                    endElevator.gameObject.AddComponent<SwitchScript>();

                    endElevator.inputSwitch = new SwitchScript[] { endElevatorTrigger.GetSwitch() };

                    endElevator.movePos = new Vector3(0, 10, 0);
                    endElevator.moveSpeed = 0.005552662f;
                    endElevator.acceleration = 0.1f;

                    endElevator.animLengthInFrames = 180;



                    break;

                case 4:
                    SpawnDoorScript door1_level4 = RuntimeEntities.CreateEnemyDoor(GameObject.Find("MFPLevel/DoorSpawns/Door1").transform.position, 1);
                    SpawnDoorScript door2_level4 = RuntimeEntities.CreateEnemyDoor(GameObject.Find("MFPLevel/DoorSpawns/Door2").transform.position, 1);
                    SpawnDoorScript door3_level4 = RuntimeEntities.CreateEnemyDoor(GameObject.Find("MFPLevel/DoorSpawns/Door3").transform.position, 1);
                    SpawnDoorScript door4_level4 = RuntimeEntities.CreateEnemyDoor(GameObject.Find("MFPLevel/DoorSpawns/Door4").transform.position, 1);
                    SpawnDoorScript door5_level4 = RuntimeEntities.CreateEnemyDoor(GameObject.Find("MFPLevel/DoorSpawns/Door5").transform.position, 1);
                    SpawnDoorScript door6_level4 = RuntimeEntities.CreateEnemyDoor(GameObject.Find("MFPLevel/DoorSpawns/Door6").transform.position, 1);
                    SpawnDoorScript door7_level4 = RuntimeEntities.CreateEnemyDoor(GameObject.Find("MFPLevel/DoorSpawns/Door7").transform.position, 1);
                    SpawnDoorScript door8_level4 = RuntimeEntities.CreateEnemyDoor(GameObject.Find("MFPLevel/DoorSpawns/Door8").transform.position, 2);
                    SpawnDoorScript door9_level4 = RuntimeEntities.CreateEnemyDoor(GameObject.Find("MFPLevel/DoorSpawns/Door9").transform.position, 1);
                    SpawnDoorScript door10_level4 = RuntimeEntities.CreateEnemyDoor(GameObject.Find("MFPLevel/DoorSpawns/Door10").transform.position, 1);
                    SpawnDoorScript door11_level4 = RuntimeEntities.CreateEnemyDoor(GameObject.Find("MFPLevel/DoorSpawns/Door11").transform.position, 2);

                    door10_level4.GetComponent<BoxCollider>().center -= new Vector3(0, 0, 50);
                    door11_level4.GetComponent<BoxCollider>().center -= new Vector3(0, 0, 50);

                    door1_level4.attackAfterDoorSpawn = true;
                    door2_level4.attackAfterDoorSpawn = true;
                    door3_level4.attackAfterDoorSpawn = true;
                    door4_level4.attackAfterDoorSpawn = true;
                    door5_level4.attackAfterDoorSpawn = true;

                    door6_level4.attackAfterDoorSpawn = true;
                    door6_level4.faceRight = true;

                    door7_level4.attackAfterDoorSpawn = true;
                    door8_level4.attackAfterDoorSpawn = true;
                    door9_level4.attackAfterDoorSpawn = true;
                    door10_level4.attackAfterDoorSpawn = true;
                    door11_level4.attackAfterDoorSpawn = true;

                    door2_level4.weapon = 3;
                    door4_level4.weapon = 6;
                    door7_level4.weapon = 3;
                    door10_level4.weapon = 3;

                    door4_level4.inputSwitch = new SwitchScript[] { door3_level4.GetSwitch() };

                    enemies[0].weapon = 6;
                    enemies[0].faceRight = false;

                    enemies[2].weapon = 3;
                    enemies[2].faceRight = false;

                    enemies[3].weapon = 3;
                    enemies[3].faceRight = false;

                    enemies[4].weapon = 3;

                    enemies[5].weapon = 3;
                    enemies[5].faceRight = false;

                    enemies[7].faceRight = false;

                    enemies[8].weapon = 6;
                    enemies[8].standStill = true;
                    enemies[8].faceRight = false;

                    enemies[9].faceRight = false;

                    enemies[12].faceRight = false;
                    enemies[12].weapon = 6;

                    enemies[14].weapon = 5;
                    enemies[14].standStill = true;
                    enemies[14].standStillInHuntMode = true;
                    enemies[14].faceRight = false;
                    enemies[14].disableWeaponPickup = true;

                    break;

                case 5:

                    SpawnDoorScript door1_level5 = RuntimeEntities.CreateEnemyDoor(GameObject.Find("MFPLevel/DoorSpawns/Door1").transform.position, 1);
                    SpawnDoorScript door2_level5 = RuntimeEntities.CreateEnemyDoor(GameObject.Find("MFPLevel/DoorSpawns/Door2").transform.position, 1);

                    enemies[1].weapon = 3;
                    enemies[2].weapon = 5;

                    enemies[3].standStill = true;
                    enemies[4].standStill = true;
                    enemies[5].standStill = true;

                    enemies[6].weapon = 5;

                    enemies[7].faceRight = false;
                    enemies[7].weapon = 3;

                    enemies[8].weapon = 6;

                    enemies[9].weapon = 6;
                    enemies[9].faceRight = false;

                    enemies[10].weapon = 5;
                    enemies[11].weapon = 3;
                    enemies[13].weapon = 5;

                    enemies[14].weapon = 3;
                    enemies[14].standStill = true;

                    enemies[15].weapon = 3;
                    enemies[15].standStill = true;

                    enemies[16].weapon = 5;
                    enemies[16].standStill = true;
                    enemies[16].standStillInHuntMode = true;
                    enemies[16].faceRight = false;

                    break;
                case 6:

                    SpawnDoorScript door1_level6 = RuntimeEntities.CreateEnemyDoor(GameObject.Find("MFPLevel/DoorSpawns/Door1").transform.position, 1);
                    SpawnDoorScript door2_level6 = RuntimeEntities.CreateEnemyDoor(GameObject.Find("MFPLevel/DoorSpawns/Door2").transform.position, 1);

                    enemies[1].standStill = true;
                    enemies[1].standStillInHuntMode = true;

                    enemies[3].faceRight = true;
                    enemies[3].standStillInHuntMode = true;
                    enemies[3].standStill = true;

                    enemies[4].faceRight = true;
                    enemies[4].standStill = true;

                    enemies[5].standStill = true;

                    enemies[6].standStill = true;
                    enemies[6].standStillInHuntMode = true;
                    enemies[6].faceRight = true;

                    enemies[13].standStill = true;

                    break;

                case 8:

                    MFPClassicAssets.root.dontAllowAutomaticFallDeath = true;
                    // MFPClassicAssets.player.skyfall = true;

                    Victor vic = GameObject.Find("MFPLevel/Victor").AddComponent<Victor>();
                    vic.gameObject.AddComponent<VictorAnimEvents>();

                    MFPClassicAssets.player.gameObject.AddComponent<PlayerWings>();

                    SimpleTriggerZoneSwitchScript arenaZone = GameObject.Find("MFPLevel/ArenaBoundsActivator").AddComponent<SimpleTriggerZoneSwitchScript>();
                    arenaZone.enableOnEnter = true;
                    arenaZone.enableOnExit = true;
                    arenaZone.gameObject.AddComponent<SwitchScript>();

                    InputSetActiveScript arenaSwitch = new GameObject().AddComponent<InputSetActiveScript>();
                    arenaSwitch.activatableObjects = new GameObject[] { GameObject.Find("MFPLevel/Cube") };

                    arenaSwitch.inputSwitch = arenaZone.GetSwitch();


                    AutoControlZoneScript flyZone = RuntimeEntities.GenerateAutoControlZone(GameObject.Find("MFPLevel/AutoControlFly"));

                    flyZone.jump = true;
                    flyZone.enableOverrideOnEnter = true;
                    flyZone.setAim = true;
                    flyZone.aimPos = new Vector3(15, 0, 0);



                    FinalBattleController finalBattle = new GameObject().AddComponent<FinalBattleController>();
                    finalBattle.startSwitch = arenaZone.GetSwitch();

                    new GameObject().AddComponent<CheckpointScript>().triggerFromSwitch = arenaZone.GetSwitch();


                    GameObject.FindObjectOfType<CameraScript>().gameObject.AddComponent<SimpleTrackPlayer>();


                    isBossLevel = true;

                    //CHECKPOINT İÇİN AUTOCONTROLÜN HEMEN EN SONUNA TRİGGER KOY 


                    break;
            }


            List<Transform> enemiesRoot = new List<Transform>();

            foreach (EnemyScript enemy in enemies)
                enemiesRoot.Add(enemy.transform);

            MFPClassicAssets.root.allEnemies = enemiesRoot.ToArray();

            MFPEditorUtils.Log("Map loaded");


        }

    }
}
