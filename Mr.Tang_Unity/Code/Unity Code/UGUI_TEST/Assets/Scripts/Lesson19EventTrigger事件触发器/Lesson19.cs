using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Lesson19 : MonoBehaviour
{
    public EventTrigger eventTrigger;
    // Start is called before the first frame update
    void Start()
    {
        // 推荐用第一种写法，简单、安全、不容易出错
        var entryUp = new EventTrigger.Entry();
        
        entryUp.eventID = EventTriggerType.PointerUp;
        entryUp.callback.AddListener((data) => {
            print("事件触发：PointerUp");
        }); 
        eventTrigger.triggers.Add(entryUp);


        var entryDown = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerDown,
            callback = new EventTrigger.TriggerEvent()
        };
        entryDown.callback.AddListener((data) => {
            print("事件触发：PointerDown");
        });
        eventTrigger.triggers.Add(entryDown);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TestPointEnter(BaseEventData data)
    {
        // PointerEventData ped = data as PointerEventData;// as写法
        // print("事件触发：TestPointEnter" + ped.position);
    }
    public void TestPointExit(BaseEventData data)
    {
        // PointerEventData ped = (PointerEventData)data; // 强制转换写法
        // print("事件触发：TestPointExit" + ped.position);
    }
}
