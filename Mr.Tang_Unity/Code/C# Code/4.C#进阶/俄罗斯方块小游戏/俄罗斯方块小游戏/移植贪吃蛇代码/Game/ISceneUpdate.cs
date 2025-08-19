using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 俄罗斯方块.Main
{   
    /// <summary>
    /// 场景更新接口 - 定义所有场景必须实现的更新方法
    /// 作用：统一场景行为规范，实现多态调用
    /// 设计模式：策略模式的基础接口
    /// </summary>
    internal interface ISceneUpdate
    {
        /// <summary>
        /// 场景更新方法 - 每帧调用一次
        /// 负责处理场景的渲染和输入逻辑
        /// </summary>
        void Update();
    }
}
