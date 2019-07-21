using UnityEngine;
using UnityEditor;

public class ClickBoard : MonoBehaviour
{
    public LogicManager Manager { get; set; }

    private int clickTime = 0;
    private Vector2 mousePos = new Vector3();


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Input.mousePosition;
            Vector3 tilePos = Camera.main.ScreenToWorldPoint(pos);
            if (clickTime == 0)
            {
                mousePos = pos;
                Manager.MouseDown(tilePos);
                //DebugLogger.Log("Mouse down [X: {0} Y: {1}]", mousePos.x, mousePos.y);
            }
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 pos = Input.mousePosition;
            Vector3 tilePos = Camera.main.ScreenToWorldPoint(pos);
            Manager.MouseDrag(tilePos);
            clickTime++;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if(clickTime > 0)
            {
                Vector3 pos = Input.mousePosition;
                Vector2 offset = mousePos - (Vector2) pos;
                Vector3 tilePos = Camera.main.ScreenToWorldPoint(pos);
                if (offset.sqrMagnitude <= 200)
                {
                    int floorX = Mathf.RoundToInt(tilePos.x);
                    int floorY = Mathf.RoundToInt(tilePos.y);
                    //DebugLogger.Log("Mouse Click [X: {0} Y: {1}]", floorX, floorY);
                    Manager.ClickTile(floorX, floorY);
                }
                else
                {
                    DebugLogger.Log("Mouse down skip [X: {0} Y: {1} dis: {2}]", pos.x, pos.y, offset.sqrMagnitude);
                }
                Manager.MouseUp(tilePos);
                clickTime = 0;
            }
        }
    }
}