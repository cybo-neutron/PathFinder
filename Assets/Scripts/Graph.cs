using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{

    public Node[,] nodes;
    public List<Node> walls = new List<Node>();

    int[,] m_mapData;
    int m_width;
    public int Width { get { return m_width; } }
    int m_height;
    public int Height { get { return m_height; } }
    public static readonly Vector2[] allDirections = {
        new Vector2(0f,1f),
        new Vector2(0f,-1f),
        new Vector2(1f,0f),
        new Vector2(-1f,0f),
        new Vector2(1f,1f),
        new Vector2(1f,-1f),
        new Vector2(-1f,1f),
        new Vector2(-1f,-1f),
    };

    //Generating the graph data from the given map data
    public void Init(int[,] mapData)
    {

        m_mapData = mapData;
        m_height = mapData.GetLength(0);
        m_width = mapData.GetLength(1);

        print("Height : " + m_height);
        print("Width : " + m_width);

        nodes = new Node[m_height, m_width];

        for (int y = 0; y < m_height; y++)
        {
            for (int x = 0; x < m_width; x++)
            {
                NodeType type = (NodeType)mapData[y, x];
                Node newNode = new Node(y, x, type);
                nodes[y, x] = newNode;

                newNode.position = new Vector3(x, 0, y);

                if (type == NodeType.Blocked)
                {
                    walls.Add(newNode);
                }
            }
        }


        //Updating the neighbours of each node i.e. only the nodes which are open/walkable will have a neighbouring node which can be traversed 
        for (int y = 0; y < m_height; y++)
        {
            for (int x = 0; x < m_width; x++)
            {
                nodes[y, x].neighbours = GetNeighbours(y, x);
            }
        }

    }

    // Checks whether the given position(x,y) is valid 
    public bool IsWithinBounds(int y, int x)
    {
        return (y >= 0 && y < m_height && x >= 0 && x < m_width);
    }

    //Gets the neighbouring nodes of node at (x,y). Max of 8 nodes(adjacent nodes and diagonal nodes)
    List<Node> GetNeighbours(int y, int x, Node[,] nodeArray, Vector2[] directions)
    {
        List<Node> neighbours = new List<Node>();

        foreach (Vector2 dir in directions)
        {
            int newX = x + (int)dir.x;
            int newY = y + (int)dir.y;

            if (IsWithinBounds(newY, newX) && nodeArray[newY, newX] != null && nodeArray[newY, newX].nodeType != NodeType.Blocked)
            {
                neighbours.Add(nodeArray[newY, newX]);
            }
        }

        return neighbours;
    }

    List<Node> GetNeighbours(int y, int x)
    {
        return GetNeighbours(y, x, nodes, allDirections);
    }

    public float GetNodeDistance(Node source, Node dest)
    {
        int dx = Mathf.Abs(source.xIndex - dest.xIndex);
        int dy = Mathf.Abs(source.yIndex - dest.yIndex);

        int min = Mathf.Min(dx, dy);
        int max = Mathf.Max(dx, dy);

        int diagonalSteps = min;
        int straightSteps = max - min;

        return (1.4f * diagonalSteps + straightSteps);
    }

}
