using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Main : MonoBehaviour
{
    void Start()
    {
        // 根据开始场景里选择的飞机，实例化对应的飞机
        RoleInfo roleInfo = GameDataMgr.Instance.GetCurrentRoleInfo(); // 获取当前选择的飞机信息
        GameObject obj = Instantiate(Resources.Load<GameObject>(roleInfo.resName)); // 实例化飞机
        obj.tag = "Player";
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;
        SetLayerRecursive(obj, LayerMask.NameToLayer("Default"));
        PlayerObject playerObj = obj.AddComponent<PlayerObject>(); // 给飞机添加PlayerObject脚本
        playerObj.speed = roleInfo.speed * 30; // 设置飞机速度，一格速度等于30
        //playerObj.MaxHP = roleInfo.HP; // 设置飞机血量
        //playerObj.CurrentHP = roleInfo.HP; // 设置当前血量
        
        // 更新血量显示
        GamePanel.Instance.ChangeHP(roleInfo.HP);
    }

    private void SetLayerRecursive(GameObject go, int layer)
    {
        go.layer = layer;
        foreach (Transform child in go.transform)
        {
            SetLayerRecursive(child.gameObject, layer);
        }
    }
}
