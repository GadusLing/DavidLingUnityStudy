using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipPanel : BasePanel<TipPanel>
{
    UILabel labInfo;
    UIButton btnSure;
    public override void Init()
    {
        labInfo = transform.Find("labInfo").GetComponent<UILabel>();
        btnSure = transform.Find("btnSure").GetComponent<UIButton>();
        // 这里试试用代码去找，在unity里面拖也是一样的

        btnSure.onClick.Add(new EventDelegate(() =>
        {
            HideMe();
        }));
        HideMe();//一开始就隐藏自己等别人激活
    }

    //外部callme拿去用的时候肯定需要自定义TIP的内容，搞个public的方法，外部call时传想要改的内容就行了
    public void changeInfo(string info)
    {
        labInfo.text = info;
    }
}

    
    

