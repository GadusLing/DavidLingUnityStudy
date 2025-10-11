using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterPanel : BasePanel<RegisterPanel>
{
    private UIInput inputUN;
    private UIInput inputPW;
    private UIButton btnSure;
    private UIButton btnCancel;
    public override void Init()
    {
        inputUN = transform.Find("inputUN").GetComponent<UIInput>();
        inputPW = transform.Find("inputPW").GetComponent<UIInput>();
        btnSure = transform.Find("btnSure").GetComponent<UIButton>();
        btnCancel = transform.Find("btnCancel").GetComponent<UIButton>();

        btnSure.onClick.Add(new EventDelegate(() =>
        {
            if (inputUN.value.Length == 0 || inputPW.value.Length == 0)
            {
                TipPanel.Instance.changeInfo("用户名或密码不能为空");
                TipPanel.Instance.ShowMe();
                return;
            }
            if(inputUN.value.Length < 7 || inputPW.value.Length < 7)
            {
                TipPanel.Instance.changeInfo("账号和密码长度必须大于6位");
                TipPanel.Instance.ShowMe();
                return;
            }
            // 添加注册逻辑，比如用户名和密码格式等，NGUI里直接通过Input的Validation完成了

            // 注册成功后可以进行下一步操作，比如进入登录界面等
            if (LoginMgr.Instance.RegisterUser(inputUN.value, inputPW.value))
            {
                TipPanel.Instance.changeInfo("注册成功");
                TipPanel.Instance.ShowMe();
                LoginPanel.Instance.SetInfo(inputUN.value, inputPW.value);
                LoginPanel.Instance.ShowMe();
                HideMe();
                return;
            }
            TipPanel.Instance.changeInfo("注册失败，用户名已存在");
            TipPanel.Instance.ShowMe();
        }));

        btnCancel.onClick.Add(new EventDelegate(() =>
        {
            //显示登录面板 隐藏自己
            LoginPanel.Instance.ShowMe();
            HideMe();
        }));
        HideMe();
    }

    public override void ShowMe()
    {
        //重写showme方法，每次显示时清空输入框，避免注册时输入错误或退出后再次打开面板仍然是错误或残留信息
        base.ShowMe();
        inputUN.value = "";
        inputPW.value = "";
    }
}
