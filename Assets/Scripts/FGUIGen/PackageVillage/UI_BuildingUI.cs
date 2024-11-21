/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageVillage
{
    public partial class UI_BuildingUI : GComponent
    {
        public UI_common_kuang_2 frame;
        public UI_Button_Common_11 btn_confirm_building;
        public UI_Dropdown cbox_building;
        public GList list_building;
        public const string URL = "ui://786ck8sbpw3fpd";

        public static UI_BuildingUI CreateInstance()
        {
            return (UI_BuildingUI)UIPackage.CreateObject("PackageVillage", "BuildingUI");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            frame = (UI_common_kuang_2)GetChild("frame");
            btn_confirm_building = (UI_Button_Common_11)GetChild("btn_confirm_building");
            cbox_building = (UI_Dropdown)GetChild("cbox_building");
            list_building = (GList)GetChild("list_building");
        }
    }
}