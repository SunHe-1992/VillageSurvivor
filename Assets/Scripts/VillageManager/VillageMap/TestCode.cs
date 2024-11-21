using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace SunHeTBS
{
    public class TestCode : MonoBehaviour
    {
        private Transform startPos, endPos;
        public Node startNode { get; set; }
        public Node goalNode { get; set; }
        public List<Node> pathArray;
        public GameObject objStartCube, objEndCube;
        private float elapsedTime = 0.0f;
        //Interval time between pathfinding
        public float intervalTime = 1.0f;

        void Start()
        {
            if (objStartCube == null)
                objStartCube = GameObject.FindGameObjectWithTag("Start");
            if (objEndCube == null)
                objEndCube = GameObject.FindGameObjectWithTag("End");
            pathArray = new List<Node>();
            FindPath();
        }
        void Update()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= intervalTime)
            {
                elapsedTime = 0.0f;
                FindPath();
            }
        }
        void FindPath()
        {
            startPos = objStartCube.transform;
            endPos = objEndCube.transform;
            //Assign StartNode and Goal Node
            var (startColumn, startRow) = GridManager.instance.GetGridCoordinates(startPos.position);
            var (goalColumn, goalRow) = GridManager.instance.GetGridCoordinates(endPos.position);
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
    }
}