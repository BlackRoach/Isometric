using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    protected TileData data;
    protected virtual void CalcOrderInLayer()
    {
        data.order = (int)(data.gridX + data.gridY);
    }

    
}
[System.Serializable]
public class TileData
{
    public string tileNum;
    public int order;
    public float gridX;
    public float gridY;

}
