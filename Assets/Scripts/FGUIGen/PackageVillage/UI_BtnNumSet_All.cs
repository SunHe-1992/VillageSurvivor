/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageVillage
{
    public partial class UI_BtnNumSet_All : GButton
    {
        public Controller upOrDown;
        public const string URL = "ui://786ck8sbrtaxo5v";

        public static UI_BtnNumSet_All CreateInstance()
        {
            return (UI_BtnNumSet_All)UIPackage.CreateObject("PackageVillage", "BtnNumSet_All");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            upOrDown = GetController("upOrDown");
        }
    }
}