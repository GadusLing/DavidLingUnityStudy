using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitPanel : BasePanel<QuitPanel>
{
    private UIButton btnSure;
    private UIButton btnClose;
    public override void Init()
    {
        btnSure = transform.Find("btnSure").GetComponent<UIButton>();
        btnClose = transform.Find("btnClose").GetComponent<UIButton>();
        btnSure.onClick.Add(new EventDelegate(() =>
        {
            SceneManager.LoadScene("BeginScene");
        }));    

        btnClose.onClick.Add(new EventDelegate(() =>
        {
            HideMe();
        }));
        HideMe();
    }

}
