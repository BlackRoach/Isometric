using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(TileManager))]


public class IsoEditor : Editor
{
    private TileManager tileManager;
   


    private float tileWidth;
    private float tileHeight;
    private const float gridConst = 0.01f;
    private void OnEnable()
    {
        tileManager = (TileManager)target;
          }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    void OnSceneGUI()
    {
        SceneView.RepaintAll();
        tileWidth = tileManager.gridSize * gridConst * .5f;
        tileHeight = tileManager.gridSize * gridConst * .25f;
        
        DrawMapGrid();
    }
    private void DrawMapGrid()
    {
        //그리드 선 색 설정
        Handles.color = tileManager.gridColor;

        //왼쪽 아래 방향 그리드 Draw
        Vector2 startPos = tileManager.transform.position;
        Vector2 endPos = tileManager.transform.position;
        
        startPos.y += tileHeight;
        endPos.x -= tileManager.mapHeight * tileWidth;
        endPos.y -= (tileManager.mapHeight - 1) * tileHeight;

        for (int i = 0; i < (tileManager.mapWidth + 1); i++)
        {
            Handles.DrawLine(startPos, endPos);

            startPos.x += tileWidth;
            startPos.y -= tileHeight;
            endPos.x += tileWidth;
            endPos.y -= tileHeight;
        }

        //오른쪽 아래 방향 그리드 Draw
        startPos = tileManager.transform.position;
        endPos = tileManager.transform.position;

        startPos.y += tileHeight;
        endPos.x += tileManager.mapWidth * tileWidth;
        endPos.y -= (tileManager.mapWidth - 1) * tileHeight;

        for (int i = 0; i < (tileManager.mapHeight + 1); i++)
        {
            Handles.DrawLine(startPos, endPos);

            startPos.x -= tileWidth;
            startPos.y -= tileHeight;
            endPos.x -= tileWidth;
            endPos.y -= tileHeight;
        }

    }
}
