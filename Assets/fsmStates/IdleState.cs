using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : State
{
    public IdleState(GameObject _npc, NavMeshAgent _agent) : base(_npc, _agent)
    {
        name = STATE.IDLE;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {           
        if(Random.Range(1, 100) < 10)
        {            
            nextState = new WalkState(npc, agent);
            stage = EVENT.EXIT;
            return;
        }
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
