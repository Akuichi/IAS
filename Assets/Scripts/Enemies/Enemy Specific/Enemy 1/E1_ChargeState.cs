using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_ChargeState : ChargeState
{
    private Enemy1 enemy;
    public E1_ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_ChargeState stateData, Enemy1 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (performCloseRangeAction)
        {
            stateMachine.ChangeState(enemy.MeleeAttackState);
        }
        else if (!isDetectingLedge || isDetectingWall)
        {
            stateMachine.ChangeState(enemy.SearchState);
        }
        else if (isChargeTimeOver)
        {            
            if(isPlayerInMinAggroRange)
            {
                stateMachine.ChangeState(enemy.PlayerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(enemy.SearchState);
            }
        }
    }

    public override void PhsyicsUpdate()
    {
        base.PhsyicsUpdate();
    }
}
