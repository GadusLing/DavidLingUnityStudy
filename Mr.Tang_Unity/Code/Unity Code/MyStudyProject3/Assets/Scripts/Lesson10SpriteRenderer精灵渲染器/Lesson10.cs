using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson10 : MonoBehaviour
{
    private GameObject character;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        character = LoadSprite("RobotBoyCrouchSprite", "RobotBoyCrouch10");
        spriteRenderer = character.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (character != null)
        {
            MoveCharacter();
        }
    }

    public GameObject LoadSprite(string albumName, string spriteName)
    {
        Sprite sprite = MultipleMgr.Instance.GetSprite(albumName, spriteName);
        if (sprite == null) return null;

        GameObject obj = new GameObject(sprite.name);
        obj.AddComponent<SpriteRenderer>().sprite = sprite;
        return obj;
    }

    private void MoveCharacter()
    {
        float speed = 5f * Time.deltaTime;
        float h = Input.GetAxis("Horizontal") * speed;
        float v = Input.GetAxis("Vertical") * speed;
        
        // 移动角色
        character.transform.Translate(h, v, 0);
        
        // 根据水平移动方向翻转精灵
        if (h != 0)
        {
            spriteRenderer.flipX = h < 0; // 向左移动时翻转
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("BulletRes"), 
            character.transform.position + new Vector3(spriteRenderer.flipX ? -1.2f : 1.2f, -0.2f, 0), 
            Quaternion.identity);
            obj.GetComponent<BulletFly>().changeMoveDir(spriteRenderer.flipX ? Vector3.left : Vector3.right);
        }
    }

   
}
