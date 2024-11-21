/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageVillage
{
    public partial class UI_VillageHome : GComponent
    {
        public UI_Button_Common_11 btn_buildings;
        public UI_Button_Common_11 btn_warehouse;
        public GTextField txt_hud;
        public UI_Button_Common_11 btn_AddVillager;
        public const string URL = "ui://786ck8sbhm4dhhk0tg";

        public static UI_VillageHome CreateInstance()
        {
            return (UI_VillageHome)UIPackage.CreateObject("PackageVillage", "VillageHome");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            btn_buildings = (UI_Button_Common_11)GetChild("btn_buildings");
            btn_warehouse = (UI_Button_Common_11)GetChild("btn_warehouse");
            txt_hud = (GTextField)GetChild("txt_hud");
            btn_AddVillager = (UI_Button_Common_11)GetChild("btn_AddVillager");
        }
    }
}