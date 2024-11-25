/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageVillage
{
    public partial class UI_SpeedController : GComponent
    {
        public Controller ctrl_speed;
        public GButton btn_pause;
        public GButton btn_x1;
        public GButton btn_x5;
        public GButton btn_x10;
        public GTextField txt_speed;
        public const string URL = "ui://786ck8sbuhl3hhk0uk";

        public static UI_SpeedController CreateInstance()
        {
            return (UI_SpeedController)UIPackage.CreateObject("PackageVillage", "SpeedController");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_speed = GetController("ctrl_speed");
            btn_pause = (GButton)GetChild("btn_pause");
            btn_x1 = (GButton)GetChild("btn_x1");
            btn_x5 = (GButton)GetChild("btn_x5");
            btn_x10 = (GButton)GetChild("btn_x10");
            txt_speed = (GTextField)GetChild("txt_speed");
        }
    }
}