using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class CheckArtifactState : State
{
    public CheckArtifactState(GameObject _npc, NavMeshAgent _agent) : base(_npc, _agent)
    {
        name = STATE.CHECK_ARTIFACT;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public async override void Update()
    {
        await Task.Delay(4000);
        VisionCone visionCone = visionSensor.GetComponent<VisionCone>();
        visionCone.ReachedArtifact = false;
        agent.isStopped = false;
        nextState = new WalkState(npc, agent);        
        stage = EVENT.EXIT;
        return;
        //base.Update();
    }


    public override void Exit()
    {
        base.Exit();
    }
}
