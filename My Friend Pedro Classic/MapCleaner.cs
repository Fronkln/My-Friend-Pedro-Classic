using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
#if DEBUG
using System.IO;
#endif

namespace MFPClassic
{
    public static class MapCleaner
    {
        private static void PreCleanup()
        {
            GameObject enemySample = GameObject.Instantiate(UnityEngine.Object.Instantiate<GameObject>(UnityEngine.Object.FindObjectOfType<SpawnDoorScript>().enemy));
            enemySample.layer = 13;
            enemySample.name = "New Enemy";
            enemySample.transform.position = MFPClassicAssets.player.transform.position;
            enemySample.SetActive(true);
            MFPClassicAssets.enemySample = GameObject.Find("New Enemy");

            MFPClassicAssets.medkit = enemySample.GetComponent<EnemyScript>().medkit;

            MFPClassicAssets.WeaponPickerSample = enemySample.transform.Find("EnemyGraphics/Armature/Center/LowerBack/UpperBack/Shoulder_R/UpperArm_R/LowerArm_R/Hand_R/pistol").gameObject;

            MFPClassicAssets.enemySample.SetActive(false);

            MFPClassicAssets.doorSpawnerSample = GameObject.FindObjectOfType<SpawnDoorScript>().gameObject;

            MFPClassicAssets.doorSpawnerSample = GameObject.Instantiate(GameObject.FindObjectOfType<SpawnDoorScript>().transform.parent.gameObject);
            MFPClassicAssets.doorSpawnerSample.GetComponentInChildren<BoxCollider>().center = new Vector3(0, 0, 25f);
            MFPClassicAssets.doorSpawnerSample.GetComponentInChildren<BoxCollider>().size = new Vector3(2, 10.5f, 10);
            MFPClassicAssets.doorSpawnerSample.SetActive(false);

            MFPClassicAssets.openableDoor = MFPClassicAssets.classicBundle.LoadAsset("OpenableDoor") as GameObject;

            MFPEditorUtils.Log("Map cleanup warmup done");
        }   

        public static void Clean()
        {
            PreCleanup();


            string[] objectblacklist = new string[]
            {
                    "glass_window_frame_large",
                    "Dust Particles",
                    "RagdollColl",
                    "CornerBeam",
                    "WallBuilder",
                    "FloorBuilder",
                    "Table",
                    "fence",
                    "Pipe",
                    "Crate,",
                    "Chair",
                    "TV",
                    "Corner",
                    "Steel_",
                    "Metal_",
                    "Quad",
                    "Quad(1)"
            };



            foreach (GameObject gameObject in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[])
            {
                if (gameObject.name.Contains("Enemy") && !gameObject.name.Contains("Graphics") && !gameObject.name.Contains("New"))
                    gameObject.SetActive(false);

                if (gameObject.layer == 14 || gameObject.layer == 15 || gameObject.layer == 16 || gameObject.layer == 19 || gameObject.layer == 8 || gameObject.layer == 1 || gameObject.layer == 18)
                {
                    if (gameObject.layer != 16)
                        UnityEngine.Object.Destroy(gameObject);
                    else
                        UnityEngine.Object.Destroy(gameObject.transform.root.gameObject);
                }

                #region Specific Name Checks If Chain Hell
                if (gameObject.name.Contains("glass_window_frame_large"))
                    GameObject.Destroy(gameObject);

                if (gameObject.name.Contains("Dust Particles"))
                    GameObject.Destroy(gameObject);

                if (gameObject.name.Contains("RagdollColl"))
                    GameObject.Destroy(gameObject);

                if (gameObject.name.Contains("CornerBeam"))
                    GameObject.Destroy(gameObject);

                if (gameObject.name.Contains("Quad"))
                    GameObject.Destroy(gameObject);

                if (gameObject.name.Contains("FloorBuilder"))
                    GameObject.Destroy(gameObject);

                if (gameObject.name.Contains("WallBuilder"))
                    GameObject.Destroy(gameObject);

                if (gameObject.name.Contains("TV"))
                    GameObject.Destroy(gameObject);
                #endregion
            }

            MFPEditorUtils.Log("Map cleanup done");

            /*
#if DEBUG
            if (!File.Exists(MFPEditorUtils.LoadFile("mapcleaner_log.txt")))
                File.Create(MFPEditorUtils.LoadFile("mapcleaner_log.txt"));

            GameObject[] remainingObjects = GameObject.FindObjectsOfType<GameObject>();

            List<string> remainingObjectsStr = new List<string>();

            foreach (GameObject obj in remainingObjects)
                if (obj.transform.parent == null)
                    remainingObjectsStr.Add(obj.name);

            File.WriteAllLines(MFPEditorUtils.LoadFile("mapcleaner_log.txt"), remainingObjectsStr.ToArray());

            
#endif
*/
        }
    }
}
