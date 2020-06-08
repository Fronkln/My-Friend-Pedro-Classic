using System;
using UnityEngine;
using UnityScript.Lang;

// Token: 0x0200009C RID: 156
[Serializable]
public class PlayerAnimationScripts : MonoBehaviour
{
    // Token: 0x060003F8 RID: 1016 RVA: 0x000020A9 File Offset: 0x000002A9
    public PlayerAnimationScripts()
    {
    }

    // Token: 0x060003F9 RID: 1017 RVA: 0x00059F74 File Offset: 0x00058174
    public virtual void Start()
    {
        this.playerScript = (PlayerScript)this.transform.parent.GetComponent(typeof(PlayerScript));
        this.footAudioR = (AudioSource)this.transform.Find("Armature/Center/Hip_R/UpperLeg_R/LowerLeg_R/Foot_R").GetComponent(typeof(AudioSource));
        this.footAudioL = (AudioSource)this.transform.Find("Armature/Center/Hip_L/UpperLeg_L/LowerLeg_L/Foot_L").GetComponent(typeof(AudioSource));
        this.playerAudio = (AudioSource)this.transform.parent.GetComponent(typeof(AudioSource));
    }

    // Token: 0x060003FA RID: 1018 RVA: 0x00003D88 File Offset: 0x00001F88
    public virtual void emitClip(int h)
    {
        this.playerScript.emitClip(h);
    }

    // Token: 0x060003FB RID: 1019 RVA: 0x00003D96 File Offset: 0x00001F96
    public virtual void finishedReloading()
    {
        this.playerScript.finishedReloading();
    }

    // Token: 0x060003FC RID: 1020 RVA: 0x00003DA3 File Offset: 0x00001FA3
    public virtual void finishedFlippingTable()
    {
        this.playerScript.finishedFlippingTable();
    }

    // Token: 0x060003FD RID: 1021 RVA: 0x00003DB0 File Offset: 0x00001FB0
    public virtual void shotgunReload()
    {
        this.playerScript.shotgunReload();
    }

    // Token: 0x060003FE RID: 1022 RVA: 0x00003DBD File Offset: 0x00001FBD
    public virtual void shotgunFinishedReloading()
    {
        this.playerScript.shotgunFinishedReloading();
    }

    // Token: 0x060003FF RID: 1023 RVA: 0x00003DCA File Offset: 0x00001FCA
    public virtual void punch(int fist)
    {
        this.playerScript.punch(fist);
    }

    public virtual void stab(int fist)
    {
        playerScript.stab(fist);
    }

    // Token: 0x06000400 RID: 1024 RVA: 0x0005A020 File Offset: 0x00058220
    public virtual void footstep(int right)
    {
        if (!this.playerScript.dontMakeFootstepSound && (this.playerScript.kXDir != (float)0 || right == 3 || right == 4))
        {
            if (this.playerScript.groundTransform != null && this.prevPlayerGroundTransform != this.playerScript.groundTransform)
            {
                this.prevPlayerGroundTransform = this.playerScript.groundTransform;
                this.groundTransformSoundScript = (SoundScript)this.playerScript.groundTransform.GetComponent(typeof(SoundScript));
            }
            if (this.groundTransformSoundScript != null && Extensions.get_length(this.groundTransformSoundScript.footstep) > 0)
            {
                if (this.playerScript.onGround && right == 1)
                {
                    this.footAudioR.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                    this.footAudioR.volume = UnityEngine.Random.Range(0.3f, 0.5f);
                    this.footAudioR.clip = this.groundTransformSoundScript.footstep[UnityEngine.Random.Range(0, Extensions.get_length(this.groundTransformSoundScript.footstep))];
                    this.footAudioR.Play();
                }
                else if (this.playerScript.onGround && right == 0)
                {
                    this.footAudioL.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                    this.footAudioL.volume = UnityEngine.Random.Range(0.3f, 0.5f);
                    this.footAudioL.clip = this.groundTransformSoundScript.footstep[UnityEngine.Random.Range(0, Extensions.get_length(this.groundTransformSoundScript.footstep))];
                    this.footAudioL.Play();
                }
                else if (right == 3 && !this.playerScript.jumpedFromSwinging)
                {
                    this.footAudioL.pitch = UnityEngine.Random.Range((float)1, 1.2f);
                    this.footAudioL.volume = 0.9f;
                    this.footAudioL.clip = this.groundTransformSoundScript.footstep[UnityEngine.Random.Range(0, Extensions.get_length(this.groundTransformSoundScript.footstep))];
                    this.footAudioL.Play();
                }
                else if (right == 4)
                {
                    this.footAudioL.pitch = UnityEngine.Random.Range(0.7f, 0.8f);
                    this.footAudioL.volume = (Mathf.Abs(this.playerScript.ySpeed) - (float)5) / (float)7;
                    this.footAudioL.clip = this.groundTransformSoundScript.footstep[UnityEngine.Random.Range(0, Extensions.get_length(this.groundTransformSoundScript.footstep))];
                    this.footAudioL.Play();
                }
            }
        }
    }

    // Token: 0x06000401 RID: 1025 RVA: 0x0005A2F0 File Offset: 0x000584F0
    public virtual void PlaySound(AudioClip snd)
    {
        this.playerAudio.volume = UnityEngine.Random.Range(0.85f, (float)1);
        this.playerAudio.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        this.playerAudio.clip = snd;
        this.playerAudio.Play();
    }

    // Token: 0x06000402 RID: 1026 RVA: 0x000020A7 File Offset: 0x000002A7
    public virtual void Main()
    {
    }

    // Token: 0x04000B77 RID: 2935
    private PlayerScript playerScript;

    // Token: 0x04000B78 RID: 2936
    private AudioSource footAudioR;

    // Token: 0x04000B79 RID: 2937
    private AudioSource footAudioL;

    // Token: 0x04000B7A RID: 2938
    private AudioSource playerAudio;

    // Token: 0x04000B7B RID: 2939
    private Transform prevPlayerGroundTransform;

    // Token: 0x04000B7C RID: 2940
    private SoundScript groundTransformSoundScript;
}
