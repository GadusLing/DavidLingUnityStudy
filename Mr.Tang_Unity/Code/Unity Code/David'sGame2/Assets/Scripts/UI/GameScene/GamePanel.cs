using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanel : BasePanel<GamePanel>
{
    private UIButton btnBack;
    public UILabel labTime;
    private List<GameObject> hpList = new List<GameObject>();
    public float currentTime = 0;
    public override void Init()
    {
        btnBack = transform.Find("btnBack").GetComponent<UIButton>();
        labTime = transform.Find("labTime").GetComponent<UILabel>();

        for (int i = 1; i <= 10; i++)
        {
            hpList.Add(transform.Find($"labHP/spr{i}/sprActive").gameObject);
        }

        btnBack.onClick.Add(new EventDelegate(() =>
        {
            QuitPanel.Instance.ShowMe();
        }));
    }

    public void ChangeHP(int hp)
    {
        for (int i = 0; i < hpList.Count; i++)
        {
            hpList[i].SetActive(i < hp);
        }
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        int hourse = (int)(currentTime / 3600);
        int minute = (int)(currentTime / 60);
        int second = (int)(currentTime % 60);
        labTime.text = $"{hourse:D2}h{minute:D2}m{second:D2}s";
    }
}
