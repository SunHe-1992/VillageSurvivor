using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SunHeTBS
{
    public class MapBuilding : MonoBehaviour
    {
        public BuildingEffectType effect = BuildingEffectType.Default;

        public string buildingName = "House";
        TMP_Text txt_name;
        UIProgressBar pBar;
        // Start is called before the first frame update
        void Start()
        {
            if (this.transform.Find("progress") != null)
                pBar = this.transform.Find("progress").GetComponent<UIProgressBar>();
            if (this.transform.Find("Canvas/txt_building") != null)
            {
                txt_name = this.transform.Find("Canvas/txt_building").GetComponent<TMP_Text>();
                txt_name.text = buildingName;
            }
            SetProgress(0f);
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

    }
}