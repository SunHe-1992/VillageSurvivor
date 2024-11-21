/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageVillage
{
    public partial class UI_ButtonCheck : GButton
    {
        public Controller ctrl_check;
        public const string URL = "ui://786ck8sban0qhhk0uc";

        public static UI_ButtonCheck CreateInstance()
        {
            return (UI_ButtonCheck)UIPackage.CreateObject("PackageVillage", "ButtonCheck");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_check = GetController("ctrl_check");
        }
    }
}