using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;
using XLua;
using XLuaTest;

[Hotfix]
public class HotfixTest
{
    public HotfixTest()
    {
        Debug.Log("HotfixTest Constructor");
    }

    public void Speak(string str)
    {
        Debug.Log("HotfixTest Speak: " + str);
    }

    ~HotfixTest()
    {
    }
}

[Hotfix]
public class HotfixTest2<T>
{
    public void Test(T str)
    {
        Debug.Log("HotfixTest2 Test: " + str);
    }
}


[Hotfix]
public class HotFixMain : MonoBehaviour
{
    HotfixTest hotTest;

    private int[] array = new int[] { 1, 2, 3 };

    // 属性
    public int Age
    {
        get
        {
            return 18;
        }
        set
        {
            Debug.Log(value);
        }
    }

    public int this[int index]
    {
        get
        {
            if (index < 0 || index >= array.Length)
            {
                Debug.LogError("索引超出范围");
                return -1;
            }
            return array[index];
        }
        set
        {
            if (index < 0 || index >= array.Length)
            {
                Debug.LogError("索引超出范围");
                return;
            }
            array[index] = value;
        }
    }

    event UnityAction myEvent;

    void Start()
    {
        XLuaManager.Instance.Init();
        XLuaManager.Instance.DoLuaFile("Main");
        Debug.Log(add(1, 2));
        Speak("Hello HotFix");

        hotTest = new HotfixTest();
        hotTest.Speak("Hello from HotfixTest");

        //StartCoroutine(TestCoroutine());

        this.Age = 25;
        Debug.Log(this.Age);

        this[0] = 100;
        Debug.Log(this[0]);

        myEvent += TestTest;
        myEvent -= TestTest;

        HotfixTest2<string> t1 = new HotfixTest2<string>();
        t1.Test("泛型类测试字符串");
        HotfixTest2<int> t2 = new HotfixTest2<int>();
        t2.Test(12345);

    }

    private void TestTest()
    {
        Debug.Log("TestTest 方法被调用");
    }

    void Update()
    {

    }

    IEnumerator TestCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("C#的协程每隔1秒打印一次");
        }
    }

    public int add(int a, int b)
    {
        return 0;
    }

    public static void Speak(string str)
    {
        Debug.Log("说话测试");
    }
}
