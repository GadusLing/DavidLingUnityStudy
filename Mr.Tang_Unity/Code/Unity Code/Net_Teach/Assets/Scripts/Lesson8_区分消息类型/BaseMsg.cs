using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMsg : BaseData
{
    public virtual int GetID() // 在数据流头部添加一个int标识符ID，来区分不同的消息类型
    {
        return 0;
    }

}
