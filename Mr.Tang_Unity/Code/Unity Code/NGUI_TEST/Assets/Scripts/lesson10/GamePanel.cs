using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanel : MonoBehaviour
{
    public lesson10_25_10_05_Button player;
    public UIButton btn;
    void Start()
    {
        btn.onClick.Add(new EventDelegate (() =>
        {
            player.fire();
        }));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
