using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_Do_Type
{
    ChangeName,
    ActiveFlase,
    DelayDes,
    Des
}

public class A27_1 : MonoBehaviour
{
    public E_Do_Type type = E_Do_Type.ChangeName;

    public GameObject B;

    // Start is called before the first frame update
    void Start()
    {
        switch(type)
        {
            case E_Do_Type.ChangeName:
                B.name = "改名之后";
                break;
            case E_Do_Type.ActiveFlase:
                B.SetActive(false);
                break;
            case E_Do_Type.DelayDes:
                Destroy(B, 5);
                break;
            case E_Do_Type.Des:
                DestroyImmediate(B);
                break;
        }
        
    }

}
