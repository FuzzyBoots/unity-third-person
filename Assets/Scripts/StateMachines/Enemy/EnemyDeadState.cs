using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public EnemyDeadState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        // toggle ragdoll
        stateMachine.Ragdoll.ToggleRaddoll(true);

        stateMachine.Weapon.gameObject.SetActive(false);

        GameObject.Destroy(stateMachine.Target);
    }

    public override void Exit()
    {
        
    }

    public override void Tick(float deltaTime)
    {
        
    }
}
