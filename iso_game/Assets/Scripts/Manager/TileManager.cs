using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class TileManager : MonoBehaviour
{



#if UNITY_EDITOR
   

    public Color gridColor;

   
    public float makegridX;
    public float makegridY;
#endif
    
    public TileMapData t_data;
    private GameObject[,] tileAvailable;
    private GameObject selectedObj;



    private void Awake()
    {
        t_data = Resources.Load("Data/TileMap Data") as TileMapData;
        t_data.CalcTileSize();
        tileAvailable = new GameObject[(int)t_data.mapWidth,(int)t_data.mapHeight];
        
    }
    private void Update()
    {
        
    }
    /// <summary>
    /// 그리드 타일의 위치를 통해 World Position을 반환하는 함수
    /// </summary>
    /// <param name="x">그리드 타일의 x 위치</param>
    /// <param name="y">그리드 타일의 y 위치</param>
    /// <param name="initPos">그리드 타일의 World Position 시작 위치</param>
    /// <param name="data">현재 계산하고자 하는 맵의 데이ㅌ</param>
    /// <returns>x,y 위치의 그리드 타일의 World Position 값을 담은 Vector2 반환</returns>
    public Vector2 CalcTilePosition(int x, int y, Vector2 initPos, ref TileMapData data)
    {
        float xpos = initPos.x;
        float ypos = initPos.y;
        xpos += (x - y) * data.tileWidth;
        ypos += -(x + y) * data.tileHeight;
        return new Vector2(xpos, ypos);
    }


    /// <summary>
    /// 마우스 커서가 올려져있는 그리드 타일의 위치를 업데이트하고 그 여부를 반환
    /// </summar>
    /// <param name="x">그리드 타일 x 위치</param>
    /// <param name="y">그리드 타일 y 위치</param>
    /// <param name="mousepos">현재 터치중인 마우스 좌표</param>
    /// <param name="data">현재 계산하고자 하는 맵의 데이터</param>
    /// <returns>그리드 맵의 범위 내에 있는지 bool 값 반환</returns>
    public bool GetMouseOnTile(ref int x, ref int y, Vector2 mousepos,ref TileMapData data)
    {

        //  xpos = (x - y) * tileWidth;
        //  ypos = -(x + y) * tileHeight;
        //  위 식을 전개하면 밑의 식을 유도 가능

        x = Mathf.FloorToInt(-((mousepos.y - data.tileHeight) / data.tileHeight * 0.5f)
            + (mousepos.x / data.tileWidth * 0.5f));
        y = Mathf.FloorToInt(-((mousepos.y - data.tileHeight) / data.tileHeight * 0.5f)
            - (mousepos.x / data.tileWidth * 0.5f));
        if (x < 0 || x >= data.mapWidth)
            return false;

        if (y < 0 || y >= data.mapHeight)
            return false;

        return true;
    }
}

