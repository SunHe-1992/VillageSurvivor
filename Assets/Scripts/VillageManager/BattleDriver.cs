using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

using System;

using UnityEngine.SceneManagement;

using UniFramework.Singleton;
using System.Linq;

namespace SunHeTBS
{


    public class BattleDriver : ISingleton
    {

        public bool running = false;
        public static BattleDriver Inst { get; private set; }
        public static void Init()
        {
            Inst = UniSingleton.CreateSingleton<BattleDriver>();
            Inst.InitMedievalData();


        }
        public void OnCreate(object createParam)
        {
            isWindowEditor = Application.isEditor;
        }

        public void OnUpdate()
        {
            DoUpdate();
        }

        public void OnDestroy()
        {
        }

        float deltaTime = 0;
        // Update is called once per frame
        void DoUpdate()
        {
            if (isWindowEditor)
            {
                CheckGMKey();
            }
            deltaTime = Time.deltaTime;
            OnBattleFrameUpdate(deltaTime);
            UpdateDecidingBuildingLocation();
        }

        private void OnBattleFrameUpdate(float deltaTime)
        {
            if (running)
            {
                DriveEntities(deltaTime);
                CheckGameOver();
            }
        }

        private void CheckGMKey()
        {
            //GM key open developer UI
            if (Input.GetKeyUp(KeyCode.F1))
            {
                FUIManager.Inst.ShowUI<UIPage_Debug>(FUIDef.FWindow.TestUI);
            }
        }



        bool isWindowEditor = false;






        #region medieval drive
        public List<Building> buildings;
        public List<Pawn> pawnList;
        public void InitMedievalData()
        {
            buildings = new List<Building>();
            pawnList = new List<Pawn>();
        }
        void DriveEntities(float dt)
        {
            float deltaTime = dt * speedScale;
            if (buildings != null)
            {
                foreach (var bd in buildings)
                {
                    bd.Update(deltaTime);
                }
            }
            if (pawnList != null)
            {
                foreach (var p in pawnList)
                {
                    p.Update(deltaTime);
                }
            }
        }
        public void InsertPawn(Pawn p)
        {
            this.pawnList.Add(p);
        }
        public Building GetBuildingBySid(int sid)
        {
            return buildings.Find(p => p.sid == sid);
        }

        public void AddOneWorker(Building bd)
        {
            var p = GetOneIdlePawn();
            if (p != null)
            {
                bd.AssignWorker(p);
            }
        }
        Pawn GetOneIdlePawn()
        {
            Pawn p = null;
            foreach (var pawn in this.pawnList)
            {
                if (pawn.isDead == false && pawn.workingPlace == null)
                {
                    p = pawn;
                    break;
                }
            }
            return p;
        }
        public void RemoveOneWorker(Building bd)
        {
            if (bd.workerList.Count > 0)
            {
                bd.RemoveWorker(bd.workerList[bd.workerList.Count - 1]);
            }
        }

        internal void AddVillager()
        {
            var villager = GameObject.Instantiate(MapBuildingMgr.Inst.VillagerPrefab);
        }

        public Pawn clickedPawn;

        public int GetDeadPawnCount()
        {
            return pawnList.Count(p => p.isDead);
        }
        public int deathCountMax = 1;
        public void CheckGameOver()
        {
            int deathCount = GetDeadPawnCount();
            if (deathCount >= deathCountMax)
            {
                this.running = false;
                //gameover;
                FUIManager.Inst.ShowUI<UIPage_GameOverUI>(FUIDef.FWindow.GameOverUI);
            }
        }
        void ResetBattleField()
        {
            //todo clean data for a new game

        }
        public void CreateMapBuilding(int buildingId, Vector3 position)
        {
            var bdObj = GameObject.Instantiate(MapBuildingMgr.Inst.BuildingPrefab);
            ProcessCreateMapBuilding(buildingId, position, bdObj);
        }

        void ProcessCreateMapBuilding(int buildingId, Vector3 position, GameObject obj)
        {
            var bdObj = obj;
            bdObj.transform.position = position;
            var mbScript = bdObj.GetComponent<MapBuilding>();
            mbScript.buildingCfgId = buildingId;
            if (obj.TryGetComponent<FollowMouse>(out FollowMouse fmComp))
            {
                fmComp.enabled = false; //disable this script
            }
            bdObj.transform.parent = MapBuildingMgr.Inst.transform;
            if (bdObj.TryGetComponent<MapBuilding>(out MapBuilding mbComp))
            {
                mbComp.Init();
            }
        }
        #endregion


        #region building: select location
        GameObject buildingGizmos = null;
        public int pendingBuildingId = 0;
        public bool DecidingBuildingLocation = false;
        public void EnterDecidingBuidlingLocation(int id)
        {
            pendingBuildingId = id;
            DecidingBuildingLocation = true;
            buildingGizmos = GameObject.Instantiate(MapBuildingMgr.Inst.BuildingPrefab);
            buildingGizmos.AddComponent<FollowMouse>();
            var trans = buildingGizmos.transform;
            trans.position = Vector3.zero;
            trans.rotation = Quaternion.identity;
            trans.localScale = Vector3.one;
            buildingGizmos.name = "buildingGizmos";
            var mbScript = buildingGizmos.GetComponent<MapBuilding>();
            if (mbScript != null)
            {
                mbScript.ApplyBuildingModel(id);
            }
        }
        public void EndDecidingBuidlingLocation(bool confirmBuilding)
        {
            if (confirmBuilding)
            {
                if (buildingGizmos != null && CheckBuildingLocationCorrect(buildingGizmos.transform.position))
                {
                    DecidingBuildingLocation = false;
                    ProcessCreateMapBuilding(pendingBuildingId, buildingGizmos.transform.position, buildingGizmos);
                    buildingGizmos = null;
                }
            }
            else
            {
                DecidingBuildingLocation = false;
                GameObject.Destroy(buildingGizmos);
                buildingGizmos = null;
            }
        }
        void UpdateDecidingBuildingLocation()
        {
            if (DecidingBuildingLocation == false)
            {
                EndDecidingBuidlingLocation(false);
                return;
            }

        }
        bool CheckBuildingLocationCorrect(Vector3 pos)
        {
            return GridManager.instance.IsInBounds(pos);
        }

        public Building FindBuildingWithEffect(cfg.SLG.BuildingEffect effect)
        {
            foreach (var bd in this.buildings)
            {
                if (bd.BdCfg.Effect == effect)
                {
                    return bd;
                }
            }
            return null;
        }
        #endregion

        #region speed control
        public float speedScale = 1f;


        #endregion
        /// <summary>
        /// eat to decrease. produce to increase
        /// </summary>
        public float foodStorage = 100f;

        public void RestartGame()
        {
            foodStorage = 100f;
            pawnList.Clear();
            buildings.Clear();
        }
    }
}
