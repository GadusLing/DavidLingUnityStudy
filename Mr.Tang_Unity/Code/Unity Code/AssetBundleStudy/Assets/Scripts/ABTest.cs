using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // // 加载AB包
        // AssetBundle ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/model");
        // // 加载AB包内的资源
        // //GameObject prefab = ab.LoadAsset<GameObject>("Cube");
        // GameObject prefab = ab.LoadAsset("Cube", typeof(GameObject)) as GameObject;// 要熟悉用typeof(GameObject)的写法 因为之后在Lua中不支持泛型 只能用typeof
        // // 实例化预制体
        // Instantiate(prefab);
        // GameObject spherePrefab = ab.LoadAsset<GameObject>("Sphere");
        // Instantiate(spherePrefab);
        // 卸载AB包
        // ab.Unload(false);// false表示只卸载AB包本身，保留已实例化的对象；true表示同时卸载已实例化的对象

        // // AB包不能重复加载 不然会报错
        // //AssetBundle ab2 = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/model");

        // 异步加载
        StartCoroutine(LoadABRes("model", "Cube"));// StartCoroutine 启动一个协程 协程允许你在多个帧之间分步执行代码，实现“等待”或“异步”效果，而不会卡住主线程

    }

    IEnumerator LoadABRes(string ABName, string resName) // IEnumerator 协程方法的返回类型，表示“可枚举对象” 
    {
        // yield return 协程的“暂停点”，告诉 Unity 这里要“等待”某个条件或下一帧再继续执行
        // yield return null; 等待一帧。
        // yield return new WaitForSeconds(1); 等待1秒。
        // yield return 某个异步操作; 等待异步操作完成（如 AssetBundleCreateRequest、AssetBundleRequest 等）。
        // 在你的代码里：
        // yield return abcr; 等待 AB 包异步加载完成。
        // yield return abq; 等待资源异步加载完成。
        // 异步加载AB包
        AssetBundleCreateRequest abcr = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/" + ABName);
        yield return abcr;// 等待加载完成
        // 加载AB包内的资源
        AssetBundleRequest abq = abcr.assetBundle.LoadAssetAsync(resName, typeof(GameObject));
        yield return abq;
        // 使用资源
        GameObject prefab = abq.asset as GameObject;
        Instantiate(prefab);
        
        // 卸载全部AB包
        //AssetBundle.UnloadAllAssetBundles(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
