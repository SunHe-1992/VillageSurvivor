/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageVillage
{
    public partial class UI_PawnHUD : GComponent
    {
        public GTextField txt_name;
        public GTextField txt_state;
        public UI_ProgressBar1 pBar_stamina;
        public UI_ProgressBar1 pBar_food;
        public GButton btn_close;
        public UI_ProgressBar1 pBar_HP;
        public const string URL = "ui://786ck8sbn4w3hhk0uf";

        public static UI_PawnHUD CreateInstance()
        {
            return (UI_PawnHUD)UIPackage.CreateObject("PackageVillage", "PawnHUD");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            txt_name = (GTextField)GetChild("txt_name");
            txt_state = (GTextField)GetChild("txt_state");
            pBar_stamina = (UI_ProgressBar1)GetChild("pBar_stamina");
            pBar_food = (UI_ProgressBar1)GetChild("pBar_food");
            btn_close = (GButton)GetChild("btn_close");
            pBar_HP = (UI_ProgressBar1)GetChild("pBar_HP");
        }
    }
}