using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNoneState : BaseState
{
    public CharacterNoneState(CharacterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter() { base.Enter(); }
    public override void Exit() { base.Exit(); }

    protected override void AimEvent(Vector2 direction)
    {
        if (CheckGround())
        {
            if (direction.magnitude >= 100f)
                _stateMachine.ChangeState(eStateType.Aim);
        }
        _stateMachine.AttackDirection = direction;
    }

    protected override void AttackEvent(Vector2 direction)
    {
        if (CheckGround())
            _stateMachine.ChangeState(eStateType.ComboAttack);
    }
}