using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson10 : MonoBehaviour
{
    //1.写一个工具类，让我们可以更加方便的加载Multiple类型的图集资源
    //2.用我提供的角色资源，制作一个通过wasd键控制其上下左右移动的功能
    void Start()
    {
        LoadAlbumOne("RobotBoyCrouchSprite", "RobotBoyCrouch10");
    }


    void Update()
    {
        //找到被加载的sprite 并且与Move进行关联
        GameObject spriteObject = GameObject.Find("RobotBoyCrouch10");
        if (spriteObject != null)
        {
            MoveCharacter(spriteObject);
        }
    }

    public void LoadAlbumOne(string AlbumName, string spriteName)
    {
        // 通过Resources.LoadAll加载图集中的所有Sprite
        Sprite[] sprites = Resources.LoadAll<Sprite>(AlbumName);
        foreach (Sprite sprite in sprites)
        {
            if (sprite.name == spriteName)
            {
                // 找到指定名称的Sprite 创建SpriteRenderer并赋值
                GameObject spriteObject = new GameObject(sprite.name);
                SpriteRenderer spriteRenderer = spriteObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = sprite;
                return;
            }
        }
        Debug.Log("未找到Sprite: " + spriteName);
    }

    public void MoveCharacter(GameObject character)
    {
        float moveSpeed = 5f;
        float moveHorizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveVertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        character.transform.Translate(new Vector3(moveHorizontal, moveVertical, 0));
    }
}
