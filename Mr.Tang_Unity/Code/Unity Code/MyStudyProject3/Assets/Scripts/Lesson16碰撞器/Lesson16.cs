using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class Lesson16 : MonoBehaviour
{
    // 在之前练习题的基础上，请用2D刚体和碰撞器制作一个玩家，它可以
    // 在一个平台上移动跳跃。（注意：移动和跳跃都通过刚体的API进行制
    // 作，类似3D物理系统中刚体加力和给速度）

    private GameObject character;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb2d;

    void Start()
    {
        character = gameObject;
        spriteRenderer = character.GetComponent<SpriteRenderer>();
        rb2d = character.GetComponent<Rigidbody2D>();
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
        float speed = 5f;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        
        // 移动角色
        //character.transform.Translate(h * speed * Time.deltaTime, v * speed * Time.deltaTime, 0);
        

        
        // 根据水平移动方向翻转精灵
        if (h != 0)
        {
            rb2d.velocity = new Vector2(h * speed, rb2d.velocity.y);
            spriteRenderer.flipX = h < 0; // 向左移动时翻转
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("BulletRes"), 
            character.transform.position + new Vector3(spriteRenderer.flipX ? -1.2f : 1.2f, -0.2f, 0), 
            Quaternion.identity);
            obj.GetComponent<BulletFly>().changeMoveDir(spriteRenderer.flipX ? Vector3.left : Vector3.right);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb2d.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
        }
    }

}