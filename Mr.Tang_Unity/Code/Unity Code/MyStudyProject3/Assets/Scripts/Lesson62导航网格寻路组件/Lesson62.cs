using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Lesson62 : MonoBehaviour
{
    public NavMeshAgent agent;
    void Start()
    {
        //agent.SetDestination()
        //agent.isStopped = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                print(hit.collider.name);
                agent.isStopped = false;
                agent.SetDestination(hit.point);
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            agent.isStopped = true;
        }
    }
}
