/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageVillage
{
    public partial class UI_AchievementItem : GComponent
    {
        public GLoader iconLoader;
        public GTextField txt_des;
        public GTextField txt_name;
        public const string URL = "ui://786ck8sbp8p4ox";

        public static UI_AchievementItem CreateInstance()
        {
            return (UI_AchievementItem)UIPackage.CreateObject("PackageVillage", "AchievementItem");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            iconLoader = (GLoader)GetChild("iconLoader");
            txt_des = (GTextField)GetChild("txt_des");
            txt_name = (GTextField)GetChild("txt_name");
        }
    }
}