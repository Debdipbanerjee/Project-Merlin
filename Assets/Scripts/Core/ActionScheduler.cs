﻿using System.Collections;
using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        MonoBehaviour currentAction;

       public void StartAction(MonoBehaviour action)
       {
            //if current action & new action is same, don't cancel anything
            if (currentAction == action) return;

            if(currentAction != null)
            {
                print("Cancelling" + currentAction);
            }
            currentAction = action;
       }
    }
}