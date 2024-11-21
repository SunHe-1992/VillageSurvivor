/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageVillage
{
    public partial class UI_InstructionUI : GComponent
    {
        public UI_common_kuang_2 frame;
        public const string URL = "ui://786ck8sban0qhhk0u9";

        public static UI_InstructionUI CreateInstance()
        {
            return (UI_InstructionUI)UIPackage.CreateObject("PackageVillage", "InstructionUI");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            frame = (UI_common_kuang_2)GetChild("frame");
        }
    }
}