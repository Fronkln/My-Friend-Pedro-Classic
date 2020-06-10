using System;
using UnityEngine;
using MFPClassic;

// Token: 0x020000DD RID: 221
[Serializable]
public class VoiceControllerScript : MonoBehaviour
{

    public bool shouldSpeak = true;

    // Token: 0x06000606 RID: 1542
    public VoiceControllerScript()
    {
        this.voicePitchRange = 1f;
        this.voicePitchWobbleMultiplier = 1f;
    }

    // Token: 0x06000607 RID: 1543
    public virtual void saveState()
    {
        this.voiceS = this.voice;
        this.voiceTimerS = this.voiceTimer;
        this.voicePitchOffsetS = this.voicePitchOffset;
        this.voicePitchRangeS = this.voicePitchRange;
        this.voicePitchWobbleMultiplierS = this.voicePitchWobbleMultiplier;
        this.voiceTargetPitchS = this.voiceTargetPitch;
        this.voiceTargetVolumeS = this.voiceTargetVolume;
        this.curVoiceCharS = this.curVoiceChar;
        this.voiceLengthS = this.voiceLength;
        this.isQuestionS = this.isQuestion;
    }

    // Token: 0x06000608 RID: 1544
    public virtual void loadState()
    {
        this.voice = this.voiceS;
        this.voiceTimer = this.voiceTimerS;
        this.voicePitchOffset = this.voicePitchOffsetS;
        this.voicePitchRange = this.voicePitchRangeS;
        this.voicePitchWobbleMultiplier = this.voicePitchWobbleMultiplierS;
        this.voiceTargetPitch = this.voiceTargetPitchS;
        this.voiceTargetVolume = this.voiceTargetVolumeS;
        this.curVoiceChar = this.curVoiceCharS;
        this.voiceLength = this.voiceLengthS;
        this.isQuestion = this.isQuestionS;
        bool flag = !(this.voice != null);
        if (!flag)
        {
            this.voice.Stop();
        }
    }

    // Token: 0x06000609 RID: 1545
    public virtual void LateUpdate()
    {
        bool doCheckpointSave = this.root.doCheckpointSave;
        if (doCheckpointSave)
        {
            this.saveState();
        }
        bool flag = !this.root.doCheckpointLoad;
        if (!flag)
        {
            this.loadState();
        }
    }

    // Token: 0x0600060A RID: 1546
    public virtual void Start()
    {
        this.root = (RootScript)GameObject.Find("Root").GetComponent(typeof(RootScript));
        this.voiceTargetPitch = 0.5f;

        voice.playOnAwake = false;
        voice.loop = false;
    }

    // Token: 0x0600060B RID: 1547
    public virtual void doVoice()
    {
        bool flag = !(this.voice != null);
        if (!flag)
        {
            this.voiceTimer += 0.3f * this.root.timescale;
            bool flag2 = (double)this.voiceTimer >= (double)this.voiceLength - 1.0;
            if (flag2)
                this.voiceTimer = this.voiceLength - 1f;

            this.voiceTargetPitch = 1f + Mathf.Repeat((float)Convert.ToInt32(this.curVoiceChar), 150f) / 150f * this.voicePitchRange;
            bool flag3 = this.curVoiceChar == ' ' || this.curVoiceChar == '.' || this.curVoiceChar == '?' || this.curVoiceChar == '!' || (double)this.voiceTimer >= (double)this.voiceLength - 1.0;



            if (shouldSpeak == false)
                if (voiceTimer < 1f)
                    shouldSpeak = true;

            if (shouldSpeak)
            {
                if (voice.isPlaying)
                    voice.Stop();


                while (true)
                {

                    int rnd = UnityEngine.Random.Range(0, MFPClassicAssets.pedroSounds.Length);

                    if (rnd != lastVoiceIndex)
                    {
                        voice.clip = MFPClassicAssets.pedroSounds[rnd];
                        lastVoiceIndex = rnd;
                        break;
                    }

                }


                voice.Play();
                shouldSpeak = false;
            }

            if (flag3)
            {
                //this.voiceTargetVolume = 0f;
                bool flag4 = this.curVoiceChar == '.' || this.curVoiceChar == '?' || this.curVoiceChar == '!';
                if (flag4)
                {
                    this.voiceTimer -= 0.2f * this.root.timescale;
                }
                else
                    this.voiceTimer -= 0.05f * this.root.timescale;
            }
            else
            {
                this.voiceTargetVolume = (float)(0.5 + (double)this.voiceTargetPitch / 5.0);
            }
            this.voiceTargetPitch += this.voicePitchOffset;
            bool flag5 = (double)Time.time - (double)this.timestamp >= 0.0125000001862645;
            if (flag5)
            {
                this.voice.volume = this.root.Damp(this.voiceTargetVolume, this.voice.volume, 0.4f);
                this.timestamp = Time.time;
            }
            bool flag6 = !this.root.paused;
            if (!flag6)
            {
                this.voice.volume = 0f;
            }
        }
    }

    // Token: 0x0600060C RID: 1548
    public virtual void Main()
    {
    }


    private static int lastVoiceIndex = -1;

    // Token: 0x0400133C RID: 4924
    private RootScript root;

    // Token: 0x0400133D RID: 4925
    public AudioSource voice;

    // Token: 0x0400133E RID: 4926
    public float voiceTimer;

    // Token: 0x0400133F RID: 4927
    public float voicePitchOffset;

    // Token: 0x04001340 RID: 4928
    public float voicePitchRange;

    // Token: 0x04001341 RID: 4929
    public float voicePitchWobbleMultiplier;

    // Token: 0x04001342 RID: 4930
    private float voiceTargetPitch;

    // Token: 0x04001343 RID: 4931
    private float voiceTargetVolume;

    // Token: 0x04001344 RID: 4932
    public char curVoiceChar;

    // Token: 0x04001345 RID: 4933
    public float voiceLength;

    // Token: 0x04001346 RID: 4934
    public bool isQuestion;

    // Token: 0x04001347 RID: 4935
    private float timestamp;

    // Token: 0x04001348 RID: 4936
    private AudioSource voiceS;

    // Token: 0x04001349 RID: 4937
    private float voiceTimerS;

    // Token: 0x0400134A RID: 4938
    private float voicePitchOffsetS;

    // Token: 0x0400134B RID: 4939
    private float voicePitchRangeS;

    // Token: 0x0400134C RID: 4940
    private float voicePitchWobbleMultiplierS;

    // Token: 0x0400134D RID: 4941
    private float voiceTargetPitchS;

    // Token: 0x0400134E RID: 4942
    private float voiceTargetVolumeS;

    // Token: 0x0400134F RID: 4943
    private char curVoiceCharS;

    // Token: 0x04001350 RID: 4944
    private float voiceLengthS;

    // Token: 0x04001351 RID: 4945
    private bool isQuestionS;
}
