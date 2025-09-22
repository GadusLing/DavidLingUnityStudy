using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialFolderTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print(Application.dataPath);
        //注意该方式获取到的路径一般情况下只在编辑模式下使用
        //我们不会在实际发布游戏后还使用该路径
        //游戏发布过后该路径就不存在了
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
