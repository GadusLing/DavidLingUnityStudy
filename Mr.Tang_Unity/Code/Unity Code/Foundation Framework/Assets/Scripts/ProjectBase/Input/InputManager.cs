using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 输入管理类 一个中间层 主要负责输入后事件的转发
/// </summary>
public class InputManager : SingletonBase<InputManager>
{
    private bool isCheckingInput = false;

    // 键位映射表，逻辑动作 -> 按键
    private Dictionary<string, KeyCode> keyMapping = new Dictionary<string, KeyCode>
    {
        { "MoveForward", KeyCode.W },
        { "MoveBackward", KeyCode.S },
        { "MoveLeft", KeyCode.A },
        { "MoveRight", KeyCode.D }
    };

    public InputManager()
    {
        MonoManager.Instance.AddUpdateListener(OnUpdate);
    }

    /// <summary>
    /// 开启或关闭输入检测（支持链式调用）
    /// </summary>
    /// <param name="enable">是否开启输入检测</param>
    /// <returns>返回 InputManager 实例（链式调用）</returns>
    public InputManager SetInputChecking(bool enable)
    {
        isCheckingInput = enable;
        return enable ? this : null; // 如果 enable 为 false，返回 null
    }

    /// <summary>
    /// 修改键位绑定
    /// </summary>
    /// <param name="action">逻辑动作名称</param>
    /// <param name="newKey">新的按键</param>
    /// <returns>返回 InputManager 实例（链式调用）</returns>
    public InputManager RebindKey(string action, KeyCode newKey)
    {
        if (keyMapping.ContainsKey(action))
        {
            keyMapping[action] = newKey;
        }
        else
        {
            Debug.LogWarning($"动作 {action} 不存在于键位映射表中！");
        }
        return this; // 始终返回自身，支持链式调用
    }

    /// <summary>
    /// 检测指定动作的按键状态并分发事件
    /// </summary>
    /// <param name="action">逻辑动作名称</param>
    private void CheckKey(string action)
    {
        if (!keyMapping.ContainsKey(action)) return;

        KeyCode key = keyMapping[action];

        if (Input.GetKeyDown(key))
        {
            EventCenter.Instance.EventTrigger("KeyDown", key);
        }
        if (Input.GetKeyUp(key))
        {
            EventCenter.Instance.EventTrigger("KeyUp", key);
        }
    }

    private void OnUpdate()
    {
        if (!isCheckingInput)
            return;

        CheckKey("MoveForward");
        CheckKey("MoveBackward");
        CheckKey("MoveLeft");
        CheckKey("MoveRight");
    }
}
