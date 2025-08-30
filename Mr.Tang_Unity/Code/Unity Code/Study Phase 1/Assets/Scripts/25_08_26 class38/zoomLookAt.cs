using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoomLookAt : MonoBehaviour
{
    // 使用之前的坦克预设体，让摄像机可以跟随其移动，并且一直看向坦克
    public GameObject Tank;
    private GameObject newTank;
    private Camera followCamera;

    // Start is called before the first frame update
    void Start()
    {
        // 实例化坦克
        newTank = Instantiate(Tank, new Vector3(0, 0, 0), Quaternion.identity);

        // 创建一个新的摄像机对象
        GameObject camObj = new GameObject("FollowCamera");
        followCamera = camObj.AddComponent<Camera>();

        // 设置摄像机为坦克的子对象
        camObj.transform.SetParent(newTank.transform);

        // 设置摄像机初始位置（可根据需要调整相对位置）
        camObj.transform.localPosition = new Vector3(0, 2, -6);
        camObj.transform.localRotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        // 让摄像机始终看向坦克
        if (followCamera != null && newTank != null)
        {
            followCamera.transform.LookAt(newTank.transform);
        }
    }
}
