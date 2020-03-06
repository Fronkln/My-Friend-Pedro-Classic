using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MFPClassic
{
    public class MapManager : MonoBehaviour
    {
        public static int currentLevel = 1;
        public List<EnemyScript> enemies = new List<EnemyScript>();


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
                    {
                        obj.gameObject.AddComponent<CustomZPusher>().pushIntensity = int.Parse(obj.transform.name.Replace("Player Z-Pusher=", ""));
                        MFPEditorUtils.Log("Z Pusher set");
                    }
                }
            }

                if (GameObject.Find("MFPLevel/BreakableGlass") != null)
                foreach(Transform glass in GameObject.Find("MFPLevel/BreakableGlass").GetComponentsInChildren<Transform>())
                    if(glass.name != "BreakableGlass")
                    {
                        if (glass.name.Contains("glass_piece"))
                        {
                            glass.gameObject.AddComponent<GlassPieceScript>();
                            glass.gameObject.AddComponent<RigidBodySlowMotion>();
                        }

                        if(glass.name == "glass_window_large")
                            glass.gameObject.AddComponent<GlassWindowScript>().glassSounds = new AudioClip[] { MFPClassicAssets.weaponSound[0], MFPClassicAssets.weaponSound[0] };
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

                newEnemy.name = obj.name;
            }

            MFPEditorUtils.Log("Enemies spawned");

            PrepareLevelEntities();

            yield return null;
        }


        void Awake()
        {
            MFPEditorUtils.Log("MapManager awake, will load level ID: " + currentLevel.ToString());
            GameObject.Find("Main Camera").GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>().enabled = false;

            // SceneManager.LoadScene(MFPClassicAssets.classicLevelsBundle.GetAllScenePaths()[0], LoadSceneMode.Additive);
            GameObject map = GameObject.Instantiate(MFPClassicAssets.classicBundle.LoadAsset("MFPLevel" + currentLevel.ToString()) as GameObject);
            map.name = "MFPLevel";

            map.transform.position = new Vector3(9999, 9999, map.transform.position.z);


            RenderSettings.ambientIntensity = 0;
            RenderSettings.ambientEquatorColor = Color.black;
            RenderSettings.ambientGroundColor = Color.black;
            RenderSettings.ambientLight = Color.black;
            RenderSettings.ambientSkyColor = Color.black;


            MFPClassicAssets.player.transform.position = new Vector3(9999, 9999, 0);
            MFPClassicAssets.player.ySpeed = (float)1;

            MFPClassicAssets.player.transform.position = GameObject.Find("MFPLevel/player_spawn").transform.position;

            MFPEditorUtils.Log(map.name);

            StartCoroutine(WaitLevelLoad());
        }


#if DEBUG

        void OnGUI()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
                GUILayout.Button(hit.transform.name);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                UnityEngine.PostProcessing.PostProcessingBehaviour behaviour = GameObject.Find("Main Camera").GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>();

                if (behaviour.enabled)
                    behaviour.enabled = false;
                else
                    behaviour.enabled = true;
            }

            if (Input.GetKeyDown(KeyCode.O))
                foreach (Light l in GameObject.Find("MFPLevel").GetComponentsInChildren<Light>())
                    l.range -= 10;

           

            if (Input.GetKeyDown(KeyCode.I))
                GameObject.FindObjectOfType<LevelChangerScript>().doTheThing();
        }
