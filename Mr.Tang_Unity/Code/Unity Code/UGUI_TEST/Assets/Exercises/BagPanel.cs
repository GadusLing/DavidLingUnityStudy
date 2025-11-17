using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel : MonoBehaviour
{
    public static BagPanel panel;
    public ScrollRect svItems;
    public Button btnClose;

    void Awake()
    {
        panel = this;
    }
    void Start()
    {
        // 动态创建n个item
        int n = 68;
        for (int i = 0; i < n; i++)
        {
            GameObject item = Instantiate(Resources.Load<GameObject>("item"));
            item.transform.SetParent(svItems.content, false);
            // 设置item的位置,初始本地位置为20，-20，每行5个，item格子本身的大小是98-98，取100-100
            item.transform.localPosition = new Vector3(20 + (i % 5) * 100, -20 - (i / 5) * 100, 0);
            // 设置content的高
            svItems.content.sizeDelta = new Vector2(0, Mathf.CeilToInt(n / 5f) * 100 + 20);// 这里的20和顶部向下的偏移-20对应，保证最后一行item能完全显示
        }

        btnClose.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
