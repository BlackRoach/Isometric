using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    protected bool available;
    
    public bool Available
    {
        get
        {
            return available;
        }
        set
        {
            available = Available;
        }
    }

    protected int order;
    protected float gridX;
    protected float gridY;
    protected void CalcOrderInLayer()
    {
        order = (int)(gridX + gridY);
    }
    
}
