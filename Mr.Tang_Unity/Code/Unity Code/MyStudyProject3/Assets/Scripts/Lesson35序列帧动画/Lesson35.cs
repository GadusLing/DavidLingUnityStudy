using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson35 : MonoBehaviour
{
    public Sprite[] sprites;
    private SpriteRenderer sr;
    float timer = 0f;
    private int currentFrame = 0;
    Animator animator;
    int HP;
    void Start()
    {
        // sr = GetComponent<SpriteRenderer>();
        // sr.sprite = sprites[currentFrame];

        animator = GetComponent<Animator>();
        HP = 5;
        
    }

    // Update is called once per frame
    void Update()
    {
        // // 每隔30毫秒切换一张图片
        // timer += Time.deltaTime;
        // if(timer >= 0.03f)
        // {
        //     currentFrame++;
        //     if(currentFrame >= sprites.Length)
        //     {
        //         currentFrame = 0;
        //     }
        //     sr.sprite = sprites[currentFrame];
        //     timer = 0f;
        // }

        if(Input.GetKeyDown(KeyCode.A))
        {
            animator.SetBool("Hited", true);
            HP--;
            
            // 立即检查死亡条件
            if(HP <= 0)
            {
                animator.SetTrigger("Dead");
                return;
            }
        }
        else if(Input.GetKeyUp(KeyCode.A))
        {
            if(HP > 0) // 只有还活着时才恢复
            {
                animator.SetBool("Hited", false);
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("isDown", true);
        }
        else if(Input.GetKeyUp(KeyCode.Space))
        {
            animator.SetBool("isDown", false);
        }
    }
}
