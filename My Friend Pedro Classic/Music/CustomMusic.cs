using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MFPClassic
{
    public class CustomMusic : MonoBehaviour
    {
        AudioSource music;
        public bool introOverride = false;
        public bool intro = true;
        public MusicDefinition musicData;


        public void Awake()
        {
            music = new GameObject().AddComponent<AudioSource>();
            music.loop = false;
            music.spatialBlend = 0;

            music.outputAudioMixerGroup = GameObject.Find("Root/Music").GetComponentInChildren<AudioSource>().outputAudioMixerGroup;
            GameObject.Find("Root/Music").SetActive(false);

            MFPEditorUtils.Log(MapManager.currentLevel.ToString());

            musicData = new MusicDefinition();

            switch (MapManager.currentLevel)
            {
                case 1:
                    musicData.loop = MFPClassicAssets.classicBundle.LoadAsset("tutorial_music_1") as AudioClip;
                    break;
                case 2:
                    introOverride = true;

                    musicData.intro = MFPClassicAssets.classicBundle.LoadAsset("music1Intro") as AudioClip;
                    musicData.loop = MFPClassicAssets.classicBundle.LoadAsset("music1") as AudioClip;
                    break;
                case 3:
                    musicData.loop = MFPClassicAssets.classicBundle.LoadAsset("music2") as AudioClip;
                    break;
                case 4:
                    introOverride = true;

                    musicData.intro = MFPClassicAssets.classicBundle.LoadAsset("music3Intro") as AudioClip;
                    musicData.loop = MFPClassicAssets.classicBundle.LoadAsset("music3") as AudioClip;
                    break;
                case 5:
                    musicData.loop = MFPClassicAssets.classicBundle.LoadAsset("music4") as AudioClip;
                    break;
                case 6:
                    musicData.loop = MFPClassicAssets.classicBundle.LoadAsset("music5") as AudioClip;
                    break;
                case 8:
                    musicData.loop = MFPClassicAssets.classicBundle.LoadAsset("bossLoop") as AudioClip;
                    break;
            }

            if (musicData.intro != null || musicData.loop != null)
                MFPEditorUtils.Log("Custom music loaded");

            if (musicData.intro != null)
            {
                music.clip = musicData.intro;

                if (introOverride)
                    music.loop = true;

                music.Play();
            }
            else
                SwitchtoActionMusic();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (introOverride)
                if (other.gameObject.transform.name == "Player")
                    SwitchtoActionMusic();
        }


        public void SwitchtoActionMusic()
        {
            music.clip = musicData.loop;
            music.Play();
            music.loop = true;

            Destroy(this.gameObject);
        }

        public void Update()
        {
            if (intro)
                if (!music.isPlaying && !introOverride)
                {
                    music.clip = musicData.loop;
                    music.Play();
                    music.loop = true;

                    Destroy(this.gameObject);
                }
        }
    }
}
