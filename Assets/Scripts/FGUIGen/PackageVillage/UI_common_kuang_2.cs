/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageVillage
{
    public partial class UI_common_kuang_2 : GLabel
    {
        public Controller hasClose;
        public GButton btn_close;
        public const string URL = "ui://786ck8sbpf6ao88";

        public static UI_common_kuang_2 CreateInstance()
        {
            return (UI_common_kuang_2)UIPackage.CreateObject("PackageVillage", "common_kuang_2");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            hasClose = GetController("hasClose");
            btn_close = (GButton)GetChild("btn_close");
        }
    }
}