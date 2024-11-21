using UnityEngine;
using System.Collections.Generic;
namespace SunHeTBS
{
    public class AStar
    {
        private float HeuristicEstimateCost(Node curNode, Node goalNode)
        {
            return (curNode.position - goalNode.position).magnitude;
        }
        public List<Node> FindPath(Node start, Node goal)
        {
            //Start Finding the path
            NodePriorityQueue openList = new NodePriorityQueue();
            openList.Enqueue(start);
            start.costSoFar = 0.0f;
            start.fScore = HeuristicEstimateCost(start, goal);
            HashSet<Node> closedList = new();
            Node node = null;

            while (openList.Length != 0)
            {
                node = openList.Dequeue();
                if (node.position == goal.position)
                {
                    return CalculatePath(node);
                }
                var neighbours = GridManager.instance.GetNeighbours(node);
                foreach (Node neighbourNode in neighbours)
                {
                    if (!closedList.Contains(neighbourNode))
                    {
                        float totalCost = node.costSoFar + GridManager.instance.StepCost;
                        float heuristicValue = HeuristicEstimateCost(neighbourNode, goal);
                        //Assign neighbour node properties
                        neighbourNode.costSoFar = totalCost;
                        neighbourNode.parent = node;
                        neighbourNode.fScore = totalCost + heuristicValue;
                        //Add the neighbour node to the queue
                        if (!closedList.Contains(neighbourNode))
                        {


                            openList.Enqueue(neighbourNode);
                        }
                    }
                }
                closedList.Add(node);
            }
            //If finished looping and cannot find the goal then return null
            if (node.position != goal.position)
            {
                Debug.LogError("Goal Not Found");
                return null;
            }
            //Calculate the path based on the final node
            return CalculatePath(node);
        }
        private List<Node> CalculatePath(Node node)
        {
            List<Node> list = new();
            while (node != null)
            {
                list.Add(node);
                node = node.parent;
            }
            list.Reverse();
            return list;
        }

    }
}

