using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SunHeTBS
{
    public class Pawn : GameEntity
    {
        #region worker attributes

        //this pawn is working in this place
        public Building workingPlace;
        public string nickName = "";
        public Order workingOrder = null;
        /// <summary>
        /// add work load for every frame
        /// </summary>
        public float workSpeed = 33f;
        public float GetWorkContributionPerSecond()
        {
            return this.workSpeed;
        }
        #region nick name
        private string[] names = new string[]
{
        "Alice", "Bob", "Charlie", "David", "Eve", "Fay", "George", "Hannah", "Ivy", "Jack",
        "Karen", "Leo", "Mona", "Nina", "Oscar", "Paul", "Quincy", "Rachel", "Steve", "Tina"
};
        private string GetRandomName()
        {
            int randomIndex = UnityEngine.Random.Range(0, names.Length); // Generates a random index
            return names[randomIndex];
        }
        void GenerateNickName()
        {
            this.nickName = GetRandomName();
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
        }
        public string hud = "";
        public override void Update(float dt)
        {
            sm.Update();
            UpdateCharacter();
            HandleMoving();

            hud = "";
            hud += $"target pos {this.TargetPos}\n";
            hud += $"this pos {this.position}\n";
            hud += $"distance to target {Vector3.Distance(this.position, TargetPos)}\n";
            hud += $"[color={GetStaminaColor()}]Stamina: {(100f * staminaValue / staminaValueMax).ToString("f1")}[/color] %\n";
            hud += $"[color={GetHungaryColor()}]Body Energy: {(100f * (bodyEnergyValue) / bodyEnergyMax).ToString("f1")}[/color] %\n";
            hud += $"[color={GetFoodColor()}]food left {foodStorage.ToString("f1")} [/color] \n";

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

        readonly float bodyEnergyMax = 100f;
        /// <summary>
        /// eat to increase.
        /// </summary>
        public float bodyEnergyValue = 50f;

        readonly float staminaValueMax = 100f;
        /// <summary>
        /// sleep to increase.
        /// </summary>
        public float staminaValue = 60f;

        /// <summary>
        /// eat to decrease. produce to increase
        /// </summary>
        float foodStorage = 100f;

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
            return this.foodStorage > 0f;
        }

        bool IsSleepEnough()
        {
            return this.staminaValue > staminaValueMax * 0.95f;
        }
        bool IsEatEnough()
        {
            return this.bodyEnergyValue > bodyEnergyMax * 0.95f;
        }
        void UpdateCharacter()
        {
            if (bodyEnergyValue > 0)
            {
                float coefficient = 1.0f;
                if (curState() == PawnState.Sleep) //if sleeping, energy cost become 66% less
                    coefficient = 0.33f;
                bodyEnergyValue -= coefficient * hungerConsumeSpeed * Time.deltaTime;
            }
            if (staminaValue > 0)
            {
                staminaValue -= staminaConsumeSpeed * Time.deltaTime;
            }

            if (IsFatigue()) //need to sleep
            {
                ChangeState(s_sleep);
                return;
            }

            if (sm.currentState == s_sleep)
            {
                //if character is sleeping, he can't feel hungary
            }
            else
            {
                if (HaveFood() && IsHungary())//need to eat
                {
                    ChangeState(s_eat);
                    return;
                }
                if (HaveFood() == false) //need to produce food
                {
                    ChangeState(s_work);
                    return;
                }
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
        Dictionary<PawnState, StateMachine.State> s_states;
        public void InitStateMachine()
        {
            #region state machine
            s_states = new Dictionary<PawnState, StateMachine.State>();
            sm = new StateMachine();

            s_Idle = sm.CreateState("Idle");
            s_Idle.OnEnter = delegate
            {
                RandomIdleWaitTime();
            };
            s_Idle.OnExit = delegate
            {
            };
            s_Idle.OnFrame = HandleIdle;

            s_sleep = sm.CreateState("Sleep");
            s_sleep.OnEnter = delegate
            {
                targetPos = Vector3.zero;
            };
            s_sleep.OnExit = delegate
            {
            };
            s_sleep.OnFrame = HandleSleep;
            s_sleep.targetBuildingType = BuildingEffectType.Sleep;


            s_eat = sm.CreateState("Eat");
            s_eat.OnEnter = delegate
            {
                targetPos = Vector3.zero;
            };
            s_eat.OnExit = delegate
            {
            };
            s_eat.OnFrame = HandleEat;
            s_eat.targetBuildingType = BuildingEffectType.Eat;


            s_work = sm.CreateState("Produce");
            s_work.OnEnter = delegate
            {
                targetPos = Vector3.zero;
            };
            s_work.OnExit = delegate
            {
            };
            s_work.OnFrame = HandleProduce;
            s_work.targetBuildingType = BuildingEffectType.Work;


            s_patrol = sm.CreateState("Patrol");
            s_patrol.OnEnter = delegate
            {
            };
            s_patrol.OnExit = delegate
            {
            };
            s_patrol.OnFrame = HandlePatrol;

            s_states[PawnState.Idle] = s_Idle;
            s_states[PawnState.Sleep] = s_sleep;
            s_states[PawnState.Work] = s_work;
            s_states[PawnState.Patrol] = s_patrol;
            s_states[PawnState.Eat] = s_eat;


            #endregion
            probabilitiesList = new List<int>()
            {
                500,15,25,13,
            };
            dicStates = new Dictionary<int, StateMachine.State>() {
                {0,s_patrol},
                {1,s_eat},
                {2,s_work},
                {3,s_sleep},
            };

            ChangeState(s_Idle);
        }
        #endregion

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

        #region  each state


        #region state: sleep
        private void HandleSleep()
        {
            if (targetPos == Vector3.zero)
                targetPos = MapBuildingMgr.Inst.GetBuildingPosition(s_sleep.targetBuildingType);
            this.staminaValue += this.staminaRecoverSpeed * Time.deltaTime;
            if (IsSleepEnough())
            {
                ChangeState(s_Idle);
            }
        }
        #endregion

        #region state: Produce
        float foodProduceSpeed = 9.0f;
        private void HandleProduce()
        {
            if (targetPos == Vector3.zero)
                targetPos = MapBuildingMgr.Inst.GetBuildingPosition(s_work.targetBuildingType);
            foodStorage += foodProduceSpeed * Time.deltaTime;
        }
        #endregion

        #region state: Eat food
        float foodConsumeSpeed = 11.0f;
        private void HandleEat()
        {
            if (targetPos == Vector3.zero)
                targetPos = MapBuildingMgr.Inst.GetBuildingPosition(s_eat.targetBuildingType);
            if (HaveFood())
            {
                foodStorage -= foodConsumeSpeed * Time.deltaTime;
                this.bodyEnergyValue += this.hungerRecoverSpeed * Time.deltaTime;
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
                //int stateIdx = GetWeightedRandomIndex(this.probabilitiesList);
                //if (dicStates.ContainsKey(stateIdx))
                //{
                //    string sName = dicStates[stateIdx].name;
                //    print($"randomly changed to {sName}");

                //    ChangeState(dicStates[stateIdx]);
                //}
                //else
                //{
                //    ChangeState(dicStates[0]);
                //}
                ChangeState(s_work);
                return;
            }
        }

        Vector3 targetPos;

        public Vector3 TargetPos { get => targetPos; set => targetPos = value; }
        public Vector3 position;
        public float move_speed = 10.0f;
        private void HandleMoving()
        {
            if (IsNearTargetPos() == false)
            {
                Vector3 moveNorm = (TargetPos - position).normalized;
                Vector3 moveDelta = moveNorm * move_speed * Time.deltaTime;
                position += moveDelta;
            }
        }

        private void HandlePatrol()
        {
            if (IsNearTargetPos())
            {
                targetPos = GetNextWaypointPos();
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
            return Vector3.Distance(position, TargetPos) < 0.2f;
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

        public Vector3 GetNextWaypointPos()
        {
            // Increment the index, wrapping around if necessary
            wpIndex = (wpIndex + 1) % waypointList.Count;

            // Return the position of the next waypoint
            return waypointList[wpIndex];
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
        void FindPath()
        {
            Vector3 startPos = this.position;
            Vector3 endPos = this.targetPos;
            //Assign StartNode and Goal Node
            var (startColumn, startRow) = GridManager.instance.GetGridCoordinates(startPos);
            var (goalColumn, goalRow) = GridManager.instance.GetGridCoordinates(endPos);
            startNode = new Node(GridManager.instance.GetGridCellCenter(startColumn, startRow));
            goalNode = new Node(GridManager.instance.GetGridCellCenter(goalColumn, goalRow));
            pathArray = new AStar().FindPath(startNode, goalNode);
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
                };
            }
        }
        #endregion
    }
}
