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

    private Vector3 mousepos;
    private Vector2 gridInitPos;

    private MODE mode;
    private float tileWidth;
    private float tileHeight;
    private int gridX;
    private int gridY;

    private bool isCreate;
    private float makegridX;
    private float makegridY;

    private int selectX;
    private int selectY;

    private const float gridConst = 0.01f;
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

        DrawGrid(gridInitPos, tileManager.mapWidth, tileManager.mapHeight, tileManager.gridColor);
    }
    private void MakeMode(Event e)
    {
        if (isCreate)
        {
            // 선택한 그리드 그리기
            DrawGrid(CalcTilePosition(selectX, selectY), makegridX, makegridY, Color.blue);

            if (GetMouseOnTile())
            {
                // 현재 마우스 포인트 스냅 그리기
                DrawGrid(CalcTilePosition(gridX, gridY), 1, 1, Color.red);



            }
        }
        else
        {
            if (GetMouseOnTile())
            {
                DrawGrid(CalcTilePosition(gridX, gridY), makegridX, makegridY, Color.blue);

                //선택한 그리드 위치 변수에 저장, 오브젝트 생성

                if (e.type == EventType.MouseDown || e.type == EventType.MouseDrag)
                {
                   
                    selectX = gridX;
                    selectY = gridY;
                    isCreate = true;

                    GameObject obj = new GameObject("madeObj");
                    Vector2 temp = CalcTilePosition(selectX, selectY);
                    temp.y -= tileHeight;
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
    private void MouseEvent()
    {
        Event e = Event.current;

        if(mode == MODE.NONE)
        {
            if (GetMouseOnTile())
            {
                DrawGrid(CalcTilePosition(gridX, gridY), 1, 1, Color.red);

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

        //각 타일 width, height값 업데이트
        tileWidth = tileManager.gridSize * gridConst * .5f;
        tileHeight = tileManager.gridSize * gridConst * .25f;

        //grid 시작지점 업데이트
        gridInitPos = tileManager.transform.position;
        gridInitPos.y = tileManager.transform.position.y + tileHeight;

        makegridX = tileManager.makegridX;
        makegridY = tileManager.makegridY;
    }
    /// <summary>
    /// 마우스 커서가 올려져있는 그리드 타일의 위치를 업데이트하고 그 여부를 반환
    /// </summary>
    /// <returns>그리드 맵의 범위 내에 있는지 bool 값 반환</returns>
    private bool GetMouseOnTile()
    {
        
        //  xpos = (x - y) * tileWidth;
        //  ypos = -(x + y) * tileHeight;
        //  위 식을 전개하면 밑의 식을 유도 가능
         
        gridX = Mathf.FloorToInt(-((mousepos.y - tileHeight) / tileHeight * 0.5f)
            + (mousepos.x / tileWidth * 0.5f));
        gridY = Mathf.FloorToInt(-((mousepos.y - tileHeight) / tileHeight * 0.5f)
           - (mousepos.x / tileWidth * 0.5f));
        if (gridX < 0 || gridX >= tileManager.mapWidth)
            return false;
        
        if (gridY < 0 || gridY >= tileManager.mapHeight)
            return false;

        return true;
    }


    /// <summary>
    /// 그리드 타일의 위치를 통해 World Position을 반환하는 함수
    /// </summary>
    /// <param name="x">그리드 타일의 x 위치</param>
    /// <param name="y">그리드 타일의 y 위치</param>
    /// <returns>x,y 위치의 그리드 타일의 World Position 값을 담은 Vector2 반환</returns>
    private Vector2 CalcTilePosition(int x, int y)
    {
        float xpos = gridInitPos.x;
        float ypos = gridInitPos.y;
        xpos = (x - y) * tileWidth;
        ypos = -(x + y) * tileHeight;
        return new Vector2(xpos, ypos+tileHeight);
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
        
        
        endPos.x -= height * tileWidth;
        endPos.y -= height * tileHeight;

        for (int i = 0; i < width + 1; i++)
        {
            Handles.DrawLine(startPos, endPos);

            startPos.x += tileWidth;
            startPos.y -= tileHeight;
            endPos.x += tileWidth;
            endPos.y -= tileHeight;
        }

        //오른쪽 아래 방향 그리드 Draw
        startPos = initPos;
        endPos = initPos;

        
        endPos.x += width * tileWidth;
        endPos.y -= width * tileHeight;

        for (int i = 0; i < height + 1; i++)
        {
            Handles.DrawLine(startPos, endPos);

            startPos.x -= tileWidth;
            startPos.y -= tileHeight;
            endPos.x -= tileWidth;
            endPos.y -= tileHeight;
        }

    }
}
