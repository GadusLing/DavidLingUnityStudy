using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatherAndSon : MonoBehaviour
{
    //请为Transform写一个拓展方法，可以将它的子对象按名字的长短进行排序改变他们的顺序，
    //名字短的在前面，名字长的在后面

    //请为Transform写一个拓展方法，传入一个名字查找子对象，即使是子对象的子对象也能查找到

    // Start is called before the first frame update
    void Start()
    {
        transform.SortChildrenByName();

        Transform target = transform.DepthFindChildren("children2");
        if (target != null)
        {
            print("找到子物体：" + target.name);
        }
        else
        {
            print("没有找到指定名字的子物体");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
