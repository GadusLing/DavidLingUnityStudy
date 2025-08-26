using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mono : MonoBehaviour
{
    // 题目：
    // 1.请说出一个继承了MonoBehaviour的脚本中
    // this、this.gameObject、this.transform分别代表什么？
    // 答案：
    // this: 代表当前脚本实例本身（注意不是这个object，而是依附在object的这个脚本实例），即这个MonoBehaviour组件
    // this.gameObject: 代表挂载这个脚本的GameObject对象
    // this.transform: 代表挂载这个脚本的GameObject的Transform组件


    // 2.一个脚本A一个脚本B，他们都挂在一个GameObject上，实现在A中
    // 的Start函数中让B脚本实活，请用代码实活

    // 3.一个脚本A一个脚本B，脚本A挂在A对象上，脚本B挂在B对象上
    // 实现在B脚本的Start函数中让A对象上的A脚本失活

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}