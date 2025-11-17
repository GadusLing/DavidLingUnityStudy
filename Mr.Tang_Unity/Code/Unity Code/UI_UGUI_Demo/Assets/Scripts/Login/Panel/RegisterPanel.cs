using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class RegisterPanel : BasePanel
{
    /// <summary>
    /// 取消按钮
    /// </summary>
    public Button btnCancel;
    /// <summary>
    /// 确认按钮
    /// </summary>
    public Button btnConfirm;
    /// <summary>
    /// 用户名输入框
    /// </summary>
    public TMP_InputField inputUN;
    /// <summary>
    /// 密码输入框
    /// </summary>
    public TMP_InputField inputPW;
    public override void Init()
    {
        btnCancel.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<LoginPanel>();
            UIManager.Instance.HidePanel<RegisterPanel>();
        });
        btnConfirm.onClick.AddListener(() =>
        {
            // 用户名不能小于6位，密码不能小于8位
            if(inputUN.text.Length < 6)
            {
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("用户名不能小于6位！");
                return;
            }
            if(inputPW.text.Length < 8)
            {
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("密码不能小于8位！");
                return;
            }
            if(LoginMgr.Instance.RegisterUser(inputUN.text, inputPW.text))
            {
                // 注册成功后 清除登录数据 新账号的数据需要重置 不然会残留上次的登录数据
                LoginMgr.Instance.ClearLoginData();

                LoginPanel loginPanel = UIManager.Instance.ShowPanel<LoginPanel>();
                // 注册完成回到login界面，自动填充用户名和密码，体验更好
                loginPanel.SetInfo(inputUN.text, inputPW.text);

                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("注册成功！");
                UIManager.Instance.HidePanel<RegisterPanel>();
            }
            else
            {
                //UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("注册失败，用户名已存在！"); //这个已经在LoginMgr里提示过了
                //之后如果发现顺序有问题再修改
                // 清空输入框，方便用户重新输入
                inputUN.text = "";
                inputPW.text = "";
            }
        });
    }
}
