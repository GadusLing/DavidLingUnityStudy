using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class InputTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InputManager.Instance.SetInputChecking(true).RebindKey("MoveForward", KeyCode.UpArrow);
        EventCenter.Instance.AddEventListener("KeyDown", (key) =>
        {
            KeyCode k = (KeyCode)key;
            switch (k)
            {
                case KeyCode.UpArrow:
                    UnityEngine.Debug.Log("按下了W键 前进");
                    break;
                case KeyCode.S:
                    UnityEngine.Debug.Log("按下了S键 后退");
                    break;
                case KeyCode.A:
                    UnityEngine.Debug.Log("按下了A键 左移");
                    break;
                case KeyCode.D:
                    UnityEngine.Debug.Log("按下了D键 右移");
                    break;
            }
        });
        EventCenter.Instance.AddEventListener("KeyUp", (key) =>
        {
            KeyCode k = (KeyCode)key;
            switch (k)
            {
                case KeyCode.UpArrow:
                    UnityEngine.Debug.Log("抬起了W键 停止前进");
                    break;
                case KeyCode.S:
                    UnityEngine.Debug.Log("抬起了S键 停止后退");
                    break;
                case KeyCode.A:
                    UnityEngine.Debug.Log("抬起了A键 停止左移");
                    break;
                case KeyCode.D:
                    UnityEngine.Debug.Log("抬起了D键 停止右移");
                    break;
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

