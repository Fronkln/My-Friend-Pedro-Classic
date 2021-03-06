﻿using System;
using System.Collections.Generic;
using System.Linq;
using Boo.Lang.Runtime;
using CompilerGenerated;
using ConfigurationLibrary;
using I2.Loc;
using Rewired;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityScript.Lang;

// Token: 0x02000087 RID: 135
[Serializable]
public class OptionsMenuScript : MonoBehaviour
{
    // Token: 0x0600034A RID: 842 RVA: 0x00003A79 File Offset: 0x00001C79
    public OptionsMenuScript()
    {
        this.leaderboardFetchTimerDoOnce = true;
    }

    // Token: 0x0600034B RID: 843 RVA: 0x000481E4 File Offset: 0x000463E4
    public virtual void Awake()
    {

        this.player = ReInput.players.GetPlayer(0);
        this.inputMapper = new InputMapper();
        this.inputMapper.options.ignoreMouseXAxis = true;
        this.inputMapper.options.ignoreMouseYAxis = true;
        this.inputMapper.InputMappedEvent += delegate { OnInputMapped(); };
        this.inputMapper.CanceledEvent += delegate { OnCancelledInputEvent(); };
        this.inputMapper.StoppedEvent += delegate { OnStoppedInputEvent(); };
        this.inputMapper.options.holdDurationToMapKeyboardModifierKeyAsPrimary = (float)0;
        this.inputMapper.options.defaultActionWhenConflictFound = InputMapper.ConflictResponse.Replace;
        this.inputMapper.options.isElementAllowedCallback = new Predicate<ControllerPollingInfo>(this.OnIsElementAllowed);
        this.inputMapper2 = new InputMapper();
        this.inputMapper2.options.ignoreMouseXAxis = true;
        this.inputMapper2.options.ignoreMouseYAxis = true;
        this.inputMapper2.InputMappedEvent += delegate { OnInputMapped2(); };
        this.inputMapper2.CanceledEvent += delegate { this.OnCancelledInputEvent2(); };
        this.inputMapper2.StoppedEvent += delegate { OnStoppedInputEvent2(); };
        this.inputMapper2.options.holdDurationToMapKeyboardModifierKeyAsPrimary = (float)0;
        this.inputMapper2.options.defaultActionWhenConflictFound = InputMapper.ConflictResponse.Replace;
        this.inputMapper2.options.isElementAllowedCallback = new Predicate<ControllerPollingInfo>(this.OnIsElementAllowed);
    }

    // Token: 0x0600034C RID: 844 RVA: 0x0004838C File Offset: 0x0004658C
    public virtual void Start()
    {
        GameObject gameObject = GameObject.Find("RootShared");
        if (gameObject == null)
        {
            gameObject = new GameObject();
            gameObject.gameObject.name = "RootShared";
            gameObject.AddComponent(typeof(RootSharedScript));
        }
        this.rootShared = (RootSharedScript)gameObject.GetComponent(typeof(RootSharedScript));

        GameObject gameObject2 = GameObject.Find("Root");
        if (gameObject2 != null)
        {
            this.foundRoot = true;
            this.root = (RootScript)gameObject2.GetComponent(typeof(RootScript));
        }
        this.theAudioSource = (AudioSource)this.GetComponent(typeof(AudioSource));
        this.theAudioSource2 = (AudioSource)this.gameObject.AddComponent(typeof(AudioSource));
        this.theAudioSource2.loop = false;
        this.theAudioSource2.playOnAwake = false;
        this.theAudioSource2.clip = this.theAudioSource.clip;
        this.theAudioSource2.outputAudioMixerGroup = this.theAudioSource.outputAudioMixerGroup;
        this.theAudioSource3 = (AudioSource)this.gameObject.AddComponent(typeof(AudioSource));
        this.theAudioSource3.loop = false;
        this.theAudioSource3.playOnAwake = false;
        this.theAudioSource3.clip = this.theAudioSource.clip;
        this.theAudioSource3.outputAudioMixerGroup = this.theAudioSource.outputAudioMixerGroup;
        if (SavedData.HasKey("SFXVolume"))
        {
            this.audioMixer.SetFloat("SFXMasterVolume", SavedData.GetFloat("SFXVolume"));
        }
        if (SavedData.HasKey("MusicVolume"))
        {
            this.audioMixer.SetFloat("MusicMasterVolume", SavedData.GetFloat("MusicVolume"));
        }
        this.theCanvasGroup = (CanvasGroup)this.GetComponent(typeof(CanvasGroup));
        this.theCanvasGroup.alpha = (float)0;
        this.theHeader = (Text)this.transform.Find("Header").GetComponent(typeof(Text));
        this.mask = this.transform.Find("Mask");
        this.contentWrapper = (RectTransform)this.mask.Find("Content").GetComponent(typeof(RectTransform));
        this.option = (RectTransform)this.contentWrapper.Find("Option").GetComponent(typeof(RectTransform));
        this.selectLine = (RectTransform)this.contentWrapper.Find("SelectLine").GetComponent(typeof(RectTransform));
        this.theRectTransform = (RectTransform)this.GetComponent(typeof(RectTransform));
        this.smallHeader = (RectTransform)this.contentWrapper.Find("SmallHeader").GetComponent(typeof(RectTransform));
        this.extraInfo = (RectTransform)this.mask.Find("ExtraInfo").GetComponent(typeof(RectTransform));
        this.extraInfoText = (Text)this.extraInfo.Find("Text").GetComponent(typeof(Text));
        this.extraInfo.gameObject.SetActive(false);
        this.levelSelectScreen = (RectTransform)this.extraInfo.Find("LevelSelectScreen").GetComponent(typeof(RectTransform));
        this.levelSelectScreenRating = (Text)this.levelSelectScreen.Find("RatingSection/TheRating").GetComponent(typeof(Text));
        this.levelSelectScreenScore = (Text)this.levelSelectScreen.Find("RatingSection/BestScore/Text").GetComponent(typeof(Text));
        this.levelSelectScreenTime = (Text)this.levelSelectScreen.Find("RatingSection/BestTime/Text").GetComponent(typeof(Text));
        this.levelSelectScreenDifficultyRectTransform = (RectTransform)this.levelSelectScreen.Find("RatingSection/DifficultyLevel/Text").GetComponent(typeof(RectTransform));
        this.levelSelectScreenDifficulty = (Text)this.levelSelectScreenDifficultyRectTransform.GetComponent(typeof(Text));
        this.levelSelectScreenLeaderboardRectTransform = (RectTransform)this.levelSelectScreen.Find("Leaderboard").GetComponent(typeof(RectTransform));
        this.levelSelectScreenLeaderboardTypeTextRectTransform = (RectTransform)this.levelSelectScreen.Find("Leaderboard/Entries/LeaderboardType").GetComponent(typeof(RectTransform));
        this.levelSelectScreenLeaderboardTypeText = (Text)this.levelSelectScreenLeaderboardTypeTextRectTransform.GetComponent(typeof(Text));
        this.levelSelectScreenLeaderboardPagePrevTextRectTransform = (RectTransform)this.levelSelectScreen.Find("Leaderboard/Entries/LeaderboardPagePrev").GetComponent(typeof(RectTransform));
        this.levelSelectScreenLeaderboardPageNextTextRectTransform = (RectTransform)this.levelSelectScreen.Find("Leaderboard/Entries/LeaderboardPageNext").GetComponent(typeof(RectTransform));
        this.leaderboardOfflineGameObject = this.levelSelectScreen.Find("LeaderboardOffline").gameObject;
        this.leaderboardOfflineTextRectTransform = (RectTransform)this.leaderboardOfflineGameObject.transform.Find("LeaderboardText").GetComponent(typeof(RectTransform));
        this.scrollbar = (RectTransform)this.mask.Find("Scrollbar").GetComponent(typeof(RectTransform));
        this.scrollbarBar = (RectTransform)this.scrollbar.Find("Bar").GetComponent(typeof(RectTransform));
        this.inputHelperScript = (InputHelperScript)GameObject.Find("Rewired Input Manager").GetComponent(typeof(InputHelperScript));
        this.startPos = this.theRectTransform.anchoredPosition;
        this.gameModifiersNotice = this.transform.Find("ModifiersDisclaimer").gameObject;
        int num = -1200;
        Vector2 anchoredPosition = this.theRectTransform.anchoredPosition;
        float num2 = anchoredPosition.x = (float)num;
        Vector2 vector = this.theRectTransform.anchoredPosition = anchoredPosition;
        int num3 = 0;
        Vector2 sizeDelta = this.theRectTransform.sizeDelta;
        float num4 = sizeDelta.y = (float)num3;
        Vector2 vector2 = this.theRectTransform.sizeDelta = sizeDelta;
        this.createNavigationHints();
        if (this.inGame)
        {
            this.buildPauseMenu();
        }
        else
        {
            if (!this.rootShared.neverChangeMouseCursor)
            {
                Cursor.SetCursor(Resources.Load("HUD/menu_cursor") as Texture2D, new Vector2((float)3, (float)3), CursorMode.Auto);
            }
            if (this.foundRoot)
            {
                this.root.SetCursorState();
            }
            else
            {
                Cursor.visible = !this.rootShared.runningOnConsole;
            }
            this.buildStartPrompt();
            this.rootShared.PrepareLeaderboardDisplayEntries();
            this.leaderboardDisplayEntry = new Text[10];
            this.leaderboardDisplayEntryScore = new Text[10];
            for (int i = 0; i < 10; i++)
            {
                this.leaderboardDisplayEntry[i] = (Text)this.levelSelectScreenLeaderboardRectTransform.Find("Entries/ScoreEntry" + (i + 1) + "/ScoreName").GetComponent(typeof(Text));
                this.leaderboardDisplayEntryScore[i] = (Text)this.levelSelectScreenLeaderboardRectTransform.Find("Entries/ScoreEntry" + (i + 1) + "/ScoreText").GetComponent(typeof(Text));
                this.leaderboardDisplayEntry[i].text = (this.leaderboardDisplayEntryScore[i].text = string.Empty);
            }
        }
        this.option.gameObject.SetActive(false);
        this.smallHeader.gameObject.SetActive(false);
        List<Resolution> list = Screen.resolutions.ToList<Resolution>();
        for (int j = 0; j < list.Count; j++)
        {
            for (int k = 0; k < list.Count; k++)
            {
                if (!RuntimeServices.EqualityOperator(list[j], list[k]) && list[j].width == list[k].width && list[j].height == list[k].height)
                {
                    list.RemoveAt(k);
                    k--;
                }
            }
        }
        this.supportedResolutions = list.ToArray();
    }

