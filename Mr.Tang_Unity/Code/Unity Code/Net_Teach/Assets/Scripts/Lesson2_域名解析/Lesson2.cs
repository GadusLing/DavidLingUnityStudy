using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Lesson2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print(Dns.GetHostName()); // 获取本机主机名

        // 获取指定域名的IP信息
        // IPHostEntry entry = Dns.GetHostEntry("httpbin.org");
        // for(int i = 0; i < entry.AddressList.Length; i++)
        // {
        //     print("ip地址:" + entry.AddressList[i]);
        // }
        // for(int i = 0; i < entry.Aliases.Length; i++)
        // {
        //     print("主机别名:" + entry.Aliases[i]);
        // }
        // print("DNS服务器名称:" + entry.HostName);


        // 异步获取指定域名的IP信息
        GetHostEntry();



    }

    private async void GetHostEntry()
    {
        IPHostEntry entry = await Dns.GetHostEntryAsync("httpbin.org");
        for (int i = 0; i < entry.AddressList.Length; i++)
        {
            print("ip地址:" + entry.AddressList[i]);
        }
        for (int i = 0; i < entry.Aliases.Length; i++)
        {
            print("主机别名:" + entry.Aliases[i]);
        }
        print("DNS服务器名称:" + entry.HostName);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
