using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("欢迎使用本UI框架！");
        UIManager.Instance.ShowPanel<LoginBKPanel>();
        UIManager.Instance.ShowPanel<LoginPanel>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
