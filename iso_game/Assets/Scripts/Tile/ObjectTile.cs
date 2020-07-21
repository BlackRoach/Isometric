using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTile : Tile
{

    protected override void CalcOrderInLayer()
    {
        base.CalcOrderInLayer();
        
        data.order++;
    }
}
