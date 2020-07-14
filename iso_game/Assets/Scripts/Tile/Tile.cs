using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    
    protected int order;
    protected float gridX;
    protected float gridY;
    protected virtual void CalcOrderInLayer()
    {
        order = (int)(gridX + gridY);
    }
    
}

