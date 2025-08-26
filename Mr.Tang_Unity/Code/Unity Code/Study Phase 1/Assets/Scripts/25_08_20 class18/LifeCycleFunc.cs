using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Unity生命周期函数题目：
 * 1. 请在纸上默写出数的各个生命周期函数
 *    并写出它们分别何时执行
 *    以及执行的先后顺序是什么
 * 
 * 2. 请用你的理解说出，生命周期函数并不是某类中的成员，为什么
 *    Unity可以自动的执行这些特殊的函数？
 */

public class LifeCycleFunc : MonoBehaviour
{
    // 1. Awake - 对象被创建时立即调用，在Start之前
    void Awake()
    {
        Debug.Log("1. Awake - 对象创建时调用");
    }

    // 2. OnEnable - 对象激活时调用，每次激活都会调用
    void OnEnable()
    {
        Debug.Log("2. OnEnable - 对象激活时调用");
    }

    // 3. Start - 第一帧更新之前调用，在Awake之后
    void Start()
    {
        Debug.Log("3. Start - 第一帧更新前调用");
    }

    // 4. FixedUpdate - 固定时间间隔调用，用于物理更新
    void FixedUpdate()
    {
        Debug.Log("4. FixedUpdate - 固定时间间隔调用");
    }

    // 5. Update - 每帧调用一次
    void Update()
    {
        Debug.Log("5. Update - 每帧调用");
    }

    // 6. LateUpdate - 在所有Update调用完成后调用
    void LateUpdate()
    {
        Debug.Log("6. LateUpdate - 所有Update后调用");
    }

    // 7. OnDisable - 对象失活时调用
    void OnDisable()
    {
        Debug.Log("7. OnDisable - 对象失活时调用");
    }

    // 8. OnDestroy - 对象被销毁时调用
    void OnDestroy()
    {
        Debug.Log("8. OnDestroy - 对象销毁时调用");
    }

    /*
     * 完整执行顺序：
     * Awake → OnEnable → Start → FixedUpdate/Update/LateUpdate(循环) → OnDisable → OnDestroy
     * 
     * 特别说明：
     * - OnEnable：每次GameObject或Component被激活时调用
     * - OnDisable：每次GameObject或Component被失活时调用
     * - 如果对象被多次激活/失活，OnEnable和OnDisable会被多次调用
     * - Start只在对象生命周期中调用一次
     * 
     * 为什么Unity能自动执行这些函数？
     * 答：Unity使用反射机制，在运行时检查继承自MonoBehaviour的类中
     * 是否包含这些特定名称的方法，如果存在就自动调用。
     * 这些方法不需要override关键字，Unity通过方法名识别并调用。
     */
}