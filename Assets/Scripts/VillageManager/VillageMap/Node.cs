using UnityEngine;
using System;

namespace SunHeTBS
{

    public class Node
    {
        public float costSoFar;
        public float fScore;
        public bool isObstacle;
        public Node parent;
        public Vector3 position;
        public Node(Vector3 pos)
        {
            fScore = 0.0f;
            costSoFar = 0.0f;
            isObstacle = false;
            parent = null;
            position = pos;
        }
        public void MarkAsObstacle()
        {
            isObstacle = true;
        }

        public override bool Equals(object obj)
        {
            return obj is Node node && position.Equals(node.position);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(position);
        }
    }
}