using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum TileType
{
    Ground,
    Object
}
public class TileController : MonoBehaviour
{
    public TileData tiledata;

    
}

[System.Serializable]
public class TileData
{
    public string tileNum;
    public int order;
    public int gridX;
    public int gridY;

}
