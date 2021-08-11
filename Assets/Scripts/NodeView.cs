using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeView : MonoBehaviour
{
    public GameObject tile;
    public GameObject arrow;
    Node m_node;

    [Range(0, 0.50f)]
    public float borderSize = 0.15f;


    public void Init(Node node)
    {
        if (tile != null)
        {
            gameObject.name = $"Node ({node.xIndex},{node.yIndex})";
            gameObject.transform.position = node.position;
            tile.transform.localScale = new Vector3(1f - borderSize, 0f, 1f - borderSize);
            m_node = node;
            EnableObject(arrow, false);
        }
    }

    void ColorNode(Color color, GameObject go)
    {
        if (go != null)
        {
            Renderer goRenderer = go.GetComponent<Renderer>();

            if (goRenderer != null)
            {
                goRenderer.material.color = color;
            }

        }
    }

    public void ColorNode(Color color)
    {
        ColorNode(color, tile);
    }

    void EnableObject(GameObject gameObject, bool state)
    {
        if (gameObject != null)
        {
            gameObject.SetActive(state);
        }
    }

    public void ShowArrow(Color color)
    {
        if (m_node != null && arrow != null && m_node.previous != null)
        {
            EnableObject(arrow, true);
            Vector3 previousNodeDirection = (m_node.previous.position - m_node.position).normalized;
            arrow.transform.rotation = Quaternion.LookRotation(previousNodeDirection);

            Renderer arrowRenderer = arrow.GetComponent<Renderer>();
            arrowRenderer.material.color = color;
        }

    }

}
