using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPanel : BasePanel<LoginPanel>
{
    // 账号密码输入框
    private UIInput inputUN;
    private UIInput inputPW;

    // 确定和注册按钮
    private UIButton btnSure;
    private UIButton btnRegister;

    // 记住密码和自动登录勾选框
    private UIToggle togPW;
    private UIToggle togAuto;
    public override void Init()
    {
        inputUN = transform.Find("inputUN").GetComponent<UIInput>();
        inputPW = transform.Find("inputPW").GetComponent<UIInput>();
        btnSure = transform.Find("btnSure").GetComponent<UIButton>();
        btnRegister = transform.Find("btnRegister").GetComponent<UIButton>();
        togPW = transform.Find("togPW").GetComponent<UIToggle>();
        togAuto = transform.Find("togAuto").GetComponent<UIToggle>();

        btnRegister.onClick.Add(new EventDelegate(() =>
        {
            //显示注册面板 隐藏自己
            RegisterPanel.Instance.ShowMe();
            HideMe();
        }));

        btnSure.onClick.Add(new EventDelegate(() =>
        {
            if (inputUN.value.Length == 0 || inputPW.value.Length == 0)
            {
                TipPanel.Instance.changeInfo("用户名或密码不能为空");
                TipPanel.Instance.ShowMe();
                return;
            }

            // 添加验证逻辑，比如用户名和密码格式等
            if(!LoginMgr.Instance.checkInfo(inputUN.value, inputPW.value))
            {
                TipPanel.Instance.changeInfo("登录失败，用户名或密码错误");
                TipPanel.Instance.ShowMe();
                return;
            }
            else
            {
                // 登陆成功后记录数据，C#中的类是引用类型，可以这样写，但是如果在C++里可能就有深浅拷贝的问题了
                LoginData data = LoginMgr.Instance.LoginData;
                data.username = inputUN.value;
                data.password = inputPW.value;
                data.rememberPassword = togPW.value;
                data.autoLogin = togAuto.value;
                LoginMgr.Instance.SaveLoginData();

                // 登录成功后可以进行下一步操作，比如进入服务器界面，隐藏自己
                if(LoginMgr.Instance.LoginData.frontServerID != 0)
                {
                    ServerPanel.Instance.ShowMe();

                }
                else
                {
                    ChooseServerPanel.Instance.ShowMe();
                }
                HideMe();
            }
        }));

        togPW.onChange.Add(new EventDelegate(() =>
        {
            //记录数据

            // 如果记住密码没选，自动登录也不能选
            if(!togPW.value) togAuto.value = false;
            
        }));

        togAuto.onChange.Add(new EventDelegate(() =>
        {
            //记录数据

            // 如果自动登录选了，记住密码也必须选
            if(togAuto.value) togPW.value = true;
            
        }));

        //获取登录数据，在loginmgr那边通过XmlDataMgr初次创建就已经加载了一个默认数据了
        LoginData data = LoginMgr.Instance.LoginData;

        //更新用户名 是否更新密码 更新两个tog

        togPW.value = data.rememberPassword;
        togAuto.value = data.autoLogin;

        //如果输入了用户名就记录用户名
        if(data.username != null)
            inputUN.value = data.username;
        //如果输入了密码并且勾选了记住密码就记录密码
        if(data.rememberPassword && data.password != null)
            inputPW.value = data.password;

        //如果勾选了自动登录就直接登录
        if(data.autoLogin)
        {
            EventDelegate.Execute(btnSure.onClick); 
        }
    }

    // 提供给外部使用快捷设置登录面板账号密码的方法
    public void SetInfo(string username, string password)
    {
        inputUN.value = username;
        inputPW.value = password;
    }

}
