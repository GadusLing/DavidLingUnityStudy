using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RadiographicInspection : MonoBehaviour
{
    void Start()
    {
        
    }
    

    void Update()
    {
        //Fire();
        RTSCtrl();
    }


    private RaycastHit hitInfo;
    public void Fire()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), 
                               out hitInfo, 
                               1000, 
                               LayerMask.GetMask("Monster")))
            {
                GameObject obj = Instantiate(Resources.Load<GameObject>("Effect/HitEff"));
                obj.transform.position = hitInfo.point + hitInfo.normal * 0.5f;
                obj.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
                Destroy(obj, 0.8f);

                obj = Instantiate(Resources.Load<GameObject>("Effect/BulletHole"));
                obj.transform.position = hitInfo.point + hitInfo.normal * 3f;
                obj.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
                //Destroy(obj, 3f);
            }
        }
    }

    public float OffsetY;
    private Transform CurSelObj;
    public void RTSCtrl()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), 
                               out hitInfo, 
                               1000, 
                               LayerMask.GetMask("Player")))
            {
                CurSelObj = hitInfo.transform;
                print(CurSelObj.name);
            }
        }

        if(Input.GetMouseButton(0) && CurSelObj != null)
        {
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), 
                               out hitInfo, 
                               1000, 
                               LayerMask.GetMask("Ground")))
            {
                CurSelObj.position = Vector3.Lerp(CurSelObj.position, hitInfo.point + Vector3.up * OffsetY, Time.deltaTime * 5f);
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            CurSelObj = null;
            print("Selection cleared");
        }

    }
    
}
