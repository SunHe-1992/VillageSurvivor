using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageVillage;
using UnityEngine;
using UniFramework.Event;
using UnityEngine.SceneManagement;


public class UIPage_VillageMenu : FUIBase
{

    UI_VillageMenuUI ui;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_VillageMenuUI;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;
        ui.btn_newGame.onClick.Set(OnBtnNewGame);
        ui.btn_exit.onClick.Set(OnBtnExit);
        ui.btn_instructions.onClick.Set(OnBtnInstruction);
        ui.btn_options.onClick.Set(OnBtnOptions);
    }

    private void OnBtnOptions(EventContext context)
    {
        FUIManager.Inst.ShowUI<UIPage_OptionsUI>(FUIDef.FWindow.OptionsUI);

    }

    private void OnBtnInstruction(EventContext context)
    {
        FUIManager.Inst.ShowUI<UIPage_InstructionUI>(FUIDef.FWindow.InstructionUI);
    }

    private void OnBtnExit(EventContext context)
    {
        Application.Quit();
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
        OnBtnClose();
    }

    void RefreshContent()
    {


    }
    void OnBtnClose()
    {
        FUIManager.Inst.HideUI(this);
    }

    void OnBtnNewGame()
    {
        string VillageSceneName = "Village";
        SceneManager.LoadScene(VillageSceneName, LoadSceneMode.Single);
        OnBtnClose();
    }
}
