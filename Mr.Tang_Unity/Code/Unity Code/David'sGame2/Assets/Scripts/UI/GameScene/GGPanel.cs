using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GGPanel : BasePanel<GGPanel>
{
    private UILabel labTime;
    private UIInput inputName;
    private UIButton btnSure;

    private int endTime;
    public override void Init()
    {
        labTime = transform.Find("sprBK/labTime").GetComponent<UILabel>();
        inputName = transform.Find("sprBK/inputName").GetComponent<UIInput>();
        btnSure = transform.Find("btnSure").GetComponent<UIButton>();

        btnSure.onClick.Add(new EventDelegate(() =>
        {
            GameDataMgr.Instance.SaveRankData(inputName.value, endTime);
            SceneManager.LoadScene("BeginScene");
        }));
        HideMe();
    }

    public override void ShowMe()
    {
        base.ShowMe();
        endTime = (int)GamePanel.Instance.currentTime;
        labTime.text = GamePanel.Instance.labTime.text;
    }

}
