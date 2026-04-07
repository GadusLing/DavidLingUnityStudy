using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ABUpdateMgr.Instance.CheckUpdate((isOver) =>
        {
            if(isOver)
            {
                print("检测更新结束,隐藏进度条");
            }
            else
            {
                print("网络出错，请检测网络或者重启游戏");
            }
        }, (str) =>
        {
            //以后可以在这里处理更新加载界面上的显示信息相关的逻辑
            print(str);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
