/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageVillage
{
    public partial class UI_GameOverUI : GComponent
    {
        public UI_common_kuang_2 frame;
        public UI_Button_Common_11 btn_playAgain;
        public UI_Button_Common_11 btn_mainmenu;
        public const string URL = "ui://786ck8sbar72hhk0ue";

        public static UI_GameOverUI CreateInstance()
        {
            return (UI_GameOverUI)UIPackage.CreateObject("PackageVillage", "GameOverUI");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            frame = (UI_common_kuang_2)GetChild("frame");
            btn_playAgain = (UI_Button_Common_11)GetChild("btn_playAgain");
            btn_mainmenu = (UI_Button_Common_11)GetChild("btn_mainmenu");
        }
    }
}