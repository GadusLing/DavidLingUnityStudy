using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LoadingPanel : BasePanel
{
    public Slider progressBar;
    public TMP_Text txtPrompt; // 提示 "点击屏幕继续"

    public override void Init()
    {
        // 可在预制体上绑定进度条，无需额外初始化
        // 可在预制体上绑定进度条与提示文本，无需额外初始化
        if (txtPrompt != null)
            txtPrompt.gameObject.SetActive(false);
    }

    public void StartLoading(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // 等待异步加载达到 0.9（资源已就绪）
        while (asyncLoad.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            if (progressBar != null)
                progressBar.value = progress;
            yield return null;
        }

        // 到达 0.9（即加载完成），把进度条置满并提示玩家点击继续
        if (progressBar != null)
            progressBar.value = 1f;

        if (txtPrompt != null)
        {
            txtPrompt.gameObject.SetActive(true);
            txtPrompt.text = "点击屏幕继续";
        }

        // 等待玩家点击/触摸/任意按键确认
        while (!Input.GetMouseButtonDown(0) && Input.touchCount == 0 && !Input.anyKeyDown)
        {
            yield return null;
        }

        // 可选短延迟，保证点击动画或反馈自然
        yield return null;

        // 清理保留 UI，但注意保留当前 LoadingPanel（避免把提示隐藏）
        CleanupPersistentUIBeforeSceneActivate();

        // 允许激活新场景
        asyncLoad.allowSceneActivation = true;

        // 等待场景真正激活完成
        while (!asyncLoad.isDone)
            yield return null;

        // 激活后隐藏加载面板（由 UIManager 管理）
        UIManager.Instance.HidePanel<LoadingPanel>();
    }

    private void CleanupPersistentUIBeforeSceneActivate()
    {
        // 1. 清除 UIManager 中的已存面板并销毁对应 GameObject
        try
        {
            var dic = UIManager.Instance.panelDic;
            var keys = new System.Collections.Generic.List<string>(dic.Keys);
            foreach (var k in keys)
            {
                var panel = dic[k];
                if (panel != null)
                    GameObject.Destroy(panel.gameObject);
            }
            dic.Clear();
        }
        catch
        {
            // 忽略可能的异常（保险处理）
        }

        // 2. 找到名为 "Canvas" 的保留 Canvas 并隐藏（避免影响新场景）
        // GameObject oldCanvas = GameObject.Find("Canvas");
        // if (oldCanvas != null)
        // {
        //     oldCanvas.SetActive(false);
        //     // 如果确实想销毁保留 Canvas，改成 Destroy(oldCanvas);
        // }

        // 3. 保证场景中只剩一个 EventSystem，销毁多余的
        var systems = GameObject.FindObjectsOfType<EventSystem>();
        if (systems != null && systems.Length > 1)
        {
            // 保留第一个，销毁其余
            for (int i = 1; i < systems.Length; i++)
            {
                if (systems[i] != null)
                    GameObject.Destroy(systems[i].gameObject);
            }
        }
    }
}
