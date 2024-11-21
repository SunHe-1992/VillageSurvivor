using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageVillage;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UniFramework.Event;
public class UIPage_Debug : FUIBase
{

    UI_TestUI ui;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_TestUI;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;
        ui.btn_test.onClick.Set(BtnTestClick);
        ui.btn_village.onClick.Set(LoadVillage);
        ui.btn_close.onClick.Set(OnBtnClose);
        ui.btn_addGold100.onClick.Set(BtnAddGold);
        ui.btn_slot.onClick.Set(BtnSlotGame);
        ui.btn_fishing.onClick.Set(BtnFishingGame);
        ui.btn_damageVillian.onClick.Set(BtnDmg);

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
    void BtnTestClick()
    {
        OnBtnClose();

        ReadJsonConfig();
    }


    void RefreshContent()
    {


    }

    void LoadVillage()
    {
        SceneManager.LoadScene("Village");
        OnBtnClose();
    }


    protected override void OnUpdate()
    {
        base.OnUpdate();


    }
    void OnBtnClose()
    {
        FUIManager.Inst.HideUI(this);
    }


    void BtnAddGold()
    {
        //TBSPlayer.UpdateGoldAmount(10000);
        //UniEvent.SendMessage(GameEventDefine.POINTS_CHANGED);
    }

    void BtnSlotGame()
    {
        OnBtnClose();
        //MinigameService.Inst.SetUpSlotGameData();
    }


    void BtnFishingGame()
    {
        OnBtnClose();
    }

    void BtnDmg()
    {

    }



    void ReadJsonConfig()
    {
        //read data by id
        //int buildingId = 10;
        //cfg.BuildingData data = ConfigManager.table.TbBuilding.Get(buildingId);

        ////list all data in this sheet
        //foreach (var _data in ConfigManager.table.TbBuilding.DataList)
        //{
        //    Debugger.Log($"data id ={_data.ID} name={_data.Name}");
        //}
    }
}
