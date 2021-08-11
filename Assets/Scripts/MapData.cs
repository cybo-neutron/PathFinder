using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class MapData : MonoBehaviour
{

    public int width = 10;
    public int height = 5;

    public TextAsset textAsset;
    public Texture2D textureMap;
    public string resourcePath = "Mapdata";
    public string resourcePathImage = "Mapdata/Images";

    private void Awake()
    {
        string levelName = SceneManager.GetActiveScene().name;
        if (textureMap == null)
        {
            textureMap = Resources.Load(resourcePathImage + $"/{levelName}") as Texture2D;
        }

        if (textAsset == null)
        {
            textAsset = Resources.Load(resourcePath + $"/{levelName}") as TextAsset;
        }
    }
    public List<string> GetMapFromTextFile(TextAsset tAsset)
    {
        List<string> lines = new List<string>();

        if (tAsset != null)
        {
            string textData = tAsset.text;
            string[] delimiters = { "\n", "\r\n" };
            lines.AddRange(textData.Split(delimiters, System.StringSplitOptions.None));
            lines.Reverse();
        }
        else
        {
            Debug.LogWarning("MAPDATA GetMapFromTextFile Error: Invalid TextAsset");
        }

        return lines;
    }

    public List<string> GetMapFromTextFile()
    {

        return GetMapFromTextFile(textAsset);
    }


    public List<string> GetMapFromTexture(Texture2D texture)
    {
        List<string> lines = new List<string>();

        for (int y = 0; y < texture.height; y++)
        {
            string newLine = "";
            for (int x = 0; x < texture.width; x++)
            {
                if (texture.GetPixel(x, y) == Color.black)
                {
                    newLine += '1';
                }
                else if (texture.GetPixel(x, y) == Color.white)
                {
                    newLine += '0';
                }
                else
                {
                    newLine += ' ';
                }
            }
            lines.Add(newLine);
        }

        return lines;
    }

    public List<string> GetMapFromTexture()
    {
        return GetMapFromTexture(textureMap);
    }
    void SetDimensions(List<string> textLines)
    {
        height = textLines.Count;

        foreach (string line in textLines)
        {
            width = Mathf.Max(width, line.Length);
        }
    }
    public int[,] MakeMap()
    {
        List<string> lines = new List<string>();
        if (textureMap != null)
        {
            lines = GetMapFromTexture();
        }
        else
        {
            lines = GetMapFromTextFile();
        }
        SetDimensions(lines);

        int[,] map = new int[height, width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                map[y, x] = (int)Char.GetNumericValue(lines[y][x]);
            }
        }

        return map;
    }


}
