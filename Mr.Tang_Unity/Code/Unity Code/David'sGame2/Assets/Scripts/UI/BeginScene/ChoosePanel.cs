using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoosePanel : BasePanel<ChoosePanel>
{
    private UIButton btnClose;
    private UIButton btnLeft;
    private UIButton btnRight;
    private UIButton btnStart;
    private Transform SpacecraftPos;
    private List<GameObject> labHP;
    private List<GameObject> labSpeed;
    private List<GameObject> labVolume;
    private GameObject currentModel; // 当前选择的model索引

    public override void Init()
    {
        btnClose = transform.Find("btnClose").GetComponent<UIButton>();
        btnLeft = transform.Find("btnLeft").GetComponent<UIButton>();
        btnRight = transform.Find("btnRight").GetComponent<UIButton>();
        btnStart = transform.Find("btnStart").GetComponent<UIButton>();
        SpacecraftPos = transform.Find("SpacecraftPos");
        labHP = new List<GameObject>();
        labSpeed = new List<GameObject>();
        labVolume = new List<GameObject>();
        for (int i = 1; i <= 10; i++)
        {
            labHP.Add(transform.Find($"sprBK/labHP/spr{i}/sprActive").gameObject);
            labSpeed.Add(transform.Find($"sprBK/labSpeed/spr{i}/sprActive").gameObject);
            labVolume.Add(transform.Find($"sprBK/labVolume/spr{i}/sprActive").gameObject);
        }

        btnClose.onClick.Add(new EventDelegate(() =>
        {
            HideMe();
            BeginPanel.Instance.ShowMe();
        }));

        btnLeft.onClick.Add(new EventDelegate(() =>
        {
            --GameDataMgr.Instance.currentRoleIndex;
            if (GameDataMgr.Instance.currentRoleIndex < 0)
            {
                GameDataMgr.Instance.currentRoleIndex = GameDataMgr.Instance.roleData.roleList.Count - 1;
            }
            ChangeCurrentModel();
        }));

        btnRight.onClick.Add(new EventDelegate(() =>
        {
            ++GameDataMgr.Instance.currentRoleIndex;
            if (GameDataMgr.Instance.currentRoleIndex >= GameDataMgr.Instance.roleData.roleList.Count)
            {
                GameDataMgr.Instance.currentRoleIndex = 0;
            }
            ChangeCurrentModel();
        }));

        btnStart.onClick.Add(new EventDelegate(() =>
        {
            // 开始游戏
            SceneManager.LoadScene("GameScene");
        }));
        HideMe();
    }

    public override void ShowMe()
    {
        base.ShowMe();
        GameDataMgr.Instance.currentRoleIndex = 0;
        ChangeCurrentModel();
    }

    public override void HideMe()
    {
        DestroyObj();
        base.HideMe();
    }

    private void ChangeCurrentModel()
    {
        DestroyObj();
        RoleInfo roleInfo = GameDataMgr.Instance.GetCurrentRoleInfo();
        currentModel = Instantiate(Resources.Load<GameObject>(roleInfo.resName), SpacecraftPos);
        //currentModel.transform.localPosition = Vector3.zero;
        //currentModel.transform.localRotation = Quaternion.identity;
        currentModel.transform.localScale = Vector3.one * roleInfo.scale;

        // 修改Model显示层级为UI层
        currentModel.layer = LayerMask.NameToLayer("UI");
        

        // 更新属性显示
        for(int i = 0; i < labHP.Count; i++)
        {
            labHP[i].SetActive(i < roleInfo.HP);
            labSpeed[i].SetActive(i < roleInfo.speed);
            labVolume[i].SetActive(i < roleInfo.volume);
        }
    }

    private void DestroyObj()
    {
        if (currentModel != null)
        {
            Destroy(currentModel);
            currentModel = null;
        }
    }

    private bool isSelecting = false;
    void Update()
    {
        SpacecraftPos.Translate(Vector3.up * Mathf.Sin(Time.time * 3) * 0.00008f, Space.World);

        if(Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), 1000, LayerMask.GetMask("UI")))
            {
                isSelecting = true;
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            isSelecting = false;
        }
        if(Input.GetMouseButton(0) && isSelecting)
        {
            currentModel.transform.rotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse X") *-20,Vector3.up);
        }
    }
}
