using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{
    private NavMeshAgent playrAgent;

    // Start is called before the first frame update
    void Start()
    {
        playrAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            bool isCollide = Physics.Raycast(ray, out hit);
            if(isCollide)
            {
                if (hit.collider.tag == "Ground")
                {
                    playrAgent.stoppingDistance = 0;//重置停止距离为0，停止距离是给npc和pickobject的交互使用的，交互更真实，地面上不用，所以这里给它重置了
                    playrAgent.SetDestination(hit.point);//朝着鼠标的点击的地方移动
                }
                else if(hit.collider.tag == "Interactable")//交互的逻辑————先确定鼠标点击了 → 角色走近 → 到附近了，交互
                {
                    hit.collider.GetComponent<InteractableObject>().OnClick(playrAgent);//所有可交互物体上都会挂Inter...脚本或它子类的脚本，这里调用它里面的Onclick方法
                }
            }
        }
    }
}
