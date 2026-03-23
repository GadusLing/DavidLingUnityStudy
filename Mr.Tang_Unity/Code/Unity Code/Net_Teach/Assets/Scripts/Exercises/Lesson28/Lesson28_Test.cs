using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Lesson28_Test : MonoBehaviour
{
    public RawImage image;
    // Start is called before the first frame update
    void Start()
    {
        if (NetWWWMgr.Instance == null)
        {
            GameObject obj = new GameObject("NetWWWMgr");
            obj.AddComponent<NetWWWMgr>();
        }

        NetWWWMgr.Instance.LoadRes<Texture>("http://192.168.1.2:8081/Http_Server/http上传的文件.jpg", (tex) =>
        {
            Debug.Log("加载Texture完成");
            image.texture = tex;
        });

        NetWWWMgr.Instance.LoadRes<byte[]>("http://192.168.1.2:8081/Http_Server/http上传的文件.jpg", (bytes) =>
        {
            Debug.Log("加载字节数组完成");
            print(Application.persistentDataPath);
            File.WriteAllBytes(Application.persistentDataPath + "/www图片.jpg", bytes);
        });

        NetWWWMgr.Instance.LoadRes<string>("http://192.168.1.2:8081/Http_Server/test.txt", (text) =>
        {
            Debug.Log("加载字符串完成");
            print(text);
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
