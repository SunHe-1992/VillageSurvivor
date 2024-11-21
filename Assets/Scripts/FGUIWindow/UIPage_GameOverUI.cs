using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using UnityEngine;
using UniFramework.Event;
using PackageVillage;

public class UIPage_GameOverUI : FUIBase
{

    UI_GameOverUI ui;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_GameOverUI;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;
        //ui.btn_ok.onClick.Set(BtnOKClick);
        //ui.btn_close.onClick.Set(OnBtnClose);
        ui.frame.GetChild("btn_close").asButton.onClick.Set(OnBtnClose);

        ui.btn_mainmenu.onClick.Set(OnBtnMainMenu);
        ui.btn_playAgain.onClick.Set(OnBtnPlayAgain);
    }

    private void OnBtnPlayAgain(EventContext context)
    {
        throw new NotImplementedException();
    }

    private void OnBtnMainMenu(EventContext context)
    {
        FUIManager.Inst.ShowUI<UIPage_VillageMenu>(FUIDef.FWindow.VillageMenuUI);
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



    }
    void OnBtnClose()
    {
        FUIManager.Inst.HideUI(this);
    }
}