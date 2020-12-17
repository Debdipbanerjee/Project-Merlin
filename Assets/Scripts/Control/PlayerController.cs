using RPG.Movement;
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
            if (Input.GetMouseButton(0))
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
            if (hasHit == true)
            {
                GetComponent<Mover>().MoveTo(hit.point);
            }

        }
    }

}