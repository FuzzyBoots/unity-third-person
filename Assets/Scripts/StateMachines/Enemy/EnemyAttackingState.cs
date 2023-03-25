using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private readonly int AttackHash = Animator.StringToHash("Bencao");
    private const float CrossFadeDuration = 0.1f;

    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Weapon.SetAttack(stateMachine.AttackDamage, stateMachine.AttackKnockback);
        stateMachine.Animator.CrossFadeInFixedTime(AttackHash, CrossFadeDuration);
    }

    public override void Exit() {  }

    public override void Tick(float deltaTime)
    {
        if (GetNormalizedTime(stateMachine.Animator) >= 1) 
        { 
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
        }
    }
}
