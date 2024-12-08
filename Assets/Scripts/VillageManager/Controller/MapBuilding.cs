using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SunHeTBS
{
    public class MapBuilding : MonoBehaviour
    {

        #region logic building
        public int buildingCfgId = 0;
        Building logicBuilding;
        void CreateLogicBuilding()
        {
            var cfg = ConfigManager.table.TbBuilding.GetOrDefault(buildingCfgId);
            if (cfg != null)
            {
                //Debug.Log(cfg.ToString());
                logicBuilding = new Building(buildingCfgId);
                logicBuilding.controller = this;
                BattleDriver.Inst.buildings.Add(logicBuilding);
                this.buildingName = logicBuilding.BdCfg.Name;
                if (txt_name != null)
                    txt_name.text = buildingName;
            }
        }
        #endregion

        public string buildingName = "House";
        TMP_Text txt_name;
        UIProgressBar pBar;

        Transform interactionTrans;
        // Start is called before the first frame update
        void Start()
        {
            Init();
        }
        public void Init()
        {
            if (this.transform.Find("progress") != null)
                pBar = this.transform.Find("progress").GetComponent<UIProgressBar>();
            if (this.transform.Find("Canvas/txt_building") != null)
            {
                txt_name = this.transform.Find("Canvas/txt_building").GetComponent<TMP_Text>();
                txt_name.text = buildingName;
            }
            if (this.transform.Find("interactionTrans") != null)
            {
                this.interactionTrans = this.transform.Find("interactionTrans");
            }
            SetProgress(0f);
            CreateLogicBuilding();
        }
        // Update is called once per frame
        void Update()
        {

        }
        public void SetProgress(float pct)
        {
            if (pBar != null)
                pBar.SetPercent(pct);
        }

        public Vector3 GetInteractionPos()
        {
            if (this.interactionTrans != null)
                return this.interactionTrans.position;
            else
                return this.transform.position;
        }

        #region on click
        void OnMouseDown()
        {
            // This will be called when the NPC is clicked
            if (this.logicBuilding == null) return;
            //Debug.Log($"Clicked on Building!" + this.name);
            if (false == FUIManager.Inst.IsWindowOpening(FUIDef.FWindow.BuildingInfo))
            {
                FUIManager.Inst.ShowUI<UIPage_BuildingInfo>(FUIDef.FWindow.BuildingInfo, null, this.logicBuilding.sid);
            }

        }
        #endregion

        #region models
        [SerializeField]
        [Tooltip("building models")]
        public List<GameObject> modelList;
        GameObject buildingModel;
        public void ApplyBuildingModel(int buildingId)
        {
            if (buildingModel != null)
                GameObject.Destroy(buildingModel);
            var cfg = ConfigManager.table.TbBuilding.GetOrDefault(buildingId);
            if (cfg != null)
            {
                string modelName = cfg.ModelName;
                var newObj = GameObject.Instantiate(FindModel(modelName));
                newObj.transform.parent = this.transform;
                newObj.transform.localPosition = Vector3.zero;
                newObj.transform.localRotation = Quaternion.identity;
                buildingModel = newObj;
                newObj.name = buildingId + "_" + modelName;
            }
        }
        GameObject FindModel(string name)
        {
            foreach (var md in modelList)
            {
                if (md.name == name)
                    return md;
            }
            return modelList[0];
        }
        #endregion

    }
}