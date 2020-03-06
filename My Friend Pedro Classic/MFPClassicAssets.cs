using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

namespace MFPClassic
{
    public static class MFPClassicAssets
    {
        public static AssetBundle classicBundle;

        public static AudioClip[] weaponSound;
        public static AudioClip[] pedroSounds;


        public static Texture2D wall_textures_1;

        public static PlayerScript player
        {
            get { return GameObject.FindObjectOfType<PlayerScript>(); }
        }

        public static RootScript root;

        public static GameObject enemySample;
        public static GameObject doorSpawnerSample;

        public static GameObject pedroSample;

        public static RectTransform healthBar2HUDRect;
        public static Image healthBar2HUD;

        public static Image slowMoIcon;

        public static Sprite hudHealthSquare;

        public static Color32 hudOrange = new Color32(255, 94, 26, 255);
        public static Color32 hudLightOrange = new Color32(249, 140, 15, 255);

        public static Font preAlphaFont;
        public static PostProcessingProfile classicGraphics;


        public static void LoadAssets()
        {
            pedroSounds = new AudioClip[4];

            for(int i = 1; i <= 4; i++)
                pedroSounds[i - 1] = classicBundle.LoadAsset("gibberish_1_" + i.ToString()) as AudioClip; 

            GameObject workaroundSQR = classicBundle.LoadAsset("workaroundimg") as GameObject;
            hudHealthSquare = workaroundSQR.GetComponent<Image>().sprite;

            wall_textures_1 = classicBundle.LoadAsset("wall_textures_1") as Texture2D;

            preAlphaFont = classicBundle.LoadAsset("SpecialElite") as Font;

            classicGraphics = classicBundle.LoadAsset("ClassicProfile") as PostProcessingProfile;

            if (pedroSounds[3] == null)
                Application.Quit();
        }

        public static void LoadGunSounds(AudioClip[] original)
        {
            MFPEditorUtils.Log("Start gun sound load");

            if (weaponSound != null)
                return;

            AudioClip pistol1_alt_mixdown = classicBundle.LoadAsset("Pistol_alt1_mixdown") as AudioClip;
           // AudioClip Submachine_gun_mixdown = classicBundle.LoadAsset("Submachine gun_mixdown") as AudioClip;

            MFPEditorUtils.Log("Pistol and uzi mixdown loaded");

            Array.Copy(original, weaponSound = new AudioClip[original.Length], original.Length);

            MFPEditorUtils.Log("Array copied");

            weaponSound[0] = pistol1_alt_mixdown;
            weaponSound[1] = pistol1_alt_mixdown;
            weaponSound[2] = pistol1_alt_mixdown;

          //  weaponSound[3] = Submachine_gun_mixdown;
          //  weaponSound[4] = Submachine_gun_mixdown;


            MFPEditorUtils.Log("Sounds set");
        }
    }
}
