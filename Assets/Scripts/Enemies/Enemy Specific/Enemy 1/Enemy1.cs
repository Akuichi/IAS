using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy1 : Entity
{
    public E1_IdleState IdleState {  get; private set; }
    public E1_MoveState MoveState { get; private set; }
    public E1_PlayerDetectedState PlayerDetectedState { get; private set; }
    public E1_ChargeState ChargeState { get; private set; }
    public E1_SearchState SearchState { get; private set; }
    public E1_MeleeAttackState MeleeAttackState { get; private set; }
    public E1_StunState StunState { get; private set; }
    public E1_DeadState DeadState { get; private set; }
    [SerializeField] private Data_IdleState idleStateData;
    [SerializeField] private Data_MoveState moveStateData;
    [SerializeField] private Data_PlayerDetected playerDetectedStateData;
    [SerializeField] private Data_ChargeState chargeStateData;
    [SerializeField] private Data_SearchState searchStateData;
    [SerializeField] private Data_MeleeAttackState meleeAttackStateData;
    [SerializeField] private Data_StunState stunStateData;
    [SerializeField] private Data_DeadState deadStateData;
    [SerializeField] private Transform meleeAttackPosition;


    //State Text
    [SerializeField] private TextMeshProUGUI enemyStateText;
    


    public override void Awake()
    {
        base.Awake();
        MoveState = new E1_MoveState(this, StateMachine, "move",moveStateData, this);
        IdleState = new E1_IdleState(this, StateMachine, "idle", idleStateData, this);
        PlayerDetectedState = new E1_PlayerDetectedState(this, StateMachine, "playerDetected", playerDetectedStateData, this);
        ChargeState = new E1_ChargeState(this, StateMachine, "charge", chargeStateData, this);
        SearchState = new E1_SearchState(this, StateMachine, "search", searchStateData, this);
        MeleeAttackState = new E1_MeleeAttackState(this, StateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        StunState = new E1_StunState(this, StateMachine, "stun", stunStateData, this);
        DeadState = new E1_DeadState(this, StateMachine, "dead", deadStateData, this);        
    }

    public override void Update()
    {
        base.Update();
        enemyStateText.text = StateMachine.CurrentState.ToString();
        enemyStateText.transform.position = this.transform.position;
    }

    private void Start()
    {
        StateMachine.Initialize(MoveState);
    }

    protected override void HandlePoiseZero()
    {
        base.HandlePoiseZero();
        StateMachine.ChangeState(StunState);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.AttackRadius);
    }
}
