/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageVillage
{
    public partial class UI_BuildingInfo : GComponent
    {
        public Controller ctrl_guide;
        public UI_common_kuang_2 frame;
        public GList list_order;
        public UI_Button_Common_11 btn_confirm_produce;
        public UI_Dropdown cbox_product;
        public GTextField txt_workers;
        public UI_NumSet numSetter_woker;
        public const string URL = "ui://786ck8sbry5ip8";

        public static UI_BuildingInfo CreateInstance()
        {
            return (UI_BuildingInfo)UIPackage.CreateObject("PackageVillage", "BuildingInfo");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_guide = GetController("ctrl_guide");
            frame = (UI_common_kuang_2)GetChild("frame");
            list_order = (GList)GetChild("list_order");
            btn_confirm_produce = (UI_Button_Common_11)GetChild("btn_confirm_produce");
            cbox_product = (UI_Dropdown)GetChild("cbox_product");
            txt_workers = (GTextField)GetChild("txt_workers");
            numSetter_woker = (UI_NumSet)GetChild("numSetter_woker");
        }
    }
}