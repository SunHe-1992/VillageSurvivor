using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using UnityEngine;
using UniFramework.Event;
using PackageVillage;

public class UIPage_OptionsUI : FUIBase
{

    UI_OptionsUI ui;

    /// <summary>
    /// 0,1,2
    /// </summary>
    int getDifficulty()
    {
        return TBSPlayer.UserDetail.difficulty;
    }
    List<UI_ButtonCheck> btns;


    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_OptionsUI;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;
        //ui.btn_ok.onClick.Set(BtnOKClick);
        ui.btn_exit.onClick.Set(OnBtnClose);
        ui.frame.GetChild("btn_close").asButton.onClick.Set(OnBtnClose);

        ui.btn_sound.onClick.Set(OnBtnSound);
        ui.btn_music.onClick.Set(OnBtnMusic);


        btns = new List<UI_ButtonCheck>()
        {
            ui.btn_easy, ui.btn_medium,ui.btn_hard
        };

        ui.btn_easy.onClick.Set(OnBtnEasy);
        ui.btn_medium.onClick.Set(OnBtnMedium);
        ui.btn_hard.onClick.Set(OnBtnHard);

    }


    private void OnBtnMedium(EventContext context)
    {
        TBSPlayer.UserDetail.difficulty = 1;
        ResetButtons();
    }

    private void OnBtnHard(EventContext context)
    {
        TBSPlayer.UserDetail.difficulty = 2;
        ResetButtons();
    }

    private void OnBtnEasy(EventContext context)
    {
        TBSPlayer.UserDetail.difficulty = 0;
        ResetButtons();
    }
    void ResetButtons()
    {
        for (int i = 0; i < btns.Count; i++)
        {
            UI_ButtonCheck btn = btns[i];
            btn.ctrl_check.selectedIndex = i == getDifficulty() ? 1 : 0;
        }

        ui.btn_music.ctrl_check.selectedIndex = TBSPlayer.UserDetail.enableMusic ? 1 : 0;
        ui.btn_sound.ctrl_check.selectedIndex = TBSPlayer.UserDetail.enableSound ? 1 : 0;
    }

    private void OnBtnMusic(EventContext context)
    {
        TBSPlayer.UserDetail.enableMusic = !TBSPlayer.UserDetail.enableMusic;
        ResetButtons();
    }

    private void OnBtnSound(EventContext context)
    {
        TBSPlayer.UserDetail.enableSound = !TBSPlayer.UserDetail.enableSound;
        ResetButtons();
    }

    protected override void OnShown()
    {
        base.OnShown();

    }


    public override void Refresh(object param)
    {
        base.Refresh(param);

        RefreshContent();
    }
    protected override void OnHide()
    {
        base.OnHide();

    }
    void BtnOKClick()
    {
        FUIManager.Inst.ShowUI<UIPage_Debug>(FUIDef.FWindow.TestUI);
        FUIManager.Inst.HideUI(this);
    }

    void RefreshContent()
    {
        ResetButtons();

    }
    void OnBtnClose()
    {
        FUIManager.Inst.HideUI(this);
    }
}