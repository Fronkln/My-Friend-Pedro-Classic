using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using I2.Loc;
using System.Reflection;


namespace MFPClassic
{

     class MFPEditorGUILogging : MonoBehaviour
    {
        public List<string> logs = new List<string>();

        private GUIStyle redStyle;


        public void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        public void OnGUI()
        {
            foreach (string log in logs)
                GUILayout.Label(log);
        }

        public void GUILog(string txt)
        {
            if(logs.Count > 10)
               logs.RemoveAt(0);

            logs.Add(txt);
        }
    }


    public static class MFPEditorUtils
    {
        private static MFPEditorGUILogging guiInstance;

        public static Texture2D LoadPNG(string filePath)
        {
            Texture2D texture2D = null;
            if (File.Exists(filePath))
            {
                byte[] data = File.ReadAllBytes(filePath);
                texture2D = new Texture2D(2, 2);    

                texture2D.LoadImage(data);
            }
            return texture2D;
        }

        public static string LoadFile(string file)
        {
            return Application.dataPath + "/MFPClassic_Jhrino/" + file;
        }

        public static void ClearLog()
        {
            File.WriteAllLines(LoadFile("mfpclassic_log.txt"), new string[1] { string.Empty });
        }

        public static void InitGUILogging()
        {
            if(guiInstance == null)
            guiInstance = new GameObject().AddComponent<MFPEditorGUILogging>();
        }

        public static void Log(string text)
        {
            Debug.Log("[MFPEDITORUTILS]:" + text);
            File.AppendAllText(LoadFile("mfpclassic_log.txt"), Environment.NewLine + text);

            if (guiInstance != null)
                guiInstance.GUILog(text);
        }

        public static void doPedroHint(string txt, float timer = -99999)
        {
            RootScript root = GameObject.Find("Root").GetComponent<RootScript>();

            if (root != null)
            {
                root.pedroHintTimer = timer;
                root.StartCoroutine(root.doPedroHint(txt));
            }
        }

        public static void DebugJSONObject(object obj)
        {
            string file = LoadFile("JSONOUTPUT/" + obj.ToString() + "_out.txt");

            if (!File.Exists(file))
                File.Create(file).Close();

            File.WriteAllLines(file, new string[1] { JsonUtility.ToJson(obj, true) });
        }


        public static void FixPlayerLoadout()
        {
            switch (MapManager.currentLevel)
            {
                case 1:
                    for (int i = 2; i <= 10; i++)
                    {
                        MFPClassicAssets.player.weaponActive[i] = false;
                        MFPClassicAssets.player.changeWeapon(1);
                    }
                    break;
                case 2:
                    if (!MFPClassicAssets.player.weaponActive[2])
                        MFPClassicAssets.player.weaponActive[2] = true;
                    break;

            }

            GameObject.FindObjectOfType<UIWeaponSelectorScript>().prepareUI();
        }

        public static GameObject SpawnPedro(Vector3 position, string name = "Pedro")
        {
            GameObject pedro = GameObject.Instantiate(MFPClassicAssets.pedroSample);
            pedro.transform.position = position + new Vector3(3 ,0, 0);
            pedro.SetActive(true);
            pedro.name = name;

            return pedro;
        }

        public static void CreateChairEnemy(GameObject chair, EnemyScript enemy, bool faceRight, bool fallOver = true)
        {
            EnemyChairScript chairScript = chair.AddComponent<EnemyChairScript>();
            chairScript.targetEnemy = enemy;

            enemy.standStill = true;
            enemy.faceRight = faceRight;
            enemy.transform.position = chairScript.gameObject.transform.position + new Vector3(-1, 0, 0);

            chairScript.fallOver = fallOver;

            MFPEditorUtils.Log("Created chair enemy from: " + enemy.transform.name);
        }

        public static SpawnDoorScript CreateEnemyDoor(Vector3 position, int enemyCount)
        {
            GameObject newDoor = GameObject.Instantiate(MFPClassicAssets.doorSpawnerSample);
            newDoor.GetComponentInChildren<SpawnDoorScript>().nrOfEnemies = enemyCount;
            newDoor.transform.position = position;

            newDoor.SetActive(true);

            return newDoor.GetComponentInChildren<SpawnDoorScript>();
        }

        public static SpeechTriggerControllerScript GenerateSpeechScript(GameObject target, Transform followTarget, string speakerName, string locStringID, AudioClip voice, bool freezePlayer, GameObject forceSpawnPedro = null)
        {

            SwitchScript switchScript = target.AddComponent<SwitchScript>();
            switchScript.output = 1;

            SpeechTriggerScript speechTriggerScript = target.AddComponent<SpeechTriggerScript>();

            if (freezePlayer)
            {
                speechTriggerScript.clickToContinue = true;
                speechTriggerScript.clickToContinueDontFreeze = false;
            }

            speechTriggerScript.followTransform = followTarget;

            speechTriggerScript.speakerName = speakerName;

            if (forceSpawnPedro != null)
                speechTriggerScript.forceSpawnPedro = forceSpawnPedro.GetComponent<PedroScript>();

            string testString = "";

            LocalizationManager.TryGetTranslation(speakerName, out testString);

            if (testString.ToLower().Contains("missing translation"))
                CreateTranslation(speakerName, speakerName);

            speechTriggerScript.activateSwitchScript = true;
            speechTriggerScript.voice = voice;

            speechTriggerScript.locStringId = locStringID;

            SpeechTriggerControllerScript controllerScript = target.AddComponent<SpeechTriggerControllerScript>();

            controllerScript.inputSwitch = new SwitchScript[0];

            return controllerScript;

        }

        public static void CreateTranslation(string termName, string translation)
        {
            string language = LocalizationManager.CurrentLanguage;

            var i2languagesPrefab = LocalizationManager.Sources[0];
            var termData = i2languagesPrefab.AddTerm(termName, eTermType.Text);

            // Find Language Index (or add the language if its a new one)
            int langIndex = i2languagesPrefab.GetLanguageIndex(language, false, false);
            if (langIndex < 0)
            {
                i2languagesPrefab.AddLanguage(language, GoogleLanguages.GetLanguageCode(language));
                langIndex = i2languagesPrefab.GetLanguageIndex(language, false, false);
            }

            termData.Languages[langIndex] = translation;
        }

        public static string NormalizeInputFieldIntValue(string value, int min, int max)
        {
            int valConverted = int.Parse(value);

            if (valConverted < min)
                return min.ToString();
            if (valConverted > max)
                return max.ToString();

            return value;
        }
    }
}