    // Token: 0x0600034D RID: 845 RVA: 0x00048C5C File Offset: 0x00046E5C
    public virtual void Update()
    {
        float num = Time.unscaledDeltaTime * (float)60;
        if (this.menuEnabled)
        {
            this.theCanvasGroup.alpha = Mathf.Clamp01(this.theCanvasGroup.alpha + ((this.curActiveMenu != (float)-2) ? 0.05f : 0.01f) * num);
            float x = this.DampUnscaled(this.startPos.x, this.theRectTransform.anchoredPosition.x, (this.curActiveMenu != (float)-2) ? 0.2f : 0.05f);
            Vector2 anchoredPosition = this.theRectTransform.anchoredPosition;
            float num2 = anchoredPosition.x = x;
            Vector2 vector = this.theRectTransform.anchoredPosition = anchoredPosition;
            if (!this.menuEnabledDoOnce)
            {
                if (!this.rootShared.neverChangeMouseCursor)
                {
                    Cursor.SetCursor(Resources.Load("HUD/menu_cursor") as Texture2D, new Vector2((float)3, (float)3), CursorMode.Auto);
                }
                if (this.foundRoot)
                {
                    this.root.SetCursorState();
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = !this.rootShared.runningOnConsole;
                }
                this.menuEnabledDoOnce = true;
            }
        }
        else
        {
            this.theCanvasGroup.alpha = Mathf.Clamp01(this.theCanvasGroup.alpha - 0.05f * num);
            float x2 = this.DampUnscaled((float)-1200, this.theRectTransform.anchoredPosition.x, 0.4f);
            Vector2 anchoredPosition2 = this.theRectTransform.anchoredPosition;
            float num3 = anchoredPosition2.x = x2;
            Vector2 vector2 = this.theRectTransform.anchoredPosition = anchoredPosition2;
            float y = this.DampUnscaled((float)0, this.theRectTransform.sizeDelta.y, 0.4f);
            Vector2 sizeDelta = this.theRectTransform.sizeDelta;
            float num4 = sizeDelta.y = y;
            Vector2 vector3 = this.theRectTransform.sizeDelta = sizeDelta;
            if (this.inGame && this.theCanvasGroup.alpha <= (float)0)
            {
                this.gameObject.SetActive(false);
            }
            if (this.menuEnabledDoOnce)
            {
                if (!this.rootShared.neverChangeMouseCursor)
                {
                    Cursor.SetCursor(Resources.Load("HUD/ingame_cursor") as Texture2D, new Vector2((float)16, (float)16), CursorMode.Auto);
                }
                if (this.inGame)
                {
                    if (this.foundRoot)
                    {
                        this.root.SetCursorState();
                    }
                    else
                    {
                        bool visible;
                        if (visible = !this.useGamepadIcons)
                        {
                            visible = !this.rootShared.runningOnConsole;
                        }
                        Cursor.visible = visible;
                    }
                }
                this.menuEnabledDoOnce = false;
            }
        }
        if (this.rootShared.allowDebugMenu && !this.inGame && Input.GetKey("f11"))
        {
            this.theCanvasGroup.alpha = (float)0;
            if (Input.GetKeyDown("f12"))
            {
                SavedData.SetInt("levelSelectMaxNr", 99);
            }
        }
        if (this.menuEnabled)
        {
            Controller lastActiveController = this.player.controllers.GetLastActiveController();
            if (!RuntimeServices.EqualityOperator(lastActiveController, null))
            {
                if (this.useGamepadIcons)
                {
                    if (lastActiveController.type == ControllerType.Keyboard || lastActiveController.type == ControllerType.Mouse)
                    {
                        PlatformPlayerPrefs.SetInt("gamepad", 0);
                        this.useGamepadIcons = false;
                        if (this.curActiveMenu == 0.2f)
                        {
                            this.rebuildMenu();
                        }
                        this.createNavigationHints();
                    }
                }
                else if (lastActiveController.type == ControllerType.Joystick)
                {
                    PlatformPlayerPrefs.SetInt("gamepad", 1);
                    this.useGamepadIcons = true;
                    if (this.curActiveMenu == 0.2f)
                    {
                        this.rebuildMenu();
                    }
                    this.createNavigationHints();
                }
            }
            if (this.timeSinceKeyboardUsed < (float)100)
            {
                this.timeSinceKeyboardUsed += num;
            }
            if (this.inputMapper.status == InputMapper.Status.Idle && this.inputMapper2.status == InputMapper.Status.Idle)
            {
                if (this.allowNavigationTimer <= (float)0)
                {
                    this.allowNavigation = true;
                }
                else
                {
                    this.allowNavigationTimer -= num;
                }
            }
            else
            {
                this.allowNavigationTimer = (float)5;
                this.allowNavigation = false;
            }
            if (this.curActiveMenu == (float)-2)
            {
                bool flag = default(bool);
                Controller controllerToUse = this.inputHelperScript.GetControllerToUse(true, false);
                if (!RuntimeServices.EqualityOperator(controllerToUse, null) && controllerToUse.GetAnyButtonDown())
                {
                    flag = true;
                }
                controllerToUse = this.inputHelperScript.GetControllerToUse(true, true);
                if (!RuntimeServices.EqualityOperator(controllerToUse, null) && controllerToUse.GetAnyButtonDown())
                {
                    flag = true;
                }
                controllerToUse = this.inputHelperScript.GetControllerToUse(false, false);
                if (!RuntimeServices.EqualityOperator(controllerToUse, null) && controllerToUse.GetAnyButtonDown())
                {
                    flag = true;
                }
                if (flag)
                {
                    this.theAudioSource.volume = UnityEngine.Random.Range(0.9f, (float)1);
                    this.theAudioSource.pitch = UnityEngine.Random.Range(1.4f, 1.5f);
                    this.theAudioSource.Play();
                    if (this.rootShared.isDemo)
                    {
                        SavedData.SetInt("difficultyMode", 0);
                        if (this.rootShared.isNintendoDemo)
                        {
                            this.rootShared.loadingScreenLevelToLoad = 3;
                        }
                        else
                        {
                            this.rootShared.loadingScreenLevelToLoad = SceneUtility.GetBuildIndexByScenePath("_Demo_video");
                        }
                        this.rootShared.levelLoadedFromLevelSelectScreen = false;
                        ((MainMenuBackgroundScript)GameObject.Find("TheCamera").GetComponent(typeof(MainMenuBackgroundScript))).doStartGame();
                        this.menuEnabled = false;
                    }
                    else
                    {
                        this.buildMainMenu();
                    }
                }
                this.optionsText[0].color = Color.Lerp(this.unselectedColor, this.highlightedColor, (Mathf.Sin(Time.time * (float)2) + (float)1) / (float)2);
                int num5 = -200;
                Vector2 anchoredPosition3 = this.theRectTransform.anchoredPosition;
                float num6 = anchoredPosition3.y = (float)num5;
                Vector2 vector4 = this.theRectTransform.anchoredPosition = anchoredPosition3;
                int num7 = -600;
                Vector2 anchoredPosition4 = this.selectLine.anchoredPosition;
                float num8 = anchoredPosition4.y = (float)num7;
                Vector2 vector5 = this.selectLine.anchoredPosition = anchoredPosition4;
            }
            else
            {
                float y2 = this.DampUnscaled((float)0, this.theRectTransform.anchoredPosition.y, 0.4f);
                Vector2 anchoredPosition5 = this.theRectTransform.anchoredPosition;
                float num9 = anchoredPosition5.y = y2;
                Vector2 vector6 = this.theRectTransform.anchoredPosition = anchoredPosition5;
                if (this.nrOfOptions > (float)0)
                {
                    if (this.allowNavigation && !this.gotMouseInput)
                    {
                        if (this.player.GetAxisRaw("UIXAxis") < -0.5f)
                        {
                            this.navInput.x = (float)-1;
                            this.timeSinceKeyboardUsed = (float)0;
                            this.mouseNavigation = false;
                        }
                        else if (this.player.GetAxisRaw("UIXAxis") > 0.5f)
                        {
                            this.navInput.x = (float)1;
                            this.timeSinceKeyboardUsed = (float)0;
                            this.mouseNavigation = false;
                        }
                        else
                        {
                            this.navInput.x = (float)0;
                        }
                        if (this.player.GetAxisRaw("UIYAxis") > 0.5f)
                        {
                            this.navInput.y = (float)1;
                            this.timeSinceKeyboardUsed = (float)0;
                            this.mouseNavigation = false;
                        }
                        else if (this.player.GetAxisRaw("UIYAxis") < -0.5f)
                        {
                            this.navInput.y = (float)-1;
                            this.timeSinceKeyboardUsed = (float)0;
                            this.mouseNavigation = false;
                        }
                        else
                        {
                            this.navInput.y = (float)0;
                        }
                    }
                    float max = (float)((!(this.optionsSlider[this.curOption] == null)) ? 50 : 48);
                    if (this.navInput.y <= (float)-1)
                    {
                        if (this.inputRepeatDelayY <= this.inputRepeatDelayYLimit)
                        {
                            this.curOptionSortedNr = Mathf.Clamp(this.curOptionSortedNr + (float)1, (float)0, this.nrOfOptions - (float)1);
                            this.curOption = this.curOptionSorted[(int)this.curOptionSortedNr];
                            this.doExtraInfoUpdate(true);
                            this.inputRepeatDelayY = (float)50;
                            this.inputRepeatDelayYLimit = Mathf.Clamp(this.inputRepeatDelayYLimit + (float)21, (float)0, max);
                            this.inputRepeatDelayX = (float)0;
                            this.inputRepeatDelayXLimit = (float)0;
                            this.updateNavigationHints();
                        }
                        else
                        {
                            this.inputRepeatDelayY -= num;
                        }
                    }
                    else if (this.navInput.y >= (float)1)
                    {
                        if (this.inputRepeatDelayY <= this.inputRepeatDelayYLimit)
                        {
                            this.curOptionSortedNr = Mathf.Clamp(this.curOptionSortedNr - (float)1, (float)0, this.nrOfOptions - (float)1);
                            this.curOption = this.curOptionSorted[(int)this.curOptionSortedNr];
                            this.doExtraInfoUpdate(true);
                            this.inputRepeatDelayY = (float)50;
                            this.inputRepeatDelayYLimit = Mathf.Clamp(this.inputRepeatDelayYLimit + (float)21, (float)0, max);
                            this.inputRepeatDelayX = (float)0;
                            this.inputRepeatDelayXLimit = (float)0;
                            this.updateNavigationHints();
                        }
                        else
                        {
                            this.inputRepeatDelayY -= num;
                        }
                    }
                    else
                    {
                        this.inputRepeatDelayY = (float)0;
                        this.inputRepeatDelayYLimit = (float)0;
                    }
                    if (this.prevCurOptionSortedNr != this.curOptionSortedNr)
                    {
                        if (this.curAudioSourceToUse == 0)
                        {
                            if (!this.theAudioSource.isPlaying)
                            {
                                this.theAudioSource.volume = UnityEngine.Random.Range(0.3f, 0.4f);
                                this.theAudioSource.pitch = 0.75f + this.curOptionSortedNr * Mathf.Sin(0.1f) * 0.1f;
                                this.theAudioSource.Play();
                            }
                            this.curAudioSourceToUse = 1;
                        }
                        else if (this.curAudioSourceToUse == 1)
                        {
                            if (!this.theAudioSource2.isPlaying)
                            {
                                this.theAudioSource2.volume = UnityEngine.Random.Range(0.3f, 0.4f);
                                this.theAudioSource2.pitch = 0.75f + this.curOptionSortedNr * Mathf.Sin(0.1f) * 0.1f;
                                this.theAudioSource2.Play();
                            }
                            this.curAudioSourceToUse = 2;
                        }
                        else if (this.curAudioSourceToUse == 2)
                        {
                            if (!this.theAudioSource3.isPlaying)
                            {
                                this.theAudioSource3.volume = UnityEngine.Random.Range(0.3f, 0.4f);
                                this.theAudioSource3.pitch = 0.75f + this.curOptionSortedNr * Mathf.Sin(0.1f) * 0.1f;
                                this.theAudioSource3.Play();
                            }
                            this.curAudioSourceToUse = 0;
                        }
                        this.prevCurOptionSortedNr = this.curOptionSortedNr;
                    }
                    float y3 = this.DampUnscaled(this.options[this.curOption].anchoredPosition.y - (float)12, this.selectLine.anchoredPosition.y, 0.8f);
                    Vector2 anchoredPosition6 = this.selectLine.anchoredPosition;
                    float num10 = anchoredPosition6.y = y3;
                    Vector2 vector7 = this.selectLine.anchoredPosition = anchoredPosition6;
                    int num11 = 0;
                    while ((float)num11 < this.nrOfOptions)
                    {
                        if (this.options[num11] != null)
                        {
                            if (this.curOption == num11)
                            {
                                this.options[num11].localScale = this.DampV3Unscaled(Vector3.one, this.options[num11].localScale, 0.8f);
                                if (this.optionsSlider[num11] != null)
                                {
                                    this.optionsSlider[num11].color = this.DampColorUnscaled((!this.disableOption[num11]) ? this.highlightedColor : this.disabledColor, this.optionsSlider[num11].color, 0.8f);
                                    this.optionsSliderContainerImage[num11].color = this.DampColorUnscaled((!this.disableOption[num11]) ? this.unselectedColor : this.disabledColor, this.optionsSliderContainerImage[num11].color, 0.8f);
                                }
                                if (this.disableOption[num11])
                                {
                                    this.optionsText[num11].color = this.DampColorUnscaled(this.disabledColor, this.optionsText[num11].color, 0.8f);
                                    this.optionsSettingText[num11].color = this.DampColorUnscaled(this.disabledColor, this.optionsSettingText[num11].color, 0.8f);
                                }
                                else if (this.optionsSettingText[num11].text == string.Empty)
                                {
                                    this.optionsText[num11].color = this.DampColorUnscaled(this.highlightedColor, this.optionsText[num11].color, 0.8f);
                                }
                                else
                                {
                                    this.optionsText[num11].color = this.DampColorUnscaled(this.selectedColor, this.optionsText[num11].color, 0.8f);
                                    this.optionsSettingText[num11].color = this.DampColorUnscaled(this.highlightedColor, this.optionsSettingText[num11].color, 0.8f);
                                }
                            }
                            else
                            {
                                this.options[num11].localScale = this.DampV3Unscaled(Vector3.one * 0.9f, this.options[num11].localScale, 0.8f);
                                if (this.optionsSlider[num11] != null)
                                {
                                    this.optionsSlider[num11].color = this.DampColorUnscaled((!this.disableOption[num11]) ? this.unselectedColor : this.disabledColor, this.optionsSlider[num11].color, 0.8f);
                                    this.optionsSliderContainerImage[num11].color = this.DampColorUnscaled((!this.disableOption[num11]) ? this.unselectedColor : this.disabledColor, this.optionsSliderContainerImage[num11].color, 0.8f);
                                }
                                if (this.disableOption[num11])
                                {
                                    this.optionsText[num11].color = this.DampColorUnscaled(this.disabledColor, this.optionsText[num11].color, 0.8f);
                                    this.optionsSettingText[num11].color = this.DampColorUnscaled(this.disabledColor, this.optionsSettingText[num11].color, 0.8f);
                                }
                                else
                                {
                                    this.optionsText[num11].color = this.DampColorUnscaled(this.unselectedColor, this.optionsText[num11].color, 0.8f);
                                    this.optionsSettingText[num11].color = this.DampColorUnscaled(this.unselectedColor, this.optionsSettingText[num11].color, 0.8f);
                                }
                            }
                        }
                        num11++;
                    }
                    if (!this.disableOption[this.curOption] && this.navInput.x >= (float)1)
                    {
                        if (this.inputRepeatDelayX <= this.inputRepeatDelayXLimit)
                        {
                            this.optionsSettingNr[this.curOption] = this.optionsSettingNr[this.curOption] + 1;
                            this.updateOptionSettings(false);
                            this.inputRepeatDelayX = (float)50;
                            this.inputRepeatDelayXLimit = Mathf.Clamp(this.inputRepeatDelayXLimit + (float)20, (float)0, max);
                        }
                        else
                        {
                            this.inputRepeatDelayX -= num;
                        }
                    }
                    else if (!this.disableOption[this.curOption] && this.navInput.x <= (float)-1)
                    {
                        if (this.inputRepeatDelayX <= this.inputRepeatDelayXLimit)
                        {
                            this.optionsSettingNr[this.curOption] = this.optionsSettingNr[this.curOption] - 1;
                            this.updateOptionSettings(false);
                            this.inputRepeatDelayX = (float)50;
                            this.inputRepeatDelayXLimit = Mathf.Clamp(this.inputRepeatDelayXLimit + (float)20, (float)0, max);
                        }
                        else
                        {
                            this.inputRepeatDelayX -= num;
                        }
                    }
                    else
                    {
                        this.inputRepeatDelayX = (float)0;
                        this.inputRepeatDelayXLimit = (float)0;
                    }
                    if (this.gotMouseInput && this.optionsSlider[this.curOption] != null && !this.disableOption[this.curOption])
                    {
                        Vector2 vector8 = default(Vector2);
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.optionsSliderContainer[this.curOption], Input.mousePosition, null, out vector8);
                        this.optionsSettingNr[this.curOption] = (int)(vector8.x / this.optionsSliderContainer[this.curOption].sizeDelta.x * (float)100);
                        this.updateOptionSettings(false);
                    }
                    if (this.allowNavigation)
                    {
                        if (this.player.GetButtonDown("UISubmit"))
                        {
                            this.executeOption();
                            this.timeSinceKeyboardUsed = (float)0;
                            this.mouseNavigation = false;
                        }
                        bool buttonDown;
                        if (buttonDown = this.player.GetButtonDown("UIStart"))
                        {
                            buttonDown = this.inGame;
                        }
                        bool flag2;
                        if (flag2 = buttonDown)
                        {
                            flag2 = this.useGamepadIcons;
                        }
                        bool flag3 = flag2;
                        if ((this.player.GetButtonDown("UIBack") || flag3) && this.menuBackOptionNr >= 0 && (float)this.menuBackOptionNr < this.nrOfOptions && this.options[this.menuBackOptionNr] != null)
                        {
                            this.curOption = this.menuBackOptionNr;
                            if (!this.mouseNavigation)
                            {
                                this.timeSinceKeyboardUsed = (float)0;
                            }
                            this.executeOption();
                            this.updateNavigationHints();
                        }
                        if (flag3)
                        {
                            this.menuEnabled = false;
                            this.buildPauseMenu();
                            this.root.resumeGame();
                        }
                        if (this.curActiveMenu == 0.2f)
                        {
                            if (this.player.GetButtonDown("UISpecial1"))
                            {
                                this.levelSelectScreenDifficultyLevel = (int)Mathf.Repeat((float)(this.levelSelectScreenDifficultyLevel + 1), (float)3);
                                if (this.levelSelectScreenDifficultyLevel == 0)
                                {
                                    this.levelSelectScreenDifficulty.text = this.rootShared.GetTranslation("diffLvl1");
                                }
                                else if (this.levelSelectScreenDifficultyLevel == 1)
                                {
                                    this.levelSelectScreenDifficulty.text = this.rootShared.GetTranslation("diffLvl2");
                                }
                                else if (this.levelSelectScreenDifficultyLevel == 2)
                                {
                                    this.levelSelectScreenDifficulty.text = this.rootShared.GetTranslation("diffLvl3");
                                }
                                this.levelSelectScreenDifficultyButton.anchoredPosition = this.levelSelectScreenDifficultyRectTransform.anchoredPosition + new Vector2(this.levelSelectScreenDifficulty.preferredWidth + this.levelSelectScreenDifficultyButton.sizeDelta.x / (float)2 + (float)10, (float)0);
                                if (this.levelSelectScreenDifficultyButton.anchoredPosition.x > (float)205)
                                {
                                    int num12 = 205;
                                    Vector2 anchoredPosition7 = this.levelSelectScreenDifficultyButton.anchoredPosition;
                                    float num13 = anchoredPosition7.x = (float)num12;
                                    Vector2 vector9 = this.levelSelectScreenDifficultyButton.anchoredPosition = anchoredPosition7;
                                }
                                this.doExtraInfoUpdate(false);
                            }
                            if (this.rootShared.allowLeaderboard)
                            {
                                if (this.rootShared.lostInternetConnection)
                                {
                                    if (!this.lostInternetConnectionDoOnce)
                                    {
                                        this.leaderboardOfflineGameObject.SetActive(true);
                                        this.levelSelectScreenLeaderboardRectTransform.gameObject.SetActive(false);
                                        this.lostInternetConnectionDoOnce = true;
                                    }
                                    if (this.player.GetButtonDown("UISpecial2"))
                                    {
                                        this.rootShared.AttemptToReconnect(this.leaderboardToUse);
                                    }
                                }
                                else
                                {
                                    if (this.lostInternetConnectionDoOnce)
                                    {
                                        this.leaderboardOfflineGameObject.SetActive(false);
                                        this.levelSelectScreenLeaderboardRectTransform.gameObject.SetActive(true);
                                        this.lostInternetConnectionDoOnce = false;
                                    }
                                    if (this.player.GetButtonDown("UISpecial2"))
                                    {
                                        this.leaderboardFilter = (int)Mathf.Repeat((float)(this.leaderboardFilter + 1), (float)((!this.rootShared.disableFriendsLeaderboardFilter) ? 3 : 2));
                                        this.rootShared.ResetLeaderboardPage();
                                        if (this.rootShared.disableFriendsLeaderboardFilter)
                                        {
                                            if (this.leaderboardFilter == 0)
                                            {
                                                int num14 = 1;
                                                Color color = ((Text)this.levelSelectScreenLeaderboardPageNextTextRectTransform.GetComponent(typeof(Text))).color;
                                                float num15 = color.a = (float)num14;
                                                Color color2 = ((Text)this.levelSelectScreenLeaderboardPageNextTextRectTransform.GetComponent(typeof(Text))).color = color;
                                                int num16 = 1;
                                                Color color3 = ((Text)this.levelSelectScreenLeaderboardPagePrevTextRectTransform.GetComponent(typeof(Text))).color;
                                                float num17 = color3.a = (float)num16;
                                                Color color4 = ((Text)this.levelSelectScreenLeaderboardPagePrevTextRectTransform.GetComponent(typeof(Text))).color = color3;
                                            }
                                            else
                                            {
                                                float a = 0.15f;
                                                Color color5 = ((Text)this.levelSelectScreenLeaderboardPageNextTextRectTransform.GetComponent(typeof(Text))).color;
                                                float num18 = color5.a = a;
                                                Color color6 = ((Text)this.levelSelectScreenLeaderboardPageNextTextRectTransform.GetComponent(typeof(Text))).color = color5;
                                                float a2 = 0.15f;
                                                Color color7 = ((Text)this.levelSelectScreenLeaderboardPagePrevTextRectTransform.GetComponent(typeof(Text))).color;
                                                float num19 = color7.a = a2;
                                                Color color8 = ((Text)this.levelSelectScreenLeaderboardPagePrevTextRectTransform.GetComponent(typeof(Text))).color = color7;
                                            }
                                        }
                                        this.doExtraInfoUpdate(true);
                                        this.leaderboardFetchTimer = (float)3;
                                    }
                                    if (this.player.GetButtonDown("UISpecial3") && (!this.rootShared.disableFriendsLeaderboardFilter || (this.rootShared.disableFriendsLeaderboardFilter && this.leaderboardFilter == 0)))
                                    {
                                        this.rootShared.PreviousLeaderboardPage();
                                        this.doExtraInfoUpdate(true);
                                        this.leaderboardFetchTimer = (float)3;
                                    }
                                    if (this.player.GetButtonDown("UISpecial4") && (!this.rootShared.disableFriendsLeaderboardFilter || (this.rootShared.disableFriendsLeaderboardFilter && this.leaderboardFilter == 0)))
                                    {
                                        this.rootShared.NextLeaderboardPage();
                                        this.doExtraInfoUpdate(true);
                                        this.leaderboardFetchTimer = (float)3;
                                    }
                                    if (this.leaderboardFetchTimer > (float)0)
                                    {
                                        this.leaderboardFetchTimer -= (float)1;
                                    }
                                    else if (!this.leaderboardFetchTimerDoOnce)
                                    {
                                        this.rootShared.DoShowLeaderboard(this.leaderboardToUse, this.leaderboardFilter);
                                        this.leaderboardFetchTimerDoOnce = true;
                                    }
                                }
                            }
                        }
                        else if (this.curActiveMenu == 2.2f)
                        {
                            if (!this.rootShared.runningOnConsole)
                            {
                                Controller lastActiveController2 = this.player.controllers.GetLastActiveController(ControllerType.Joystick);
                                if (!RuntimeServices.EqualityOperator(this.lastActiveGamepadController, lastActiveController2))
                                {
                                    MonoBehaviour.print("lastActiveGamepadController:     " + this.lastActiveGamepadController);
                                    this.lastActiveGamepadController = lastActiveController2;
                                    this.buildControlsMenu();
                                }
                            }
                        }
                        else if (this.curActiveMenu == (float)2)
                        {
                            if (this.disableOption[1])
                            {
                                if (!RuntimeServices.EqualityOperator(this.inputHelperScript.GetControllerToUse(false, false), null))
                                {
                                    this.buildControlsMenu();
                                }
                            }
                            else if (RuntimeServices.EqualityOperator(this.inputHelperScript.GetControllerToUse(false, false), null))
                            {
                                this.buildControlsMenu();
                            }
                        }
                    }
                }
            }
            float value = (this.curActiveMenu != (float)-2) ? (Mathf.Abs(this.yOffset) + (float)20) : ((float)40);
            float num20 = Mathf.Clamp(value, (float)0, (float)420);
            float y4 = this.DampUnscaled(num20, this.theRectTransform.sizeDelta.y, 0.4f);
            Vector2 sizeDelta2 = this.theRectTransform.sizeDelta;
            float num21 = sizeDelta2.y = y4;
            Vector2 vector10 = this.theRectTransform.sizeDelta = sizeDelta2;
            float num22 = -this.yOffset + (float)20 - num20;
            if (num22 > (float)0 && this.curActiveMenu != (float)-2)
            {
                if (this.player.GetAxis("UIScroll") > (float)0)
                {
                    this.targetScrollPos -= (float)30;
                    this.mouseNavigation = true;
                }
                else if (this.player.GetAxis("UIScroll") < (float)0)
                {
                    this.targetScrollPos += (float)30;
                    this.mouseNavigation = true;
                }
                if (!this.mouseNavigation)
                {
                    if (this.options[this.curOption].anchoredPosition.y + this.targetScrollPos > (float)-20 || this.curOptionSortedNr == (float)0)
                    {
                        this.targetScrollPos -= (float)15;
                    }
                    else if (this.options[this.curOption].anchoredPosition.y + this.targetScrollPos < (float)-400 || this.curOptionSortedNr == this.nrOfOptions - (float)1)
                    {
                        this.targetScrollPos += (float)15;
                    }
                }
                this.targetScrollPos = Mathf.Clamp(this.targetScrollPos, (float)0, num22);
                float y5 = this.DampUnscaled(this.targetScrollPos, this.contentWrapper.anchoredPosition.y, 0.4f);
                Vector2 anchoredPosition8 = this.contentWrapper.anchoredPosition;
                float num23 = anchoredPosition8.y = y5;
                Vector2 vector11 = this.contentWrapper.anchoredPosition = anchoredPosition8;
                float x3 = this.DampUnscaled((float)4, this.scrollbar.anchoredPosition.x, 0.4f);
                Vector2 anchoredPosition9 = this.scrollbar.anchoredPosition;
                float num24 = anchoredPosition9.x = x3;
                Vector2 vector12 = this.scrollbar.anchoredPosition = anchoredPosition9;
                float y6 = this.theRectTransform.sizeDelta.y - (float)10;
                Vector2 sizeDelta3 = this.scrollbar.sizeDelta;
                float num25 = sizeDelta3.y = y6;
                Vector2 vector13 = this.scrollbar.sizeDelta = sizeDelta3;
                float y7 = Mathf.Clamp(this.scrollbar.sizeDelta.y - num22 * 0.8f, (float)50, this.scrollbar.sizeDelta.y);
                Vector2 sizeDelta4 = this.scrollbarBar.sizeDelta;
                float num26 = sizeDelta4.y = y7;
                Vector2 vector14 = this.scrollbarBar.sizeDelta = sizeDelta4;
                float y8 = this.contentWrapper.anchoredPosition.y / num22 * -(this.scrollbar.sizeDelta.y - this.scrollbarBar.sizeDelta.y);
                Vector2 anchoredPosition10 = this.scrollbarBar.anchoredPosition;
                float num27 = anchoredPosition10.y = y8;
                Vector2 vector15 = this.scrollbarBar.anchoredPosition = anchoredPosition10;
            }
            else
            {
                int num28 = 0;
                Vector2 anchoredPosition11 = this.contentWrapper.anchoredPosition;
                float num29 = anchoredPosition11.y = (float)num28;
                Vector2 vector16 = this.contentWrapper.anchoredPosition = anchoredPosition11;
                int num30 = -10;
                Vector2 anchoredPosition12 = this.scrollbar.anchoredPosition;
                float num31 = anchoredPosition12.x = (float)num30;
                Vector2 vector17 = this.scrollbar.anchoredPosition = anchoredPosition12;
            }
        }
    }

    // Token: 0x0600034E RID: 846 RVA: 0x0004A8A0 File Offset: 0x00048AA0
    public virtual void buildStartPrompt()
    {
        this.clearMenu();
        this.curActiveMenu = (float)-2;
        this.theHeader.text = string.Empty;
        this.nrOfOptions = (float)1;
        this.menuBackOptionNr = -1;
        this.curOptionSortedNr = (float)0;
        this.setUpArrays();
        this.createOption(0, this.rootShared.GetTranslation("startPrompt"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)-20);
        int num = 145;
        Vector2 anchoredPosition = this.options[0].anchoredPosition;
        float num2 = anchoredPosition.x = (float)num;
        Vector2 vector = this.options[0].anchoredPosition = anchoredPosition;
        if (this.rootShared.isDemo)
        {
            this.rootShared.gamepadAimSens = (float)1;
        }
        this.uiConfirmHintCanvasGroup.alpha = (float)0;
        this.uiBackHintCanvasGroup.alpha = (float)0;
        this.updateOptionSettings(true);
    }

    // Token: 0x0600034F RID: 847 RVA: 0x0004A980 File Offset: 0x00048B80
    public virtual void buildDemoPauseMenu()
    {
        this.clearMenu();
        this.curActiveMenu = (float)-111;
        this.theHeader.text = this.rootShared.GetTranslation("mPaused");
        this.nrOfOptions = (float)4;
        this.menuBackOptionNr = 0;
        this.curOptionSortedNr = (float)0;
        this.setUpArrays();
        this.createOption(0, this.rootShared.GetTranslation("mResume"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)0);
        this.createOption(1, this.rootShared.GetTranslation("mRestart"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)10);
        this.createOption(2, this.rootShared.GetTranslation("mAimSens"), "Slider", TextAnchor.MiddleLeft, true, true, (float)0);
        this.createOption(3, this.rootShared.GetTranslation("mExit"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)10);
        this.optionsSettingNr[2] = Mathf.RoundToInt(this.rootShared.gamepadAimSens * (float)100);
        this.curOption = 0;
        this.curOptionSortedNr = (float)0;
        this.updateOptionSettings(true);
    }

    // Token: 0x06000350 RID: 848 RVA: 0x0004AA8C File Offset: 0x00048C8C
    public virtual void buildPauseMenu()
    {
        if (this.rootShared.isDemo && !this.rootShared.isNintendoDemo)
        {
            this.buildDemoPauseMenu();
        }
        else
        {
            this.clearMenu();
            this.curActiveMenu = (float)-1;
            this.theHeader.text = this.rootShared.GetTranslation("mPaused");
            this.nrOfOptions = (float)5;
            if (this.rootShared.allowDebugMenu)
            {
                this.nrOfOptions += (float)1;
            }
            else if (this.rootShared.runningOnConsole)
            {
                this.nrOfOptions -= (float)1;
            }
            this.menuBackOptionNr = 0;
            this.curOptionSortedNr = (float)0;
            this.setUpArrays();
            if (this.rootShared.allowDebugMenu)
            {
                this.createOption(5, "DEBUG", string.Empty, TextAnchor.MiddleLeft, false, false, (float)0);
            }
            this.createOption(0, this.rootShared.GetTranslation("mResume"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)0);
            this.createOption(1, this.rootShared.GetTranslation("mRestart"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)10);
            this.createOption(2, this.rootShared.GetTranslation("mOptions"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)0);
            this.createOption(3, this.rootShared.GetTranslation("mExit"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)10);
            if (this.rootShared.allowDebugMenu || !this.rootShared.runningOnConsole)
            {
                this.createOption(4, this.rootShared.GetTranslation("mQuit"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)0);
            }
            if (this.rootShared.isDemo && !this.rootShared.isNintendoDemo)
            {
                this.disableOption[2] = true;
                if (!this.rootShared.runningOnConsole)
                {
                    this.disableOption[4] = true;
                }
            }
            if (this.rootShared.allowDebugMenu && this.rootShared.runningOnConsole)
            {
                this.disableOption[4] = true;
            }
            this.curOption = 0;
            this.curOptionSortedNr = (float)0;
            this.updateOptionSettings(true);
            this.gameModifiersNotice.SetActive(this.rootShared.gameModifiersCheck());
        }
    }

    // Token: 0x06000351 RID: 849 RVA: 0x0004ACEC File Offset: 0x00048EEC
    public virtual void buildDebugMenu()
    {
        this.clearMenu();
        this.curActiveMenu = (float)-789;
        this.theHeader.text = "DEBUG";
        this.nrOfOptions = (float)7;
        this.menuBackOptionNr = 4;
        this.curOptionSortedNr = (float)0;
        this.setUpArrays();
        this.createOption(0, "God-mode", this.rootShared.GetTranslation("mOff") + "|" + this.rootShared.GetTranslation("mOn"), TextAnchor.MiddleLeft, true, true, (float)0);
        this.createOption(1, "Go to next level", string.Empty, TextAnchor.MiddleLeft, false, false, (float)10);
        this.createOption(2, "Go to previous level", string.Empty, TextAnchor.MiddleLeft, false, false, (float)0);
        this.createOption(5, "Unlock all levels", string.Empty, TextAnchor.MiddleLeft, false, false, (float)0);
        this.createOption(3, "Unlock all weapons", string.Empty, TextAnchor.MiddleLeft, false, false, (float)10);
        this.createOption(6, "Disable all directional lights", string.Empty, TextAnchor.MiddleLeft, false, false, (float)10);
        this.createOption(4, this.rootShared.GetTranslation("mBack"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)20);
        this.optionsSettingNr[0] = ((!this.rootShared.godMode) ? 0 : 1);
        this.updateOptionSettings(true);
    }

    // Token: 0x06000352 RID: 850 RVA: 0x0004AE34 File Offset: 0x00049034
    public virtual void buildExperimentalPrompt()
    {
        this.clearMenu();
        this.curActiveMenu = 0.001f;
        this.theHeader.text = this.rootShared.GetTranslation("mExpHeader");
        this.nrOfOptions = (float)1;
        this.menuBackOptionNr = 0;
        this.curOptionSortedNr = (float)0;
        this.setUpArrays();
        this.extraInfo.gameObject.SetActive(true);
        this.createOption(0, this.rootShared.GetTranslation("mProceed"), string.Empty, TextAnchor.MiddleRight, false, false, (float)300);
        this.extraInfoText.text = this.rootShared.GetTranslation("mExpNotice");
        this.updateOptionSettings(true);
    }

    // Token: 0x06000353 RID: 851 RVA: 0x0004AEE4 File Offset: 0x000490E4
    public virtual void buildMainMenu()
    {
        this.clearMenu();
        this.curActiveMenu = (float)0;
        this.theHeader.text = this.rootShared.GetTranslation("mMain");
        this.nrOfOptions = (float)7;
        if (this.rootShared.runningOnConsole)
        {
            this.nrOfOptions -= (float)1;
        }
        this.menuBackOptionNr = -1;
        this.curOptionSortedNr = (float)0;
        this.setUpArrays();
        this.createOption(0, this.rootShared.GetTranslation("mContinue"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)0);


        this.createOption(1, this.rootShared.GetTranslation("mNewGame"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)10);
        this.createOption(2, this.rootShared.GetTranslation("mLvlSel"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)0);
        this.createOption(5, this.rootShared.GetTranslation("mGameMod") + ((!this.rootShared.gameModifiersCheck()) ? string.Empty : " *"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)0);
        this.createOption(3, this.rootShared.GetTranslation("mOptions"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)10);
        this.createOption(4, this.rootShared.GetTranslation("mCredits"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)0);
        if (!this.rootShared.runningOnConsole)
        {
            this.createOption(6, this.rootShared.GetTranslation("mQuit"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)20);
        }
        if (SavedData.GetInt("level") == 0 || SavedData.GetInt("level") == 53)
        {
            this.disableOption[0] = true;
            this.curOption = 1;
            this.curOptionSortedNr = (float)1;
        }
        else
        {
            this.curOption = 0;
            this.curOptionSortedNr = (float)0;
        }
        this.updateOptionSettings(true);
        this.gameModifiersNotice.SetActive(this.rootShared.gameModifiersCheck());

    }

    // Token: 0x06000354 RID: 852 RVA: 0x0004B0E8 File Offset: 0x000492E8
    public virtual void buildNewGameMenu()
    {
        this.clearMenu();
        this.curActiveMenu = 0.1f;
        this.theHeader.text = this.rootShared.GetTranslation("mDiffLvl");
        this.nrOfOptions = (float)4;
        this.menuBackOptionNr = 3;
        this.curOptionSortedNr = (float)0;
        this.setUpArrays();
        this.extraInfo.gameObject.SetActive(true);
        this.createOption(0, this.rootShared.GetTranslation("diffLvl1"), string.Empty, TextAnchor.MiddleRight, false, false, (float)0);
        this.createOption(1, this.rootShared.GetTranslation("diffLvl2"), string.Empty, TextAnchor.MiddleRight, false, false, (float)0);
        this.createOption(2, this.rootShared.GetTranslation("diffLvl3"), string.Empty, TextAnchor.MiddleRight, false, false, (float)0);
        this.createOption(3, this.rootShared.GetTranslation("mBack"), string.Empty, TextAnchor.MiddleRight, false, false, (float)20);
        this.curOptionSortedNr = (float)0;
        this.curOption = 0;
        this.doExtraInfoUpdate(false);
        this.updateOptionSettings(true);
    }

    // Token: 0x06000355 RID: 853 RVA: 0x0004B1F4 File Offset: 0x000493F4
    public virtual void buildLevelSelectMenu()
    {
        this.clearMenu();
        this.curActiveMenu = 0.2f;
        this.theHeader.text = this.rootShared.GetTranslation("mLvlSel");
        this.nrOfOptions = (float)41;
        this.menuBackOptionNr = 40;
        this.curOptionSortedNr = (float)0;
        this.setUpArrays();
        this.extraInfo.gameObject.SetActive(true);
        this.extraInfoText.text = string.Empty;
        this.levelSelectScreenLeaderboardRectTransform.gameObject.SetActive(this.rootShared.allowLeaderboard);
        if (!this.rootShared.allowLeaderboard || (this.rootShared.allowLeaderboard && !this.rootShared.lostInternetConnection))
        {
            this.leaderboardOfflineGameObject.SetActive(false);
        }
        else if (this.rootShared.allowLeaderboard && this.rootShared.lostInternetConnection)
        {
            this.leaderboardOfflineGameObject.SetActive(true);
            this.levelSelectScreenLeaderboardRectTransform.gameObject.SetActive(false);
        }
        this.levelSelectScreenDifficultyLevel = SavedData.GetInt("difficultyMode");
        if (this.levelSelectScreenDifficultyLevel == 0)
        {
            this.levelSelectScreenDifficulty.text = this.rootShared.GetTranslation("diffLvl1");
        }
        else if (this.levelSelectScreenDifficultyLevel == 1)
        {
            this.levelSelectScreenDifficulty.text = this.rootShared.GetTranslation("diffLvl2");
        }
        else if (this.levelSelectScreenDifficultyLevel == 2)
        {
            this.levelSelectScreenDifficulty.text = this.rootShared.GetTranslation("diffLvl3");
        }
        if (this.levelSelectScreenDifficultyButton != null)
        {
            UnityEngine.Object.Destroy(this.levelSelectScreenDifficultyButton.gameObject);
        }
        this.levelSelectScreenDifficultyButton = (RectTransform)this.inputHelperScript.GetInputSymbol("UISPECIAL1", !this.useGamepadIcons).GetComponent(typeof(RectTransform));
        this.levelSelectScreenDifficultyButton.SetParent(this.levelSelectScreenDifficulty.transform.parent, false);
        this.levelSelectScreenDifficultyButton.localScale = Vector3.one;
        this.levelSelectScreenDifficultyButton.anchoredPosition = this.levelSelectScreenDifficultyRectTransform.anchoredPosition + new Vector2(this.levelSelectScreenDifficulty.preferredWidth + this.levelSelectScreenDifficultyButton.sizeDelta.x / (float)2 + (float)10, (float)0);
        if (this.levelSelectScreenDifficultyButton.anchoredPosition.x > (float)205)
        {
            int num = 205;
            Vector2 anchoredPosition = this.levelSelectScreenDifficultyButton.anchoredPosition;
            float num2 = anchoredPosition.x = (float)num;
            Vector2 vector = this.levelSelectScreenDifficultyButton.anchoredPosition = anchoredPosition;
        }
        if (this.levelSelectScreenLeaderboardTypeButton != null)
        {
            UnityEngine.Object.Destroy(this.levelSelectScreenLeaderboardTypeButton.gameObject);
        }
        this.levelSelectScreenLeaderboardTypeButton = (RectTransform)this.inputHelperScript.GetInputSymbol("UISPECIAL2", !this.useGamepadIcons).GetComponent(typeof(RectTransform));
        this.levelSelectScreenLeaderboardTypeButton.SetParent(this.levelSelectScreenLeaderboardTypeText.transform.parent, false);
        this.levelSelectScreenLeaderboardTypeButton.localScale = Vector3.one;
        float x = 42.12917f;
        Vector2 anchoredPosition2 = this.levelSelectScreenLeaderboardTypeTextRectTransform.anchoredPosition;
        float num3 = anchoredPosition2.x = x;
        Vector2 vector2 = this.levelSelectScreenLeaderboardTypeTextRectTransform.anchoredPosition = anchoredPosition2;
        this.levelSelectScreenLeaderboardTypeButton.anchoredPosition = this.levelSelectScreenLeaderboardTypeTextRectTransform.anchoredPosition - new Vector2(this.levelSelectScreenLeaderboardTypeButton.sizeDelta.x / (float)2 + (float)10, (float)0);
        float num4 = this.levelSelectScreenLeaderboardTypeButton.anchoredPosition.x - this.levelSelectScreenLeaderboardTypeButton.sizeDelta.x / (float)2 - (float)5;
        if (num4 < (float)0)
        {
            float x2 = this.levelSelectScreenLeaderboardTypeButton.anchoredPosition.x + -num4;
            Vector2 anchoredPosition3 = this.levelSelectScreenLeaderboardTypeButton.anchoredPosition;
            float num5 = anchoredPosition3.x = x2;
            Vector2 vector3 = this.levelSelectScreenLeaderboardTypeButton.anchoredPosition = anchoredPosition3;
            float x3 = this.levelSelectScreenLeaderboardTypeTextRectTransform.anchoredPosition.x + -num4;
            Vector2 anchoredPosition4 = this.levelSelectScreenLeaderboardTypeTextRectTransform.anchoredPosition;
            float num6 = anchoredPosition4.x = x3;
            Vector2 vector4 = this.levelSelectScreenLeaderboardTypeTextRectTransform.anchoredPosition = anchoredPosition4;
        }
        if (this.levelSelectScreenLeaderboardPageButton1 != null)
        {
            UnityEngine.Object.Destroy(this.levelSelectScreenLeaderboardPageButton1.gameObject);
        }
        this.levelSelectScreenLeaderboardPageButton1 = (RectTransform)this.inputHelperScript.GetInputSymbol("UISPECIAL3", !this.useGamepadIcons).GetComponent(typeof(RectTransform));
        this.levelSelectScreenLeaderboardPageButton1.SetParent(this.levelSelectScreenLeaderboardPagePrevTextRectTransform.parent, false);
        this.levelSelectScreenLeaderboardPageButton1.localScale = Vector3.one;
        this.levelSelectScreenLeaderboardPageButton1.anchoredPosition = this.levelSelectScreenLeaderboardPagePrevTextRectTransform.anchoredPosition - new Vector2(this.levelSelectScreenLeaderboardPageButton1.sizeDelta.x / (float)2 + (float)10, (float)0);
        if (this.levelSelectScreenLeaderboardPageButton2 != null)
        {
            UnityEngine.Object.Destroy(this.levelSelectScreenLeaderboardPageButton2.gameObject);
        }
        this.levelSelectScreenLeaderboardPageButton2 = (RectTransform)this.inputHelperScript.GetInputSymbol("UISPECIAL4", !this.useGamepadIcons).GetComponent(typeof(RectTransform));
        this.levelSelectScreenLeaderboardPageButton2.SetParent(this.levelSelectScreenLeaderboardPageNextTextRectTransform.parent, false);
        this.levelSelectScreenLeaderboardPageButton2.localScale = Vector3.one;
        this.levelSelectScreenLeaderboardPageButton2.anchoredPosition = this.levelSelectScreenLeaderboardPageNextTextRectTransform.anchoredPosition - new Vector2(this.levelSelectScreenLeaderboardPageButton2.sizeDelta.x / (float)2 + (float)10, (float)0);
        if (this.leaderboardOfflineButton != null)
        {
            UnityEngine.Object.Destroy(this.leaderboardOfflineButton.gameObject);
        }
        this.leaderboardOfflineButton = (RectTransform)this.inputHelperScript.GetInputSymbol("UISPECIAL2", !this.useGamepadIcons).GetComponent(typeof(RectTransform));
        this.leaderboardOfflineButton.anchorMax = (this.leaderboardOfflineButton.anchorMin = new Vector2((float)0, (float)1));
        this.leaderboardOfflineButton.SetParent(this.leaderboardOfflineTextRectTransform.parent, false);
        this.leaderboardOfflineButton.localScale = Vector3.one;
        this.leaderboardOfflineButton.anchoredPosition = this.leaderboardOfflineTextRectTransform.anchoredPosition - new Vector2(this.leaderboardOfflineButton.sizeDelta.x / (float)2 + (float)10, (float)0);
        this.createOption(40, this.rootShared.GetTranslation("mBack"), string.Empty, TextAnchor.MiddleRight, false, false, (float)0);

        this.createSmallHeader(this.rootShared.GetTranslation("mClassicTheme"), (float)10);
        this.createOption(0, this.rootShared.GetTranslation("mClassicTutorial"), string.Empty, TextAnchor.MiddleRight, false, false, (float)10);
        this.createOption(1, this.rootShared.GetTranslation("mLvl1"), string.Empty, TextAnchor.MiddleRight, false, false, (float)0);
        this.createOption(2, this.rootShared.GetTranslation("mLvl2"), string.Empty, TextAnchor.MiddleRight, false, false, (float)0);
        this.createOption(3, this.rootShared.GetTranslation("mLvl3"), string.Empty, TextAnchor.MiddleRight, false, false, (float)0);
        this.createOption(4, this.rootShared.GetTranslation("mLvl4"), string.Empty, TextAnchor.MiddleRight, false, false, (float)0);
        this.createOption(5, this.rootShared.GetTranslation("mLvl5"), string.Empty, TextAnchor.MiddleRight, false, false, (float)0);
        this.createOption(7, this.rootShared.GetTranslation("mAgren"), string.Empty, TextAnchor.MiddleRight, false, false, (float)0);

        int @int = 0;

        if (PlatformPlayerPrefs.HasKey("mfpClassicUnlockedLevels"))
            @int = PlatformPlayerPrefs.GetInt("mfpClassicUnlockedLevels");

        for (int i = 0; i <= 7; i++)
        {
            if (this.getLevelSelectLevelNr(i) > @int)
            {
                MFPClassic.MFPEditorUtils.Log("Level select nr: " + getLevelSelectLevelNr(i).ToString() + " " + @int.ToString());
                this.disableOption[i] = true;
            }
        }
        this.curOptionSortedNr = (float)0;
        this.curOption = 40;
        this.updateOptionSettings(true);
    }

    // Token: 0x06000356 RID: 854 RVA: 0x0004BF24 File Offset: 0x0004A124
    public virtual void createGameModifierOption(int optionNumber, float addExtraSpace, bool isSlider, string translationString, int unlockWorld, int unlockLevel)
    {
        if (SavedData.GetInt(translationString + "Unlocked") == 1)
        {
            this.createOption(optionNumber, this.rootShared.GetTranslation(translationString), (!isSlider) ? (this.rootShared.GetTranslation("mOff") + "|" + this.rootShared.GetTranslation("mOn")) : "Slider", TextAnchor.MiddleRight, true, true, addExtraSpace);
        }
        else
        {
            this.createOption(optionNumber, this.rootShared.GetTranslation("mLocked"), "[ " + this.rootShared.GetTranslation("mFoundIn") + " <color=#aaaaaaff>" + this.rootShared.GetTranslation("mTheme" + unlockWorld) + " - " + this.rootShared.GetTranslation("mLvl" + unlockLevel) + "</color> ]", TextAnchor.MiddleRight, true, true, addExtraSpace);
            this.disableOption[optionNumber] = true;
        }
    }

    // Token: 0x06000357 RID: 855 RVA: 0x0004C058 File Offset: 0x0004A258
    public virtual void buildGameModifiersMenu()
    {
        this.clearMenu();
        this.curActiveMenu = (float)7;
        this.theHeader.text = this.rootShared.GetTranslation("mGameMod");
        if (SavedData.GetInt("levelSelectMaxNr") < 53)
        {
            this.nrOfOptions = (float)1;
            this.menuBackOptionNr = 0;
            this.curOptionSortedNr = (float)0;
            this.setUpArrays();
            this.createOption(0, this.rootShared.GetTranslation("mBack"), string.Empty, TextAnchor.MiddleRight, false, false, (float)100);
            this.extraInfo.gameObject.SetActive(true);
            this.extraInfoText.text = this.rootShared.GetTranslation("mModInfo");
        }
        else
        {
            this.nrOfOptions = (float)16;
            this.menuBackOptionNr = 0;
            this.curOptionSortedNr = (float)0;
            this.setUpArrays();
            this.createOption(0, this.rootShared.GetTranslation("mBack"), string.Empty, TextAnchor.MiddleRight, false, false, (float)0);
            this.createOption(1, this.rootShared.GetTranslation("mDefault"), string.Empty, TextAnchor.MiddleCenter, false, false, (float)5);
            this.createSmallHeader(this.rootShared.GetTranslation("mGameMod"), (float)10);
            this.createOption(2, this.rootShared.GetTranslation("mAllWep"), this.rootShared.GetTranslation("mOff") + "|" + this.rootShared.GetTranslation("mOn"), TextAnchor.MiddleRight, true, true, (float)5);
            this.createGameModifierOption(3, (float)0, false, "mInfAm", 1, 7);
            this.createGameModifierOption(7, (float)0, false, "mIncAccur", 4, 2);
            this.createGameModifierOption(15, (float)0, true, "mBulSpeP", 2, 6);
            this.createGameModifierOption(14, (float)0, true, "mBulSpeE", 4, 7);
            this.createGameModifierOption(4, (float)10, false, "mOneSEn", 2, 1);
            this.createGameModifierOption(5, (float)0, false, "MOneSPl", 5, 1);
            this.createGameModifierOption(10, (float)10, false, "mInfFocus", 4, 8);
            this.createGameModifierOption(8, (float)0, true, "mSlowScale", 4, 9);
            this.createGameModifierOption(9, (float)0, true, "mPlaySpeed", 4, 3);
            this.createGameModifierOption(12, (float)10, false, "mBigHeads", 3, 4);
            this.createGameModifierOption(13, (float)0, true, "mPlaySize", 2, 4);
            this.createGameModifierOption(6, (float)10, false, "mCinematic", 2, 8);
            this.createGameModifierOption(11, (float)0, false, "mSideCam", 3, 3);
            this.optionsSettingNr[2] = ((!this.rootShared.modAllWeapons) ? 0 : 1);
            this.optionsSettingNr[3] = ((!this.rootShared.modInfiniteAmmo) ? 0 : 1);
            this.optionsSettingNr[4] = ((!this.rootShared.modOneShotEnemies) ? 0 : 1);
            this.optionsSettingNr[5] = ((!this.rootShared.modOneShotPlayer) ? 0 : 1);
            this.optionsSettingNr[7] = ((!this.rootShared.modIncreaseAccuracy) ? 0 : 1);
            this.optionsSettingNr[8] = (int)this.rootShared.modFocusSlowdownScale;
            this.optionsSettingNr[9] = (int)this.rootShared.modPlayerSpeed;
            this.optionsSettingNr[10] = ((!this.rootShared.modInfiniteFocus) ? 0 : 1);
            this.optionsSettingNr[11] = ((!this.rootShared.modSideOnCamera) ? 0 : 1);
            this.optionsSettingNr[12] = ((!this.rootShared.modBigHead) ? 0 : 1);
            this.optionsSettingNr[13] = (int)this.rootShared.modPlayerSize;
            this.optionsSettingNr[14] = (int)this.rootShared.modEnemyBulletSpeed;
            this.optionsSettingNr[15] = (int)this.rootShared.modPlayerBulletSpeed;
            this.optionsSettingNr[6] = ((!this.rootShared.modCinematicCamera) ? 0 : 1);
        }
        this.curOptionSortedNr = (float)0;
        this.curOption = 0;
        this.updateOptionSettings(true);
    }

    // Token: 0x06000358 RID: 856 RVA: 0x0004C454 File Offset: 0x0004A654
    public virtual void buildOptionsMenu()
    {
        this.clearMenu();
        this.curActiveMenu = (float)1;
        this.theHeader.text = this.rootShared.GetTranslation("mOptions");
        this.nrOfOptions = (float)6;
        if (this.rootShared.runningOnConsole)
        {
            this.nrOfOptions -= (float)1;
        }
        this.menuBackOptionNr = 1;
        this.curOptionSortedNr = (float)0;
        this.setUpArrays();
        this.createOption(0, this.rootShared.GetTranslation("mControls"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)0);
        if (!this.rootShared.runningOnConsole)
        {
            this.createOption(5, this.rootShared.GetTranslation("mVid"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)0);
        }
        this.createOption(2, this.rootShared.GetTranslation("mAud"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)0);
        this.createOption(3, this.rootShared.GetTranslation("mGame"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)0);
        this.createOption(4, this.rootShared.GetTranslation("mLang"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)0);
        this.createOption(1, this.rootShared.GetTranslation("mBack"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)20);
        if (this.inGame)
        {
            this.disableOption[4] = true;
        }
        this.updateOptionSettings(true);
        this.gameModifiersNotice.SetActive(false);
    }

    // Token: 0x06000359 RID: 857 RVA: 0x0004C5D0 File Offset: 0x0004A7D0
    public virtual void SetInputButton(string theAction, bool positiveAxis, bool isKeyboard, int range, string category)
    {
        MonoBehaviour.print("Setting input for " + theAction);
        InputMapper.Context context = new InputMapper.Context();
        Controller controllerToUse = this.GetControllerToUse(isKeyboard, false);
        if (!RuntimeServices.EqualityOperator(controllerToUse, null))
        {
            ActionElementMap actionElementMapToUse = this.GetActionElementMapToUse(controllerToUse, theAction, positiveAxis);
            context.actionName = theAction;
            context.controllerMap = this.player.controllers.maps.GetMap(controllerToUse.type, controllerToUse.id, category, "Default");
            if (!RuntimeServices.EqualityOperator(actionElementMapToUse, null))
            {
                context.controllerMap.DeleteElementMap(actionElementMapToUse.id);
            }
            if (range == -1)
            {
                context.actionRange = AxisRange.Negative;
            }
            else if (range == 1)
            {
                context.actionRange = AxisRange.Positive;
            }
            else
            {
                context.actionRange = AxisRange.Full;
            }
            this.inputMapper.Start(context);
            if (isKeyboard)
            {
                InputMapper.Context context2 = new InputMapper.Context();
                controllerToUse = this.GetControllerToUse(isKeyboard, true);
                actionElementMapToUse = this.GetActionElementMapToUse(controllerToUse, theAction, positiveAxis);
                context2.actionName = theAction;
                context2.controllerMap = this.player.controllers.maps.GetMap(controllerToUse.type, controllerToUse.id, category, "Default");
                if (!RuntimeServices.EqualityOperator(actionElementMapToUse, null))
                {
                    context2.controllerMap.DeleteElementMap(actionElementMapToUse.id);
                }
                if (range == -1)
                {
                    context2.actionRange = AxisRange.Negative;
                }
                else if (range == 1)
                {
                    context2.actionRange = AxisRange.Positive;
                }
                else
                {
                    context2.actionRange = AxisRange.Full;
                }
                this.inputMapper2.Start(context2);
            }
            this.optionsInputSymbolHolder[this.curOption].gameObject.SetActive(false);
            this.optionsSettingText[this.curOption].text = this.rootShared.GetTranslation("mWaitInput");
            this.uiConfirmHintCanvasGroup.alpha = 0.1f;
            this.uiBackHintCanvasGroup.alpha = 0.1f;
        }
    }

    // Token: 0x0600035A RID: 858 RVA: 0x00003A88 File Offset: 0x00001C88
    public virtual Controller GetControllerToUse(bool isKeyboard, bool getMouse)
    {
        return this.inputHelperScript.GetControllerToUse(isKeyboard, getMouse);
    }

    // Token: 0x0600035B RID: 859 RVA: 0x00003A97 File Offset: 0x00001C97
    public virtual ActionElementMap GetActionElementMapToUse(Controller controller, string theAction, bool positiveAxis)
    {
        return this.inputHelperScript.GetActionElementMapToUse(controller, theAction, positiveAxis);
    }

    // Token: 0x0600035C RID: 860 RVA: 0x00003AA7 File Offset: 0x00001CA7
    public virtual string GetInputButtonName(string theAction, bool positiveAxis, bool isKeyboard)
    {
        return this.inputHelperScript.GetInputButtonName(theAction, positiveAxis, isKeyboard);
    }

    // Token: 0x0600035D RID: 861 RVA: 0x00003AB7 File Offset: 0x00001CB7
    public virtual bool checkForMissingInput(bool isKeyboard)
    {
        return this.inputHelperScript.checkForMissingInput(isKeyboard);
    }

    // Token: 0x0600035E RID: 862 RVA: 0x0004C7BC File Offset: 0x0004A9BC
    public virtual void buildKeyboardControlsMenu()
    {
        this.clearMenu();
        this.curActiveMenu = 2.1f;
        this.theHeader.text = this.rootShared.GetTranslation("mKBM");
        this.nrOfOptions = (float)29;
        this.menuBackOptionNr = 0;
        this.curOptionSortedNr = (float)0;
        this.setUpArrays();
        float num = (float)2;
        this.createOption(0, this.rootShared.GetTranslation("mBack"), string.Empty, TextAnchor.MiddleRight, false, false, (float)0);
        this.createOption(26, this.rootShared.GetTranslation("mCurMode"), this.rootShared.GetTranslation("mCurM1") + "|" + this.rootShared.GetTranslation("mCurM2"), TextAnchor.MiddleRight, true, true, (float)10);
        this.createOption(27, this.rootShared.GetTranslation("mAimSens"), "Slider", TextAnchor.MiddleRight, true, true, (float)0);
        this.createOption(15, this.rootShared.GetTranslation("mDefault"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)10);
        this.createSmallHeader(this.rootShared.GetTranslation("mMove"), (float)10);
        this.createOptionWithInputSymbol(1, this.rootShared.GetTranslation("mMoveL"), this.GetInputButtonName("Move", false, true), true, TextAnchor.MiddleRight, false, true, num);
        this.createOptionWithInputSymbol(2, this.rootShared.GetTranslation("mMoveR"), this.GetInputButtonName("Move", true, true), true, TextAnchor.MiddleRight, false, true, num);
        this.createOptionWithInputSymbol(3, this.rootShared.GetTranslation("mJump"), this.GetInputButtonName("Jump", true, true), true, TextAnchor.MiddleRight, false, true, num);
        this.createOptionWithInputSymbol(4, this.rootShared.GetTranslation("mCrouch"), this.GetInputButtonName("Crouch", false, true), true, TextAnchor.MiddleRight, false, true, num);
        this.createOptionWithInputSymbol(5, this.rootShared.GetTranslation("mDodge"), this.GetInputButtonName("Dodge", true, true), true, TextAnchor.MiddleRight, false, true, num);
        this.createOptionWithInputSymbol(7, this.rootShared.GetTranslation("mFocus"), this.GetInputButtonName("Focus", true, true), true, TextAnchor.MiddleRight, false, true, num);
        this.createSmallHeader(this.rootShared.GetTranslation("mActions"), (float)0);
        this.createOptionWithInputSymbol(6, this.rootShared.GetTranslation("mInteract"), this.GetInputButtonName("Interact", true, true), true, TextAnchor.MiddleRight, false, true, num);
        this.createOptionWithInputSymbol(8, this.rootShared.GetTranslation("mKick"), this.GetInputButtonName("Kick", true, true), true, TextAnchor.MiddleRight, false, true, num);
        this.createOptionWithInputSymbol(9, this.rootShared.GetTranslation("mFire"), this.GetInputButtonName("Fire", true, true), true, TextAnchor.MiddleRight, false, true, num);
        this.createOptionWithInputSymbol(10, this.rootShared.GetTranslation("mFire2"), this.GetInputButtonName("Fire2", true, true), true, TextAnchor.MiddleRight, false, true, num);
        this.createOptionWithInputSymbol(11, this.rootShared.GetTranslation("mReload"), this.GetInputButtonName("Reload", true, true), true, TextAnchor.MiddleRight, false, true, num);
        this.createSmallHeader(this.rootShared.GetTranslation("mWeapons"), (float)0);
        this.createOptionWithInputSymbol(12, this.rootShared.GetTranslation("mChgWep"), this.GetInputButtonName("Change Weapon", true, true), true, TextAnchor.MiddleRight, false, true, num);
        this.createOptionWithInputSymbol(13, this.rootShared.GetTranslation("mNextWep"), this.GetInputButtonName("Scroll Weapon", true, true), true, TextAnchor.MiddleRight, false, true, num);
        this.createOptionWithInputSymbol(14, this.rootShared.GetTranslation("mPrevWep"), this.GetInputButtonName("Scroll Weapon", false, true), true, TextAnchor.MiddleRight, false, true, num);
        this.createOptionWithInputSymbol(16, this.rootShared.GetTranslation("mWep1"), this.GetInputButtonName("Pistol", true, true), true, TextAnchor.MiddleRight, false, true, (float)10 + num);
        this.createOptionWithInputSymbol(17, this.rootShared.GetTranslation("mWep2"), this.GetInputButtonName("Dual Pistols", true, true), true, TextAnchor.MiddleRight, false, true, num);
        this.createOptionWithInputSymbol(18, this.rootShared.GetTranslation("mWep3"), this.GetInputButtonName("Submachine Gun", true, true), true, TextAnchor.MiddleRight, false, true, num);
        this.createOptionWithInputSymbol(19, this.rootShared.GetTranslation("mWep4"), this.GetInputButtonName("Dual Submachine Guns", true, true), true, TextAnchor.MiddleRight, false, true, num);
        this.createOptionWithInputSymbol(20, this.rootShared.GetTranslation("mWep5"), this.GetInputButtonName("Shotgun", true, true), true, TextAnchor.MiddleRight, false, true, num);
        this.createOptionWithInputSymbol(21, this.rootShared.GetTranslation("mWep6"), this.GetInputButtonName("Assault Rifle", true, true), true, TextAnchor.MiddleRight, false, true, num);
        this.createOptionWithInputSymbol(22, this.rootShared.GetTranslation("mWep7"), this.GetInputButtonName("Rifle", true, true), true, TextAnchor.MiddleRight, false, true, num);
        this.createSmallHeader(this.rootShared.GetTranslation("mSpecial"), (float)0);
        this.createOptionWithInputSymbol(23, this.rootShared.GetTranslation("mMotoUp"), this.GetInputButtonName("MotorcycleY", true, true), true, TextAnchor.MiddleRight, false, true, num);
        this.createOptionWithInputSymbol(24, this.rootShared.GetTranslation("mMotoDown"), this.GetInputButtonName("MotorcycleY", false, true), true, TextAnchor.MiddleRight, false, true, num);
        this.createOptionWithInputSymbol(25, this.rootShared.GetTranslation("mMotoWheel"), this.GetInputButtonName("MotorcycleWheelie", true, true), true, TextAnchor.MiddleRight, false, true, num);
        this.createOptionWithInputSymbol(28, this.rootShared.GetTranslation("mRestart"), this.GetInputButtonName("Restart Level", true, true), true, TextAnchor.MiddleRight, false, true, num);
        this.optionsSettingNr[26] = ((!this.rootShared.simulateMousePos) ? 0 : 1);
        this.optionsSettingNr[27] = Mathf.RoundToInt(this.rootShared.simulateMousePosSensitivity * (float)100);
        this.disableOption[27] = (this.optionsSettingNr[26] == 0);
        this.curOptionSortedNr = (float)0;
        this.curOption = 0;
        this.updateOptionSettings(true);
    }

    // Token: 0x0600035F RID: 863 RVA: 0x0004CDB4 File Offset: 0x0004AFB4
    public virtual void buildGamepadControlsMenu()
    {
        this.clearMenu();
        this.curActiveMenu = 2.2f;
        if (this.rootShared.runningOnConsole)
        {
            this.theHeader.text = this.rootShared.GetTranslation("mControls");
        }
        else
        {
            this.theHeader.text = this.rootShared.GetTranslation("mGamepad");
        }
        this.nrOfOptions = (float)26;
        this.menuBackOptionNr = 0;
        this.curOptionSortedNr = (float)0;
        this.setUpArrays();
        if (!this.dontCheckForMissingGamepadInputsMapped && this.checkForMissingInput(false))
        {
            this.player.controllers.maps.LoadDefaultMaps(ControllerType.Joystick);
        }
        this.dontCheckForMissingGamepadInputsMapped = false;
        float extraYOffset = (float)2;
        this.createOption(0, this.rootShared.GetTranslation("mBack"), string.Empty, TextAnchor.MiddleRight, false, false, (float)0);
        this.createOption(23, (Application.platform != RuntimePlatform.Switch) ? this.rootShared.GetTranslation("mVibrate") : this.rootShared.GetTranslation("mRumble"), this.rootShared.GetTranslation("mOff") + "|" + this.rootShared.GetTranslation("mOn"), TextAnchor.MiddleRight, true, true, (float)10);
        this.createOption(24, this.rootShared.GetTranslation("mAimSens"), "Slider", TextAnchor.MiddleRight, true, true, (float)0);
        this.createOption(25, this.rootShared.GetTranslation("mAimAssist"), this.rootShared.GetTranslation("mQLow") + "|" + this.rootShared.GetTranslation("mQMid") + "|" + this.rootShared.GetTranslation("mQHi"), TextAnchor.MiddleRight, true, true, (float)0);
        this.createOption(19, this.rootShared.GetTranslation("mDefault"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)10);
        this.createSmallHeader(this.rootShared.GetTranslation("mAim"), (float)10);
        this.createOptionWithInputSymbol(15, this.rootShared.GetTranslation("mAimL"), this.GetInputButtonName("Aim Horizontal", false, false), false, TextAnchor.MiddleRight, false, true, extraYOffset);
        this.createOptionWithInputSymbol(16, this.rootShared.GetTranslation("mAimR"), this.GetInputButtonName("Aim Horizontal", true, false), false, TextAnchor.MiddleRight, false, true, extraYOffset);
        this.createOptionWithInputSymbol(17, this.rootShared.GetTranslation("mAimU"), this.GetInputButtonName("Aim Vertical", true, false), false, TextAnchor.MiddleRight, false, true, extraYOffset);
        this.createOptionWithInputSymbol(18, this.rootShared.GetTranslation("mAimD"), this.GetInputButtonName("Aim Vertical", false, false), false, TextAnchor.MiddleRight, false, true, extraYOffset);
        this.createSmallHeader(this.rootShared.GetTranslation("mMove"), (float)0);
        this.createOptionWithInputSymbol(1, this.rootShared.GetTranslation("mMoveL"), this.GetInputButtonName("Move", false, false), false, TextAnchor.MiddleRight, false, true, extraYOffset);
        this.createOptionWithInputSymbol(2, this.rootShared.GetTranslation("mMoveR"), this.GetInputButtonName("Move", true, false), false, TextAnchor.MiddleRight, false, true, extraYOffset);
        this.createOptionWithInputSymbol(3, this.rootShared.GetTranslation("mJump"), this.GetInputButtonName("Jump", true, false), false, TextAnchor.MiddleRight, false, true, extraYOffset);
        this.createOptionWithInputSymbol(4, this.rootShared.GetTranslation("mCrouch"), this.GetInputButtonName("Crouch", false, false), false, TextAnchor.MiddleRight, false, true, extraYOffset);
        this.createOptionWithInputSymbol(5, this.rootShared.GetTranslation("mDodge"), this.GetInputButtonName("Dodge", true, false), false, TextAnchor.MiddleRight, false, true, extraYOffset);
        this.createOptionWithInputSymbol(7, this.rootShared.GetTranslation("mFocus"), this.GetInputButtonName("Focus", true, false), false, TextAnchor.MiddleRight, false, true, extraYOffset);
        this.createSmallHeader(this.rootShared.GetTranslation("mActions"), (float)0);
        this.createOptionWithInputSymbol(6, this.rootShared.GetTranslation("mInteract"), this.GetInputButtonName("Interact", true, false), false, TextAnchor.MiddleRight, false, true, extraYOffset);
        this.createOptionWithInputSymbol(8, this.rootShared.GetTranslation("mKick"), this.GetInputButtonName("Kick", true, false), false, TextAnchor.MiddleRight, false, true, extraYOffset);
        this.createOptionWithInputSymbol(9, this.rootShared.GetTranslation("mFire"), this.GetInputButtonName("Fire", true, false), false, TextAnchor.MiddleRight, false, true, extraYOffset);
        this.createOptionWithInputSymbol(10, this.rootShared.GetTranslation("mFire2"), this.GetInputButtonName("Fire2", true, false), false, TextAnchor.MiddleRight, false, true, extraYOffset);
        this.createOptionWithInputSymbol(11, this.rootShared.GetTranslation("mReload"), this.GetInputButtonName("Reload", true, false), false, TextAnchor.MiddleRight, false, true, extraYOffset);
        this.createSmallHeader(this.rootShared.GetTranslation("mWeapons"), (float)0);
        this.createOptionWithInputSymbol(12, this.rootShared.GetTranslation("mChgWep"), this.GetInputButtonName("Change Weapon", true, false), false, TextAnchor.MiddleRight, false, true, extraYOffset);
        this.createOptionWithInputSymbol(13, this.rootShared.GetTranslation("mNextWep"), this.GetInputButtonName("Scroll Weapon", true, false), false, TextAnchor.MiddleRight, false, true, extraYOffset);
        this.createOptionWithInputSymbol(14, this.rootShared.GetTranslation("mPrevWep"), this.GetInputButtonName("Scroll Weapon", false, false), false, TextAnchor.MiddleRight, false, true, extraYOffset);
        this.createSmallHeader(this.rootShared.GetTranslation("mSpecial"), (float)0);
        this.createOptionWithInputSymbol(20, this.rootShared.GetTranslation("mMotoUp"), this.GetInputButtonName("MotorcycleY", true, false), false, TextAnchor.MiddleRight, false, true, extraYOffset);
        this.createOptionWithInputSymbol(21, this.rootShared.GetTranslation("mMotoDown"), this.GetInputButtonName("MotorcycleY", false, false), false, TextAnchor.MiddleRight, false, true, extraYOffset);
        this.createOptionWithInputSymbol(22, this.rootShared.GetTranslation("mMotoWheel"), this.GetInputButtonName("MotorcycleWheelie", true, false), false, TextAnchor.MiddleRight, false, true, extraYOffset);
        this.optionsSettingNr[23] = (this.rootShared.disableRumble ? 0 : 1);
        this.optionsSettingNr[24] = Mathf.RoundToInt(this.rootShared.gamepadAimSens * (float)100);
        this.optionsSettingNr[25] = this.rootShared.aimAssistMode;
        this.curOptionSortedNr = (float)0;
        this.curOption = 0;
        this.lastActiveGamepadController = this.player.controllers.GetLastActiveController(ControllerType.Joystick);
        this.updateOptionSettings(true);
    }

    // Token: 0x06000360 RID: 864 RVA: 0x0004D414 File Offset: 0x0004B614
    public virtual void buildControlsMenu()
    {
        this.clearMenu();
        this.curActiveMenu = (float)2;
        this.theHeader.text = this.rootShared.GetTranslation("mControls");
        this.nrOfOptions = (float)3;
        this.menuBackOptionNr = 2;
        this.curOptionSortedNr = (float)0;
        this.setUpArrays();
        this.createOption(0, this.rootShared.GetTranslation("mSetKBM"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)0);
        this.createOption(1, this.rootShared.GetTranslation("mSetGamepad"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)0);
        this.createOption(2, this.rootShared.GetTranslation("mBack"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)20);
        if (RuntimeServices.EqualityOperator(this.inputHelperScript.GetControllerToUse(false, false), null))
        {
            this.disableOption[1] = true;
        }
        this.updateOptionSettings(true);
    }

    // Token: 0x06000361 RID: 865 RVA: 0x0004D4FC File Offset: 0x0004B6FC
    public virtual void buildVideoMenu()
    {
        this.clearMenu();
        this.curActiveMenu = (float)3;
        this.theHeader.text = this.rootShared.GetTranslation("mVid");
        this.nrOfOptions = (float)7;
        this.menuBackOptionNr = 4;
        this.curOptionSortedNr = (float)0;
        this.setUpArrays();
        string text = string.Empty;
        bool flag = default(bool);
        this.optionsSettingNr[0] = this.supportedResolutions.Length - 1;
        for (int i = 0; i < this.supportedResolutions.Length; i++)
        {
            text += this.supportedResolutions[i].width + "x" + this.supportedResolutions[i].height;
            if (i < this.supportedResolutions.Length - 1)
            {
                text += "|";
            }
            if (this.optionsSettingNr[0] == this.supportedResolutions.Length - 1)
            {
                if (Screen.width == this.supportedResolutions[i].width && Screen.height == this.supportedResolutions[i].height)
                {
                    this.optionsSettingNr[0] = i;
                    flag = true;
                }
                if (!flag && Screen.width < this.supportedResolutions[i].width)
                {
                    this.optionsSettingNr[0] = i;
                }
            }
        }
        this.createOption(0, this.rootShared.GetTranslation("mRes"), text, TextAnchor.MiddleRight, true, false, (float)0);
        this.createOption(2, this.rootShared.GetTranslation("mFullSc"), this.rootShared.GetTranslation("mOff") + "|" + this.rootShared.GetTranslation("mOn"), TextAnchor.MiddleRight, true, true, (float)0);
        this.createOption(1, this.rootShared.GetTranslation("mQual"), this.rootShared.GetTranslation("mQLow") + "|" + this.rootShared.GetTranslation("mQMid") + "|" + this.rootShared.GetTranslation("mQHi"), TextAnchor.MiddleRight, true, true, (float)0);
        this.createOption(5, this.rootShared.GetTranslation("mVSync"), this.rootShared.GetTranslation("mOff") + "|" + this.rootShared.GetTranslation("mOn"), TextAnchor.MiddleRight, true, true, (float)10);
        this.createOption(6, this.rootShared.GetTranslation("mFPS"), this.rootShared.GetTranslation("mOff") + "|30|60|75|100|120|144|165|240", TextAnchor.MiddleRight, true, true, (float)0);
        this.createOption(3, this.rootShared.GetTranslation("mApply"), string.Empty, TextAnchor.MiddleRight, false, false, (float)20);
        this.createOption(4, this.rootShared.GetTranslation("mBack"), string.Empty, TextAnchor.MiddleRight, false, false, (float)0);
        this.optionsSettingText[0].text = string.Empty;
        if (this.optionsSettingNr[0] > 0)
        {
            this.optionsSettingText[0].text = this.optionsSettingText[0].text + "<    ";
        }
        this.optionsSettingText[0].text = this.optionsSettingText[0].text + (Screen.width + "x" + Screen.height);
        if (this.optionsSettingNr[0] < this.supportedResolutions.Length - 1)
        {
            this.optionsSettingText[0].text = this.optionsSettingText[0].text + "    >";
        }
        this.optionsSettingNr[1] = QualitySettings.GetQualityLevel();
        this.optionsSettingNr[2] = ((!Screen.fullScreen) ? 0 : 1);
        this.optionsSettingNr[5] = (int)Mathf.Clamp01((float)QualitySettings.vSyncCount);
        if (PlatformPlayerPrefs.HasKey("TargetFPS"))
        {
            this.optionsSettingNr[6] = PlatformPlayerPrefs.GetInt("TargetFPS");
        }
        else
        {
            this.optionsSettingNr[6] = 2;
        }
        this.disableOption[3] = true;
        this.disableOption[6] = (this.optionsSettingNr[5] == 1);
        this.updateOptionSettings(true);
    }

    // Token: 0x06000362 RID: 866 RVA: 0x0004D974 File Offset: 0x0004BB74
    public virtual void buildAudioMenu()
    {
        this.clearMenu();
        this.curActiveMenu = (float)4;
        this.theHeader.text = this.rootShared.GetTranslation("mAud");
        this.nrOfOptions = (float)3;
        this.menuBackOptionNr = 2;
        this.curOptionSortedNr = (float)0;
        this.setUpArrays();
        this.createOption(0, this.rootShared.GetTranslation("mSFXVol"), "Slider", TextAnchor.MiddleRight, true, true, (float)0);
        this.createOption(1, this.rootShared.GetTranslation("mMusicVol"), "Slider", TextAnchor.MiddleRight, true, true, (float)0);
        this.createOption(2, this.rootShared.GetTranslation("mBack"), string.Empty, TextAnchor.MiddleRight, false, false, (float)20);
        float num = 0f;
        this.audioMixer.GetFloat("MusicMasterVolume", out num);
        if (num <= (float)-80)
        {
            num = (float)0;
        }
        else
        {
            num = ((float)40 + num) / (float)40 * (float)100;
        }
        this.optionsSettingNr[1] = Mathf.RoundToInt(num);
        float num2 = 0f;
        this.audioMixer.GetFloat("SFXMasterVolume", out num2);
        if (num2 <= (float)-80)
        {
            num2 = (float)0;
        }
        else
        {
            num2 = ((float)40 + num2) / (float)40 * (float)100;
        }
        this.optionsSettingNr[0] = Mathf.RoundToInt(num2);
        if (this.player.GetButton("UISpecial1") && this.player.GetButton("UISpecial4") && this.player.GetButton("UISpecial3"))
        {
            SavedData.SetInt("levelSelectMaxNr", 53);
            SavedData.SetInt("mInfAmUnlocked", 1);
            SavedData.SetInt("mIncAccurUnlocked", 1);
            SavedData.SetInt("mBulSpePUnlocked", 1);
            SavedData.SetInt("mBulSpeEUnlocked", 1);
            SavedData.SetInt("mOneSEnUnlocked", 1);
            SavedData.SetInt("MOneSPlUnlocked", 1);
            SavedData.SetInt("mInfFocusUnlocked", 1);
            SavedData.SetInt("mSlowScaleUnlocked", 1);
            SavedData.SetInt("mPlaySpeedUnlocked", 1);
            SavedData.SetInt("mBigHeadsUnlocked", 1);
            SavedData.SetInt("mPlaySizeUnlocked", 1);
            SavedData.SetInt("mCinematicUnlocked", 1);
            SavedData.SetInt("mSideCamUnlocked", 1);
        }
        this.updateOptionSettings(true);
    }

    // Token: 0x06000363 RID: 867 RVA: 0x0004DB9C File Offset: 0x0004BD9C
    public virtual void buildGameplayMenu()
    {
        this.clearMenu();
        this.curActiveMenu = (float)5;
        this.theHeader.text = this.rootShared.GetTranslation("mGame");
        this.nrOfOptions = (float)9;
        if (this.rootShared.runningOnConsole)
        {
            this.nrOfOptions -= (float)1;
        }
        if (this.rootShared.chineseBuild)
        {
        }
        this.menuBackOptionNr = 0;
        this.curOptionSortedNr = (float)0;
        this.setUpArrays();
        this.createOption(1, this.rootShared.GetTranslation("mDifficulty"), this.rootShared.GetTranslation("diffLvl1") + "|" + this.rootShared.GetTranslation("diffLvl2") + "|" + this.rootShared.GetTranslation("diffLvl3"), TextAnchor.MiddleRight, true, true, (float)0);
        this.createOption(7, this.rootShared.GetTranslation("mDisCheck"), this.rootShared.GetTranslation("mOff") + "|" + this.rootShared.GetTranslation("mOn"), TextAnchor.MiddleRight, true, true, (float)0);
        if (!this.rootShared.chineseBuild)
        {
            this.createOption(2, this.rootShared.GetTranslation("mBlood"), this.rootShared.GetTranslation("mOff") + "|" + this.rootShared.GetTranslation("mOn"), TextAnchor.MiddleRight, true, true, (float)10);
        }
        this.createOption(3, this.rootShared.GetTranslation("mScrShake"), "Slider", TextAnchor.MiddleRight, true, true, (float)((!this.rootShared.chineseBuild) ? 0 : 10));
        this.createOption(4, this.rootShared.GetTranslation("mPHints"), this.rootShared.GetTranslation("mOff") + "|" + this.rootShared.GetTranslation("mOn"), TextAnchor.MiddleRight, true, true, (float)10);
        this.createOption(5, this.rootShared.GetTranslation("mTimer"), this.rootShared.GetTranslation("mOff") + "|" + this.rootShared.GetTranslation("mOn"), TextAnchor.MiddleRight, true, true, (float)0);
        this.createOption(6, this.rootShared.GetTranslation("mHUD"), this.rootShared.GetTranslation("mOff") + "|" + this.rootShared.GetTranslation("mOn"), TextAnchor.MiddleRight, true, true, (float)0);
        if (!this.rootShared.runningOnConsole)
        {
            this.createOption(8, this.rootShared.GetTranslation("mClear"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)10);
        }
        this.createOption(0, this.rootShared.GetTranslation("mBack"), string.Empty, TextAnchor.MiddleRight, false, false, (float)20);
        this.optionsSettingNr[1] = SavedData.GetInt("difficultyMode");
        if (SavedData.GetInt("haveSavedGameOptions") == 1)
        {
            if (!this.rootShared.chineseBuild)
            {
                this.optionsSettingNr[2] = SavedData.GetInt("bloodAndGore");
            }
            this.optionsSettingNr[3] = SavedData.GetInt("screenshake");
        }
        else
        {
            if (!this.rootShared.chineseBuild)
            {
                this.optionsSettingNr[2] = 1;
            }
            this.optionsSettingNr[3] = 100;
        }
        this.optionsSettingNr[4] = ((SavedData.GetInt("DisablePedroHints") != 0) ? 0 : 1);
        this.optionsSettingNr[5] = ((!this.rootShared.showUITimer) ? 0 : 1);
        this.optionsSettingNr[6] = ((!this.rootShared.hideHUD) ? 1 : 0);
        this.optionsSettingNr[7] = ((!this.rootShared.modDisableCheckpoints) ? 0 : 1);
        if (this.inGame)
        {
            this.disableOption[1] = true;
            this.disableOption[7] = true;
            if (!this.rootShared.runningOnConsole)
            {
                this.disableOption[8] = true;
            }
        }
        this.updateOptionSettings(true);
    }

    // Token: 0x06000364 RID: 868 RVA: 0x0004DFF4 File Offset: 0x0004C1F4
    public virtual void buildClearDataConfirmScreen()
    {
        this.clearMenu();
        this.curActiveMenu = 5.1f;
        this.theHeader.text = this.rootShared.GetTranslation("mClearHeader");
        this.nrOfOptions = (float)2;
        this.menuBackOptionNr = 1;
        this.curOptionSortedNr = (float)0;
        this.setUpArrays();
        this.extraInfo.gameObject.SetActive(true);
        this.createOption(0, this.rootShared.GetTranslation("mProceed"), string.Empty, TextAnchor.MiddleRight, false, false, (float)0);
        this.createOption(1, this.rootShared.GetTranslation("mCancel"), string.Empty, TextAnchor.MiddleRight, false, false, (float)0);
        this.extraInfoText.text = this.rootShared.GetTranslation("mClearSure");
        this.updateOptionSettings(true);
    }

    // Token: 0x06000365 RID: 869 RVA: 0x0004E0C0 File Offset: 0x0004C2C0
    public virtual void buildExitToMainMenuConfirmScreen()
    {
        this.clearMenu();
        this.curActiveMenu = -1.1f;
        this.theHeader.text = this.rootShared.GetTranslation("mExit");
        this.nrOfOptions = (float)2;
        this.menuBackOptionNr = 1;
        this.curOptionSortedNr = (float)0;
        this.setUpArrays();
        this.createOption(0, this.rootShared.GetTranslation("mExit"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)0);
        this.createOption(1, this.rootShared.GetTranslation("mCancel"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)5);
        this.updateOptionSettings(true);
    }

    // Token: 0x06000366 RID: 870 RVA: 0x0004E160 File Offset: 0x0004C360
    public virtual void buildQuitConfirmScreen()
    {
        this.clearMenu();
        this.curActiveMenu = -1.2f;
        this.theHeader.text = this.rootShared.GetTranslation("mQuit");
        this.nrOfOptions = (float)2;
        this.menuBackOptionNr = 1;
        this.curOptionSortedNr = (float)0;
        this.setUpArrays();
        this.createOption(0, this.rootShared.GetTranslation("mQuit"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)0);
        this.createOption(1, this.rootShared.GetTranslation("mCancel"), string.Empty, TextAnchor.MiddleLeft, false, false, (float)5);
        this.updateOptionSettings(true);
    }

    // Token: 0x06000367 RID: 871 RVA: 0x0004E200 File Offset: 0x0004C400
    public virtual void buildLanguageMenu()
    {
        this.clearMenu();
        this.curActiveMenu = (float)6;
        this.theHeader.text = this.rootShared.GetTranslation("mLang");
        this.nrOfOptions = (float)2;
        this.menuBackOptionNr = 1;
        this.curOptionSortedNr = (float)0;
        this.setUpArrays();
        this.gotMouseInput = false;
        string text = string.Empty;
        List<string> allLanguages = LocalizationManager.GetAllLanguages(true);
        for (int i = 0; i < allLanguages.Count; i++)
        {
            string translation = LocalizationManager.GetTranslation("LangName", true, 0, true, false, null, allLanguages[i]);
            text += translation;
            if (i < allLanguages.Count - 1)
            {
                text += "|";
            }
            if (allLanguages[i] == LocalizationManager.CurrentLanguage)
            {
                this.optionsSettingNr[0] = i;
            }
        }
        this.createOption(0, this.rootShared.GetTranslation("mLang"), text, TextAnchor.MiddleRight, true, true, (float)0);
        this.createOption(1, this.rootShared.GetTranslation("mBack"), string.Empty, TextAnchor.MiddleRight, false, false, (float)20);
        this.updateOptionSettings(true);
    }

    // Token: 0x06000368 RID: 872 RVA: 0x0004E31C File Offset: 0x0004C51C
    public virtual void rebuildMenu()
    {
        int num = this.curOption;
        int num2 = (int)this.curOptionSortedNr;
        float num3 = this.targetScrollPos;
        if (this.curActiveMenu == 2.2f)
        {
            this.dontCheckForMissingGamepadInputsMapped = true;
            this.buildGamepadControlsMenu();
        }
        else if (this.curActiveMenu == 2.1f)
        {
            this.buildKeyboardControlsMenu();
        }
        else if (this.curActiveMenu == (float)6)
        {
            this.buildLanguageMenu();
        }
        else if (this.curActiveMenu == (float)7)
        {
            this.buildGameModifiersMenu();
        }
        else if (this.curActiveMenu == 0.2f)
        {
            this.buildLevelSelectMenu();
        }
        this.curOption = num;
        this.curOptionSortedNr = (float)num2;
        this.targetScrollPos = num3;
        this.options[this.curOption].localScale = Vector3.one;
        this.optionsText[this.curOption].color = this.selectedColor;
        this.optionsSettingText[this.curOption].color = Color.white * (float)150;
        this.updateNavigationHints();
    }

    // Token: 0x06000369 RID: 873 RVA: 0x0004E430 File Offset: 0x0004C630
    public virtual void OnInputMapped()
    {
        MonoBehaviour.print("Input mapped");
        MonoBehaviour.print("InputMapper1 Status: " + this.inputMapper.status);
        MonoBehaviour.print("InputMapper2 Status: " + this.inputMapper2.status);
        this.inputMapper2.Stop();
        this.rebuildMenu();
    }

    // Token: 0x0600036A RID: 874 RVA: 0x0004E498 File Offset: 0x0004C698
    public virtual void OnInputMapped2()
    {
        MonoBehaviour.print("Input mapped 2");
        MonoBehaviour.print("InputMapper1 Status: " + this.inputMapper.status);
        MonoBehaviour.print("InputMapper2 Status: " + this.inputMapper2.status);
        this.inputMapper.Stop();
        this.rebuildMenu();
    }

    // Token: 0x0600036B RID: 875 RVA: 0x00003AC5 File Offset: 0x00001CC5
    public virtual void OnConflictFound()
    {
        MonoBehaviour.print("Conflict found");
        this.rebuildMenu();
    }

    // Token: 0x0600036C RID: 876 RVA: 0x00003AD7 File Offset: 0x00001CD7
    public virtual void OnCancelledInputEvent()
    {
        MonoBehaviour.print("Cancelled input");
        this.rebuildMenu();
    }

    // Token: 0x0600036D RID: 877 RVA: 0x00003AE9 File Offset: 0x00001CE9
    public virtual void OnCancelledInputEvent2()
    {
        MonoBehaviour.print("Cancelled input 2");
        this.rebuildMenu();
    }

    // Token: 0x0600036E RID: 878 RVA: 0x00003AFB File Offset: 0x00001CFB
    public virtual void OnStoppedInputEvent()
    {
        MonoBehaviour.print("Stopped input");
        this.inputMapper2.Stop();
    }

    // Token: 0x0600036F RID: 879 RVA: 0x00003B12 File Offset: 0x00001D12
    public virtual void OnStoppedInputEvent2()
    {
        MonoBehaviour.print("Stopped input 2");
        this.inputMapper.Stop();
    }

    // Token: 0x06000370 RID: 880 RVA: 0x0004E500 File Offset: 0x0004C700
    public virtual bool OnIsElementAllowed(ControllerPollingInfo info)
    {
        bool result;
        if (info.controllerType == ControllerType.Keyboard && info.keyboardKey == KeyCode.Escape)
        {
            this.inputMapper.Stop();
            this.inputMapper2.Stop();
            result = false;
        }
        else
        {
            result = (info.controllerType != ControllerType.Joystick || (!(info.elementIdentifier.name == "PS Button") && !(info.elementIdentifier.name == "Plus") && !(info.elementIdentifier.name == "Start") && !(info.elementIdentifier.name == "Options")));
        }
        return result;
    }

    // Token: 0x06000371 RID: 881 RVA: 0x0004E5E0 File Offset: 0x0004C7E0
    public virtual void executeOption()
    {
        if (!this.disableOption[this.curOption])
        {
            if (!this.theAudioSource.isPlaying && (!this.gotMouseInput || !(this.optionsSlider[this.curOption] != null)))
            {
                if (this.optionChanged[this.curOption])
                {
                    this.theAudioSource.volume = UnityEngine.Random.Range(0.5f, 0.7f);
                    this.theAudioSource.pitch = UnityEngine.Random.Range(0.7f, 0.8f);
                }
                else
                {
                    this.theAudioSource.volume = UnityEngine.Random.Range(0.9f, (float)1);
                    this.theAudioSource.pitch = UnityEngine.Random.Range(1.4f, 1.5f);
                }
                this.theAudioSource.Play();
            }
            this.theAudioSource2.Stop();
            this.theAudioSource3.Stop();
            if (this.curActiveMenu == (float)-1)
            {
                if (this.curOption == 0)
                {
                    this.menuEnabled = false;
                    this.root.resumeGame();
                }
                else if (this.curOption == 1)
                {
                    this.root.restartLevel();
                }
                else if (this.curOption == 2)
                {
                    this.buildOptionsMenu();
                }
                else if (this.curOption == 3)
                {
                    this.buildExitToMainMenuConfirmScreen();
                }
                else if (this.curOption == 4)
                {
                    this.buildQuitConfirmScreen();
                }
                else if (this.curOption == 5)
                {
                    this.buildDebugMenu();
                }
            }
            else if (this.curActiveMenu == -1.1f)
            {
                if (this.curOption == 0)
                {
                    this.root.quitToMainMenu();
                }
                else if (this.curOption == 1)
                {
                    this.buildPauseMenu();
                }
            }
            else if (this.curActiveMenu == -1.2f)
            {
                if (this.curOption == 0)
                {
                    this.root.quitGame();
                }
                else if (this.curOption == 1)
                {
                    this.buildPauseMenu();
                }
            }
            else if (this.curActiveMenu == (float)-111)
            {
                if (this.curOption == 0)
                {
                    this.menuEnabled = false;
                    this.root.resumeGame();
                }
                else if (this.curOption == 1)
                {
                    this.root.restartLevel();
                }
                else if (this.curOption == 2)
                {
                    this.rootShared.gamepadAimSens = UnityBuiltins.parseFloat(this.optionsSettingNr[2]) / (float)100;
                }
                else if (this.curOption == 3)
                {
                    this.root.quitToMainMenu();
                }
            }
            else if (this.curActiveMenu == 0.001f)
            {
                if (this.curOption == 0)
                {
                    this.buildMainMenu();
                }
            }
            else if (this.curActiveMenu == (float)0)
            {
                if (this.curOption == 0)
                {
                    this.rootShared.loadingScreenLevelToLoad = SavedData.GetInt("level") + 1;
                    this.rootShared.levelLoadedFromLevelSelectScreen = false;
                    ((MainMenuBackgroundScript)GameObject.Find("TheCamera").GetComponent(typeof(MainMenuBackgroundScript))).doStartGame();
                    this.menuEnabled = false;
                }
                else if (this.curOption == 1)
                {
                    this.buildNewGameMenu();
                }
                else if (this.curOption == 2)
                {
                    this.buildLevelSelectMenu();
                }
                else if (this.curOption == 3)
                {
                    this.buildOptionsMenu();
                }
                else if (this.curOption == 4)
                {
                    this.rootShared.loadingScreenLevelToLoad = 54;
                    this.rootShared.levelLoadedFromLevelSelectScreen = false;
                    ((MainMenuBackgroundScript)GameObject.Find("TheCamera").GetComponent(typeof(MainMenuBackgroundScript))).doStartGame();
                    this.menuEnabled = false;
                }
                else if (this.curOption == 5)
                {
                    this.buildGameModifiersMenu();
                }
                else if (this.curOption == 6)
                {
                    Application.Quit();
                }
            }
            else if (this.curActiveMenu == 0.1f)
            {
                if (this.curOption == 0)
                {
                    SavedData.SetInt("difficultyMode", 0);
                    MFPClassic.MapManager.currentLevel = 1;
                    this.rootShared.loadingScreenLevelToLoad = 6;
                    this.rootShared.levelLoadedFromLevelSelectScreen = false;
                    ((MainMenuBackgroundScript)GameObject.Find("TheCamera").GetComponent(typeof(MainMenuBackgroundScript))).doStartGame();
                    this.menuEnabled = false;
                }
                if (this.curOption == 1)
                {
                    SavedData.SetInt("difficultyMode", 1);
                    MFPClassic.MapManager.currentLevel = 1;
                    this.rootShared.loadingScreenLevelToLoad = 6;
                    this.rootShared.levelLoadedFromLevelSelectScreen = false;
                    ((MainMenuBackgroundScript)GameObject.Find("TheCamera").GetComponent(typeof(MainMenuBackgroundScript))).doStartGame();
                    this.menuEnabled = false;
                }
                else if (this.curOption == 2)
                {
                    SavedData.SetInt("difficultyMode", 2);
                    MFPClassic.MapManager.currentLevel = 1;
                    this.rootShared.loadingScreenLevelToLoad = 6;
                    this.rootShared.levelLoadedFromLevelSelectScreen = false;
                    ((MainMenuBackgroundScript)GameObject.Find("TheCamera").GetComponent(typeof(MainMenuBackgroundScript))).doStartGame();
                    this.menuEnabled = false;
                }
                else if (this.curOption == 3)
                {
                    this.buildMainMenu();
                    this.curOptionSortedNr = (float)1;
                    this.curOption = this.curOptionSorted[(int)this.curOptionSortedNr];
                }
            }
            else if (this.curActiveMenu == 0.2f)
            {
                if (this.curOption == 40)
                {
                    this.buildMainMenu();
                    this.curOptionSortedNr = (float)2;
                    this.curOption = this.curOptionSorted[(int)this.curOptionSortedNr];
                }
                else
                {
                    SavedData.SetInt("difficultyMode", this.levelSelectScreenDifficultyLevel);
                    this.rootShared.loadingScreenLevelToLoad = /* this.getLevelSelectLevelNr(this.curOption);*/ 6;
                    MFPClassic.MapManager.currentLevel = this.getLevelSelectLevelNr(this.curOption);
                    this.rootShared.levelLoadedFromLevelSelectScreen = true;
                    ((MainMenuBackgroundScript)GameObject.Find("TheCamera").GetComponent(typeof(MainMenuBackgroundScript))).doStartGame();
                    this.menuEnabled = false;
                }
            }
            else if (this.curActiveMenu == (float)1)
            {
                if (this.curOption == 0)
                {
                    if (this.rootShared.runningOnConsole)
                    {
                        this.buildGamepadControlsMenu();
                    }
                    else
                    {
                        this.buildControlsMenu();
                    }
                }
                else if (this.curOption == 5)
                {
                    this.buildVideoMenu();
                }
                else if (this.curOption == 2)
                {
                    this.buildAudioMenu();
                }
                else if (this.curOption == 3)
                {
                    this.buildGameplayMenu();
                }
                else if (this.curOption == 4)
                {
                    this.buildLanguageMenu();
                }
                else if (this.curOption == 1)
                {
                    if (this.inGame)
                    {
                        this.buildPauseMenu();
                        this.curOptionSortedNr = (float)2;
                        this.curOption = this.curOptionSorted[(int)this.curOptionSortedNr];
                    }
                    else
                    {
                        this.buildMainMenu();
                        this.curOptionSortedNr = (float)4;
                        this.curOption = this.curOptionSorted[(int)this.curOptionSortedNr];
                    }
                }
            }
            else if (this.curActiveMenu == (float)2)
            {
                if (this.curOption == 0)
                {
                    this.buildKeyboardControlsMenu();
                }
                else if (this.curOption == 1)
                {
                    this.buildGamepadControlsMenu();
                }
                else if (this.curOption == 2)
                {
                    this.buildOptionsMenu();
                    this.curOptionSortedNr = (float)0;
                    this.curOption = this.curOptionSorted[(int)this.curOptionSortedNr];
                }
            }
            else if (this.curActiveMenu == 2.1f)
            {
                if (this.curOption == 0)
                {
                    if (this.checkForMissingInput(true))
                    {
                        this.optionsText[0].text = this.rootShared.GetTranslation("twError");
                        this.optionsText[0].color = Color.red;
                        this.optionsSettingText[0].text = this.rootShared.GetTranslation("mNoInEr");
                        this.optionsSettingText[0].color = Color.red;
                        this.curOption = (int)(this.curOptionSortedNr = (float)0);
                        this.targetScrollPos = (float)0;
                        int num = 0;
                        Vector2 anchoredPosition = this.contentWrapper.anchoredPosition;
                        float num2 = anchoredPosition.y = (float)num;
                        Vector2 vector = this.contentWrapper.anchoredPosition = anchoredPosition;
                    }
                    else
                    {
                        SavedData.SetInt("cursorMode", this.optionsSettingNr[26]);
                        SavedData.SetFloat("mouseAimSens", this.rootShared.simulateMousePosSensitivity);
                        ReInput.userDataStore.Save();
                        this.buildControlsMenu();
                        this.curOptionSortedNr = (float)0;
                        this.curOption = this.curOptionSorted[(int)this.curOptionSortedNr];
                        if (this.inGame)
                        {
                            this.root.updateInputIcons = true;
                        }
                    }
                }
                else if (this.curOption == 1)
                {
                    this.SetInputButton("Move", false, true, -1, "Default");
                }
                else if (this.curOption == 2)
                {
                    this.SetInputButton("Move", true, true, 1, "Default");
                }
                else if (this.curOption == 3)
                {
                    this.SetInputButton("Jump", true, true, 1, "Default");
                }
                else if (this.curOption == 4)
                {
                    this.SetInputButton("Crouch", false, true, -1, "Default");
                }
                else if (this.curOption == 5)
                {
                    this.SetInputButton("Dodge", true, true, 1, "Default");
                }
                else if (this.curOption == 6)
                {
                    this.SetInputButton("Interact", true, true, 1, "Default");
                }
                else if (this.curOption == 7)
                {
                    this.SetInputButton("Focus", true, true, 1, "Default");
                }
                else if (this.curOption == 8)
                {
                    this.SetInputButton("Kick", true, true, 1, "Default");
                }
                else if (this.curOption == 9)
                {
                    this.SetInputButton("Fire", true, true, 1, "Default");
                }
                else if (this.curOption == 10)
                {
                    this.SetInputButton("Fire2", true, true, 1, "Default");
                }
                else if (this.curOption == 11)
                {
                    this.SetInputButton("Reload", true, true, 1, "Default");
                }
                else if (this.curOption == 12)
                {
                    this.SetInputButton("Change Weapon", true, true, 1, "Default");
                }
                else if (this.curOption == 13)
                {
                    this.SetInputButton("Scroll Weapon", true, true, 1, "Default");
                }
                else if (this.curOption == 14)
                {
                    this.SetInputButton("Scroll Weapon", false, true, -1, "Default");
                }
                else if (this.curOption == 15)
                {
                    this.rootShared.simulateMousePos = false;
                    this.rootShared.simulateMousePosSensitivity = 0.25f;
                    this.player.controllers.maps.LoadDefaultMaps(ControllerType.Keyboard);
                    this.player.controllers.maps.LoadDefaultMaps(ControllerType.Mouse);
                    this.rebuildMenu();
                }
                else if (this.curOption == 16)
                {
                    this.SetInputButton("Pistol", true, true, 1, "Default");
                }
                else if (this.curOption == 17)
                {
                    this.SetInputButton("Dual Pistols", true, true, 1, "Default");
                }
                else if (this.curOption == 18)
                {
                    this.SetInputButton("Submachine Gun", true, true, 1, "Default");
                }
                else if (this.curOption == 19)
                {
                    this.SetInputButton("Dual Submachine Guns", true, true, 1, "Default");
                }
                else if (this.curOption == 20)
                {
                    this.SetInputButton("Shotgun", true, true, 1, "Default");
                }
                else if (this.curOption == 21)
                {
                    this.SetInputButton("Assault Rifle", true, true, 1, "Default");
                }
                else if (this.curOption == 22)
                {
                    this.SetInputButton("Rifle", true, true, 1, "Default");
                }
                else if (this.curOption == 23)
                {
                    this.SetInputButton("MotorcycleY", true, true, 1, "Motorcycle");
                }
                else if (this.curOption == 24)
                {
                    this.SetInputButton("MotorcycleY", false, true, -1, "Motorcycle");
                }
                else if (this.curOption == 25)
                {
                    this.SetInputButton("MotorcycleWheelie", true, true, 1, "Motorcycle");
                }
                else if (this.curOption == 28)
                {
                    this.SetInputButton("Restart Level", true, true, 1, "Default");
                }
                else if (this.curOption == 26)
                {
                    this.rootShared.simulateMousePos = (this.optionsSettingNr[26] == 1);
                    this.disableOption[27] = (this.optionsSettingNr[26] == 0);
                }
                else if (this.curOption == 27)
                {
                    this.rootShared.simulateMousePosSensitivity = UnityBuiltins.parseFloat(this.optionsSettingNr[27]) / (float)100;
                }
            }
            else if (this.curActiveMenu == 2.2f)
            {
                if (this.curOption == 15)
                {
                    this.SetInputButton("Aim Horizontal", false, false, -1, "Default");
                }
                else if (this.curOption == 16)
                {
                    this.SetInputButton("Aim Horizontal", true, false, 1, "Default");
                }
                else if (this.curOption == 17)
                {
                    this.SetInputButton("Aim Vertical", true, false, 1, "Default");
                }
                else if (this.curOption == 18)
                {
                    this.SetInputButton("Aim Vertical", false, false, -1, "Default");
                }
                else if (this.curOption == 1)
                {
                    this.SetInputButton("Move", false, false, -1, "Default");
                }
                else if (this.curOption == 2)
                {
                    this.SetInputButton("Move", true, false, 1, "Default");
                }
                else if (this.curOption == 3)
                {
                    this.SetInputButton("Jump", true, false, 1, "Default");
                }
                else if (this.curOption == 4)
                {
                    this.SetInputButton("Crouch", false, false, -1, "Default");
                }
                else if (this.curOption == 5)
                {
                    this.SetInputButton("Dodge", true, false, 1, "Default");
                }
                else if (this.curOption == 6)
                {
                    this.SetInputButton("Interact", true, false, 1, "Default");
                }
                else if (this.curOption == 7)
                {
                    this.SetInputButton("Focus", true, false, 1, "Default");
                }
                else if (this.curOption == 8)
                {
                    this.SetInputButton("Kick", true, false, 1, "Default");
                }
                else if (this.curOption == 9)
                {
                    this.SetInputButton("Fire", true, false, 1, "Default");
                }
                else if (this.curOption == 10)
                {
                    this.SetInputButton("Fire2", true, false, 1, "Default");
                }
                else if (this.curOption == 11)
                {
                    this.SetInputButton("Reload", true, false, 1, "Default");
                }
                else if (this.curOption == 12)
                {
                    this.SetInputButton("Change Weapon", true, false, 1, "Default");
                }
                else if (this.curOption == 13)
                {
                    this.SetInputButton("Scroll Weapon", true, false, 1, "Default");
                }
                else if (this.curOption == 14)
                {
                    this.SetInputButton("Scroll Weapon", false, false, -1, "Default");
                }
                else if (this.curOption == 20)
                {
                    this.SetInputButton("MotorcycleY", true, false, 1, "Motorcycle");
                }
                else if (this.curOption == 21)
                {
                    this.SetInputButton("MotorcycleY", false, false, -1, "Motorcycle");
                }
                else if (this.curOption == 22)
                {
                    this.SetInputButton("MotorcycleWheelie", true, false, 1, "Motorcycle");
                }
                else if (this.curOption == 19)
                {
                    this.optionsSettingNr[23] = 1;
                    this.rootShared.disableRumble = false;
                    SavedData.SetInt("DisableRumble", 0);
                    this.rootShared.gamepadAimSens = (float)1;
                    this.rootShared.aimAssistMode = 1;
                    this.optionsSettingNr[24] = 100;
                    this.optionsSettingNr[25] = 1;
                    this.player.controllers.maps.LoadDefaultMaps(ControllerType.Joystick);
                    this.rebuildMenu();
                }
                else if (this.curOption == 23)
                {
                    this.rootShared.disableRumble = (this.optionsSettingNr[23] == 0);
                }
                else if (this.curOption == 24)
                {
                    this.rootShared.gamepadAimSens = UnityBuiltins.parseFloat(this.optionsSettingNr[24]) / (float)100;
                }
                else if (this.curOption == 25)
                {
                    this.rootShared.aimAssistMode = this.optionsSettingNr[25];
                }
                else if (this.curOption == 0)
                {
                    if (this.checkForMissingInput(false))
                    {
                        this.optionsText[0].text = this.rootShared.GetTranslation("twError");
                        this.optionsText[0].color = Color.red;
                        this.optionsSettingText[0].text = this.rootShared.GetTranslation("mNoInEr");
                        this.optionsSettingText[0].color = Color.red;
                        this.curOption = (int)(this.curOptionSortedNr = (float)0);
                        this.targetScrollPos = (float)0;
                        int num3 = 0;
                        Vector2 anchoredPosition2 = this.contentWrapper.anchoredPosition;
                        float num4 = anchoredPosition2.y = (float)num3;
                        Vector2 vector2 = this.contentWrapper.anchoredPosition = anchoredPosition2;
                    }
                    else
                    {
                        SavedData.SetFloat("gamepadAimSens", this.rootShared.gamepadAimSens);
                        SavedData.SetInt("DisableRumble", (!this.rootShared.disableRumble) ? 0 : 1);
                        SavedData.SetInt("AimAssistMode", this.rootShared.aimAssistMode);
                        SavedData.Save();
                        ReInput.userDataStore.Save();
                        if (this.rootShared.runningOnConsole)
                        {
                            this.buildOptionsMenu();
                            this.curOptionSortedNr = (float)0;
                        }
                        else
                        {
                            this.buildControlsMenu();
                            this.curOptionSortedNr = (float)1;
                        }
                        this.curOption = this.curOptionSorted[(int)this.curOptionSortedNr];
                        if (this.inGame)
                        {
                            this.root.updateInputIcons = true;
                        }
                    }
                }
            }
            else if (this.curActiveMenu == (float)3)
            {
                if (this.curOption == 0 || this.curOption == 1 || this.curOption == 2 || this.curOption == 5 || this.curOption == 6)
                {
                    this.disableOption[3] = false;
                    this.disableOption[6] = (this.optionsSettingNr[5] == 1);
                }
                else if (this.curOption == 3)
                {
                    if (this.optionChanged[0])
                    {
                        Screen.SetResolution(this.supportedResolutions[this.optionsSettingNr[0]].width, this.supportedResolutions[this.optionsSettingNr[0]].height, Screen.fullScreen, 60);
                        this.rootShared.adjustUIForAspectRatio();
                    }
                    if (this.optionChanged[1])
                    {
                        QualitySettings.SetQualityLevel(this.optionsSettingNr[1], true);
                        this.rootShared.curVisualQualityLevel = this.optionsSettingNr[1];
                        if (this.inGame)
                        {
                            this.root.setupGraphicsSettings();
                        }
                        PlatformPlayerPrefs.SetInt("QualitySetting", this.optionsSettingNr[1] + 1);
                        PlatformPlayerPrefs.Save();
                    }
                    if (this.optionChanged[2])
                    {
                        Screen.fullScreen = (this.optionsSettingNr[2] == 1);
                    }
                    if (this.optionChanged[5])
                    {
                        QualitySettings.vSyncCount = (int)Mathf.Clamp01((float)this.optionsSettingNr[5]);
                        PlatformPlayerPrefs.SetInt("VSync", QualitySettings.vSyncCount);
                        PlatformPlayerPrefs.Save();
                    }
                    if (this.optionChanged[6])
                    {
                        if (this.optionsSettingNr[6] == 1)
                        {
                            Application.targetFrameRate = 30;
                        }
                        else if (this.optionsSettingNr[6] == 2)
                        {
                            Application.targetFrameRate = 60;
                        }
                        else if (this.optionsSettingNr[6] == 3)
                        {
                            Application.targetFrameRate = 75;
                        }
                        else if (this.optionsSettingNr[6] == 4)
                        {
                            Application.targetFrameRate = 100;
                        }
                        else if (this.optionsSettingNr[6] == 5)
                        {
                            Application.targetFrameRate = 120;
                        }
                        else if (this.optionsSettingNr[6] == 6)
                        {
                            Application.targetFrameRate = 144;
                        }
                        else if (this.optionsSettingNr[6] == 7)
                        {
                            Application.targetFrameRate = 165;
                        }
                        else if (this.optionsSettingNr[6] == 8)
                        {
                            Application.targetFrameRate = 240;
                        }
                        else
                        {
                            Application.targetFrameRate = 0;
                        }
                        PlatformPlayerPrefs.SetInt("TargetFPS", this.optionsSettingNr[6]);
                        PlatformPlayerPrefs.Save();
                    }
                    for (int i = 0; i < this.optionChanged.Length; i++)
                    {
                        this.optionChanged[i] = false;
                    }
                    this.disableOption[3] = true;
                }
                else if (this.curOption == 4)
                {
                    this.buildOptionsMenu();
                    this.curOptionSortedNr = (float)1;
                    this.curOption = this.curOptionSorted[(int)this.curOptionSortedNr];
                }
            }
            else if (this.curActiveMenu == (float)4)
            {
                if (this.curOption == 0)
                {
                    float num5 = (float)this.optionsSettingNr[0];
                    if (num5 > (float)0)
                    {
                        num5 = (float)-40 + (float)40 * (num5 / (float)100);
                    }
                    else
                    {
                        num5 = (float)-80;
                    }
                    this.audioMixer.SetFloat("SFXMasterVolume", num5);
                }
                else if (this.curOption == 1)
                {
                    float num6 = (float)this.optionsSettingNr[1];
                    if (num6 > (float)0)
                    {
                        num6 = (float)-40 + (float)40 * (num6 / (float)100);
                    }
                    else
                    {
                        num6 = (float)-80;
                    }
                    this.audioMixer.SetFloat("MusicMasterVolume", num6);
                }
                else if (this.curOption == 2)
                {
                    float value = 0f;
                    float value2 = 0f;
                    this.audioMixer.GetFloat("SFXMasterVolume", out value2);
                    this.audioMixer.GetFloat("MusicMasterVolume", out value);
                    SavedData.SetFloat("MusicVolume", value);
                    SavedData.SetFloat("SFXVolume", value2);
                    this.buildOptionsMenu();
                    this.curOptionSortedNr = (float)2;
                    if (this.rootShared.runningOnConsole)
                    {
                        this.curOptionSortedNr -= (float)1;
                    }
                    this.curOption = this.curOptionSorted[(int)this.curOptionSortedNr];
                }
            }
            else if (this.curActiveMenu == (float)5)
            {
                if (this.curOption == 0 || this.curOption == 4 || this.curOption == 5 || this.curOption == 6 || this.curOption == 7 || this.curOption == 8)
                {
                    this.rootShared.hideHUD = (this.optionsSettingNr[6] == 0);
                    SavedData.SetInt("ShowHUD", this.optionsSettingNr[6]);
                    if (!this.inGame)
                    {
                        SavedData.SetInt("difficultyMode", this.optionsSettingNr[1]);
                    }
                    else
                    {
                        float num7 = (float)this.optionsSettingNr[3];
                        this.root.optionsScreenShakeMultiplier = num7 / (float)100;
                        this.root.doGore = (this.optionsSettingNr[2] == 1);
                        this.root.doHideHUD(this.rootShared.hideHUD);
                    }
                    this.rootShared.disablePedroHints = (this.optionsSettingNr[4] == 0);
                    SavedData.SetInt("haveSavedGameOptions", 1);
                    SavedData.SetInt("DisablePedroHints", (this.optionsSettingNr[4] != 1) ? 1 : 0);
                    this.rootShared.showUITimer = (this.optionsSettingNr[5] == 1);
                    SavedData.SetInt("ShowUITimer", (!this.rootShared.showUITimer) ? 0 : 1);
                    SavedData.SetInt("bloodAndGore", this.optionsSettingNr[2]);
                    SavedData.SetInt("screenshake", this.optionsSettingNr[3]);
                    this.rootShared.modDisableCheckpoints = (this.optionsSettingNr[7] == 1);
                    SavedData.SetInt("modDisableCheckpoints", this.optionsSettingNr[7]);
                }
                if (this.curOption == 0)
                {
                    this.buildOptionsMenu();
                    this.curOptionSortedNr = (float)3;
                    if (this.rootShared.runningOnConsole)
                    {
                        this.curOptionSortedNr -= (float)1;
                    }
                    this.curOption = this.curOptionSorted[(int)this.curOptionSortedNr];
                }
                else if (this.curOption == 8)
                {
                    this.buildClearDataConfirmScreen();
                }
            }
            else if (this.curActiveMenu == 5.1f)
            {
                if (this.curOption == 0)
                {
                    if (!this.rootShared.runningOnConsole)
                    {
                        QualitySettings.SetQualityLevel(1, true);
                        this.rootShared.curVisualQualityLevel = 1;
                        QualitySettings.vSyncCount = 0;
                        Application.targetFrameRate = 60;
                    }
                    this.player.controllers.maps.LoadDefaultMaps(ControllerType.Joystick);
                    this.player.controllers.maps.LoadDefaultMaps(ControllerType.Keyboard);
                    this.player.controllers.maps.LoadDefaultMaps(ControllerType.Mouse);
                    this.rootShared.simulateMousePos = false;
                    this.rootShared.simulateMousePosSensitivity = 0.25f;
                    this.rootShared.disableRumble = false;
                    this.rootShared.gamepadAimSens = (float)1;
                    this.audioMixer.SetFloat("SFXMasterVolume", (float)0);
                    this.audioMixer.SetFloat("MusicMasterVolume", (float)0);
                    SavedData.DeleteAll();
                    PlatformPlayerPrefs.DeleteAll();
                    ((StatsTrackerScript)this.rootShared.GetComponent(typeof(StatsTrackerScript))).loadStats();
                    this.rootShared.disablePedroHints = false;
                    this.rootShared.showUITimer = false;
                    this.rootShared.hideHUD = false;
                    this.rootShared.modDisableCheckpoints = false;
                    this.rootShared.restoreDefaultGameModifiers();
                    this.buildStartPrompt();
                }
                else if (this.curOption == 1)
                {
                    this.buildGameplayMenu();
                    this.curOptionSortedNr = (float)7;
                    this.curOption = this.curOptionSorted[(int)this.curOptionSortedNr];
                }
            }
            else if (this.curActiveMenu == (float)6)
            {
                if (this.curOption == 0)
                {
                    LocalizationManager.CurrentLanguage = LocalizationManager.GetAllLanguages(true)[this.optionsSettingNr[0]];
                    SavedData.SetString("language", LocalizationManager.CurrentLanguageCode);
                    this.createNavigationHints();
                    this.rebuildMenu();
                    this.curOption = 0;
                    this.options[0].localScale = Vector3.one;
                    this.optionsText[0].color = this.selectedColor;
                    this.optionsSettingText[0].color = Color.white * (float)150;
                }
                else if (this.curOption == 1)
                {
                    this.buildOptionsMenu();
                    this.curOptionSortedNr = (float)4;
                    if (this.rootShared.runningOnConsole)
                    {
                        this.curOptionSortedNr -= (float)1;
                    }
                    this.curOption = this.curOptionSorted[(int)this.curOptionSortedNr];
                }
            }
            else if (this.curActiveMenu == (float)7)
            {
                if (this.curOption == 0)
                {
                    if (Extensions.get_length(this.optionsSettingNr) > 1)
                    {
                        SavedData.SetInt("modAllWeapons", (!this.rootShared.modAllWeapons) ? 0 : 1);
                        SavedData.SetInt("modInfiniteAmmo", (!this.rootShared.modInfiniteAmmo) ? 0 : 1);
                        SavedData.SetInt("modOneShotEnemies", (!this.rootShared.modOneShotEnemies) ? 0 : 1);
                        SavedData.SetInt("modOneShotPlayer", (!this.rootShared.modOneShotPlayer) ? 0 : 1);
                        SavedData.SetInt("modIncreaseAccuracy", (!this.rootShared.modIncreaseAccuracy) ? 0 : 1);
                        SavedData.SetInt("modFocusSlowdownScale", (int)this.rootShared.modFocusSlowdownScale);
                        SavedData.SetInt("modPlayerSpeed", (int)this.rootShared.modPlayerSpeed);
                        SavedData.SetInt("modInfiniteFocus", (!this.rootShared.modInfiniteFocus) ? 0 : 1);
                        SavedData.SetInt("modSideOnCamera", (!this.rootShared.modSideOnCamera) ? 0 : 1);
                        SavedData.SetInt("modBigHead", (!this.rootShared.modBigHead) ? 0 : 1);
                        SavedData.SetInt("modPlayerSize", (int)this.rootShared.modPlayerSize);
                        SavedData.SetInt("modEnemyBulletSpeed", (int)this.rootShared.modEnemyBulletSpeed);
                        SavedData.SetInt("modPlayerBulletSpeed", (int)this.rootShared.modPlayerBulletSpeed);
                        SavedData.SetInt("modCinematicCamera", (!this.rootShared.modCinematicCamera) ? 0 : 1);
                    }
                    this.buildMainMenu();
                    this.curOptionSortedNr = (float)3;
                    this.curOption = this.curOptionSorted[(int)this.curOptionSortedNr];
                }
                else if (this.curOption == 1)
                {
                    this.rootShared.restoreDefaultGameModifiers();
                    this.rebuildMenu();
                }
                else if (this.curOption == 2)
                {
                    this.rootShared.modAllWeapons = (this.optionsSettingNr[2] == 1);
                }
                else if (this.curOption == 3)
                {
                    this.rootShared.modInfiniteAmmo = (this.optionsSettingNr[3] == 1);
                }
                else if (this.curOption == 4)
                {
                    this.rootShared.modOneShotEnemies = (this.optionsSettingNr[4] == 1);
                }
                else if (this.curOption == 5)
                {
                    this.rootShared.modOneShotPlayer = (this.optionsSettingNr[5] == 1);
                }
                else if (this.curOption == 7)
                {
                    this.rootShared.modIncreaseAccuracy = (this.optionsSettingNr[7] == 1);
                }
                else if (this.curOption == 8)
                {
                    this.rootShared.modFocusSlowdownScale = (float)this.optionsSettingNr[8];
                }
                else if (this.curOption == 9)
                {
                    this.rootShared.modPlayerSpeed = (float)this.optionsSettingNr[9];
                }
                else if (this.curOption == 10)
                {
                    this.rootShared.modInfiniteFocus = (this.optionsSettingNr[10] == 1);
                }
                else if (this.curOption == 11)
                {
                    this.rootShared.modSideOnCamera = (this.optionsSettingNr[11] == 1);
                }
                else if (this.curOption == 12)
                {
                    this.rootShared.modBigHead = (this.optionsSettingNr[12] == 1);
                }
                else if (this.curOption == 13)
                {
                    this.rootShared.modPlayerSize = (float)this.optionsSettingNr[13];
                }
                else if (this.curOption == 14)
                {
                    this.rootShared.modEnemyBulletSpeed = (float)this.optionsSettingNr[14];
                }
                else if (this.curOption == 15)
                {
                    this.rootShared.modPlayerBulletSpeed = (float)this.optionsSettingNr[15];
                }
                else if (this.curOption == 6)
                {
                    this.rootShared.modCinematicCamera = (this.optionsSettingNr[6] == 1);
                }
                this.gameModifiersNotice.SetActive(this.rootShared.gameModifiersCheck());
            }
            else if (this.curActiveMenu == (float)-789)
            {
                if (this.curOption == 0)
                {
                    this.rootShared.godMode = (this.optionsSettingNr[0] == 1);
                }
                else if (this.curOption == 1)
                {
                    this.root.resetTimeStuff();
                    this.rootShared.loadingScreenLevelToLoad = SceneManager.GetActiveScene().buildIndex + 1;
                    SceneManager.LoadScene("_Loading_Screen");
                }
                else if (this.curOption == 2)
                {
                    this.root.resetTimeStuff();
                    this.rootShared.loadingScreenLevelToLoad = SceneManager.GetActiveScene().buildIndex - 1;
                    SceneManager.LoadScene("_Loading_Screen");
                }
                else if (this.curOption == 3)
                {
                    ((PlayerScript)GameObject.Find("Player").GetComponent(typeof(PlayerScript))).unlockAllWeapons();
                }
                else if (this.curOption == 4)
                {
                    this.buildPauseMenu();
                }
                else if (this.curOption == 5)
                {
                    SavedData.SetInt("levelSelectMaxNr", 99);
                }
                else if (this.curOption == 6)
                {
                    int j = 0;
                    Light[] array = UnityEngine.Object.FindObjectsOfType<Light>();
                    int length = array.Length;
                    while (j < length)
                    {
                        if (array[j].type == LightType.Directional)
                        {
                            array[j].enabled = false;
                        }
                        j++;
                    }
                }
            }
        }
        else
        {
            if (!this.theAudioSource.isPlaying)
            {
                this.theAudioSource.volume = UnityEngine.Random.Range(0.3f, 0.5f);
                this.theAudioSource.pitch = UnityEngine.Random.Range(0.3f, 0.4f);
                this.theAudioSource.Play();
            }
            this.theAudioSource2.Stop();
            this.theAudioSource3.Stop();
        }
    }

    // Token: 0x06000372 RID: 882 RVA: 0x000507CC File Offset: 0x0004E9CC
    public virtual int getLevelSelectLevelNr(int nr)
    {
        int num = nr + 1;
        if (num >= 13)
        {
            num++;
        }
        if (num >= 15)
        {
            num++;
        }
        if (num >= 29)
        {
            num++;
        }
        if (num >= 30)
        {
            num++;
        }
        if (num >= 40)
        {
            num++;
        }
        if (num >= 42)
        {
            num++;
        }
        if (num >= 51)
        {
            num++;
        }
        return num;
    }

    // Token: 0x06000373 RID: 883 RVA: 0x00050834 File Offset: 0x0004EA34
    public virtual void setUpArrays()
    {
        this.options = new RectTransform[(int)this.nrOfOptions];
        this.optionsText = new Text[(int)this.nrOfOptions];
        this.optionsSettingText = new Text[(int)this.nrOfOptions];
        this.optionsSlider = new Image[(int)this.nrOfOptions];
        this.optionsSliderContainer = new RectTransform[(int)this.nrOfOptions];
        this.optionsSliderContainerImage = new Image[(int)this.nrOfOptions];
        this.optionsInputSymbolHolder = new RectTransform[(int)this.nrOfOptions];
        this.optionsSettingOptions = new string[(int)this.nrOfOptions];
        this.optionsSettingNr = new int[(int)this.nrOfOptions];
        this.optionChanged = new bool[(int)this.nrOfOptions];
        this.disableOption = new bool[(int)this.nrOfOptions];
        this.optionRefreshOnUpdateAll = new bool[(int)this.nrOfOptions];
        this.curOptionSorted = new int[(int)this.nrOfOptions];
        this.optionExecuteOnChange = new bool[(int)this.nrOfOptions];
    }

    // Token: 0x06000374 RID: 884 RVA: 0x00050940 File Offset: 0x0004EB40
    public virtual void clearMenu()
    {
        if (!RuntimeServices.EqualityOperator(this.options, null) && this.options.Length > 0)
        {
            for (int i = 0; i < this.options.Length; i++)
            {
                if (this.options[i] != null)
                {
                    UnityEngine.Object.Destroy(this.options[i].gameObject);
                }
            }
        }
        this.yOffset = (float)-40;
        this.nrOfOptions = (float)0;
        this.setUpArrays();
        this.targetScrollPos = (float)0;
        int num = 0;
        Vector2 anchoredPosition = this.contentWrapper.anchoredPosition;
        float num2 = anchoredPosition.x = (float)num;
        Vector2 vector = this.contentWrapper.anchoredPosition = anchoredPosition;
        this.extraInfo.gameObject.SetActive(false);
        this.extraInfoText.text = string.Empty;
        this.levelSelectScreen.gameObject.SetActive(false);
        if (!RuntimeServices.EqualityOperator(this.smallHeaders, null))
        {
            int j = 0;
            RectTransform[] array = this.smallHeaders;
            int length = array.Length;
            while (j < length)
            {
                UnityEngine.Object.Destroy(array[j].gameObject);
                j++;
            }
            this.smallHeaders = new RectTransform[0];
        }
    }

    // Token: 0x06000375 RID: 885 RVA: 0x00050A8C File Offset: 0x0004EC8C
    public virtual void updateOptionSettings(bool updateAll)
    {
        int num = (!updateAll) ? this.curOption : 0;
        while ((float)num < ((!updateAll) ? ((float)(this.curOption + 1)) : this.nrOfOptions))
        {
            if (!updateAll || (updateAll && this.optionRefreshOnUpdateAll[num]))
            {
                if (this.optionsSlider[num] != null)
                {
                    this.optionsSettingNr[num] = Mathf.Clamp(this.optionsSettingNr[num], 0, 100);
                    this.optionsSettingText[num].text = this.optionsSettingNr[num].ToString();
                    float num2 = (float)this.optionsSettingNr[num];
                    float x = num2 / (float)100;
                    Vector3 localScale = this.optionsSlider[num].transform.localScale;
                    float num3 = localScale.x = x;
                    Vector3 vector = this.optionsSlider[num].transform.localScale = localScale;
                }
                else if (this.optionsSettingOptions[num] != string.Empty)
                {
                    string[] array = this.optionsSettingOptions[num].Split(new char[]
                    {
                        "|"[0]
                    });
                    this.optionsSettingNr[num] = Mathf.Clamp(this.optionsSettingNr[num], 0, array.Length - 1);
                    this.optionsSettingText[num].text = string.Empty;
                    if (this.optionsSettingNr[num] > 0)
                    {
                        this.optionsSettingText[num].text = this.optionsSettingText[num].text + "<    ";
                    }
                    else
                    {
                        this.optionsSettingText[num].text = this.optionsSettingText[num].text + "      ";
                    }
                    this.optionsSettingText[num].text = this.optionsSettingText[num].text + array[this.optionsSettingNr[num]];
                    if (this.optionsSettingNr[num] < array.Length - 1)
                    {
                        this.optionsSettingText[num].text = this.optionsSettingText[num].text + "    >";
                    }
                }
            }
            num++;
        }
        if (!updateAll && this.optionsSettingText[this.curOption].text != string.Empty)
        {
            this.optionsSettingText[this.curOption].color = Color.white * (float)150;
            this.optionChanged[this.curOption] = true;
            if (this.optionExecuteOnChange[this.curOption])
            {
                this.executeOption();
            }
        }
        this.updateNavigationHints();
    }

    // Token: 0x06000376 RID: 886 RVA: 0x00050D44 File Offset: 0x0004EF44
    public virtual void doExtraInfoUpdate(bool updateLeaderboard)
    {
        if (this.curActiveMenu == 0.1f)
        {
            if (this.curOption == 0)
            {
                this.extraInfoText.text = this.rootShared.GetTranslation("diffLvl1Descrpt");
            }
            else if (this.curOption == 1)
            {
                this.extraInfoText.text = this.rootShared.GetTranslation("diffLvl2Descrpt");
            }
            else if (this.curOption == 2)
            {
                this.extraInfoText.text = this.rootShared.GetTranslation("diffLvl3Descrpt");
            }
            else
            {
                this.extraInfoText.text = string.Empty;
            }
        }
        else if (this.curActiveMenu == 0.2f)
        {
            if (this.curOption < 40)
            {
                this.levelSelectScreen.gameObject.SetActive(true);
                int levelSelectLevelNr = this.getLevelSelectLevelNr(this.curOption);
                if (updateLeaderboard)
                {
                    string @string = SavedData.GetString(CryptoString.Encrypt("lvlScore" + levelSelectLevelNr + "diff" + this.levelSelectScreenDifficultyLevel + "ID"));
                    if (this.leaderboardToUse != this.rootShared.GetLeaderboardName(levelSelectLevelNr, @string, false))
                    {
                        this.rootShared.ResetLeaderboardPage();
                        this.leaderboardToUse = this.rootShared.GetLeaderboardName(levelSelectLevelNr, @string, false);
                    }
                    if (this.leaderboardFilter == 0)
                    {
                        this.levelSelectScreenLeaderboardTypeText.text = this.rootShared.GetTranslation("mType") + " " + this.rootShared.GetTranslation("mGlobal");
                    }
                    else if (this.leaderboardFilter == 1)
                    {
                        this.levelSelectScreenLeaderboardTypeText.text = this.rootShared.GetTranslation("mType") + " " + this.rootShared.GetTranslation("mUser");
                    }
                    else if (this.leaderboardFilter == 2)
                    {
                        this.levelSelectScreenLeaderboardTypeText.text = this.rootShared.GetTranslation("mType") + " " + this.rootShared.GetTranslation("mFriends");
                    }
                    this.clearLeaderboardEntries();
                    this.leaderboardDisplayEntry[0].text = this.rootShared.GetTranslation("mLoading");
                    this.leaderboardFetchTimer = (float)30;
                    this.leaderboardFetchTimerDoOnce = false;
                }
                int @int = SavedData.GetInt(CryptoString.Encrypt("lvlRating" + levelSelectLevelNr + "diff" + this.levelSelectScreenDifficultyLevel));
                if (!RuntimeServices.EqualityOperator(@int, null) && @int != 0)
                {
                    if (@int == 1)
                    {
                        this.levelSelectScreenRating.text = this.rootShared.GetTranslation("esRateC");
                    }
                    else if (@int == 2)
                    {
                        this.levelSelectScreenRating.text = this.rootShared.GetTranslation("esRateB");
                    }
                    else if (@int == 3)
                    {
                        this.levelSelectScreenRating.text = this.rootShared.GetTranslation("esRateA");
                    }
                    else if (@int == 4)
                    {
                        this.levelSelectScreenRating.text = this.rootShared.GetTranslation("esRateS");
                    }
                    int int2 = SavedData.GetInt(CryptoString.Encrypt("lvlScore" + levelSelectLevelNr + "diff" + this.levelSelectScreenDifficultyLevel));
                    this.levelSelectScreenScore.text = this.rootShared.addCommasToNumber((float)int2);
                    float @float = SavedData.GetFloat(CryptoString.Encrypt("lvlTime" + levelSelectLevelNr + "diff" + this.levelSelectScreenDifficultyLevel));
                    this.levelSelectScreenTime.text = this.convertToTimeFormat(@float);
                }
                else
                {
                    this.levelSelectScreenRating.text = string.Empty;
                    this.levelSelectScreenScore.text = "-";
                    this.levelSelectScreenTime.text = "-";
                }
            }
            else
            {
                this.levelSelectScreen.gameObject.SetActive(false);
            }
        }
    }

    // Token: 0x06000377 RID: 887 RVA: 0x00051188 File Offset: 0x0004F388
    public virtual void mouseSetCurOption(int nr)
    {
        if (this.allowNavigation && this.timeSinceKeyboardUsed > (float)20 && this.menuEnabled)
        {
            if (!this.gotMouseInput)
            {
                this.mouseNavigation = true;
                this.curOption = nr;
                this.doExtraInfoUpdate(true);
                int num = 0;
                for (int i = 0; i < this.curOptionSorted.Length; i++)
                {
                    if (this.curOptionSorted[i] == nr)
                    {
                        this.curOptionSortedNr = (float)i;
                        break;
                    }
                }
            }
            else
            {
                this.mouseReleaseOptionNr = nr;
            }
        }
    }

    // Token: 0x06000378 RID: 888 RVA: 0x00051224 File Offset: 0x0004F424
    public virtual void mouseClick(Vector2 pos)
    {
        if (this.allowNavigation && this.menuEnabled)
        {
            if (this.optionsSlider[this.curOption] != null)
            {
                this.gotMouseInput = true;
            }
            else if (this.optionsSettingOptions[this.curOption] == "   ")
            {
                this.gotMouseInput = false;
                this.executeOption();
            }
            else if (this.optionsSettingOptions[this.curOption] != string.Empty)
            {
                this.navInput.x = (float)((pos.x <= (float)0) ? -1 : 1);
                this.gotMouseInput = true;
            }
            else
            {
                this.gotMouseInput = false;
                this.executeOption();
            }
            this.mouseNavigation = true;
            this.mouseReleaseOptionNr = this.curOption;
        }
    }

    // Token: 0x06000379 RID: 889 RVA: 0x00003B29 File Offset: 0x00001D29
    public virtual void mouseClickRelease()
    {
        if (this.menuEnabled)
        {
            this.gotMouseInput = false;
            this.mouseNavigation = true;
            this.mouseSetCurOption(this.mouseReleaseOptionNr);
        }
    }

    // Token: 0x0600037A RID: 890 RVA: 0x00051308 File Offset: 0x0004F508
    public virtual void createOptionWithInputSymbol(int nr, string optionText, string symbolName, bool isKeyboard, TextAnchor alignment, bool executeOnChange, bool refreshOnUpdateAll, float extraYOffset)
    {
        this.createOption(nr, optionText, "   ", alignment, executeOnChange, refreshOnUpdateAll, extraYOffset + (float)7);
        if (symbolName != "  ")
        {
            GameObject inputSymbolFromButtonName = this.inputHelperScript.GetInputSymbolFromButtonName(symbolName, isKeyboard);
            RectTransform rectTransform = (RectTransform)inputSymbolFromButtonName.GetComponent(typeof(RectTransform));
            rectTransform.SetParent(this.optionsInputSymbolHolder[nr], false);
            if (isKeyboard && symbolName.Contains("Mouse"))
            {
                rectTransform.localScale = Vector3.one * 0.75f;
            }
            else
            {
                rectTransform.localScale = Vector3.one;
            }
            rectTransform.pivot = new Vector2((float)0, 0.5f);
            rectTransform.anchoredPosition = new Vector2((float)0, (float)13);
        }
    }

    // Token: 0x0600037B RID: 891 RVA: 0x000513E4 File Offset: 0x0004F5E4
    public virtual void createOption(int nr, string optionText, string theOptions, TextAnchor alignment, bool executeOnChange, bool refreshOnUpdateAll, float extraYOffset)
    {
        this.options[nr] = (RectTransform)UnityEngine.Object.Instantiate<GameObject>(this.option.gameObject, this.contentWrapper).GetComponent(typeof(RectTransform));
        this.options[nr].gameObject.SetActive(true);
        this.options[nr].localScale = Vector3.one * 0.9f;
        this.optionsText[nr] = (Text)this.options[nr].GetComponent(typeof(Text));
        this.optionsText[nr].text = optionText;
        this.optionsText[nr].alignment = alignment;
        this.optionsSettingText[nr] = (Text)this.options[nr].Find("Setting").GetComponent(typeof(Text));
        this.optionsText[nr].color = (this.optionsSettingText[nr].color = this.unselectedColor);
        this.optionRefreshOnUpdateAll[nr] = refreshOnUpdateAll;
        this.optionExecuteOnChange[nr] = executeOnChange;
        this.optionsSettingOptions[nr] = theOptions;
        if (theOptions == string.Empty)
        {
            this.optionsSettingText[nr].text = string.Empty;
        }
        if (theOptions == "Slider")
        {
            this.optionsSettingText[nr].text = string.Empty;
            this.optionsSliderContainer[nr] = (RectTransform)this.options[nr].Find("Slider").GetComponent(typeof(RectTransform));
            this.optionsSliderContainerImage[nr] = (Image)this.optionsSliderContainer[nr].GetComponent(typeof(Image));
            this.optionsSettingText[nr] = (Text)this.options[nr].Find("Slider/SliderValue").GetComponent(typeof(Text));
            this.optionsSlider[nr] = (Image)this.options[nr].Find("Slider/SliderFill").GetComponent(typeof(Image));
        }
        else
        {
            this.options[nr].Find("Slider").gameObject.SetActive(false);
        }
        this.optionsInputSymbolHolder[nr] = (RectTransform)this.options[nr].Find("InputSymbolHolder").GetComponent(typeof(RectTransform));
        this.curOptionSorted[(int)this.curOptionSortedNr] = nr;
        this.curOption = nr;
        this.curOptionSortedNr = Mathf.Clamp(this.curOptionSortedNr + (float)1, (float)0, this.nrOfOptions - (float)1);
        ((OptionsMenuClickAreaScript)this.options[nr].Find("ClickArea").GetComponent(typeof(OptionsMenuClickAreaScript))).optionNr = nr;
        this.yOffset -= extraYOffset;
        float y = this.yOffset;
        Vector2 anchoredPosition = this.options[nr].anchoredPosition;
        float num = anchoredPosition.y = y;
        Vector2 vector = this.options[nr].anchoredPosition = anchoredPosition;
        this.yOffset -= (float)23;
    }

    // Token: 0x0600037C RID: 892 RVA: 0x0005171C File Offset: 0x0004F91C
    public virtual void createSmallHeader(string headerText, float extraYOffset)
    {
        UnityScript.Lang.Array array = new UnityScript.Lang.Array();
        if (!RuntimeServices.EqualityOperator(this.smallHeaders, null))
        {
            array = new UnityScript.Lang.Array(this.smallHeaders);
        }
        array.Push((RectTransform)UnityEngine.Object.Instantiate<RectTransform>(this.smallHeader, this.contentWrapper).GetComponent(typeof(RectTransform)));
        this.smallHeaders = (array.ToBuiltin(typeof(RectTransform)) as RectTransform[]);
        this.smallHeaders[this.smallHeaders.Length - 1].gameObject.SetActive(true);
        ((Text)this.smallHeaders[this.smallHeaders.Length - 1].GetComponent(typeof(Text))).text = headerText;
        if (this.extraInfo.gameObject.activeSelf)
        {
            RectTransform rectTransform = this.smallHeaders[this.smallHeaders.Length - 1];
            float x = rectTransform.anchoredPosition.x - (float)120;
            Vector2 anchoredPosition = rectTransform.anchoredPosition;
            float num = anchoredPosition.x = x;
            Vector2 vector = rectTransform.anchoredPosition = anchoredPosition;
        }
        this.yOffset -= extraYOffset + (float)6;
        float y = this.yOffset;
        Vector2 anchoredPosition2 = this.smallHeaders[this.smallHeaders.Length - 1].anchoredPosition;
        float num2 = anchoredPosition2.y = y;
        Vector2 vector2 = this.smallHeaders[this.smallHeaders.Length - 1].anchoredPosition = anchoredPosition2;
        this.yOffset -= (float)23;
    }

    // Token: 0x0600037D RID: 893 RVA: 0x000518BC File Offset: 0x0004FABC
    public virtual void createNavigationHints()
    {
        float alpha = (float)1;
        float alpha2 = (float)1;
        if (this.uiBackHint != null)
        {
            alpha = this.uiConfirmHintCanvasGroup.alpha;
            alpha2 = this.uiBackHintCanvasGroup.alpha;
            UnityEngine.Object.Destroy(this.uiBackHint);
            UnityEngine.Object.Destroy(this.uiConfirmHint);
        }
        this.uiConfirmHint = this.rootShared.createHintText("<UISUBMIT> " + this.rootShared.GetTranslation("uiConfirm"), "uiConfirm", this.transform, this.useGamepadIcons, true);
        this.uiConfirmHintCanvasGroup = (CanvasGroup)this.uiConfirmHint.AddComponent(typeof(CanvasGroup));
        this.uiConfirmHintCanvasGroup.alpha = alpha;
        this.uiConfirmHint.transform.localScale = this.uiConfirmHint.transform.localScale * 0.75f;
        RectTransform rectTransform = (RectTransform)this.uiConfirmHint.GetComponent(typeof(RectTransform));
        rectTransform.anchorMin = (rectTransform.anchorMax = new Vector2((float)1, (float)0));
        rectTransform.anchoredPosition = new Vector2(-rectTransform.sizeDelta.x - (float)30, (float)-15);
        this.uiBackHint = this.rootShared.createHintText("<UIBACK> " + this.rootShared.GetTranslation("mBack"), "uiBack", this.transform, this.useGamepadIcons, true);
        this.uiBackHintCanvasGroup = (CanvasGroup)this.uiBackHint.AddComponent(typeof(CanvasGroup));
        this.uiBackHintCanvasGroup.alpha = alpha2;
        this.uiBackHint.transform.localScale = this.uiBackHint.transform.localScale * 0.75f;
        RectTransform rectTransform2 = (RectTransform)this.uiBackHint.GetComponent(typeof(RectTransform));
        rectTransform2.anchorMin = (rectTransform2.anchorMax = new Vector2((float)1, (float)0));
        rectTransform2.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x - rectTransform2.sizeDelta.x * 0.75f - (float)20, (float)-15);
    }

    // Token: 0x0600037E RID: 894 RVA: 0x00051AF8 File Offset: 0x0004FCF8
    public virtual void updateNavigationHints()
    {
        if (this.curActiveMenu != (float)-2)
        {
            if (this.menuBackOptionNr >= 0)
            {
                this.uiBackHintCanvasGroup.alpha = (float)1;
            }
            else
            {
                this.uiBackHintCanvasGroup.alpha = (float)0;
            }
            this.uiConfirmHintCanvasGroup.alpha = (float)1;
            if (this.optionsSettingOptions[this.curOption] != "   " && (this.optionsSlider[this.curOption] != null || this.disableOption[this.curOption] || this.optionsSettingOptions[this.curOption] != string.Empty))
            {
                this.uiConfirmHintCanvasGroup.alpha = 0.1f;
            }
        }
        else
        {
            this.uiConfirmHintCanvasGroup.alpha = (this.uiBackHintCanvasGroup.alpha = (float)0);
        }
    }

    // Token: 0x0600037F RID: 895 RVA: 0x00051BE8 File Offset: 0x0004FDE8
    public virtual void clearLeaderboardEntries()
    {
        for (int i = 0; i < 10; i++)
        {
            this.leaderboardDisplayEntry[i].text = (this.leaderboardDisplayEntryScore[i].text = string.Empty);
        }
    }

    // Token: 0x06000380 RID: 896 RVA: 0x00051C2C File Offset: 0x0004FE2C
    public virtual string convertToTimeFormat(float timer)
    {
        int num = Mathf.FloorToInt(timer / 60f);
        int num2 = Mathf.FloorToInt(timer - (float)(num * 60));
        int num3 = Mathf.FloorToInt((float)100 * (timer - (float)(num * 60) - (float)num2));
        return string.Format("{0:0}:{1:00}.<size=16>{2:00}</size>", num, num2, num3);
    }

    // Token: 0x06000381 RID: 897 RVA: 0x00003B50 File Offset: 0x00001D50
    public virtual float DampUnscaled(float target, float source, float smoothing)
    {
        return Mathf.Lerp(source, target, (float)1 - Mathf.Exp(-smoothing * (Time.unscaledDeltaTime * (float)60)));
    }

    // Token: 0x06000382 RID: 898 RVA: 0x00003B6D File Offset: 0x00001D6D
    public virtual Vector3 DampV3Unscaled(Vector3 target, Vector3 source, float smoothing)
    {
        return Vector3.Lerp(source, target, (float)1 - Mathf.Exp(-smoothing * (Time.unscaledDeltaTime * (float)60)));
    }

    // Token: 0x06000383 RID: 899 RVA: 0x00003B8A File Offset: 0x00001D8A
    public virtual Color DampColorUnscaled(Color target, Color source, float smoothing)
    {
        return Color.Lerp(source, target, (float)1 - Mathf.Exp(-smoothing * (Time.unscaledDeltaTime * (float)60)));
    }

    // Token: 0x06000384 RID: 900 RVA: 0x000020A7 File Offset: 0x000002A7
    public virtual void Main()
    {
    }

    // Token: 0x040009E0 RID: 2528
    public bool menuEnabled;

    // Token: 0x040009E1 RID: 2529
    private bool menuEnabledDoOnce;

    // Token: 0x040009E2 RID: 2530
    public bool inGame;

    // Token: 0x040009E3 RID: 2531
    public AudioMixer audioMixer;

    // Token: 0x040009E4 RID: 2532
    private AudioSource theAudioSource;

    // Token: 0x040009E5 RID: 2533
    private AudioSource theAudioSource2;

    // Token: 0x040009E6 RID: 2534
    private AudioSource theAudioSource3;

    // Token: 0x040009E7 RID: 2535
    private int curAudioSourceToUse;

    // Token: 0x040009E8 RID: 2536
    public Color selectedColor;

    // Token: 0x040009E9 RID: 2537
    public Color highlightedColor;

    // Token: 0x040009EA RID: 2538
    public Color unselectedColor;

    // Token: 0x040009EB RID: 2539
    public Color disabledColor;

    // Token: 0x040009EC RID: 2540
    private bool foundRoot;

    // Token: 0x040009ED RID: 2541
    private RootScript root;

    // Token: 0x040009EE RID: 2542
    private RootSharedScript rootShared;

    // Token: 0x040009EF RID: 2543
    private CanvasGroup theCanvasGroup;

    // Token: 0x040009F0 RID: 2544
    private Text theHeader;

    // Token: 0x040009F1 RID: 2545
    private Transform mask;

    // Token: 0x040009F2 RID: 2546
    private RectTransform contentWrapper;

    // Token: 0x040009F3 RID: 2547
    private RectTransform option;

    // Token: 0x040009F4 RID: 2548
    private RectTransform selectLine;

    // Token: 0x040009F5 RID: 2549
    private RectTransform theRectTransform;

    // Token: 0x040009F6 RID: 2550
    private RectTransform smallHeader;

    // Token: 0x040009F7 RID: 2551
    private float yOffset;

    // Token: 0x040009F8 RID: 2552
    private Vector2 startPos;

    // Token: 0x040009F9 RID: 2553
    private float nrOfOptions;

    // Token: 0x040009FA RID: 2554
    private RectTransform[] options;

    // Token: 0x040009FB RID: 2555
    private Text[] optionsText;

    // Token: 0x040009FC RID: 2556
    private Text[] optionsSettingText;

    // Token: 0x040009FD RID: 2557
    private Image[] optionsSlider;

    // Token: 0x040009FE RID: 2558
    private RectTransform[] optionsSliderContainer;

    // Token: 0x040009FF RID: 2559
    private Image[] optionsSliderContainerImage;

    // Token: 0x04000A00 RID: 2560
    private RectTransform[] optionsInputSymbolHolder;

    // Token: 0x04000A01 RID: 2561
    private string[] optionsSettingOptions;

    // Token: 0x04000A02 RID: 2562
    private int[] optionsSettingNr;

    // Token: 0x04000A03 RID: 2563
    private bool[] optionChanged;

    // Token: 0x04000A04 RID: 2564
    private bool[] disableOption;

    // Token: 0x04000A05 RID: 2565
    private bool[] optionRefreshOnUpdateAll;

    // Token: 0x04000A06 RID: 2566
    private bool[] optionExecuteOnChange;

    // Token: 0x04000A07 RID: 2567
    private RectTransform[] smallHeaders;

    // Token: 0x04000A08 RID: 2568
    private RectTransform extraInfo;

    // Token: 0x04000A09 RID: 2569
    private Text extraInfoText;

    // Token: 0x04000A0A RID: 2570
    private RectTransform levelSelectScreen;

    // Token: 0x04000A0B RID: 2571
    private Text levelSelectScreenRating;

    // Token: 0x04000A0C RID: 2572
    private Text levelSelectScreenScore;

    // Token: 0x04000A0D RID: 2573
    private Text levelSelectScreenTime;

    // Token: 0x04000A0E RID: 2574
    private RectTransform levelSelectScreenDifficultyRectTransform;

    // Token: 0x04000A0F RID: 2575
    private Text levelSelectScreenDifficulty;

    // Token: 0x04000A10 RID: 2576
    private int levelSelectScreenDifficultyLevel;

    // Token: 0x04000A11 RID: 2577
    private RectTransform levelSelectScreenDifficultyButton;

    // Token: 0x04000A12 RID: 2578
    private RectTransform levelSelectScreenLeaderboardRectTransform;

    // Token: 0x04000A13 RID: 2579
    private int leaderboardFilter;

    // Token: 0x04000A14 RID: 2580
    private RectTransform levelSelectScreenLeaderboardTypeTextRectTransform;

    // Token: 0x04000A15 RID: 2581
    private Text levelSelectScreenLeaderboardTypeText;

    // Token: 0x04000A16 RID: 2582
    private RectTransform levelSelectScreenLeaderboardTypeButton;

    // Token: 0x04000A17 RID: 2583
    private RectTransform levelSelectScreenLeaderboardPagePrevTextRectTransform;

    // Token: 0x04000A18 RID: 2584
    private RectTransform levelSelectScreenLeaderboardPageNextTextRectTransform;

    // Token: 0x04000A19 RID: 2585
    private RectTransform levelSelectScreenLeaderboardPageButton1;

    // Token: 0x04000A1A RID: 2586
    private RectTransform levelSelectScreenLeaderboardPageButton2;

    // Token: 0x04000A1B RID: 2587
    private GameObject leaderboardOfflineGameObject;

    // Token: 0x04000A1C RID: 2588
    private RectTransform leaderboardOfflineTextRectTransform;

    // Token: 0x04000A1D RID: 2589
    private RectTransform leaderboardOfflineButton;

    // Token: 0x04000A1E RID: 2590
    private Text[] leaderboardDisplayEntry;

    // Token: 0x04000A1F RID: 2591
    private Text[] leaderboardDisplayEntryScore;

    // Token: 0x04000A20 RID: 2592
    private string leaderboardToUse;

    // Token: 0x04000A21 RID: 2593
    private float leaderboardFetchTimer;

    // Token: 0x04000A22 RID: 2594
    private bool leaderboardFetchTimerDoOnce;

    // Token: 0x04000A23 RID: 2595
    private int[] curOptionSorted;

    // Token: 0x04000A24 RID: 2596
    public float curOptionSortedNr;

    // Token: 0x04000A25 RID: 2597
    private float prevCurOptionSortedNr;

    // Token: 0x04000A26 RID: 2598
    public int curOption;

    // Token: 0x04000A27 RID: 2599
    private float curActiveMenu;

    // Token: 0x04000A28 RID: 2600
    private float inputRepeatDelayX;

    // Token: 0x04000A29 RID: 2601
    private float inputRepeatDelayXLimit;

    // Token: 0x04000A2A RID: 2602
    private float inputRepeatDelayY;

    // Token: 0x04000A2B RID: 2603
    private float inputRepeatDelayYLimit;

    // Token: 0x04000A2C RID: 2604
    private bool allowNavigation;

    // Token: 0x04000A2D RID: 2605
    private float allowNavigationTimer;

    // Token: 0x04000A2E RID: 2606
    private Vector2 navInput;

    // Token: 0x04000A2F RID: 2607
    private bool gotMouseInput;

    // Token: 0x04000A30 RID: 2608
    private int mouseReleaseOptionNr;

    // Token: 0x04000A31 RID: 2609
    private float timeSinceKeyboardUsed;

    // Token: 0x04000A32 RID: 2610
    private RectTransform scrollbar;

    // Token: 0x04000A33 RID: 2611
    private RectTransform scrollbarBar;

    // Token: 0x04000A34 RID: 2612
    private float targetScrollPos;

    // Token: 0x04000A35 RID: 2613
    private bool mouseNavigation;

    // Token: 0x04000A36 RID: 2614
    private int menuBackOptionNr;

    // Token: 0x04000A37 RID: 2615
    private Player player;

    // Token: 0x04000A38 RID: 2616
    private InputMapper inputMapper;

    // Token: 0x04000A39 RID: 2617
    private InputMapper inputMapper2;

    // Token: 0x04000A3A RID: 2618
    private bool useGamepadIcons;

    // Token: 0x04000A3B RID: 2619
    private Resolution[] supportedResolutions;

    // Token: 0x04000A3C RID: 2620
    private InputHelperScript inputHelperScript;

    // Token: 0x04000A3D RID: 2621
    private Controller lastActiveGamepadController;

    // Token: 0x04000A3E RID: 2622
    private bool dontCheckForMissingGamepadInputsMapped;

    // Token: 0x04000A3F RID: 2623
    private GameObject uiBackHint;

    // Token: 0x04000A40 RID: 2624
    private CanvasGroup uiBackHintCanvasGroup;

    // Token: 0x04000A41 RID: 2625
    private GameObject uiConfirmHint;

    // Token: 0x04000A42 RID: 2626
    private CanvasGroup uiConfirmHintCanvasGroup;

    // Token: 0x04000A43 RID: 2627
    private bool lostInternetConnectionDoOnce;

    // Token: 0x04000A44 RID: 2628
    private GameObject gameModifiersNotice;
}
