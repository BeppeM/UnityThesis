using UnityEngine;
using UnityEngine.AI;

public class State 
{
    public enum STATE
    {
        IDLE, WALK, CHECK_ARTIFACT
    };

    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };

    public STATE name;
    protected EVENT stage;
    // The avatar
    protected GameObject npc;
    // The avatar's sensor to see the environment
    protected GameObject visionSensor;
    protected NavMeshAgent agent;
    protected State nextState;

    public State(GameObject _npc, NavMeshAgent _agent)
    {
        npc = _npc;
        agent = _agent;
        stage = EVENT.ENTER;
        // Retrieve the avatar's eyes
        visionSensor = npc.transform.Find("visionCone").gameObject;
    }

    public virtual void Enter() {        
        Debug.Log("Agent " + npc.name + " entered in " + name.ToString());
        stage = EVENT.UPDATE; 
    }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() {
        Debug.Log("Agent " + npc.name + " leaving the state " + name.ToString());
        stage = EVENT.EXIT; 
    }

    public State Process()
    {
        if(stage == EVENT.ENTER) Enter();
        if(stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }

        return this;
    }

}
