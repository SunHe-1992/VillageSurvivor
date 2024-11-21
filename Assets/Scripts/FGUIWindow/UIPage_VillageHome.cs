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

        ui.txt_hud.text = msg;
    }
}
