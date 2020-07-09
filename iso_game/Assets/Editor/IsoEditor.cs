using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileManager))]


public class IsoEditor : Editor
{
    private TileManager tileManager;

    private Vector3 mousepos;

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
        UpdateMousePosition();
        Event e = Event.current;
        if (e.type == EventType.MouseDown || e.type == EventType.MouseDrag)
        {
            Debug.Log(MouseOnTile());
        }
        tileWidth = tileManager.gridSize * gridConst * .5f;
        tileHeight = tileManager.gridSize * gridConst * .25f;
        
        DrawMapGrid();
    }
    private void UpdateMousePosition()
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        mousepos = ray.origin;
    }
    private Vector2 MouseOnTile()
    {
        float mouseHitGridX = Mathf.FloorToInt((mousepos.y - tileHeight))
        float mouseHitGridY = Mathf.FloorToInt(-((mousepos.y - tileHeight) / tileHeight * 0.5f)
            - (mousepos.x / tileWidth * 0.5f));
        return new Vector2(mouseHitGridX,mouseHitGridY);
    }
    private Vector2 CalcTilePosition(int x, int y)
    {
        float xpos = tileManager.transform.position.x;
        float ypos = tileManager.transform.position.y;
        xpos -= x * tileWidth;
        xpos += y * tileWidth;
        ypos -= (x + y) * tileHeight;
        return new Vector2(xpos, ypos);
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
