using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SunHeTBS
{
    public class MapBuildingMgr : MonoBehaviour
    {
        public static MapBuildingMgr Inst;
        public List<MapBuilding> buildingList;
        public GameObject VillagerPrefab;
        public GameObject BuildingPrefab;
        private void Awake()
        {
            Inst = this;
        }
        private void OnDestroy()
        {
            Inst = null;
        }
        // Start is called before the first frame update
        void Start()
        {
            buildingList = new List<MapBuilding>();
            var list = this.gameObject.GetComponentsInChildren<MapBuilding>();
            buildingList.AddRange(list);
        }

        // Update is called once per frame
        void Update()
        {

        }
       
      
    }

}