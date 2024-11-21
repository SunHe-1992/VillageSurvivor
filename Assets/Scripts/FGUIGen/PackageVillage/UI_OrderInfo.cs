/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageVillage
{
    public partial class UI_OrderInfo : GComponent
    {
        public Controller ctrl_paused;
        public UI_ProgressBar1 sliderHP;
        public GTextField txt_orderName;
        public UI_NumSet numberSet;
        public UI_Button_Common_11 btn_pause;
        public UI_Button_Common_11 btn_delete;
        public const string URL = "ui://786ck8sbfe3qpa";

        public static UI_OrderInfo CreateInstance()
        {
            return (UI_OrderInfo)UIPackage.CreateObject("PackageVillage", "OrderInfo");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_paused = GetController("ctrl_paused");
            sliderHP = (UI_ProgressBar1)GetChild("sliderHP");
            txt_orderName = (GTextField)GetChild("txt_orderName");
            numberSet = (UI_NumSet)GetChild("numberSet");
            btn_pause = (UI_Button_Common_11)GetChild("btn_pause");
            btn_delete = (UI_Button_Common_11)GetChild("btn_delete");
        }
    }
}