using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using cfg;
namespace SunHeTBS
{
    public class Building : GameEntity
    {
        public MapBuilding controller;
        private static int seqId = 0;
        public int sid = 0;
        public string name = "";
        float constructionTime = 10f;
        public bool constructing = false;
        /// <summary>
        /// id in Building config
        /// </summary>
        public int buildingId;

        public List<Order> orderList;
        BuildingData _bdCfg;

        public BuildingData BdCfg { get => _bdCfg; private set => _bdCfg = value; }

        public List<ProductData> productList;
        public List<Pawn> workerList;
        /// <summary>
        /// runtime building class
        /// </summary>
        public Building(int cfgId)
        {
            buildingId = cfgId;
            this.sid = ++seqId;
            this.orderList = new List<Order>();
            CreateConstructionOrder();

            ReadConfigs(cfgId);
        }

        private void ReadConfigs(int cfgId)
        {
            BdCfg = ConfigManager.table.TbBuilding.Get(cfgId);
            //read product list
            productList = new List<ProductData>();
            foreach (var pd in ConfigManager.table.Product.DataList)
            {
                if (pd.BuildingId == buildingId)
                {
                    productList.Add(pd);
                }
            }

            workerList = new List<Pawn>();
        }

        public override void Update(float dt)
        {
            AssignWorkerToOrder();
            foreach (Order order in orderList)
            {
                //only active if someone is working
                if (order.paused == false)
                {
                    order.AccumulateWork(dt);
                }

                if (order.tobeDeleted)
                {
                    order.PrepareDelete();
                }
            }

            orderList.RemoveAll(p => p.tobeDeleted);

        }

        void CreateConstructionOrder()
        {
            var order = new Order(0, this, 1000);
            this.orderList.Add(order);
            order.type = OrderType.Construction;
            order.name = "Construction";
            order.maxWorkerCount = 5; //in construction, allow 5 workers doing this job
            this.constructing = true;
        }
        public int productId = 0;
        public void CreateNewOrder(int _productId)
        {
            productId = _productId;
            var pdCfg = ConfigManager.table.Product.Get(_productId);
            var order = new Order(_productId, this, pdCfg.WorkLoad);
            this.orderList.Add(order);
            order.type = OrderType.Crafting;
            order.name = pdCfg.Description;

        }
        public void FinishProduceOrder()
        {

            var pdCfg = ConfigManager.table.Product.Get(this.productId);
            ConsumeItems(pdCfg.ResourceItemId, pdCfg.ResourceAmount);
            InsertItems(pdCfg.ProductItemId, pdCfg.ProductAmount);


        }
        void ConsumeItems(int itemId, int amount)
        {
            if (itemId != 0 & amount > 0)
            {
                TBSPlayer.RemoveItem(itemId, amount);
            }
        }
        void InsertItems(int itemId, int amount)
        {
            if (itemId != 0 & amount > 0)
            {
                TBSPlayer.InsertItem(itemId, amount);
            }
        }

        /// <summary>
        /// assign a worker into this building
        /// </summary>
        /// <param name="worker"></param>
        public void AssignWorker(Pawn worker)
        {
            this.workerList.Add(worker);
            worker.workingPlace = this;
            worker.workingBuilding = this;
        }
        public void RemoveWorker(Pawn worker)
        {
            //remove from order
            foreach (var order in orderList)
            {
                order.RemoveWorker(worker);
            }
            worker.workingOrder = null;

            //remove from building
            this.workerList.Remove(worker);
            worker.workingPlace = null;
        }
        void AssignWorkerToOrder()
        {
            Order orderNeedsWorker = null;
            foreach (var order in orderList)
            {
                if (order.WorkersFull() == false)
                {
                    orderNeedsWorker = order;
                    break;
                }
            }
            if (orderNeedsWorker != null)
            {
                //if there are workers belong to this building but without any order
                foreach (var worker in this.workerList)
                {
                    if (worker.workingOrder == null)
                    {
                        orderNeedsWorker.AddWorker(worker);
                        if (orderNeedsWorker.WorkersFull())
                        {
                            break;
                        }
                    }
                }
            }
        }


    }

    public enum OrderType
    {
        Default = 0,
        Construction = 1, //build a building
        Crafting = 2, //comsume items and generate items
    }
    public class Order
    {
        public string name;
        public Building bd;
        public OrderType type;
        public int productId;
        // 工作总量，默认1000
        public float TotalWork { get; set; } = 1000;

        // 已完成工作量，初始0
        public float CompletedWork { get; set; } = 0;

        public uint repeatTime = 1;
        public bool tobeDeleted = false;
        public bool paused = false;
        public List<Pawn> workerList = new List<Pawn>();
        public int maxWorkerCount = 1;

        ProductData pdCfg;
        // 构造函数，可以自定义初始化
        public Order(int pdId, Building building, float totalWork = 1000)
        {
            this.productId = pdId;
            this.bd = building;
            TotalWork = totalWork;
            pdCfg = ConfigManager.table.Product.GetOrDefault(this.productId);
            if (pdCfg != null)
            {
                this.maxWorkerCount = pdCfg.MaxWorkers;
            }
        }
        void Reset()
        {
            this.type = OrderType.Default;
            this.TotalWork = 1000;
            this.CompletedWork = 0;
            this.repeatTime = 1;
            this.tobeDeleted = false;
            this.paused = false;
        }
        public void PrepareDelete()
        {
            foreach (var worker in this.workerList)
            {
                worker.workingOrder = null;
            }
            this.workerList.Clear();
        }
        // 计算剩余工作量
        public float RemainingWork
        {
            get { return TotalWork - CompletedWork; }
        }

        // 检查是否完成
        public bool IsCompleted
        {
            get { return CompletedWork >= TotalWork; }
        }
        // 累计工作量
        public void AccumulateWork(float deltaTime)
        {
            float workContribution = 0f;
            // 假设每秒完成的工作量
            foreach (Pawn worker in workerList)
            {
                if (worker.IsWorking())
                {
                    workContribution += worker.GetWorkContributionPerSecond();
                }
            }

            CompletedWork += workContribution * deltaTime;

            // 确保CompletedWork不会超过TotalWork
            if (CompletedWork >= TotalWork)
            {
                FinishWork();
            }
        }
        void FinishWork()
        {
            Debug.Log("FinishWork");
            if (this.type == OrderType.Construction)
            {
                bd.constructing = false;
                tobeDeleted = true;
            }
            else if (this.type == OrderType.Crafting)
            {
                AddScore();
                //generate ,consume
                repeatTime--;
                if (repeatTime == 0)
                {
                    tobeDeleted = true;
                    bd.FinishProduceOrder();
                }
                else
                {
                    this.Reset();
                }

            }
        }
        void AddScore()
        {

            TBSPlayer.UserDetail.AddScore(pdCfg.Score);
        }
        public bool WorkersFull()
        {
            return this.workerList.Count == this.maxWorkerCount;
        }
        public void AddWorker(Pawn p)
        {
            this.workerList.Add(p);
            p.workingOrder = this;
            p.workingBuilding = this.bd;

        }
        public void RemoveWorker(Pawn p)
        {
            this.workerList.Remove(p);
            if (p.workingOrder == this)
            {
                p.workingOrder = null;
            }
            if (p.workingBuilding == this.bd)
                p.workingBuilding = null;
        }

    }
}
