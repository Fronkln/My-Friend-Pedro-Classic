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

    class MFPEditorDebuggerRuntime : MonoBehaviour
    {
        public List<string> logs = new List<string>();

        private GUIStyle redStyle;


        public void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        public void OnGUI()
        {
            if (logs == null) logs = new List<string>();


            if (logs.Count > 10)
                logs.RemoveAt(0);

            foreach (string log in logs)
                if (log != null)
                    GUILayout.Label(log);

            if (Camera.main != null)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform != null)
                    {
                        GUILayout.Button(hit.transform.name);
                        if (Input.GetKeyDown(KeyCode.U))
                        {

                            SwitchABMoveScript carScript = GameObject.Find("Car_3 (5)").GetComponent<SwitchABMoveScript>();

                            if (carScript.inputSwitch.Length != 0)
                                MFPEditorUtils.Log(carScript.inputSwitch[0].gameObject.name);

                            MFPEditorUtils.Log(carScript.invert.ToString());
                            MFPEditorUtils.Log(carScript.moveOffset.ToString());
                            MFPEditorUtils.Log(carScript.movePos.ToString());
                            MFPEditorUtils.Log(carScript.moveSpeed.ToString());
                            MFPEditorUtils.Log(carScript.noInputSwitchAnimPause.ToString());
                            MFPEditorUtils.Log(carScript.onOffSwitchBehaviour.ToString());
                            MFPEditorUtils.Log(carScript.outputOnMovement.ToString());
                            MFPEditorUtils.Log(carScript.outputOnReachedEnd.ToString());
                            MFPEditorUtils.Log(carScript.returnAnimLengthInFrames.ToString());
                            MFPEditorUtils.Log(carScript.scaleSpeedDependingOnPlayerSpeedGameModifier.ToString());
                            MFPEditorUtils.Log(carScript.useLocalPos.ToString());


                            MFPEditorUtils.Log("------------");

                            foreach (Component comp in hit.transform.gameObject.GetComponents<Component>())
                            {
                                MFPEditorUtils.Log(comp.ToString() + " " + comp.gameObject.name);
                                ExtensiveLogging.Log(comp);
                                MFPEditorUtils.Log("------------");
                            }

                            foreach (Component comp in hit.transform.gameObject.GetComponentsInChildren<Component>())
                            {
                                MFPEditorUtils.Log(comp.ToString() + " " + comp.gameObject.name);
                                ExtensiveLogging.Log(comp);
                                MFPEditorUtils.Log("------------");
                            }
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                MFPClassicAssets.player.transform.position = new Vector3(MFPClassicAssets.player.mousePos.x, MFPClassicAssets.player.mousePos.y, 0.0f);
                MFPClassicAssets.player.ySpeed = 1f;
            }
        }

        public void GUILog(string txt)
        {
            logs.Add(txt);
        }
    }


    public static class MFPEditorUtils
    {
        private static MFPEditorDebuggerRuntime guiInstance;

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
            if (guiInstance == null)
                guiInstance = new GameObject().AddComponent<MFPEditorDebuggerRuntime>();
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

        public static void UnlockWeapons(int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                MFPClassicAssets.player.weaponActive[i] = true;
                MFPClassicAssets.player.ammo[i] = MFPClassicAssets.player.ammoFullClip[i];
                MFPClassicAssets.player.ammoTotal[i] = MFPClassicAssets.player.ammoTotal[i] + MFPClassicAssets.player.ammoFullClip[i] * 2f;
            }
        }


        public static void FixPlayerLoadout()
        {

            if (MFPClassicAssets.rootShared != null && MFPClassicAssets.rootShared.modAllWeapons)
                return;

#if !DEBUG
            if (MFPClassicAssets.rootShared.levelLoadedFromLevelSelectScreen)
                for (int i = 2; i <= 10; i++)
                    MFPClassicAssets.player.weaponActive[i] = false;
#endif


            MFPClassicAssets.player.weaponActive[0] = true;


            if (MapManager.currentLevel != 1 && MFPClassicAssets.rootShared.levelLoadedFromLevelSelectScreen)
                for (int i = 1; i <= 10; i++)
                {
                    MFPClassicAssets.player.ammoTotal[i] = (int)MFPClassicAssets.root.ammoMax[i] / 12;
                }

            switch (MapManager.currentLevel)
            {
                case 1:

                    if (!MFPClassicAssets.rootShared.modAllWeapons)
                        for (int i = 1; i <= 10; i++)
                        {
                            MFPClassicAssets.player.ammoTotal[i] = (int)MFPClassicAssets.root.ammoMax[i] / 12;
                            if (i != 1)
                                MFPClassicAssets.player.weaponActive[i] = false;
                        }


                    MFPClassicAssets.player.weaponActive[1] = true;
                    MFPClassicAssets.player.changeWeapon(1);
                    break;
                case 2:
                    MFPClassicAssets.player.weaponActive[2] = true;

                    if (MFPClassicAssets.rootShared.levelLoadedFromLevelSelectScreen)
                        MFPClassicAssets.player.changeWeapon(2);
                    break;
                case 3:
                    goto case 2;
                case 4:
                    if (MFPClassicAssets.rootShared.levelLoadedFromLevelSelectScreen)
                    {
                        UnlockWeapons(1, 4);
                        MFPClassicAssets.player.changeWeapon(4);
                    }

                    break;
                case 5:
                    if (MFPClassicAssets.rootShared.levelLoadedFromLevelSelectScreen)
                    {
                        UnlockWeapons(1, 5);
                        MFPClassicAssets.player.changeWeapon(5);
                    }
                    break;
                case 6:
                    if (MFPClassicAssets.rootShared.levelLoadedFromLevelSelectScreen)
                    {
                        UnlockWeapons(1, 6);
                        MFPClassicAssets.player.changeWeapon(6);
                    }
                    break;

                case 8:
                    if (MFPClassicAssets.rootShared.levelLoadedFromLevelSelectScreen)
                    {
                        UnlockWeapons(1, 6);
                        MFPClassicAssets.player.changeWeapon(6);
                    }
                    break;
            }

            GameObject.FindObjectOfType<UIWeaponSelectorScript>().prepareUI();
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