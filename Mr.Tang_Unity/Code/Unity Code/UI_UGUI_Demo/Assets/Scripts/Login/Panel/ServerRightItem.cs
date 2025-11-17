using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.U2D;

namespace Login
{
    public class ServerRightItem : MonoBehaviour
    {
        [Header("UI组件")]
        /// <summary>
        /// 按钮本身
        /// </summary>
        public Button btnSelf;
        /// <summary>
        /// 是否是新服
        /// </summary>
        public Image imgNew;
        /// <summary>
        /// 状态图
        /// </summary>
        public Image imgState;
        /// <summary>
        /// 名字
        /// </summary>
        public TMP_Text txtName;
        
        [Header("数据")]
        /// <summary>
        /// 当前按钮 代表哪个服务器之后会使用其中的数据
        /// </summary>
        public ServerInfo nowServerInfo;
        
        void Start()
        {
            btnSelf.onClick.AddListener(() => {
                // 记录当前选择的服务器ID
                LoginMgr.Instance.LoginData.frontServerId = nowServerInfo.id;

                // 显示服务器面板
                UIManager.Instance.ShowPanel<ServerPanel>();
                //隐藏当前面板
                UIManager.Instance.HidePanel<ChooseServerPanel>();
            });
        }

        /// <summary>
        /// 提供给外部初始化右侧按钮数据的方法
        /// </summary>
        /// <param name="info">每个右侧按钮上的服务器信息</param>
        public void InitInfo(ServerInfo info)
        {
            nowServerInfo = info;
            txtName.text = info.id + "区 " + info.name;
            imgNew.gameObject.SetActive(info.isNew);
            // 加载图集
            SpriteAtlas sa = Resources.Load<SpriteAtlas>("Login");
            switch (info.state)//0：默认不显示  1：流畅  2：繁忙  3：火爆  4：维护
            {
                case 0:
                    imgState.gameObject.SetActive(false);
                    break;
                case 1:
                    imgState.sprite = sa.GetSprite("ui_DL_liuchang_01");
                    break;
                case 2:
                    imgState.sprite = sa.GetSprite("ui_DL_fanhua_01");
                    break;
                case 3:
                    imgState.sprite = sa.GetSprite("ui_DL_huobao_01");
                    break;
                case 4:
                    imgState.sprite = sa.GetSprite("ui_DL_weihu_01");
                    break;
            }
        }
        
    }
}
