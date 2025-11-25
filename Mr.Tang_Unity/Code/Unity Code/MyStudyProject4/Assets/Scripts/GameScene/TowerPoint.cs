using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPoint : MonoBehaviour
{   
    /// <summary>
    /// 造塔点关联的炮塔对象
    /// </summary>
    private GameObject towerObj = null;
    /// <summary>
    /// 造塔点关联的炮塔信息
    /// </summary>
    public TowerInfo curTowerInfo = null;
    /// <summary>
    /// 提供给外部配置 该造塔点能够造的三座不同类型初始塔的ID
    /// </summary>
    public List<int> chooseIDList;

    /// <summary>
    /// 外部调用 在该造塔点造塔
    /// </summary>
    /// <param name="id"></param>
    public void CreatTower(int id)
    {
        TowerInfo info = GameDataMgr.Instance.towerInfoList[id - 1];
        // 先判断金币 如果金币不够就直接返回
        if (GameLevelMgr.Instance.player.Gold < info.cost)
            return;
        // 扣除金币
        GameLevelMgr.Instance.player.AddGold(-info.cost);
        // 实例化塔 先判断之前有没有塔 有的话先销毁掉
        if(towerObj != null)
        {
            Destroy(towerObj);
            towerObj = null;
        }
        // 实例化塔
        towerObj = Instantiate(Resources.Load<GameObject>(info.prefabRes), transform.position, Quaternion.identity);
        // 初始化塔
        towerObj.GetComponent<TowerObject>().InitInfo(info);
        // 记录当前塔信息
        curTowerInfo = info;
        // 造塔成功后 更新UI界面内容
        if(curTowerInfo.nextLevelId != 0)
        {
            UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);
        }
        else
        {
            // 如果现在已经有塔并且是最高级了 就不显示造塔按钮面板了
            UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 如果现在已经有塔并且是最高级了 就不显示造塔按钮面板了
        if( curTowerInfo != null && curTowerInfo.nextLevelId == 0)
            return;
        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);
    }

    private void OnTriggerExit(Collider other)
    {
        // 如果希望玩家离开造塔点时 造塔按钮面板消失 就传null过去
        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);
    }
}
