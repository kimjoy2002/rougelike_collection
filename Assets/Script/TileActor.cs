using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileActor : MonoBehaviour
{
    public int Column { get; set; }
    public int Row { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        //EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

        //EventTrigger.Entry entry_PointerDown = new EventTrigger.Entry();
        //entry_PointerDown.eventID = EventTriggerType.PointerDown;
        //entry_PointerDown.callback.AddListener((data) => { OnMouseDown(); });
        //eventTrigger.triggers.Add(entry_PointerDown);
    }

    public void OnMouseDown2()
    {
        DebugLogger.Log("onClick! x: " + Column + ", y:" + Row);
    }

}
