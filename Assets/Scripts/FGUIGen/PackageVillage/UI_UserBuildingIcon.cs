/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageVillage
{
    public partial class UI_UserBuildingIcon : GComponent
    {
        public GLoader iconLoader;
        public GTextField txt_name;
        public const string URL = "ui://786ck8sbabonph";

        public static UI_UserBuildingIcon CreateInstance()
        {
            return (UI_UserBuildingIcon)UIPackage.CreateObject("PackageVillage", "UserBuildingIcon");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            iconLoader = (GLoader)GetChild("iconLoader");
            txt_name = (GTextField)GetChild("txt_name");
        }
    }
}