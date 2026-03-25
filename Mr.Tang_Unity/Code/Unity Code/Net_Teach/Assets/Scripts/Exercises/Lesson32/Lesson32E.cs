using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Lesson32E : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetMsg());
    }

    IEnumerator GetMsg()
    {
        UnityWebRequest req = new UnityWebRequest("web服务器地址", UnityWebRequest.kHttpVerbPOST);
        DownLoadHandlerMsg handler = new DownLoadHandlerMsg();
        req.downloadHandler = handler;
        yield return req.SendWebRequest();
        if(req.result == UnityWebRequest.Result.Success)
        {
           PlayerMsg msg = handler.GetMsg<PlayerMsg>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
