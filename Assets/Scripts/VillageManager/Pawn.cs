using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SunHeTBS
{
    public class Pawn : GameEntity
    {
        #region worker attributes

        public NPCController Controller;

        //this pawn is working in this place
        public Building workingPlace;
        public string nickName = "";
        Gender gender = Gender.Male;
        int age = 18;
        public Order workingOrder = null;
        public Building workingBuilding = null;

        /// <summary>
        /// add work load for every frame
        /// </summary>
        public float workSpeed = 33f;

        public float GetWorkContributionPerSecond()
        {
            return this.workSpeed;
        }

        #region generate personal info : name, gender

        private string GetRandomName()
        {
            return CharacterGenerator.Inst.GenerateFullName();
        }

        Gender GetRandomGender()
        {
            // Get a random value between 0 and 1 (inclusive)
            int randomValue = Random.Range(0, 2);

            // Cast the random integer to Gender enum
            return (Gender)randomValue;
        }

        void GenerateNickName()
        {
            this.nickName = GetRandomName();
            this.gender = GetRandomGender();
            this.age = Random.Range(18, 65);
        }

        public string GetCharacterInfo()
        {
            return $"Nickname: {nickName}\nGender: {gender}\nAge: {age}";
        }

        public string GetStateInfo()
        {
            return sm.currentState.name;
        }

        #endregion

        #endregion

        public override string ToString()
        {
            return $"pawn:{nickName}";
        }

        public Pawn()
        {
            GenerateNickName();
            InitStateMachine();

            pathArray = new List<Node>();

            hPModifierList = new List<HPModifier>();
            hPModifierList.Add(new HPModifier(HPModifier.HPModifierType.BodyEnergy, 0.03f, 12f, 0.66f, 5f));
            hPModifierList.Add(new HPModifier(HPModifier.HPModifierType.Stamina, 0.05f, 10f, 1.0f, 1f));
        }

        public string hud = "";

        public override void Update(float dt)
        {
            if (this.isDead == false)
            {
                sm.Update(dt);
                UpdateCharacter(dt);

                hud = this.nickName;
                //hud += $"{sm.currentState.name}, ";
                //hud += $"target pos {this.targetPos}, ";
                ////hud += $"this pos {this.position}\n";
                //hud += $"distance to target {Vector3.Distance(this.position, targetPos)}, ";
                //hud += $"<color={GetStaminaColor()}>Stamina: {(100f * staminaValue / staminaValueMax).ToString("f1")}</color> %\n";
                //hud += $"<color={GetHungaryColor()}>Body Energy: {(100f * (bodyEnergyValue) / bodyEnergyMax).ToString("f1")}</color> %\n";
                //hud += $"<color={GetFoodColor()}>food left {foodStorage.ToString("f1")} </color> \n";
            }
            else //dead pawn
            {
            }
        }

        string color_red = "#ff0000";
        string color_green = "#00cc00";

        string GetStaminaColor()
        {
            string color = color_red;
            if (curState() == PawnState.Sleep)
                color = color_green;
            return color;
        }

        string GetFoodColor()
        {
            string color = color_red;
            if (curState() == PawnState.Work)
                color = color_green;
            return color;
        }

        string GetHungaryColor()
        {
            string color = color_red;
            if (curState() == PawnState.Eat)
                color = color_green;
            return color;
        }

        public float bodyEnergyMax = 100f;

        /// <summary>
        /// eat to increase.
        /// </summary>
        public float bodyEnergyValue = 50f;

        public float staminaValueMax = 100f;

        /// <summary>
        /// sleep to increase.
        /// </summary>
        public float staminaValue = 60f;


        float hungerConsumeSpeed = 2.5f;
        float hungerRecoverSpeed = 33f;
        float staminaConsumeSpeed = 3.0f;
        float staminaRecoverSpeed = 40.0f;

        bool IsHungary()
        {
            return this.bodyEnergyValue < bodyEnergyMax * 0.2f;
        }

        bool IsFatigue()
        {
            return this.staminaValue < staminaValueMax * 0.25f;
        }

        bool HaveFood()
        {
            return BattleDriver.Inst.foodStorage > 0f;
        }

        bool IsSleepEnough()
        {
            return this.staminaValue > staminaValueMax * 0.95f;
        }

        bool IsEatEnough()
        {
            return this.bodyEnergyValue > bodyEnergyMax * 0.95f;
        }

        #region HP management

        public float HP = 1000f;
        public float HPMax = 1000f;
        float energyDangerPct = 0.05f;
        float HPDeceraseSpeed_energy = 10f;
        List<HPModifier> hPModifierList;

        public void GetDamage(float dmgValue)
        {
            CostHP(dmgValue);
        }

        public void CostHP(float dmgValue)
        {
            this.HP -= dmgValue;
            this.HP = Mathf.Max(0, HP);
            this.HP = Mathf.Min(HPMax, HP);
            CheckDeath();
        }

        public void GetHeal(float dmgValue)
        {
            RecoverHP(dmgValue);
        }

        public void RecoverHP(float dmgValue)
        {
            this.HP += dmgValue;
            this.HP = Mathf.Max(0, HP);
            this.HP = Mathf.Min(HPMax, HP);
            CheckDeath();
        }

        void CheckDeath()
        {
            if (this.HP <= 0)
            {
                DoDeath();
            }
        }

        public bool isDead = false;

        void DoDeath()
        {
            this.isDead = true;
            Debug.Log(this.ToString() + $" died");
            //todo 
        }

        #endregion

        void UpdateCharacter(float dt)
        {
            if (bodyEnergyValue > 0)
            {
                float coefficient = 1.0f;
                if (curState() == PawnState.Sleep) //if sleeping, energy cost become 66% less
                    coefficient = 0.33f;
                bodyEnergyValue -= coefficient * hungerConsumeSpeed * dt;
            }

            if (staminaValue > 0)
            {
                staminaValue -= staminaConsumeSpeed * dt;
            }

            if (IsFatigue() && HavePlaceToRest()) //need to sleep
            {
                GoToRest();
                return;
            }

            if (sm.currentState == s_sleep)
            {
                //if character is sleeping, he can't feel hungary
            }
            else
            {
                if (HaveFood() && IsHungary() && HavePlaceToEat()) //need to eat
                {
                    GoToEat();
                    return;
                }
            }

            foreach (HPModifier hpm in hPModifierList)
            {
                hpm.Update(this, dt);
            }
        }

        #region state machine

        public StateMachine sm;

        public void ChangeState(StateMachine.State st)
        {
            sm.TransitionTo(st);
        }

        /// <summary>
        /// do nothing
        /// </summary>
        StateMachine.State s_Idle;

        /// <summary>
        /// sleep
        /// </summary>
        StateMachine.State s_sleep;

        /// <summary>
        /// eat food
        /// </summary>
        StateMachine.State s_eat;

        /// <summary>
        /// working
        /// </summary>
        StateMachine.State s_work;

        /// <summary>
        /// patrol
        /// </summary>
        StateMachine.State s_patrol;

        /// <summary>
        /// moving
        /// </summary>
        StateMachine.State s_move;

        /// <summary>
        /// when reached destination,change to this state
        /// </summary>
        StateMachine.State pendingState = null;

        Dictionary<PawnState, StateMachine.State> s_states;

        public void InitStateMachine()
        {
            #region state machine

            s_states = new Dictionary<PawnState, StateMachine.State>();
            sm = new StateMachine();

            s_Idle = sm.CreateState("Idle");
            s_Idle.OnEnter = delegate
            {
                if (this.position == Vector3.zero) //in the initial pos, go to somewhere else
                {
                    GoToRandomPlace();
                    return;
                }

                //when Enter idel:
                int ranInt = Random.Range(0, 100);
                if (ranInt < 50)
                {
                    GoToRandomPlace();
                }
                else
                {
                    RandomIdleWaitTime(); //wait for seconds
                }
            };
            s_Idle.OnExit = delegate { };
            s_Idle.OnFrame = HandleIdle;

            s_sleep = sm.CreateState("Sleep");
            s_sleep.OnEnter = delegate { };
            s_sleep.OnExit = delegate { };
            s_sleep.OnFrame = HandleSleep;
            s_sleep.targetBuildingType = cfg.SLG.BuildingEffect.Rest;


            s_eat = sm.CreateState("Eat");
            s_eat.OnEnter = delegate { };
            s_eat.OnExit = delegate { };
            s_eat.OnFrame = HandleEat;
            s_eat.targetBuildingType = cfg.SLG.BuildingEffect.Canteen;


            s_work = sm.CreateState("Work");
            s_work.OnEnter = delegate { };
            s_work.OnExit = delegate { };
            s_work.OnFrame = HandleWork;


            s_patrol = sm.CreateState("Patrol");
            s_patrol.OnEnter = delegate { };
            s_patrol.OnExit = delegate { };
            s_patrol.OnFrame = HandlePatrol;

            s_move = sm.CreateState("Move");
            s_move.OnEnter = delegate { };
            s_move.OnExit = delegate { };
            s_move.OnFrame = HandleMoving;


            s_states[PawnState.Idle] = s_Idle;
            s_states[PawnState.Sleep] = s_sleep;
            s_states[PawnState.Work] = s_work;
            s_states[PawnState.Patrol] = s_patrol;
            s_states[PawnState.Eat] = s_eat;

            #endregion

            probabilitiesList = new List<int>()
            {
                500, 15, 25, 13,
            };
            dicStates = new Dictionary<int, StateMachine.State>()
            {
                { 0, s_patrol },
                { 1, s_eat },
                { 2, s_work },
                { 3, s_sleep },
            };

            ChangeState(s_Idle);
        }

        #endregion

        void GoToRest()
        {
            if (sm.currentState == s_sleep)
                return;

            var bd = BattleDriver.Inst.FindBuildingWithEffect(s_sleep.targetBuildingType);
            if (bd != null)
            {
                var pos = bd.controller.GetInteractionPos();
                SetDestnation(pos);
                pendingState = s_sleep;
                ChangeState(s_move);
            }
        }

        void GoToEat()
        {
            if (sm.currentState == s_eat)
                return;
            var bd = BattleDriver.Inst.FindBuildingWithEffect(s_eat.targetBuildingType);
            if (bd != null)
            {
                var pos = bd.controller.GetInteractionPos();
                SetDestnation(pos);
                pendingState = s_eat;
                ChangeState(s_move);
            }
        }

        void GoToWork()
        {
            if (sm.currentState == s_work)
                return;
            if (this.workingBuilding != null)
            {
                var pos = workingBuilding.controller.GetInteractionPos();
                SetDestnation(pos);
                pendingState = s_work;
                ChangeState(s_move);
            }
        }

        void GoToRandomPlace()
        {
            var pos = GetRandomWaypointPos();
            SetDestnation(pos);
            pendingState = s_Idle;
            ChangeState(s_move);
        }

        #region random switch stats

        Dictionary<int, StateMachine.State> dicStates;
        List<int> probabilitiesList;

        public static int GetWeightedRandomIndex(List<int> probabilities)
        {
            // Calculate the cumulative sum of probabilities
            int cumulativeSum = 0;
            for (int i = 0; i < probabilities.Count; i++)
            {
                cumulativeSum += probabilities[i];
                probabilities[i] = cumulativeSum;
            }

            // Generate a random number between 0 and the total sum
            int randomNumber = UnityEngine.Random.Range(0, cumulativeSum);

            // Find the index where the random number falls within the cumulative sum
            int index = probabilities.BinarySearch(randomNumber);
            if (index < 0)
            {
                index = ~index;
            }

            return index;
        }

        #endregion

        #region each state

        #region state: sleep

        private void HandleSleep()
        {
            this.staminaValue += this.staminaRecoverSpeed * sm.deltaTime;
            if (IsSleepEnough())
            {
                ChangeState(s_Idle);
            }
        }

        #endregion

        #region state: Produce

        float foodProduceSpeed = 9.0f;

        private void HandleWork()
        {
            if (!HaveWorkTodo())
            {
                ChangeState(s_Idle);
            }
            else
            {
                BattleDriver.Inst.foodStorage += foodProduceSpeed * sm.deltaTime;
            }
        }

        bool HaveWorkTodo()
        {
            return (this.workingBuilding != null && this.workingOrder != null);
        }

        float DistanceToWorkingPosition()
        {
            Vector3 pos1 = this.workingBuilding.controller.GetInteractionPos();
            return Vector3.Distance(pos1, this.position);
        }

        bool IsNearWorkingPosition()
        {
            return DistanceToWorkingPosition() < 0.1f;
        }

        #endregion

        #region state: Eat food

        float foodConsumeSpeed = 11.0f;

        private void HandleEat()
        {
            if (HaveFood())
            {
                BattleDriver.Inst.foodStorage -= foodConsumeSpeed * sm.deltaTime;
                this.bodyEnergyValue += this.hungerRecoverSpeed * sm.deltaTime;
            }
            else
            {
                ChangeState(s_Idle);
                return;
            }

            if (IsEatEnough())
            {
                ChangeState(s_Idle);
                return;
            }
        }

        bool HavePlaceToEat()
        {
            return null != BattleDriver.Inst.FindBuildingWithEffect(cfg.SLG.BuildingEffect.Canteen);
        }

        bool HavePlaceToRest()
        {
            return null != BattleDriver.Inst.FindBuildingWithEffect(cfg.SLG.BuildingEffect.Rest);
        }

        #endregion

        #region state: Idle

        float idleWaitTime;

        void RandomIdleWaitTime()
        {
            idleWaitTime = UnityEngine.Random.Range(1.5f, 4.5f);
        }

        private void HandleIdle()
        {
            if (s_Idle.elpsedTime > idleWaitTime)
            {
                if (HaveWorkTodo())
                {
                    GoToWork();
                }
                else
                {
                    SetDestnation(GetRandomWaypointPos());
                }
            }
            else
            {
                if (IsNearTargetPos()) //random walk to a waypoint
                {
                    SetDestnation(GetRandomWaypointPos());
                }
            }

            BattleDriver.Inst.foodStorage += foodProduceSpeed * sm.deltaTime;
        }

        /// <summary>
        /// current node pos
        /// </summary>
        Vector3 targetPos;

        /// <summary>
        /// the end node pos
        /// </summary>
        Vector3 destPos;

        /// <summary>
        /// self current position
        /// </summary>
        public Vector3 position;

        public float move_speed = 10.0f;

        private void HandleMoving()
        {
            if (IsNearTargetPos() == false) //go to target pos
            {
                Vector3 moveNorm = (targetPos - position).normalized;
                Vector3 moveDelta = moveNorm * move_speed * sm.deltaTime;
                float dist1 = Vector3.Distance(position, targetPos);
                if (moveDelta.magnitude > dist1)
                {
                    position = targetPos;
                }
                else
                {
                    position += moveDelta;
                }
            }
            else //find next node pos
            {
                var (newPos, haveNext) = GetNextNodePos();
                if (haveNext)
                {
                    targetPos = newPos;
                }
                else
                {
                    ChangeState(pendingState == null ? s_Idle : pendingState);
                }
            }

            BattleDriver.Inst.foodStorage += foodProduceSpeed * sm.deltaTime;
        }

        private void HandlePatrol()
        {
            if (IsNearTargetPos())
            {
                SetDestnation(GetNextWaypointPos());
            }
            else
            {
            }

            if (s_patrol.elpsedTime > 5) //after 5 seconds in this state, switch to idle
            {
                ChangeState(s_Idle);
            }
        }

        #endregion

        #endregion

        #region waypoints

        public List<Vector3> waypointList = new List<Vector3>();

        bool IsNearTargetPos()
        {
            return Vector3.Distance(position, targetPos) < 0.2f;
        }

        public Vector3 GetRandomPosition(Vector3 position, float distance)
        {
            // Generate a random angle in radians
            float angle = UnityEngine.Random.Range(0f, 2 * Mathf.PI);

            // Calculate the x and z components of the random position
            float x = position.x + distance * Mathf.Cos(angle);
            float z = position.z + distance * Mathf.Sin(angle);

            // Return the random position with Y set to 0
            var originalVector = new Vector3(x, 0f, z);
            float constrainedX = Mathf.Clamp(originalVector.x, -50, 50);
            float constrainedZ = Mathf.Clamp(originalVector.z, -50, 50);

            // Create the new constrained vector
            Vector3 constrainedVector = new Vector3(constrainedX, originalVector.y, constrainedZ);
            return constrainedVector;
        }


        int wpIndex = 0;

        Vector3 GetNextWaypointPos()
        {
            // Increment the index, wrapping around if necessary
            wpIndex = (wpIndex + 1) % waypointList.Count;

            // Return the position of the next waypoint
            return waypointList[wpIndex];
        }

        Vector3 GetRandomWaypointPos()
        {
            // Ensure the list is not empty to avoid errors
            if (waypointList == null || waypointList.Count == 0)
            {
                Debug.LogWarning("Waypoint list is empty or null!");
                return Vector3.zero; // Return a default value if the list is empty
            }

            // Generate a random index within the range of the waypoint list
            int randomIndex = Random.Range(0, waypointList.Count);

            // Return the position of the randomly selected waypoint
            return waypointList[randomIndex];
        }

        public void SetDestnation(Vector3 pos)
        {
            destPos = pos;
            FindPath();
        }

        #endregion

        public PawnState curState()
        {
            return sm.currentState.state;
        }

        public bool IsWorking()
        {
            return this.sm.currentState == s_work;
        }


        #region Path finding

        public Node startNode { get; set; }
        public Node goalNode { get; set; }
        public List<Node> pathArray;
        int pathArrayIndex = 0;

        void FindPath()
        {
            Vector3 startPos = this.position;
            Vector3 endPos = this.destPos;
            //Assign StartNode and Goal Node
            var (startColumn, startRow) = GridManager.instance.GetGridCoordinates(startPos);
            var (goalColumn, goalRow) = GridManager.instance.GetGridCoordinates(endPos);
            startNode = new Node(GridManager.instance.GetGridCellCenter(startColumn, startRow));
            goalNode = new Node(GridManager.instance.GetGridCellCenter(goalColumn, goalRow));
            pathArray = new AStar().FindPath(startNode, goalNode);
            pathArrayIndex = 0;
        }

        Vector3 GetCurrentNodePos()
        {
            if (pathArrayIndex < pathArray.Count)
            {
                var node = pathArray[pathArrayIndex];
                return node.position;
            }
            else
                return position;
        }

        (Vector3, bool) GetNextNodePos()
        {
            bool haveNext = true;
            pathArrayIndex++;
            if (pathArrayIndex < pathArray.Count)
            {
                var node = pathArray[pathArrayIndex];
                return (node.position, haveNext);
            }
            else
            {
                haveNext = false;
                return (Vector3.zero, haveNext);
            }
        }

        void OnDrawGizmos()
        {
            if (pathArray == null)
                return;
            if (pathArray.Count > 0)
            {
                int index = 1;
                foreach (Node node in pathArray)
                {
                    if (index < pathArray.Count)
                    {
                        Node nextNode = pathArray[index];
                        Debug.DrawLine(node.position, nextNode.position, Color.green);
                        index++;
                    }
                }

                ;
            }
        }

        #endregion
    }

    internal class HPModifier
    {
        public enum HPModifierType
        {
            BodyEnergy = 0,
            Stamina = 1,
        }

        HPModifierType name;

        float dangerPct = 0.05f;
        float HPDeceraseSpeed = 10f;
        float recoverPct = 0.66f;
        float HPRecoverSpeed = 3.3f;

        public HPModifier(HPModifierType name, float dangerPct, float hPDeceraseSpeed, float recoverPct,
            float hPRecoverSpeed)
        {
            this.name = name;
            this.dangerPct = dangerPct;
            HPDeceraseSpeed = hPDeceraseSpeed;
            this.recoverPct = recoverPct;
            HPRecoverSpeed = hPRecoverSpeed;
        }

        public void Update(Pawn p, float deltaTime)
        {
            float curValue = 0;
            float maxValue = 0;
            if (this.name == HPModifierType.BodyEnergy)
            {
                curValue = p.bodyEnergyValue;
                maxValue = p.bodyEnergyMax;
            }
            else if (this.name == HPModifierType.Stamina)
            {
                curValue = p.staminaValue;
                maxValue = p.staminaValueMax;
            }

            if (curValue < maxValue * this.dangerPct)
            {
                float changeValue = deltaTime * this.HPDeceraseSpeed;
                p.GetDamage(changeValue);
            }

            if (curValue > maxValue * this.recoverPct)
            {
                float changeValue = deltaTime * this.HPRecoverSpeed;
                p.GetHeal(changeValue);
            }
        }
    }
}