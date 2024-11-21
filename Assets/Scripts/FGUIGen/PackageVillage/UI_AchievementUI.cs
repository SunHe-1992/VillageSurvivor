/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageVillage
{
    public partial class UI_AchievementUI : GComponent
    {
        public UI_common_kuang_2 frame;
        public GButton btn_close;
        public GList list_achi;
        public const string URL = "ui://786ck8sbp8p4ow";

        public static UI_AchievementUI CreateInstance()
        {
            return (UI_AchievementUI)UIPackage.CreateObject("PackageVillage", "AchievementUI");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            frame = (UI_common_kuang_2)GetChild("frame");
            btn_close = (GButton)GetChild("btn_close");
            list_achi = (GList)GetChild("list_achi");
        }
    }
}