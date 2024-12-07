using UnityEngine;
using System.Collections.Generic;
namespace SunHeTBS
{
    public class GridManager : MonoBehaviour
    {
        private static GridManager staticInstance = null;
        public static GridManager instance
        {
            get
            {
                if (staticInstance == null)
                {
                    staticInstance = FindObjectOfType(typeof(GridManager)) as GridManager;
                    if (staticInstance == null)
                        Debug.Log("Could not locate an GridManager object. \n");

                }
                return staticInstance;
            }
            set { staticInstance = value; }
        }
        // Ensure that the instance is destroyed when the game is stopped in the editor.
        void OnApplicationQuit()
        {
            staticInstance = null;
        }

        public int numOfRows;
        public int numOfColumns;
        public float gridCellSize;
        public float obstacleEpsilon = 0.2f;
        public bool showGrid = true;
        public bool showObstacleBlocks = true;
        public Node[,] nodes { get; set; }
        public Vector3 Origin
        {
            get { return transform.position; }
        }
        public float StepCost
        {
            get { return gridCellSize; }
        }

        void Awake()
        {
            instance = this;
            ComputeGrid();

            FillRandomMapObjects();
        }
        void ComputeGrid()
        {
            //Initialise the nodes
            nodes = new Node[numOfColumns, numOfRows];
            for (int i = 0; i < numOfColumns; i++)
            {
                for (int j = 0; j < numOfRows; j++)
                {
                    Vector3 cellPos = GetGridCellCenter(i, j);
                    Node node = new(cellPos);
                    var collisions = Physics.OverlapSphere(cellPos, gridCellSize / 2 - obstacleEpsilon,
                    1 << LayerMask.NameToLayer("Obstacles"));
                    if (collisions.Length != 0)
                    {
                        node.MarkAsObstacle();
                    }
                    nodes[i, j] = node;
                }
            }
        }

        public Vector3 GetGridCellCenter(int col, int row)
        {
            Vector3 cellPosition = GetGridCellPosition(col, row);
            cellPosition.x += gridCellSize / 2.0f;
            cellPosition.z += gridCellSize / 2.0f;
            return cellPosition;
        }
        public Vector3 GetGridCellPosition(int col, int row)
        {
            float xPosInGrid = col * gridCellSize;
            float zPosInGrid = row * gridCellSize;
            return Origin + new Vector3(xPosInGrid, 0.0f, zPosInGrid);
        }
        public (int, int) GetGridCoordinates(Vector3 pos)
        {
            if (!IsInBounds(pos))
            {
                return (-1, -1);
            }
            int col = (int)Mathf.Floor((pos.x - Origin.x) / gridCellSize);
            int row = (int)Mathf.Floor((pos.z - Origin.z) / gridCellSize);
            return (col, row);
        }

        public bool IsInBounds(Vector3 pos)
        {
            float width = numOfColumns * gridCellSize;
            float height = numOfRows * gridCellSize;
            return (pos.x >= Origin.x && pos.x <= Origin.x + width
            && pos.z <= Origin.z + height && pos.z >= Origin.z);
        }
        public bool IsTraversable(int col, int row)
        {
            return col >= 0 && row >= 0 && col < numOfColumns && row < numOfRows && !nodes[col, row].isObstacle;
        }
        public List<Node> GetNeighbours(Node node)
        {
            List<Node> result = new();
            var (column, row) = GetGridCoordinates(node.position);
            if (IsTraversable(column - 1, row))
            {
                result.Add(nodes[column - 1, row]);
            }
            if (IsTraversable(column + 1, row))
            {
                result.Add(nodes[column + 1, row]);
            }
            if (IsTraversable(column, row - 1))
            {
                result.Add(nodes[column, row - 1]);
            }
            if (IsTraversable(column, row + 1))
            {
                result.Add(nodes[column, row + 1]);
            }
            return result;
        }
        void OnDrawGizmos()
        {
            if (showGrid)
            {
                DebugDrawGrid(Color.blue);
            }
            //Grid Start Position
            Gizmos.DrawSphere(Origin, 0.5f);
            if (nodes == null) return;
            //Draw Obstacle obstruction
            if (showObstacleBlocks)
            {
                Vector3 cellSize = new Vector3(gridCellSize, 1.0f, gridCellSize);
                Gizmos.color = Color.red;
                for (int i = 0; i < numOfColumns; i++)
                {
                    for (int j = 0; j < numOfRows; j++)
                    {
                        if (nodes != null && nodes[i, j].isObstacle)
                        {
                            Gizmos.DrawCube(GetGridCellCenter(i, j), cellSize);
                        }
                    }
                }
            }
        }
        public void DebugDrawGrid(Color color)
        {
            float width = (numOfColumns * gridCellSize);
            float height = (numOfRows * gridCellSize);
            // Draw the horizontal grid lines
            for (int i = 0; i < numOfRows + 1; i++)
            {
                Vector3 startPos = Origin + i * gridCellSize * new Vector3(0.0f, 0.0f, 1.0f);
                Vector3 endPos = startPos + width * new Vector3(1.0f, 0.0f, 0.0f);
                Debug.DrawLine(startPos, endPos, color);
            }
            // Draw the vertial grid lines
            for (int i = 0; i < numOfColumns + 1; i++)
            {
                Vector3 startPos = Origin + i * gridCellSize * new Vector3(1.0f, 0.0f, 0.0f);
                Vector3 endPos = startPos + height * new Vector3(0.0f, 0.0f, 1.0f);
                Debug.DrawLine(startPos, endPos, color);
            }
        }

        void FillRandomMapObjects()
        {
            CleanAllObj();
            for (int i = 0; i < numOfRows + 1; i++)
            {
                for (int j = 0; j < numOfColumns + 1; j++)
                {
                    if (IsFill())
                    {
                        Vector3 pos = new Vector3(
                            j * gridCellSize + +Random.Range(0f, 1f),
                            0f,
                            i * gridCellSize + Random.Range(0f, 1f)
                            );
                        FillOneMapObject(pos, RandomObj());
                    }
                }
            }

        }
        [SerializeField]
        [Tooltip("0-100, random objects filling percent")]
        public float fillPercent = 30f;
        bool IsFill()
        {
            return Random.Range(0f, 100f) < fillPercent;
        }
        void CleanAllObj()
        {
            if (NewObjParent != null)
            {
                NewObjParent.SetActive(false);
                Destroy(NewObjParent);
            }
        }
        GameObject RandomObj()
        {
            int idx = Random.Range(0, mapObjList.Count);
            return mapObjList[idx];
        }

        private GameObject NewObjParent;
        [SerializeField]
        [Tooltip("Random map objects pool")]
        public List<GameObject> mapObjList;
        void FillOneMapObject(Vector3 pos, GameObject obj)
        {
            if (NewObjParent == null)
            {
                NewObjParent = new GameObject("MapObjects");
                NewObjParent.transform.position = Origin;
            }
            var newObj = GameObject.Instantiate(obj);
            newObj.transform.SetParent(NewObjParent.transform);
            newObj.transform.position = pos;
            float ranY = Random.Range(0.0f, 359f);
            newObj.transform.localRotation = Quaternion.Euler(0f, ranY, 0f);
        }
    }
}