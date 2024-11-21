using System.Collections.Generic;
public class FUIDef
{
    /// <summary>
    /// window names
    /// </summary>
    public enum FWindow
    {
        Default,
        BuildingInfo,
        BuildingUI,
        AchievementUI,
        QuestUI,
        StoreUI,
        InventoryUI,
        TestUI,
        VillageHome,
        VillageMenuUI,
        InstructionUI,
        OptionsUI,
        GameOverUI,
    }
    /// <summary>
    /// package names
    /// </summary>
    public enum FPackage
    {
        PackageVillage,
    }
    /// <summary>
    /// dic : key=window name, value=package name
    /// </summary>
    public static Dictionary<FWindow, FPackage> windowUIpair = new Dictionary<FWindow, FPackage>()
    {
        {FWindow.TestUI, FPackage.PackageVillage},
        //village ui
        {FWindow.InventoryUI, FPackage.PackageVillage},
        {FWindow.StoreUI, FPackage.PackageVillage},
        {FWindow.QuestUI, FPackage.PackageVillage},
        {FWindow.AchievementUI, FPackage.PackageVillage},
        {FWindow.BuildingInfo, FPackage.PackageVillage},
        {FWindow.BuildingUI, FPackage.PackageVillage},
        {FWindow.VillageHome, FPackage.PackageVillage},
        {FWindow.VillageMenuUI, FPackage.PackageVillage},
        {FWindow.InstructionUI, FPackage.PackageVillage},
        {FWindow.OptionsUI, FPackage.PackageVillage},
        {FWindow.GameOverUI, FPackage.PackageVillage},
    };
}
