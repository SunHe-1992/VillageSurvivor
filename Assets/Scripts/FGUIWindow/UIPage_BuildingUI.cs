using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageVillage;
using UnityEngine;
using UniFramework.Event;
using SunHeTBS;
public class UIPage_BuildingUI : FUIBase
{

    UI_BuildingUI ui;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_BuildingUI;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;
        ui.frame.title = "Building";
        ui.frame.GetChild("btn_close").asButton.onClick.Set(OnBtnClose);

        InitData();
        ui.btn_confirm_building.onClick.Set(ConfirmBuilding);

        ui.list_building.itemRenderer = RendererUserBuilding;
    }


    List<string> list_name = new List<string>();
    List<string> list_id = new List<string>();
    private void InitData()
    {
        list_name = new List<string>();
        list_id = new List<string>();
        foreach (var data in ConfigManager.table.TbBuilding.DataList)
        {
            if (data.Visible == false) continue;

            list_id.Add(data.ID.ToString());
            string name = $"{data.Category}_{data.Name}";
            list_name.Add(name);
        }
        ui.cbox_building.items = list_name.ToArray();
        ui.cbox_building.values = list_id.ToArray();
        ui.cbox_building.selectedIndex = 0;

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

        ui.list_building.numItems = BattleDriver.Inst.buildings.Count;
    }
    void OnBtnClose()
    {
        FUIManager.Inst.HideUI(this);
    }


    private void ConfirmBuilding(EventContext context)
    {
        int idx = ui.cbox_building.selectedIndex;
        int bdId = int.Parse(list_id[idx]);
        var cfg = ConfigManager.table.TbBuilding.GetOrDefault(bdId);
        if (cfg != null)
        {
            Debug.Log(cfg.ToString());
            OnBtnClose();
            DelayInvoker.Inst.DelayInvoke(
                () => { BattleDriver.Inst.EnterDecidingBuidlingLocation(bdId); }
                , 0.2f);
        }
        else
        {
            ;
        }
        //TBSPlayer.CreateBuilding(cfg.ID);
        //var bd = new Building(bdId);
        //BattleDriver.Inst.buildings.Add(bd);
        //RefreshContent();

    }
    private void RendererUserBuilding(int index, GObject item)
    {
        var data = BattleDriver.Inst.buildings[index];
        var mItem = item as UI_UserBuildingIcon;
        var cfg = ConfigManager.table.TbBuilding.Get(data.buildingId);

        string packName = FUIDef.FPackage.PackageVillage.ToString();
        mItem.iconLoader.url = $"ui://{packName}/{cfg.IconName}";//icon from fairy
        mItem.txt_name.text = cfg.Name;
        mItem.onClick.Set(() =>
        {
            //  open UI of this building;
            FUIManager.Inst.ShowUI<UIPage_BuildingInfo>(FUIDef.FWindow.BuildingInfo, null, data.sid);
        });

    }
}
