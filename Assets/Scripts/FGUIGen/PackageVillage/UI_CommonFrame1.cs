/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageVillage
{
    public partial class UI_CommonFrame1 : GLabel
    {
        public GButton btn_back;
        public const string URL = "ui://786ck8sbonn9i";

        public static UI_CommonFrame1 CreateInstance()
        {
            return (UI_CommonFrame1)UIPackage.CreateObject("PackageVillage", "CommonFrame1");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            btn_back = (GButton)GetChild("btn_back");
        }
    }
}