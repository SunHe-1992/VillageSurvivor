using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageVillage;
using UnityEngine;
using UniFramework.Event;


public class UIPage_Inventory : FUIBase
{

    UI_InventoryUI ui;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_InventoryUI;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;
        //ui.btn_ok.onClick.Set(BtnOKClick);
        ui.frame.GetChild("btn_close").asButton.onClick.Set(OnBtnClose);
        ui.frame.title = "Inventory";
        ui.inventory_list.itemRenderer = this.ListItemRenderer;
        ui.itemDetailCom.btn_close.onClick.Set(HideDetailCom);
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
    public override void NotifyRefresh()
    {
        base.NotifyRefresh();
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
        HideDetailCom();
        int count = TBSPlayer.UserDetail.items.Count;
        this.ui.inventory_list.numItems = count;
        ui.txt_gold.text = "¡Á" + TBSPlayer.GetGoldAmount();
    }
    void OnBtnClose()
    {
        FUIManager.Inst.HideUI(this);
    }

    void ListItemRenderer(int index, GObject obj)
    {
        var mItem = obj as UI_InventoryItem;
        var info = TBSPlayer.UserDetail.items[index];
        var cfg = ConfigManager.table.Item.Get(info.itemId);

        UIService.Inst.ShowItemComp(mItem, info.itemId, info.itemCount);

        mItem.data = index;
        mItem.onClick.Set(OnItemClick);
    }
    void OnItemClick(EventContext ec)
    {
        int index = (int)(ec.sender as GObject).data;
        var info = TBSPlayer.UserDetail.items[index];
        Debugger.Log("click item id  = " + info.itemId);
        var cfg = ConfigManager.table.Item.Get(info.itemId);

        UIService.Inst.ShowItemDetail(this.ui.itemDetailCom, info.itemId, info.itemCount);
    }
    void HideDetailCom()
    {
        ui.itemDetailCom.visible = false;
    }
}
