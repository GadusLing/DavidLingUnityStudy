using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

// 网络资源加载管理器，支持通过 WWW 协议和 UnityWebRequest 异步加载多种类型资源（如图片、音频、AssetBundle等），单例常驻场景，所有异步加载都走这里，方便统一管理和调试
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

    /// <summary>
    /// 使用 <see cref="UnityWebRequest"/> 异步加载网络或本地资源，并在完成后把结果转换为目标类型 <typeparamref name="T"/> 后通过回调返回。
    /// </summary>
    /// <typeparam name="T">目标类型，支持 Texture2D、Sprite、AssetBundle、AudioClip、TextAsset、string、byte[] 等常见类型，
    ///                     如果走下载到本地这条自定义规则，这里用 FileInfo 作为结果类型来描述最终落盘文件，后续也方便继续读取或校验</typeparam>
    /// <param name="path">网络资源 URL（例如 http://...）。</param>
    /// <param name="callback">加载并转换完成后的回调，参数为类型 <typeparamref name="T"/> 的实例或 null。</param>
    /// <param name="localPath">可选，如果要走本地落盘规则则传入保存路径；下载完成后会按 FileInfo 规则返回对应文件信息。</param>
    /// <param name="type">如果下载音效切片文件，则指定音频类型以便正确解析（默认MP3）。</param>
    public void UnityWebRequestLoad<T>(string path, UnityAction<T> callback, string localPath = "", AudioType type = AudioType.MPEG) where T : class
    {
        StartCoroutine(UnityWebRequestLoadAsync(path, callback, localPath, type)); // 启动协程，异步加载
    }

    private IEnumerator UnityWebRequestLoadAsync<T>(string path, UnityAction<T> callback, string localPath, AudioType type) where T : class
    {
        UnityWebRequest req = new UnityWebRequest(path, UnityWebRequest.kHttpVerbGET); // 手动创建 GET 请求，后面按目标类型挂不同的下载处理器

        if (typeof(T) == typeof(byte[]) || typeof(T) == typeof(string) || typeof(T) == typeof(TextAsset))
            req.downloadHandler = new DownloadHandlerBuffer(); // 文本和字节数据都先放进缓冲区，后面再按目标类型转换
        else if (typeof(T) == typeof(Texture) || typeof(T) == typeof(Texture2D) || typeof(T) == typeof(Sprite))
            req.downloadHandler = new DownloadHandlerTexture(true); // 图片类型直接用贴图处理器，省掉手动解码字节流的步骤
        else if (typeof(T) == typeof(AssetBundle))
            req.downloadHandler = new DownloadHandlerAssetBundle(req.url, 0); // AssetBundle 走专用处理器，保持和老师课堂里的写法一致
        else if (typeof(T) == typeof(FileInfo) && !string.IsNullOrEmpty(localPath))
            req.downloadHandler = new DownloadHandlerFile(localPath); // 这里对应原来课堂里“File 代表下载到本地”的规则，只是结果对象升级成了 FileInfo
        else if (typeof(T) == typeof(AudioClip))
            req = UnityWebRequestMultimedia.GetAudioClip(path, type); // 音频直接换成 Unity 官方封装好的多媒体请求，和老师演示的思路保持一致
        else
        {
            Debug.LogError($"UnityWebRequest 不支持的资源类型：{typeof(T)}");
            yield break;
        }

        yield return req.SendWebRequest(); // 发送请求，协程挂起直到数据返回

        if (req.result == UnityWebRequest.Result.Success) // 加载成功，下面把下载处理器里的内容转成目标对象
        {
            T result = ConvertResult<T>(req, localPath); // 根据 T 类型把数据转成目标对象
            if (result != null)
                callback?.Invoke(result); // 回调给外部，外部直接拿到想要的类型
            else
                Debug.LogError($"不支持的资源类型：{typeof(T)}"); // 泛型类型没覆盖到，提醒开发者
        }
        else
            Debug.LogError($"加载资源失败：{req.error}"); // 网络/路径/权限等问题，直接报错

        req.Dispose(); // 请求走完后及时释放底层句柄，避免长期堆积网络资源
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

    /// <summary>
    /// 类型转换工具方法，专门服务于上面的 UnityWebRequest 加载流程：
    /// 目的是把下载处理器中的结果转成调用方真正想要的类型，这样外部依然只关心泛型 T，不需要关心底层到底挂了哪个处理器。
    /// 这里整体沿用老师那套“前面按类型分流，后面按类型取结果”的思路，只是在文件下载分支上把课堂里的 File 占位规则升级成了 FileInfo 返回值。
    /// </summary>
    private T ConvertResult<T>(UnityWebRequest req, string localPath) where T : class
    {
        if (typeof(T) == typeof(byte[])) return req.downloadHandler.data as T; // Buffer 处理器下载下来的原始字节流，直接回给外部即可
        if (typeof(T) == typeof(string)) return req.downloadHandler.text as T; // 字符串本质上也是 Buffer 处理器里的文本结果
        if (typeof(T) == typeof(TextAsset)) return new TextAsset(req.downloadHandler.text) as T; // 文本资源继续包装成 TextAsset，保持和前面 WWW 版本的使用体验一致

        if (typeof(T) == typeof(Texture) || typeof(T) == typeof(Texture2D)) return DownloadHandlerTexture.GetContent(req) as T; // 纹理类型直接从贴图处理器里取结果
        if (typeof(T) == typeof(Sprite))
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(req); // Sprite 本质还是先拿到纹理，再在内存里切一张完整精灵出来
            if (texture == null) { Debug.LogError("加载 Sprite 失败：纹理为空"); return null; }
            Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite as T;
        }

        if (typeof(T) == typeof(AssetBundle)) return DownloadHandlerAssetBundle.GetContent(req) as T; // AssetBundle 走专用处理器，转换时也直接从专用接口取内容
        if (typeof(T) == typeof(FileInfo) && !string.IsNullOrEmpty(localPath)) return new FileInfo(localPath) as T; // 文件下载器已经把内容写到磁盘，这里返回 FileInfo，既保留原本“下载到本地”的规则，也让外部能继续拿到文件描述信息
        if (typeof(T) == typeof(AudioClip)) return DownloadHandlerAudioClip.GetContent(req) as T; // 音频请求已经换成多媒体请求，这里直接从音频处理器中取剪辑对象

        return null; // 上游没有覆盖到的类型，这里统一返回 null，让外层输出错误日志
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


    
    // 外部统一入口：上传本地文件到服务器，调用方只需传文件名、本地路径和回调，底层细节全封装，便于统一管理和调试
    public void UploadFile(string fileName, string localPath, UnityAction<UnityWebRequest.Result> callback)
    {
        StartCoroutine(UploadFileAsync(fileName, localPath, callback)); // 启动协程，异步上传
    }

    // 文件上传协程，核心流程：先把本地文件读成字节流，组装成表单，发起 UnityWebRequest.Post 请求，上传完成后回调结果
    // 用 IMultipartFormSection 组装数据，支持多种类型扩展，上传结果通过回调返回给调用者，便于后续处理（如 UI 提示、重试等）
    private IEnumerator UploadFileAsync(string fileName, string localPath, UnityAction<UnityWebRequest.Result> callback)
    {
        List<IMultipartFormSection> dataList = new List<IMultipartFormSection>(); // 构建表单数据列表，支持多文件和多字段
        dataList.Add(new MultipartFormFileSection(fileName, File.ReadAllBytes(localPath))); // 读取本地文件为字节流，打包成表单字段，字段名为 fileName
        UnityWebRequest req = UnityWebRequest.Post(HTTP_SERVER_PATH, dataList); // 创建 POST 请求，目标为服务器地址，附带表单数据

        yield return req.SendWebRequest(); // 异步发送请求，协程挂起直到上传完成或出错

        callback?.Invoke(req.result); // 上传结果回调给外部，外部可根据结果做 UI 提示或重试
        if (req.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("上传成功"); // 上传成功，输出日志
        }
        else
        {
            Debug.LogError($"上传失败：{req.error} {req.responseCode} {req.result}"); // 上传失败，输出详细错误信息，便于排查
        }
    }

    

}
