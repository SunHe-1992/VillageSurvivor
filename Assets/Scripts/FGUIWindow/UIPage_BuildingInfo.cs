using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using PackageVillage;
using UnityEngine;
using UniFramework.Event;
using SunHeTBS;
/// <summary>
/// UI for 1 building and products
/// </summary>
public class UIPage_BuildingInfo : FUIBase
{

    UI_BuildingInfo ui;

    Building bdInfo;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_BuildingInfo;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;
        ui.frame.title = "BuildingInfo";
        ui.frame.GetChild("btn_close").asButton.onClick.Set(OnBtnClose);
        ui.list_order.itemRenderer = this.OrderListRenderer;
        ui.btn_confirm_produce.onClick.Set(OnClickProduce);

        ui.numSetter_woker.btn_add.onClick.Set(AddOneWorker);
        ui.numSetter_woker.btn_del.onClick.Set(RemoveOneWorker);

    }



    private void OnClickProduce(EventContext context)
    {
        int idx = ui.cbox_product.selectedIndex;
        if (idx < list_id.Count)
        {
            if (int.TryParse(list_id[idx], out int productId))
            {

                bdInfo.CreateNewOrder(productId);
            }
        }
    }

    protected override void OnShown()
    {
        base.OnShown();

    }


    public override void Refresh(object param)
    {
        base.Refresh(param);

        int sid = (int)param;
        bdInfo = BattleDriver.Inst.GetBuildingBySid(sid);
        InitCBox();
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
        string bdName = bdInfo.BdCfg.Name;
        ui.frame.title = bdName;


    }
    List<string> list_name = new List<string>();
    List<string> list_id = new List<string>();
    private void InitCBox()
    {
        list_name.Clear();
        list_id.Clear();
        if (bdInfo != null)
        {
            foreach (var pd in bdInfo.productList)
            {
                list_name.Add(pd.Description);
                list_id.Add(pd.ID.ToString());
            }
            ui.cbox_product.items = list_name.ToArray();
            ui.cbox_product.values = list_id.ToArray();
            ui.cbox_product.selectedIndex = 0;
        }
    }

    void OnBtnClose()
    {
        FUIManager.Inst.HideUI(this);
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        RefreshOrderList();

        bool constructing = bdInfo.constructing;
        ui.btn_confirm_produce.enabled = !constructing;
        ui.cbox_product.enabled = !constructing;

        RefreshWorkers();
    }

    #region workers
    private void RefreshWorkers()
    {
        int workersCount = bdInfo.workerList.Count;
        string workerStr = $"Workers count {workersCount}\n";
        foreach (var worker in this.bdInfo.workerList)
        {
            workerStr += worker.nickName.ToString() + "\n";
        }

        ui.txt_workers.text = workerStr;

        ui.numSetter_woker.input_num.text = workersCount + "";

    }
    private void RemoveOneWorker(EventContext context)
    {
        BattleDriver.Inst.RemoveOneWorker(this.bdInfo);
    }

    private void AddOneWorker(EventContext context)
    {
        BattleDriver.Inst.AddOneWorker(this.bdInfo);
    }
    #endregion
    void RefreshOrderList()
    {
        if (this.bdInfo == null) return;
        ui.list_order.numItems = this.bdInfo.orderList.Count;


    }
    void OrderListRenderer(int index, GObject obj)
    {
        Order orderInfo = this.bdInfo.orderList[index];
        UI_OrderInfo mItem = obj as UI_OrderInfo;
        mItem.txt_orderName.text = orderInfo.name;
        var numSet = mItem.numberSet as UI_NumSet;

        numSet.input_num.text = orderInfo.repeatTime + "";
        numSet.btn_add.onClick.Set(() => { orderInfo.repeatTime++; });
        numSet.btn_del.onClick.Set(() => { orderInfo.repeatTime--; });

        mItem.sliderHP.value = orderInfo.CompletedWork;
        mItem.sliderHP.max = orderInfo.TotalWork;


        mItem.btn_pause.onClick.Set(() => { orderInfo.paused = !orderInfo.paused; });
        mItem.btn_delete.onClick.Set(() => { orderInfo.tobeDeleted = true; });
        mItem.ctrl_paused.selectedIndex = orderInfo.paused ? 1 : 0;
    }
}
