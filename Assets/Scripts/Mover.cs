using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            MoveToCursor();
        }
    }

    private void MoveToCursor()
    {
        //casting ray from camera to a point
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit); //putting information of ray into hit

        //if ray hits somewhere, player moves to that point, otherwise not
        if(hasHit == true)
        {
            GetComponent<NavMeshAgent>().destination = hit.point;
        }

    }
}
