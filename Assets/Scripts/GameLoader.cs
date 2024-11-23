using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;
using UnityEngine.UIElements;
using UniFramework.Singleton;
using UniFramework.Animation;
using UniFramework.Tween;

using UniFramework.Event;
using UniFramework.Module;
using SunHeTBS;
using UnityEngine.SceneManagement;
public class GameLoader : MonoBehaviour
{
    public static GameLoader Instance = null;
    public static bool started = false;
    private void Awake()
    {
        Instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (started == false)
        {
            started = true;
            // 初始化事件系统
            UniEvent.Initalize();

            // 初始化管理系统
            UniModule.Initialize();


            this.InitEnv();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void InitEnv()
    {
        UniSingleton.Initialize();
        UniTween.Initalize();
        BattleDriver.Init();
        //init Uni singletons
        //framework
        FUIManager.Init();
        ConfigManager.Init();
        BindFGUI.BindAll();//fairy code bind 
        UIService.Init();
        LoadData();//load json configs

        FUIManager.ReSetBundle();//FUIManager initialize
        LoadFontRes();
        TBSPlayer.SetUserDetail();

        CharacterGenerator.Inst = new CharacterGenerator();
    }


    //load config data
    private void LoadData()
    {

        ConfigManager.LoadJsonInfos(() =>
        {

        });
    }


    UnityEngine.Font loadFontObj;
    private void LoadFontRes()
    {

        loadFontObj = null;
        #region 字体加载


        LoadFontDone();

        #endregion
    }
    int needDoneAllNum;
    void LoadFontDone()
    {
        //preload ui packages
        List<FUIDef.FPackage> packageList = new List<FUIDef.FPackage>()
        {
            FUIDef.FPackage.PackageVillage,
        };
        needDoneAllNum = packageList.Count;

        foreach (var package in packageList)
        {
            FUIManager.Inst.IncPackageReference(package.ToString());
            FUIManager.Inst.PreAddPackage(package.ToString(), loadCommonDone);
        }

    }

    void loadCommonDone()
    {
        needDoneAllNum = needDoneAllNum - 1;

        if (needDoneAllNum == 0)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            TBSPlayer.fromGD = true;
            FUIManager.Inst.ShowUI<UIPage_VillageMenu>(FUIDef.FWindow.VillageMenuUI);
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Village")
        {
            SunHeTBS.BattleDriver.Inst.running = true;
            FUIManager.Inst.ShowUI<UIPage_VillageHome>(FUIDef.FWindow.VillageHome);
        }
    }
}
