/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageVillage
{
    public partial class UI_Dropdown : GComboBox
    {
        public GButton button;
        public const string URL = "ui://786ck8sbzd9g41";

        public static UI_Dropdown CreateInstance()
        {
            return (UI_Dropdown)UIPackage.CreateObject("PackageVillage", "Dropdown");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            button = (GButton)GetChild("button");
        }
    }
}