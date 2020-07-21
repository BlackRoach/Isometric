using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(TileManager))]


public class IsoEditor : Editor
{
    private enum MODE
    {
        NONE,
        MAKE,
        EDIT
    }
    private TileManager tileManager;
    private TileMapData t_data;
    private Vector3 mousepos;
    

    private MODE mode;
    
    private int gridX;
    private int gridY;

    private bool isCreate;
    private float makegridX;
    private float makegridY;

    private int selectX;
    private int selectY;

    private void OnEnable()
    {
        tileManager = (TileManager)target;
       
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if(mode == MODE.NONE)
        {
            GUILayout.BeginHorizontal();
            GUI.color = Color.white;
            if(GUILayout.Button("Make mode"))
            {
                isCreate = false;
                mode = MODE.MAKE;
            }
            if (GUILayout.Button("Edit mode"))
            {
                mode = MODE.EDIT;
            }
            GUILayout.EndHorizontal();
        }
        else if(mode == MODE.MAKE)
        {
            GUI.color = Color.gray;
            
            if (GUILayout.Button("Stop Make mode"))
            {
                mode = MODE.NONE;
            }
            
        }
        else if (mode == MODE.EDIT)
        {
            GUI.color = Color.gray;
            if (GUILayout.Button("Stop Edit mode"))
            {
                mode = MODE.NONE;
            }
        }
    }

    void OnSceneGUI()
    {
        SceneView.RepaintAll();
        UpdateVariable();


        MouseEvent();

        DrawGrid(t_data.gridInitpos, t_data.mapWidth, t_data.mapHeight, tileManager.gridColor);
    }
    /// <summary>
    /// 편집 모드 중 Make 모드의 기능을 모아놓은 함수
    /// </summary>
    /// <param name="e">current 를 받기 위한 매개변수</param>
    private void MakeMode(Event e)
    {
        if (isCreate)
        {
            // 선택한 그리드 그리기
            DrawGrid(tileManager.CalcTilePosition(selectX, selectY, t_data.gridInitpos, ref t_data),
                makegridX, makegridY, Color.blue);

            if (tileManager.GetMouseOnTile(ref gridX, ref gridY,
                mousepos, ref t_data))
            {
                // 현재 마우스 포인트 스냅 그리기
                DrawGrid(tileManager.CalcTilePosition(gridX, gridY, t_data.gridInitpos, ref t_data),
                    1, 1, Color.red);



            }
        }
        else
        {
            if (tileManager.GetMouseOnTile(ref gridX, ref gridY,
                mousepos, ref t_data))
            {
                DrawGrid(tileManager.CalcTilePosition(gridX, gridY, t_data.gridInitpos, ref t_data),
                    makegridX, makegridY, Color.blue);

                //선택한 그리드 위치 변수에 저장, 오브젝트 생성

                if (e.type == EventType.MouseDown || e.type == EventType.MouseDrag)
                {
                   
                    selectX = gridX;
                    selectY = gridY;
                    isCreate = true;

                    GameObject obj = new GameObject("madeObj");
                    Vector2 temp = tileManager.CalcTilePosition(gridX, gridY, t_data.gridInitpos,ref t_data);
                    temp.y -= t_data.tileHeight;
                    obj.transform.position = temp;
                    obj.tag = "Object";
                    obj.AddComponent<ObjectTile>();


                    GameObject child = new GameObject("ObjSprite");
                    child.transform.parent = obj.transform;
                    child.transform.localPosition = Vector3.zero;
                    child.AddComponent<SpriteRenderer>();
                }
            }

        }
    }
    /// <summary>
    /// 마우스 이벤트 관련 코드 작성 함수
    /// </summary>
    private void MouseEvent()
    {
        Event e = Event.current;

        if(mode == MODE.NONE)
        {
            if (tileManager.GetMouseOnTile(ref gridX, ref gridY,
                mousepos, ref t_data))
            {
                DrawGrid(tileManager.CalcTilePosition(gridX, gridY,t_data.gridInitpos,ref t_data), 1, 1, Color.red);

                if (e.type == EventType.MouseDown || e.type == EventType.MouseDrag)
                {

                }
            }
        }
        else if(mode == MODE.MAKE)
        {

            MakeMode(e);
        }
        
    }
    /// <summary>
    /// 각종 변수들의 업데이트를 묶어놓은 함수
    /// </summary>
    private void UpdateVariable()
    {
        // 마우스 위치 업데이트
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        mousepos = ray.origin;
        t_data = tileManager.t_data;
        //각 타일 width, height값 업데이트
        t_data.CalcTileSize();
        //grid 시작지점 업데이트
        t_data.SetInitPos(tileManager.transform.position);
        //생성할 오브젝트의 타일크기 업데이
        makegridX = tileManager.makegridX;
        makegridY = tileManager.makegridY;
    }
    


    


   /// <summary>
   /// Isometric Grid를 그려주는 함수
   /// </summary>
   /// <param name="initPos">Grid가 처음 그려질 위치</param>
   /// <param name="width">Grid 넓이</param>
   /// <param name="height">Grid 높이</param>
   /// <param name="color">Grid 색깔</param>
    private void DrawGrid(Vector2 initPos, float width, float height, Color color)
    {
        //그리드 선 색 설정
        Handles.color = color;
        
        //왼쪽 아래 방향 그리드 Draw
        Vector2 startPos = initPos;
        Vector2 endPos = initPos;
        
        
        endPos.x -= height * t_data.tileWidth;
        endPos.y -= height * t_data.tileHeight;

        for (int i = 0; i < width + 1; i++)
        {
            Handles.DrawLine(startPos, endPos);

            startPos.x += t_data.tileWidth;
            startPos.y -= t_data.tileHeight;
            endPos.x += t_data.tileWidth;
            endPos.y -= t_data.tileHeight;
        }

        //오른쪽 아래 방향 그리드 Draw
        startPos = initPos;
        endPos = initPos;

        
        endPos.x += width * t_data.tileWidth;
        endPos.y -= width * t_data.tileHeight;

        for (int i = 0; i < height + 1; i++)
        {
            Handles.DrawLine(startPos, endPos);

            startPos.x -= t_data.tileWidth;
            startPos.y -= t_data.tileHeight;
            endPos.x -= t_data.tileWidth;
            endPos.y -= t_data.tileHeight;
        }

    }
}