#endif


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

                    PlayerScript playerScript = MFPClassicAssets.player;

                    lScript.dontShowEndScreen = true;

                    SpawnDoorScript door1 = MFPEditorUtils.CreateEnemyDoor(GameObject.Find("MFPLevel/Door1").transform.position, 1);
                    door1.GetComponent<BoxCollider>().enabled = false;

                    GameObject speech_Level1Trigger1 = GameObject.Find("MFPLevel/SpeechTriggers/trigger1");
                    GameObject speech_Level1Trigger2 = GameObject.Find("MFPLevel/SpeechTriggers/trigger2");
                    GameObject speech_Level1Trigger3 = GameObject.Find("MFPLevel/SpeechTriggers/trigger3");
                    GameObject speech_Level1Trigger4 = GameObject.Find("MFPLevel/SpeechTriggers/trigger4");
                    GameObject speech_Level1Trigger5 = GameObject.Find("MFPLevel/SpeechTriggers/trigger5");
                    GameObject speech_Level1Trigger6 = GameObject.Find("MFPLevel/SpeechTriggers/trigger6");

                    GameObject speech_Level1Pedro = MFPEditorUtils.SpawnPedro(GameObject.Find("MFPLevel/Pedro").transform.position);
                    GameObject speech_Level1Pedro2 = MFPEditorUtils.SpawnPedro(GameObject.Find("MFPLevel/Pedro2").transform.position);
                    GameObject speech_Level1Pedro3 = MFPEditorUtils.SpawnPedro(GameObject.Find("MFPLevel/Pedro3").transform.position);
                    GameObject speech_Level1Pedro4 = MFPEditorUtils.SpawnPedro(GameObject.Find("MFPLevel/Pedro4").transform.position);
                    GameObject speech_Level1Pedro5 = MFPEditorUtils.SpawnPedro(GameObject.Find("MFPLevel/Pedro5").transform.position);
                    GameObject speech_Level1Pedro6 = MFPEditorUtils.SpawnPedro(GameObject.Find("MFPLevel/Pedro6").transform.position);

                    MFPEditorUtils.GenerateSpeechScript(speech_Level1Trigger1, speech_Level1Pedro.transform, "Pedro", "pedro01_level1_MFPClassic", MFPClassicAssets.pedroSounds[UnityEngine.Random.Range(0, MFPClassicAssets.pedroSounds.Length)], true, speech_Level1Pedro);
                    speech_Level1Trigger1.GetComponent<SpeechTriggerControllerScript>().triggerWithoutPlayer = true;

                    MFPEditorUtils.GenerateSpeechScript(speech_Level1Trigger2, speech_Level1Pedro2.transform, "Pedro", "pedro02_level1_MFPClassic", MFPClassicAssets.pedroSounds[UnityEngine.Random.Range(0, MFPClassicAssets.pedroSounds.Length)], true, speech_Level1Pedro2);
                    SpeechTriggerControllerScript speechScript3_lv1 = MFPEditorUtils.GenerateSpeechScript(speech_Level1Trigger3, speech_Level1Pedro3.transform, "Pedro", "pedro03_level1_MFPClassic", MFPClassicAssets.pedroSounds[UnityEngine.Random.Range(0, MFPClassicAssets.pedroSounds.Length)], true, speech_Level1Pedro3);

                    door1.inputSwitch = new SwitchScript[] { speechScript3_lv1.GetComponent<SwitchScript>() };
                    door1.standStill = true;

                    MFPEditorUtils.GenerateSpeechScript(speech_Level1Trigger4, speech_Level1Pedro4.transform, "Pedro", "pedro04_level1_MFPClassic", MFPClassicAssets.pedroSounds[UnityEngine.Random.Range(0, MFPClassicAssets.pedroSounds.Length)], true, speech_Level1Pedro4);
                    MFPEditorUtils.GenerateSpeechScript(speech_Level1Trigger5, speech_Level1Pedro5.transform, "Pedro", "pedro05_level1_MFPClassic", MFPClassicAssets.pedroSounds[UnityEngine.Random.Range(0, MFPClassicAssets.pedroSounds.Length)], true, speech_Level1Pedro5);
                    MFPEditorUtils.GenerateSpeechScript(speech_Level1Trigger6, speech_Level1Pedro6.transform, "Pedro", "pedro06_level1_MFPClassic", MFPClassicAssets.pedroSounds[UnityEngine.Random.Range(0, MFPClassicAssets.pedroSounds.Length)], true, speech_Level1Pedro6);
                    break;

                case 2:

                    GameObject.FindObjectOfType<LevelChangerScript>().dontShowEndScreen = false;

                    GameObject speech_Level2Trigger1 = GameObject.Find("MFPLevel/SpeechTriggers/trigger1");
                    GameObject speech_Level2Pedro1 = MFPEditorUtils.SpawnPedro(GameObject.Find("MFPLevel/Pedro").transform.position);

                    MFPClassicAssets.root.nrOfEnemiesTotal = enemies.Count / 2 - 6;

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

                    MFPEditorUtils.CreateChairEnemy(GameObject.Find("MFPLevel/WoodenChair/WoodenChair_0 (3)"), enemies[2], false);
                    MFPEditorUtils.CreateChairEnemy(GameObject.Find("MFPLevel/WoodenChair/WoodenChair_0 (1)"), enemies[4], false);

                    MFPEditorUtils.GenerateSpeechScript(speech_Level2Trigger1, speech_Level2Pedro1.transform, "Pedro", "pedro01_level2_MFPClassic", MFPClassicAssets.pedroSounds[UnityEngine.Random.Range(0, MFPClassicAssets.pedroSounds.Length)], true, speech_Level2Pedro1);
                    break;


                case 3:
                    SpawnDoorScript door1_level3 = MFPEditorUtils.CreateEnemyDoor(GameObject.Find("MFPLevel/DoorSpawns/Door1").transform.position, 5);
                    SpawnDoorScript door2_level3 = MFPEditorUtils.CreateEnemyDoor(GameObject.Find("MFPLevel/DoorSpawns/Door2").transform.position, 5);

                    door1_level3.attackAfterDoorSpawn = true;

                    door2_level3.inputSwitch = new SwitchScript[] { door1_level3.GetSwitch() };
                    door2_level3.attackAfterDoorSpawn = true;

                    ElevatorScript elevatorTrigger = GameObject.Find("MFPLevel/ElevatorEntranceTrigger").gameObject.AddComponent<ElevatorScript>();
                    elevatorTrigger.spawnDoors = new SpawnDoorScript[] { door1_level3, door2_level3 };

                    SwitchABMoveScript elevator = GameObject.Find("MFPLevel/Elevator").AddComponent<SwitchABMoveScript>();

                    elevator.gameObject.AddComponent<SwitchScript>();

                    elevator.outputOnReachedEnd = true;
                    elevator.gameObject.transform.parent = null;


                    elevator.inputSwitch = new SwitchScript[] { elevatorTrigger.GetSwitch()};
                    elevatorTrigger.inputSwitch = elevator.GetSwitch();

                    elevator.movePos = new Vector3(0, -11.54f, 0);

                    elevator.moveSpeed = 0.005552662f;
                    elevator.acceleration = 0.1f;

                    elevator.animLengthInFrames = 180;


                    enemies[0].standStill = true;
                    enemies[0].standStillInHuntMode = true;
                    enemies[0].weapon = 3;
                    enemies[0].faceRight = false;

                    enemies[1].standStill = true;
                    enemies[1].faceRight = false;

                    break;
            }


            List<Transform> enemiesRoot = new List<Transform>();

            foreach (EnemyScript enemy in enemies)
                enemiesRoot.Add(enemy.transform);

            MFPClassicAssets.root.allEnemies = enemiesRoot.ToArray();


        }

    }
}
