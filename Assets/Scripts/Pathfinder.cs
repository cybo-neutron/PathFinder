using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Pathfinder : MonoBehaviour
{
    Node m_startNode;
    Node m_goalNode;
    Graph m_graph;
    GraphView m_graphView;

    Queue<Node> m_frontierNodes;    //The nodes which are currently exploring
    List<Node> m_exploredNodes;     //The nodes which have been visited
    List<Node> m_pathNodes;             //the group of nodes which form the fastest route from the start node to goal node

    public Color startColor = Color.green;
    public Color goalColor = Color.red;
    public Color frontierColor = Color.magenta;
    public Color exploredColor = Color.gray;
    public Color pathColor = Color.cyan;
    public Color arrowColor = new Color32(220, 220, 153, 255);
    public Color arrowHighlightColor = new Color32(255, 255, 153, 255);

    public bool showIterations = true;
    public bool showColors = true;
    public bool showArrows = true;
    public bool exitOnGoal = true;

    bool isComplete = false;
    int m_iterations = 0;

    public enum Mode
    {
        BreadthFirstSearch,
        Dijkstra
    }

    public Mode mode = Mode.BreadthFirstSearch;
    public void Init(Graph graph, GraphView graphView, Node start, Node goal)
    {
        if (start == null || goal == null || graph == null || graphView == null)
        {
            Debug.LogWarning("PATHFINDER Init error : missing component(s)!");
            return;
        }

        if (start.nodeType == NodeType.Blocked || goal.nodeType == NodeType.Blocked)
        {
            Debug.LogWarning("PATHFINDER Init error : start and goal nodes must be unblocked");
            return;
        }

        Debug.Log($"Start : {start.xIndex} {start.yIndex}");
        Debug.Log($"Goal : {goal.xIndex} {goal.yIndex}");


        m_graph = graph;
        m_graphView = graphView;
        m_startNode = start;
        m_goalNode = goal;

        ShowColors();

        m_frontierNodes = new Queue<Node>();
        m_frontierNodes.Enqueue(start);
        m_exploredNodes = new List<Node>();
        m_pathNodes = new List<Node>();

        //Reseting the graph nodes if it was previously visited.
        for (int y = 0; y < m_graph.Height; y++)
        {
            for (int x = 0; x < m_graph.Width; x++)
            {
                m_graph.nodes[y, x].Reset();
            }
        }

        isComplete = false;
        m_iterations = 0;
    }


    void ShowColors(GraphView graphView, Node start, Node goal)
    {
        if (graphView == null || start == null || goal == null)
        {
            return;
        }

        if (m_frontierNodes != null)
        {
            graphView.ColorNodes(m_frontierNodes.ToList(), frontierColor);
        }

        if (m_exploredNodes != null)
        {
            graphView.ColorNodes(m_exploredNodes.ToList(), exploredColor);
        }

        if (m_pathNodes != null && m_pathNodes.Count > 0)
        {
            graphView.ColorNodes(m_pathNodes, pathColor);
        }

        NodeView startNodeView = graphView.nodeViews[start.yIndex, start.xIndex];
        if (startNodeView != null)
        {
            startNodeView.ColorNode(startColor);
        }

        NodeView goalNodeView = graphView.nodeViews[goal.yIndex, goal.xIndex];
        if (goalNodeView != null)
        {
            goalNodeView.ColorNode(goalColor);
        }
    }
    void ShowColors()
    {
        ShowColors(m_graphView, m_startNode, m_goalNode);
    }

    public IEnumerator SearchRoutine(float timeStep)
    {
        float startTime = Time.time;
        yield return null;

        while (!isComplete)
        {
            if (m_frontierNodes.Count > 0)
            {
                Node currentNode = m_frontierNodes.Dequeue();
                m_iterations++;

                if (!m_exploredNodes.Contains(currentNode))
                {
                    m_exploredNodes.Add(currentNode);
                }

                if (mode == Mode.BreadthFirstSearch)
                {
                    ExpandFrontierBreadthFirst(currentNode);
                }
                else if (mode == Mode.Dijkstra)
                {

                }



                if (m_frontierNodes.Contains(m_goalNode))
                {
                    m_pathNodes = GetPathNodes(m_goalNode);
                    if (exitOnGoal)
                    {
                        isComplete = true;
                    }
                }

                if (showIterations)
                {
                    ShowDiagnostics();

                    yield return new WaitForSeconds(timeStep);

                }


            }
            else
            {
                isComplete = true;
            }
        }

        ShowDiagnostics();
        Debug.Log($"PATHFINDER SearchRoutine: elapsed time = {Time.time - startTime} seconds");


    }

    void ShowDiagnostics()
    {
        if (showColors)
        {
            ShowColors();
        }

        if (showArrows)
        {
            //Show arrows
            if (m_graphView)
            {
                m_graphView.ShowNodeArrows(m_frontierNodes.ToList(), arrowColor);

                if (m_frontierNodes.Contains(m_goalNode))
                {
                    m_graphView.ShowNodeArrows(m_pathNodes, arrowHighlightColor);
                }
            }

        }
    }
    void ExpandFrontierBreadthFirst(Node currentNode)
    {
        if (currentNode != null)
        {
            foreach (Node node in currentNode.neighbours)
            {
                if (!m_exploredNodes.Contains(node) && !m_frontierNodes.Contains(node))
                {
                    node.previous = currentNode;
                    m_frontierNodes.Enqueue(node);
                }
            }
        }
    }

    void ExpandFrontierDijkstra(Node currentNode)
    {
        if (currentNode != null)
        {
            foreach (Node node in currentNode.neighbours)
            {
                if (!m_exploredNodes.Contains(node) && !m_frontierNodes.Contains(node))
                {
                    node.previous = currentNode;
                    m_frontierNodes.Enqueue(node);
                }
            }
        }
    }

    List<Node> GetPathNodes(Node endNode)
    {
        List<Node> path = new List<Node>();
        if (endNode == null)
        {
            return path;
        }
        path.Add(endNode);

        Node currentNode = endNode.previous;
        while (currentNode != null)
        {
            path.Add(currentNode);
            currentNode = currentNode.previous;
        }



        return path;
    }

}
