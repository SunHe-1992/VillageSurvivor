using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

using System;

using UnityEngine.SceneManagement;

using UniFramework.Singleton;

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

        }

        private void OnBattleFrameUpdate(float deltaTime)
        {
            if (running)
                DriveEntities(deltaTime);
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
            if (buildings != null)
            {
                foreach (var bd in buildings)
                {
                    bd.Update(dt);
                }
            }
            if (pawnList != null)
            {
                foreach (var p in pawnList)
                {
                    p.Update(dt);
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
                if (pawn.workingPlace == null)
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
        #endregion


    }
}
