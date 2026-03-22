using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson25E_Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // print(Application.persistentDataPath);
        // HttpMgr.Instance.DownLoadFile("test.txt", Application.persistentDataPath + "/httpDownloadedAsync.txt", (status) =>
        // {
        //     if (status == System.Net.HttpStatusCode.OK)
        //     {
        //         Debug.Log("文件下载成功");
        //     }
        //     else
        //     {
        //         Debug.Log("文件下载失败，状态码: " + status);
        //     }
        // });
        HttpMgr.Instance.UpLoadFile("封装后上传.png", Application.streamingAssetsPath + "/test.png", (status) =>
        {
            if (status == System.Net.HttpStatusCode.OK)
            {
                Debug.Log("文件上传成功");
            }
            else
            {
                Debug.Log("文件上传失败，状态码: " + status);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
