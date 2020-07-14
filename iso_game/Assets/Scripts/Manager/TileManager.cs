using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class TileManager : MonoBehaviour
{



#if UNITY_EDITOR
    public int gridSize;
    public Color gridColor;

    public float mapWidth;
    public float mapHeight;
   
    public float makegridX;
    public float makegridY;
#endif
    private static TileManager instance;
    public static TileManager Instance
    {
        get { return instance; }
    }

    private GameObject[,] tileAvailable;

    private int width;
    private int height;
    private void Awake()
    { 
        if (instance != null)
        {
            DestroyImmediate(this.GetComponent<TileManager>());
            return;
        }
        instance = this;

        // 나중에 json으로 받음
        width = 17;
        height = 17;
        
        tileAvailable = new GameObject[width,height];
        
    }

}
