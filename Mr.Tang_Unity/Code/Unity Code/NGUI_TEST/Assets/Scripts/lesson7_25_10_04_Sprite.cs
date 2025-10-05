using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lesson7_25_10_04_Sprite : MonoBehaviour
{
    public UISprite sprite;
    // Start is called before the first frame update
    void Start()
    {
        // sprite.width = 200;
        // sprite.height = 300;
        sprite.spriteName = "bk";

        NGUIAtlas atlas = Resources.Load<NGUIAtlas>("Atlas/LoginAtlas");
        sprite.atlas = atlas;
        sprite.spriteName = "ui_DL_anniuxiao_01";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
