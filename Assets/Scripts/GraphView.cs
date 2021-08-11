using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Graph))]
public class GraphView : MonoBehaviour
{
    public GameObject nodeViewPrefab;
    public NodeView[,] nodeViews;
    public Color baseColor = Color.white;
    public Color wallColor = Color.gray;



    //Populating all the tiles according the the data in graph. Assigining each point in the graph with node views.
    public void Init(Graph graph)
    {
        if (graph == null)
        {
            Debug.LogWarning("GRAPHVIEW : No graph to initialize");
            return;
        }

        nodeViews = new NodeView[graph.Height, graph.Width];

        Debug.Log($"Graph Height : {graph.Height}");
        Debug.Log($"Graph Width : {graph.Width}");




        foreach (Node node in graph.nodes)
        {
            GameObject instance = Instantiate(nodeViewPrefab, Vector3.zero, Quaternion.identity);
            NodeView nodeView = instance.GetComponent<NodeView>();


            if (nodeView != null)
            {
                nodeView.Init(node);
                nodeViews[node.yIndex, node.xIndex] = nodeView;

                if (node.nodeType == NodeType.Blocked)
                {
                    nodeView.ColorNode(wallColor);
                }
                else
                {
                    nodeView.ColorNode(baseColor);
                }
            }
        }
    }


    public void ColorNodes(List<Node> nodes, Color color)
    {
        foreach (Node node in nodes)
        {
            if (node != null)
            {
                NodeView nodeView = nodeViews[node.yIndex, node.xIndex];

                if (nodeView != null)
                {
                    nodeView.ColorNode(color);
                }

            }

        }
    }


    public void ShowNodeArrows(Node node, Color color)
    {
        if (node != null)
        {
            NodeView nodeView = nodeViews[node.yIndex, node.xIndex];
            if (nodeView != null)
            {
                nodeView.ShowArrow(color);
            }
        }
    }

    public void ShowNodeArrows(List<Node> nodes, Color color)
    {
        foreach (Node node in nodes)
        {
            ShowNodeArrows(node, color);
        }
    }

}
