using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NetWWWMgr : MonoBehaviour
{
    private static NetWWWMgr _instance;
    public static NetWWWMgr Instance => _instance;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadRes<T>(string path, UnityAction<T> callback) where T : class
    {
        StartCoroutine(LoadResCoroutine(path, callback));
    }

    private IEnumerator LoadResCoroutine<T>(string path, UnityAction<T> callback) where T : class
    {
        WWW www = new WWW(path);
        yield return www;

        if (www.error == null)
        {
            T result = ConvertResult<T>(www);
            if (result != null)
            {
                callback?.Invoke(result);
            }
            else
            {
                Debug.LogError($"不支持的资源类型：{typeof(T)}");
            }
        }
        else
        {
            Debug.LogError($"加载资源失败：{www.error}");
        }
    }

    private T ConvertResult<T>(WWW www) where T : class
    {
        if (typeof(T) == typeof(AssetBundle))
        {
            return www.assetBundle as T;
        }

        if (typeof(T) == typeof(Texture) || typeof(T) == typeof(Texture2D))
        {
            return www.texture as T;
        }

        if (typeof(T) == typeof(Sprite))
        {
            Texture2D texture = www.texture;
            if (texture == null)
            {
                Debug.LogError("加载 Sprite 失败：纹理为空");
                return null;
            }

            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
            return sprite as T;
        }

        if (typeof(T) == typeof(AudioClip))
        {
            return www.GetAudioClip() as T;
        }

        if (typeof(T) == typeof(TextAsset))
        {
            return new TextAsset(www.text) as T;
        }

        if (typeof(T) == typeof(string))
        {
            return www.text as T;
        }

        if (typeof(T) == typeof(byte[]))
        {
            return www.bytes as T;
        }

        return null;
    }




}
