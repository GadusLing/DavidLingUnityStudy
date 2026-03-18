using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson20E_Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FtpMgr.Instance.UploadFile("Lesson20ETest.png", Application.streamingAssetsPath + "/test.png", () =>
        {
            Debug.Log("上传完成了，委托回调了");
        });
        print("没有卡住主线程");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
