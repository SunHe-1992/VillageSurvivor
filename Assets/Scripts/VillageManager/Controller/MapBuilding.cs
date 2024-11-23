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
                BattleDriver.Inst.buildings.Add(logicBuilding);
                this.buildingName = logicBuilding.BdCfg.Name;
                if (txt_name != null)
                    txt_name.text = buildingName;
            }
        }
        #endregion
        public BuildingEffectType effect = BuildingEffectType.Default;

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
            Debug.Log($"Clicked on Building!" + this.name);


        }
        #endregion
    }
}