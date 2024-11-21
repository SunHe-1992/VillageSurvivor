/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageVillage
{
    public partial class UI_VillageMenuUI : GComponent
    {
        public UI_Button_Common_11 btn_newGame;
        public UI_Button_Common_11 btn_exit;
        public GTextField txt_title;
        public GTextField txt_names;
        public UI_Button_Common_11 btn_options;
        public UI_Button_Common_11 btn_instructions;
        public Transition t0;
        public const string URL = "ui://786ck8sbm0fbpl";

        public static UI_VillageMenuUI CreateInstance()
        {
            return (UI_VillageMenuUI)UIPackage.CreateObject("PackageVillage", "VillageMenuUI");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            btn_newGame = (UI_Button_Common_11)GetChild("btn_newGame");
            btn_exit = (UI_Button_Common_11)GetChild("btn_exit");
            txt_title = (GTextField)GetChild("txt_title");
            txt_names = (GTextField)GetChild("txt_names");
            btn_options = (UI_Button_Common_11)GetChild("btn_options");
            btn_instructions = (UI_Button_Common_11)GetChild("btn_instructions");
            t0 = GetTransition("t0");
        }
    }
}