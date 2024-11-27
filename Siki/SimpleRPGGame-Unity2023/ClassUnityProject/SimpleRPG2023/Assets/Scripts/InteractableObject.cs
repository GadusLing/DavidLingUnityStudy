using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InteractableObject : MonoBehaviour
{
    private NavMeshAgent playerAgent;
    private bool haveInteracted = false;

    public void OnClick(NavMeshAgent playerAgent)
    {
        this.playerAgent = playerAgent;
        playerAgent.stoppingDistance = 2;//剩余2米就停下，不要爬到别人身上去了
        playerAgent.SetDestination(transform.position);
        haveInteracted = false;

        //Interact();
    }

    private void Update()
    {
        if(playerAgent != null && haveInteracted == false && playerAgent.pathPending == false)
        {
            if(playerAgent.remainingDistance <= 2)//主角离可交互物的停止距离之前设计的是2，所以这里是2
            {
                Interact();
                haveInteracted = true;
            }
        }
    }


    protected virtual void Interact()
    {
        print("Interacting with Interactable Object.");
    }

}
