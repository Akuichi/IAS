using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
{
    protected Data_DeadState stateData;
    public DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_DeadState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        GameObject.Instantiate(stateData.DeathBloodParticle, entity.transform.position, stateData.DeathBloodParticle.transform.rotation);
        GameObject.Instantiate(stateData.DeathChunkParticle, entity.transform.position, stateData.DeathChunkParticle.transform.rotation);
        entity.gameObject.SetActive(false);

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhsyicsUpdate()
    {
        base.PhsyicsUpdate();
    }
}
