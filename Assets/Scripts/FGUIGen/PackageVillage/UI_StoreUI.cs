/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageVillage
{
    public partial class UI_StoreUI : GComponent
    {
        public UI_common_kuang_2 frame;
        public GList inventory_list;
        public UI_ItemDetail itemDetailCom;
        public GTextField txt_gold;
        public const string URL = "ui://786ck8sb7ot0lw";

        public static UI_StoreUI CreateInstance()
        {
            return (UI_StoreUI)UIPackage.CreateObject("PackageVillage", "StoreUI");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            frame = (UI_common_kuang_2)GetChild("frame");
            inventory_list = (GList)GetChild("inventory_list");
            itemDetailCom = (UI_ItemDetail)GetChild("itemDetailCom");
            txt_gold = (GTextField)GetChild("txt_gold");
        }
    }
}