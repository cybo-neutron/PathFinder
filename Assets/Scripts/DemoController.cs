using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoController : MonoBehaviour
{
    public MapData mapData;
    public Graph graph;
    public Pathfinder pathfinder;
    public int startX;
    public int startY;
    public int goalX;
    public int goalY;


    public float timeStep = 0.1f;


    private void Start()
    {
        if (mapData != null && graph != null)
        {
            int[,] mapInstance = mapData.MakeMap();
            graph.Init(mapInstance);

            GraphView graphView = graph.gameObject.GetComponent<GraphView>();
            if (graphView != null)
            {
                graphView.Init(graph);
            }

            if (graph.IsWithinBounds(startY, startX) && graph.IsWithinBounds(goalY, goalX) && pathfinder != null)
            {
                Node startNode = graph.nodes[startY, startX];
                Node goalNode = graph.nodes[goalY, goalX];

                pathfinder.Init(graph, graphView, startNode, goalNode);
                StartCoroutine(pathfinder.SearchRoutine(timeStep));
            }
            else
            {
                Debug.LogWarning("Warning : start or goal not is out of Bounds");
            }



        }
    }

}
