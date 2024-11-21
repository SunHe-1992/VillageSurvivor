/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageVillage
{
    public partial class UI_ItemDetail : GComponent
    {
        public GButton btn_close;
        public GButton btn_use;
        public GTextField txt_detail;
        public UI_InventoryItem itemCom;
        public const string URL = "ui://786ck8sbgm4tlv";

        public static UI_ItemDetail CreateInstance()
        {
            return (UI_ItemDetail)UIPackage.CreateObject("PackageVillage", "ItemDetail");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            btn_close = (GButton)GetChild("btn_close");
            btn_use = (GButton)GetChild("btn_use");
            txt_detail = (GTextField)GetChild("txt_detail");
            itemCom = (UI_InventoryItem)GetChild("itemCom");
        }
    }
}