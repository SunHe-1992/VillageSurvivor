/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageVillage
{
    public partial class UI_Dropdown2 : GComponent
    {
        public GList list;
        public const string URL = "ui://786ck8sbzd9g47";

        public static UI_Dropdown2 CreateInstance()
        {
            return (UI_Dropdown2)UIPackage.CreateObject("PackageVillage", "Dropdown2");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            list = (GList)GetChild("list");
        }
    }
}