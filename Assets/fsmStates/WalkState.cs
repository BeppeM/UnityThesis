using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class WalkState : State
{
    public WalkState(GameObject _npc, NavMeshAgent _agent) : base(_npc, _agent)
    {
        name = STATE.WALK;
    }

    public override void Enter()
    {                
        base.Enter();
    }

    public override void Update()
    {        
        // Check if agent has seen something
        VisionCone visionCone = visionSensor.GetComponent<VisionCone>();

        bool reachedArtifact = visionCone.ReachedArtifact;
        
        if (reachedArtifact)
        {
            Debug.Log("Agent " + npc.name + " found something.");
            // Move to Check artifact
            nextState = new CheckArtifactState(npc, agent);
            stage = EVENT.EXIT;
            return;
        }

        // Let agent walk
        AutonomousWalking autonomousWalking = npc.GetComponent<AutonomousWalking>();
        autonomousWalking.Walk();

        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
