using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;
using TMPro;

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
        public static RootSharedScript rootShared;

        public static GameObject enemySample;
        public static GameObject medkit;

        public static GameObject openableDoor;
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

        public static Material character_head_test_bald;


        public static GameObject m4Gun;
        public static Sprite m4Sprite;


        //Boss Fight
        public static GameObject miniVictor;
        public static GameObject WeaponPickerSample;

        //EnemyAssets

        public static Mesh torsorBlacklongSleeve;

        public static Material colorablePants;
        public static Material hands02;

        public static Mesh head02NoBandanaNoBeanie;
        public static GameObject head02Bandana;
        public static GameObject head02Beanie;
        public static GameObject head02Cap;

        public static Material head02Black;

        public static Material blackEnemyWhiteTop;

        public static Material whiteEnemyBlackShirt, blackEnemyBlackShirt;


        public static TextMeshProUGUI secondaryAmmoText;

        public static void LoadAssets()
        {
            pedroSounds = new AudioClip[4];

            for(int i = 1; i <= 4; i++)
                pedroSounds[i - 1] = classicBundle.LoadAsset("gibberish_1_" + i.ToString()) as AudioClip; 

            GameObject workaroundSQR = classicBundle.LoadAsset("workaroundimg") as GameObject;
            hudHealthSquare = workaroundSQR.GetComponent<Image>().sprite;

            wall_textures_1 = classicBundle.LoadAsset("wall_textures_1") as Texture2D;

            preAlphaFont = classicBundle.LoadAsset("Harting_plain") as Font;

            classicGraphics = classicBundle.LoadAsset("ClassicProfile") as PostProcessingProfile;

            torsorBlacklongSleeve = classicBundle.LoadAsset("TorsorBlackLongSleeve") as Mesh;

            m4Gun = classicBundle.LoadAsset("m4") as GameObject;

            Texture2D m4IcoTex = classicBundle.LoadAsset("m4_ico_2") as Texture2D;
            m4Sprite = Sprite.Create(m4IcoTex, new Rect(0.0f, 0.0f, m4IcoTex.width, m4IcoTex.height), new Vector2(0.5f, 0.5f), 100.0f);


            miniVictor = classicBundle.LoadAsset("miniVictor") as GameObject;
            miniVictor.AddComponent<MiniVictor>();

            colorablePants = classicBundle.LoadAsset("legs_01_color") as Material;
            hands02 = classicBundle.LoadAsset("hands_02") as Material;

            head02NoBandanaNoBeanie = classicBundle.LoadAsset("Head03") as Mesh;
            head02Beanie = classicBundle.LoadAsset("head02Beanie") as GameObject;
            head02Bandana = classicBundle.LoadAsset("head02Bandana") as GameObject;
            head02Cap = classicBundle.LoadAsset("head02Cap") as GameObject;

            head02Black = classicBundle.LoadAsset("blackman_enemy_head_01") as Material;

            blackEnemyWhiteTop = classicBundle.LoadAsset("blackman_white_tanktop_texture") as Material;

            whiteEnemyBlackShirt = classicBundle.LoadAsset("black_shirt_texture") as Material;
            blackEnemyBlackShirt = classicBundle.LoadAsset("blackman_shirt_texture") as Material;

            character_head_test_bald = classicBundle.LoadAsset("character_head_01_TEST") as Material;

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
