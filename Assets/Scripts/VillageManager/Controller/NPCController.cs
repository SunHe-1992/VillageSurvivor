using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace SunHeTBS
{
    public class NPCController : MonoBehaviour
    {
        Pawn _pawn;

        StateMachine getSM()
        {
            return _pawn.sm;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (this._pawn == null)
            {
                this._pawn = new Pawn();
                _pawn.Controller = this;
                BattleDriver.Inst.InsertPawn(this._pawn);
                this.gameObject.name = this._pawn.nickName;
            }
            GenerateWayPoints();
            this.txt_status = this.transform.Find("Canvas/txt_status").GetComponent<TMP_Text>();
        }

        // Update is called once per frame
        void Update()
        {
            HandleMoving();

            if (this.txt_status != null && getSM().currentState != null)
            {
                string hudMsg = _pawn.hud;
                this.txt_status.text = hudMsg;
            }

        }



        public TMPro.TMP_Text txt_status;





        #region moving and patrol



        #endregion



        void GenerateWayPoints()
        {
            var waypointList = new List<Vector3>();
            foreach (var go in GameObject.FindGameObjectsWithTag("WayPoint"))
            {
                waypointList.Add(go.transform.position);
            }
            _pawn.waypointList = waypointList;
        }
        private void HandleMoving()
        {
            //this.transform.position = _pawn.position;
            this.transform.position = Vector3.Lerp(this.transform.position, _pawn.position, 0.1f);
        }

        #region on click
        void OnMouseDown()
        {
            if (this._pawn == null) return;
            // This will be called when the NPC is clicked
            Debug.Log($"Clicked on NPC!" + this.name);
            BattleDriver.Inst.clickedPawn = this._pawn;
            UIPage_VillageHome.showPawnHUD = true;
        }
        #endregion
    }
}
