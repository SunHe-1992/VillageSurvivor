using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using UnityEngine;
using UniFramework.Event;
using PackageVillage;

public class UIPage_AchievementUI : FUIBase
{

    UI_AchievementUI ui;
    protected override void OnInit()
    {
        base.OnInit();
        ui = this.contentPane as UI_AchievementUI;
        this.uiShowType = UIShowType.WINDOW;
        this.animationType = (int)FUIManager.OpenUIAnimationType.NoAnimation;
        ui.btn_close.onClick.Set(OnBtnClose);
        ui.list_achi.itemRenderer = this.AchiListRenderer;
        ui.frame.title = "Achievements";
        ui.frame.GetChild("btn_close").asButton.onClick.Set(OnBtnClose);
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
        var list = TBSPlayer.UserDetail.achievementList;
        ui.list_achi.numItems = list.Count;

    }
    void OnBtnClose()
    {
        FUIManager.Inst.HideUI(this);
    }
    void AchiListRenderer(int index, GObject obj)
    {
        var mItem = obj as UI_AchievementItem;
        var list = TBSPlayer.UserDetail.achievementList;
        int achiId = list[index];
        var cfg = ConfigManager.table.TbAchi.Get(achiId);
        mItem.txt_name.text = cfg.Title;
        mItem.txt_des.text = cfg.Content;
        //mItem.iconLoader.url = "ui://"
        string packName = FUIDef.FPackage.PackageVillage.ToString();
        mItem.iconLoader.url = $"ui://{packName}/{cfg.Icon}";//icon from fairy

    }
}
