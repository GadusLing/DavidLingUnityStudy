using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 网络资源加载管理器，支持通过 WWW 协议异步加载多种类型资源（如图片、音频、AssetBundle等），单例常驻场景，所有异步加载都走这里，方便统一管理和调试
public class NetWWWMgr : MonoBehaviour
{

    private static NetWWWMgr _instance; // 单例实例，保证全局唯一
    public static NetWWWMgr Instance => _instance; // 外部访问入口
    private string HTTP_SERVER_PATH = "http://192.168.1.2:8081/Http_Server/"; // 服务器地址

    void Awake()
    {
        if (_instance != null) // 场景里已有实例，说明重复挂载，直接销毁自己，避免多例
        {
            Destroy(gameObject);
            return;
        }
        _instance = this; // 首次初始化，赋值单例
        DontDestroyOnLoad(gameObject); // 场景切换不销毁，保证全局可用
    }

    // 外部统一入口：加载网络或本地资源，支持泛型回调，调用方只关心类型和回调，不关心底层实现
    public void LoadRes<T>(string path, UnityAction<T> callback) where T : class
    {
        StartCoroutine(LoadResCoroutine(path, callback)); // 启动协程，异步加载
    }

    // 资源加载协程，核心流程：先用 WWW 拉取数据，拉完后根据目标类型做转换，最后回调给调用者
    // 这里的泛型 T 决定了后续怎么处理数据，比如 Texture2D、AudioClip、AssetBundle 都能复用同一套流程
    private IEnumerator LoadResCoroutine<T>(string path, UnityAction<T> callback) where T : class
    {
        WWW www = new WWW(path); // 创建 WWW 请求，支持 http/file/ftp 等协议
        yield return www; // 等待加载完成，协程挂起直到数据返回

        if (www.error == null) // 加载成功，下面要做类型转换
        {
            T result = ConvertResult<T>(www); // 根据 T 类型把数据转成目标对象
            if (result != null)
            {
                callback?.Invoke(result); // 回调给外部，外部直接拿到想要的类型
            }
            else
            {
                Debug.LogError($"不支持的资源类型：{typeof(T)}"); // 泛型类型没覆盖到，提醒开发者
            }
        }
        else
        {
            Debug.LogError($"加载资源失败：{www.error}"); // 网络/路径/权限等问题，直接报错
        }
    }

    // 类型转换工具方法，专门服务于上面的加载流程：
    // 目的是把 WWW 返回的数据转成调用方真正想要的类型（比如 Texture2D、Sprite、AssetBundle 等），这样外部用起来不用关心底层细节
    // 如果遇到没覆盖的类型，直接返回 null 并报错，方便后续扩展
    private T ConvertResult<T>(WWW www) where T : class
    {
        if (typeof(T) == typeof(AssetBundle)) return www.assetBundle as T; // 直接取 assetBundle
        if (typeof(T) == typeof(Texture) || typeof(T) == typeof(Texture2D)) return www.texture as T; // 直接取 texture
        if (typeof(T) == typeof(Sprite)) // Sprite 需要手动从 Texture2D 创建
        {
            Texture2D texture = www.texture;
            if (texture == null) { Debug.LogError("加载 Sprite 失败：纹理为空"); return null; }
            Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite as T;
        }
        if (typeof(T) == typeof(AudioClip)) return www.GetAudioClip() as T; // 音频
        if (typeof(T) == typeof(TextAsset)) return new TextAsset(www.text) as T; // 文本
        if (typeof(T) == typeof(string)) return www.text as T; // 字符串
        if (typeof(T) == typeof(byte[])) return www.bytes as T; // 二进制
        return null; // 没有覆盖到的类型，直接返回 null
    }

    public void SendMsg<T>(BaseMsg msg, UnityAction<T> callback) where T : BaseMsg
    {
        StartCoroutine(SendMsgAsync(msg, callback));
    }

    /// <summary>
    /// 发送消息的协程，使用 WWWForm 来封装消息数据，发送
    /// 这里的设计是为了演示如何通过 WWW 来发送数据到服务器，实际项目中可能会有更复杂的协议和数据格式 不要用这个来做正式的网络通信，这只是一个教学示例，展示如何把消息对象序列化成字节数组，通过 WWW 发送，并模拟服务器响应的处理流程
    /// </summary>
    /// <typeparam name="T">期望从服务器返回的类型，协程会根据这个类型来处理响应数据</typeparam>
    /// <param name="msg">要发送的消息对象，必须继承自 BaseMsg，并实现 Writing 方法来序列化成字节数组</param>
    /// <param name="callback">消息发送完成后的回调，参数类型为 T，调用者可以根据这个类型来处理服务器的响应</param>
    private IEnumerator SendMsgAsync<T>(BaseMsg msg, UnityAction<T> callback) where T : BaseMsg
    {
        WWWForm data = new WWWForm();
        data.AddBinaryData("Msg", msg.Writing());
        WWW www = new WWW(HTTP_SERVER_PATH, data); // 下面是另一种写法，直接发个字节数组，都行，关键是要和后端约定好格式
        //WWW www = new WWW(HTTP_SERVER_PATH, msg.Writing());

        yield return www; 

        // 由于没有真实的web服务器和协议，这里模拟，就认为后端发回来的内容也是一个继承自BaseMsg的消息对象的字节数组
        if (www.error == null)
        {
            // 先解析 消息类型ID 和 消息长度（这里假设协议是前4字节是消息ID，后4字节是消息长度，剩下的才是消息内容）
            int index = 0;
            int msgID = BitConverter.ToInt32(www.bytes, index);
            index += 4;
            int msgLength = BitConverter.ToInt32(www.bytes, index);
            index += 4;
            //反序列化BaseMsg
            BaseMsg baseMsg = null;
            switch (msgID)
            {
                case 1001: // 假设有个测试消息类型
                    baseMsg = new PlayerMsg(); // 创建对应的消息对象
                    baseMsg.Reading(www.bytes, index); // 从字节数组中读取数据，填充到消息对象里
                    break;
                default:
                    Debug.LogError($"未知的消息类型ID：{msgID}");
                    break;
            }
            if(baseMsg != null)
            {
                callback?.Invoke(baseMsg as T); // 回调给调用者，外部直接拿到想要的类型
            }
            else
            {
                Debug.LogError($"消息解析失败，无法转换成目标类型：{typeof(T)}");
            }
        }
        else
        {
            Debug.LogError($"发送消息失败：{www.error}");
        }

    }
}
