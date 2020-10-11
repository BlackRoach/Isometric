using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>타일맵의 초기위치, 그리드 크기, 맵 크기 등을 담은 스크립터블 오브젝트</para>
/// </summary>
[System.Serializable]
[CreateAssetMenu(fileName = "TileMap Data", menuName = "Scriptable Object/TileMap Data", order = int.MaxValue)]
public class TileMapData : ScriptableObject
{
    public Vector2 gridInitpos;
    public float gridSize;
    public float mapWidth;
    public float mapHeight;
    public float tileWidth;
    public float tileHeight;
    private const float gridConst = 0.01f;
    public float GridConst { get { return gridConst; } }

    public void CalcTileSize()
    {
        tileWidth = gridSize * gridConst * .5f;
        tileHeight = gridSize * gridConst * .25f;
    }
    public void SetInitPos(Vector3 position)
    {
        gridInitpos = position;
        gridInitpos.y = position.y + tileHeight;
    }
}
