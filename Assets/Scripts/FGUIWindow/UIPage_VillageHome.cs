using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageVillage;
using UnityEngine;
using UniFramework.Event;
using SunHeTBS;
public class UIPage_VillageHome : FUIBase
{
    UI_VillageHome ui;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_VillageHome;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;
        ui.btn_buildings.onClick.Set(OnBtnBuilding);
        ui.btn_warehouse.onClick.Set(OnBtnWarehouse);
        ui.btn_AddVillager.onClick.Set(OnBtnAddVillager);
        ui.pawnHUDcomp.btn_close.onClick.Set(OnPawnHUDClose);
        ui.btn_help.onClick.Set(OnBtnHelpClick);

        var spdCtrl = ui.speedCtrl;
        spdCtrl.btn_pause.onClick.Set(OnBtnSpeedPause);
        spdCtrl.btn_x1.onClick.Set(OnBtnSpeedX1);
        spdCtrl.btn_x5.onClick.Set(OnBtnSpeedX5);
        spdCtrl.btn_x10.onClick.Set(OnBtnSpeedX10);
    }

    private void OnBtnHelpClick(EventContext context)
    {
        FUIManager.Inst.ShowUI<UIPage_InstructionUI>(FUIDef.FWindow.InstructionUI);
    }

    private void OnBtnAddVillager(EventContext context)
    {
        BattleDriver.Inst.AddVillager();
    }


    private void OnBtnWarehouse(EventContext context)
    {
        FUIManager.Inst.ShowUI<UIPage_Inventory>(FUIDef.FWindow.InventoryUI);

    }

    private void OnBtnBuilding(EventContext context)
    {
        FUIManager.Inst.ShowUI<UIPage_BuildingUI>(FUIDef.FWindow.BuildingUI);

    }

    protected override void OnShown()
    {
        base.OnShown();
        ShowPawnHUD(false);
        ui.btn_platform.visible = false;
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
    protected override void OnUpdate()
    {
        base.OnUpdate();
        RefreshHUD();
    }
    void RefreshHUD()
    {
        string msg = "";
        //score:
        msg += $"score={TBSPlayer.UserDetail.score}";
        msg += $"DecidingBuildingLocation={BattleDriver.Inst.DecidingBuildingLocation}";

        ui.txt_hud.text = msg;

        if (showPawnHUD)
        {
            this.displayingPawn = BattleDriver.Inst.clickedPawn;
        }
        if (displayingPawn != null && showPawnHUD)
        {
            RefreshPawnHUD();
        }

        ui.speedCtrl.txt_speed.text = $"Speed:{BattleDriver.Inst.speedScale}";
    }
    #region pawn HUD
    public static bool showPawnHUD = false;
    Pawn displayingPawn = null;
    void ShowPawnHUD(bool isshow)
    {
        showPawnHUD = isshow;
        ui.ctrl_pawnHUD.selectedIndex = isshow ? 1 : 0;
    }
    void RefreshPawnHUD()
    {
        ui.ctrl_pawnHUD.selectedIndex = 1;
        var comp = ui.pawnHUDcomp;
        comp.txt_name.text = displayingPawn.GetCharacterInfo();
        comp.txt_state.text = displayingPawn.GetStateInfo();
        comp.pBar_stamina.max = displayingPawn.staminaValueMax;
        comp.pBar_stamina.value = displayingPawn.staminaValue;
        comp.pBar_food.max = displayingPawn.bodyEnergyMax;
        comp.pBar_food.value = displayingPawn.bodyEnergyValue;
        comp.pBar_HP.max = displayingPawn.HPMax;
        comp.pBar_HP.value = displayingPawn.HP;
    }

    private void OnPawnHUDClose(EventContext context)
    {
        ShowPawnHUD(false);
        this.displayingPawn = null;
    }
    #endregion

    #region speed control

    void OnBtnSpeedPause()
    {
        BattleDriver.Inst.speedScale = 0;
    }
    void OnBtnSpeedX1()
    {
        BattleDriver.Inst.speedScale = 1;
    }
    void OnBtnSpeedX5()
    {
        BattleDriver.Inst.speedScale = 5;
    }
    void OnBtnSpeedX10()
    {
        BattleDriver.Inst.speedScale = 10;
    }
    #endregion
}
