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
                    playrAgent.stoppingDistance = 0;//����ֹͣ����Ϊ0��ֹͣ�����Ǹ�npc��pickobject�Ľ���ʹ�õģ���������ʵ�������ϲ��ã������������������
                    playrAgent.SetDestination(hit.point);//�������ĵ���ĵط��ƶ�
                }
                else if(hit.collider.tag == "Interactable")//�������߼�����������ȷ��������� �� ��ɫ�߽� �� �������ˣ�����
                {
                    hit.collider.GetComponent<InteractableObject>().OnClick(playrAgent);//���пɽ��������϶����Inter...�ű���������Ľű�����������������Onclick����
                }
            }
        }
    }
}
