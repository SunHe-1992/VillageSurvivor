/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageVillage
{
    public partial class UI_OptionsUI : GComponent
    {
        public UI_common_kuang_2 frame;
        public UI_Button_Common_11 btn_exit;
        public UI_ButtonCheck btn_music;
        public UI_ButtonCheck btn_sound;
        public UI_ButtonCheck btn_easy;
        public UI_ButtonCheck btn_medium;
        public UI_ButtonCheck btn_hard;
        public const string URL = "ui://786ck8sban0qhhk0u8";

        public static UI_OptionsUI CreateInstance()
        {
            return (UI_OptionsUI)UIPackage.CreateObject("PackageVillage", "OptionsUI");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            frame = (UI_common_kuang_2)GetChild("frame");
            btn_exit = (UI_Button_Common_11)GetChild("btn_exit");
            btn_music = (UI_ButtonCheck)GetChild("btn_music");
            btn_sound = (UI_ButtonCheck)GetChild("btn_sound");
            btn_easy = (UI_ButtonCheck)GetChild("btn_easy");
            btn_medium = (UI_ButtonCheck)GetChild("btn_medium");
            btn_hard = (UI_ButtonCheck)GetChild("btn_hard");
        }
    }
}