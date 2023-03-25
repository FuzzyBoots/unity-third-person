public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Ragdoll.ToggleRaddoll(true);
        stateMachine.WeaponDamage.gameObject.SetActive(false);
    }

    public override void Exit()
    {
        
    }

    public override void Tick(float deltaTime)
    {
        
    }
}
