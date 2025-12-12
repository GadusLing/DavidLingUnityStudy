using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestTest
{
    public TestTest()
    {
        MonoManager.Instance.StartCoroutine(TestCoroutine());
    }
    IEnumerator TestCoroutine()
    {
        Debug.Log("TestTest Coroutine");
        yield return new WaitForSeconds(1f);
    }
    public void Update()
    {
        Debug.Log("TestTest Update");
    }
}


public class Test : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        TestTest testTest = new TestTest();
        MonoManager.Instance.AddUpdateListener(testTest.Update);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            PoolManager.Instance.GetObjFromPool("Test/Cube", (obj) =>
            {
                obj.transform.localScale = Vector3.one * 2f;
            });
            // GameObject obj = ResManager.Instance.LoadRes<GameObject>("Test/Cube");
            // obj.transform.localScale = Vector3.one * 2f;
        }
        if(Input.GetMouseButtonDown(1))
        {
            PoolManager.Instance.GetObjFromPool("Test/Sphere", (obj) =>
            {
                
            });
            // ResManager.Instance.LoadResAsync<GameObject>("Test/Sphere", (obj) =>
            // {
            //     obj.transform.localScale = Vector3.one * 2f;
            // });
        }
    }
}
