using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int a;
    protected TileData data;
    protected virtual void CalcOrderInLayer()
    {
        data.order = (int)(data.gridX + data.gridY);
    }
    public void ChangePosition(int x, int y)
    {
        data.gridX = x;
        data.gridY = y;
        CalcOrderInLayer();
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
