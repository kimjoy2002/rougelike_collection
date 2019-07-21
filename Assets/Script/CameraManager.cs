using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraManager : MonoBehaviour
{
    public int minX;
    public int maxX;
    public int minY;
    public int maxY;

    public float moveSpeed; //움직이는 속도(필요없음?)
    public float zoomSpeed; //줌 속도
    public float minOrtho; //최대 줌인
    public float maxOrtho; //최대 줌아웃
    private float smoothSpeed = 10.0f;

    public Transform cam;
    private float targetOrtho;

    Vector2 prevPos = Vector2.zero;
    float prevDistance = 0f;

    // Start is called before the first frame update
    void Start()
    {
        DebugLogger.Log("CameraManager Start");
        cam = Camera.main.transform;
        targetOrtho = Camera.main.orthographicSize;
    }



    public void OnDrag()
    {
        int touchCount = Input.touchCount;
        if (touchCount == 0 || touchCount == 1)
        {
            Vector2 curPos = (touchCount == 0) ? (Vector2)Input.mousePosition : Input.GetTouch(0).position;

            if (prevPos == Vector2.zero)
            {
                prevPos = curPos;
                return;
            }
            Vector3 dis = (curPos - prevPos);


            cam.position -= dis * moveSpeed / 10 * Time.deltaTime;

            if (cam.position.x <= minX)
                cam.position = new Vector3(minX, cam.position.y, cam.position.z);
            else if (cam.position.x >= maxX)
                cam.position = new Vector3(maxX, cam.position.y, cam.position.z);

            if (cam.position.y <= minY)
                cam.position = new Vector3(cam.position.x, minY, cam.position.z);
            else if (cam.position.y >= maxY)
                cam.position = new Vector3(cam.position.x, maxY, cam.position.z);

            prevPos = curPos;
        }
        else if(touchCount == 2)
        {
            if (prevDistance == 0)
            {
                prevDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                return;
            }
            float curDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
            float move = prevDistance - curDistance;

            Vector3 pos = cam.position;

            if(move < 0)
            {
                pos.y -= zoomSpeed * Time.deltaTime;
            }
            else if(move > 0)
            {
                pos.y += zoomSpeed * Time.deltaTime;
            }

            cam.position = pos;
            prevDistance = curDistance;
        }
    }

    public void ExitDrag()
    {
        prevPos = Vector2.zero;
        prevDistance = 0f;
    }

    public void OnWheel()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") ;

        if (scroll != 0.0f)
        {
            targetOrtho -= scroll * zoomSpeed;
            targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);
        }

        Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize,  targetOrtho, smoothSpeed * Time.deltaTime);
    }
}
