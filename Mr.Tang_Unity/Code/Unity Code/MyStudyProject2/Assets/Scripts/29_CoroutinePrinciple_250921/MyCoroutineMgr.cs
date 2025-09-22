using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YieldReturnTime
{
    // 这个类用来表示等待时间的对象

    //记录下次还要执行的迭代器接口
    public IEnumerator ie;
    //记录下次执行的时间点
    public float time;
}

public class MyCoroutineMgr : MonoBehaviour
{
    private static MyCoroutineMgr _instance;
    public static MyCoroutineMgr Instance => _instance;

    private List<YieldReturnTime> Ylist = new List<YieldReturnTime>();
        
    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        
    }


    void Update()
    {
        for (int i = Ylist.Count - 1; i >= 0; --i)
        {
            if(Time.time >= Ylist[i].time)
            {
                //到了执行的时间点
                if(Ylist[i].ie.MoveNext())
                {
                    //协程没有结束
                    if(Ylist[i].ie.Current is int)
                    {
                        //继续等待
                        Ylist[i].time = Time.time + (int)Ylist[i].ie.Current;
                    }
                    else
                    {
                        //协程函数中yield return后面不是int类型，直接结束这个协程
                        Ylist.RemoveAt(i);
                    }
                }
                else
                    Ylist.RemoveAt(i);
            }
        }
    }

    public void MyStartCoroutine(IEnumerator ie)
    {
        if(ie.MoveNext())
        {
            // 当前协程没有结束
            // ie.Current就是yield return后面的值
            if(ie.Current is int)
            {
                YieldReturnTime yrt = new YieldReturnTime();
                yrt.ie = ie;
                yrt.time = Time.time + (int)ie.Current;
                //把记录的信息记录到数据容器当中
                //因为可能有多个协程函数开启所以用一个List来存储
                Ylist.Add(yrt);
            }
        }
    }
}
