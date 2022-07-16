using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : BehaviourComponent
{
    public override bool OnTick()
    {
        base.OnTick();
        return false;
    }
    public override void OnAction()
    {
        base.OnAction();
    }
}
