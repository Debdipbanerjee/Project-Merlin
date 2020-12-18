using RPG.Combat;
using RPG.Movement;
using System;
using System.Collections;
using UnityEngine;


namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            InteractWithCombat();
            InteractWithMovement();
        }

        private void InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                //if any item has rigidbody or collider then it's enemy
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue; //No enemy, so leave the rest below & check new item

                //if there's enemy, Attack
                if(Input.GetMouseButtonDown(0))
                {
                    GetComponent<Fighter>().Attack(target);
                }
            }

        }

        private void InteractWithMovement()
        {
            if (Input.GetMouseButton(0))
            {
                MoveToCursor();
            }
        }

        private void MoveToCursor()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit); //putting information of ray into hit

            //if ray hits somewhere, player moves to that point, otherwise not
            if (hasHit == true)
            {
                GetComponent<Mover>().MoveTo(hit.point);
            }

        }

        private static Ray GetMouseRay()
        {
            //casting ray from camera to a point
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }

}