using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageVillage;
using UnityEngine;
using UniFramework.Event;

public class UIPage_Store : FUIBase
{

    UI_StoreUI ui;
    UserItem buyingItem;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_StoreUI;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;
        //ui.btn_ok.onClick.Set(BtnOKClick);
        ui.frame.GetChild("btn_close").asButton.onClick.Set(OnBtnClose);
        ui.frame.title = "Store";
        ui.inventory_list.itemRenderer = this.ListItemRenderer;
        ui.itemDetailCom.btn_close.onClick.Set(HideDetailCom);
        ui.itemDetailCom.btn_use.title = "Buy";
        ui.itemDetailCom.btn_use.onClick.Set(ClickBuyItem);

    }
    protected override void OnShown()
    {
        base.OnShown();
        buyingItem = null;
        GenerateStoreData();
    }


    public override void Refresh(object param)
    {
        base.Refresh(param);

        RefreshContent();
    }
    protected override void OnHide()
    {
        base.OnHide();
        buyingItem = null;
    }
    void BtnOKClick()
    {
        FUIManager.Inst.ShowUI<UIPage_Debug>(FUIDef.FWindow.TestUI);
        FUIManager.Inst.HideUI(this);
    }

    void RefreshContent()
    {
        HideDetailCom();
        //display items
        this.ui.inventory_list.numItems = itemList.Count;
        ui.txt_gold.text = "��" + TBSPlayer.GetGoldAmount();
    }
    void OnBtnClose()
    {
        FUIManager.Inst.HideUI(this);
    }

    void ListItemRenderer(int index, GObject obj)
    {
        var mItem = obj as UI_InventoryItem;
        var info = itemList[index];
        var cfg = ConfigManager.table.Item.Get(info.itemId);

        UIService.Inst.ShowItemComp(mItem, info.itemId, info.itemCount);

        mItem.data = index;
        mItem.onClick.Set(OnItemClick);

        mItem.txt_count.text = "$" + cfg.Price;
    }
    void OnItemClick(EventContext ec)
    {
        int index = (int)(ec.sender as GObject).data;
        var info = TBSPlayer.UserDetail.items[index];
        buyingItem = info;
        Debugger.Log("click item id  = " + info.itemId);
        var cfg = ConfigManager.table.Item.Get(info.itemId);
        UIService.Inst.ShowItemDetail(this.ui.itemDetailCom, info.itemId, info.itemCount);
        //ui.itemDetailCom.itemCom.txt_count.text = ;
        string priceStr = "$" + cfg.Price + " Buy";
        ui.itemDetailCom.btn_use.onClick.Set(ClickBuyItem);
        ui.itemDetailCom.btn_use.title = priceStr;
        bool btnEnabled = true;
        if (TBSPlayer.GetGoldAmount() < cfg.Price)
            btnEnabled = false;

        ui.itemDetailCom.btn_use.enabled = btnEnabled;
    }
    void HideDetailCom()
    {
        ui.itemDetailCom.visible = false;
    }

    #region store data

    List<UserItem> itemList;
    void GenerateStoreData()
    {
        itemList = new List<UserItem>();
        itemList.Add(new UserItem(100, 333));
        itemList.Add(new UserItem(101, 555));

        for (int i = 103; i <= 110; i++)
        {
            itemList.Add(new UserItem(i, 99));
        }
    }
    #endregion

    void ClickBuyItem()
    {
        if (buyingItem != null)
        {
            var cfg = ConfigManager.table.Item.Get(buyingItem.itemId);
            if (TBSPlayer.IsAffordGold((long)cfg.Price))
            {
                TBSPlayer.SpendGold((long)cfg.Price);
                TBSPlayer.InsertItem(buyingItem.itemId, 1);
                HideDetailCom();
                RefreshContent();
            }
            else
            {
                Debugger.Log("can not afford");
            }
        }
    }
}
