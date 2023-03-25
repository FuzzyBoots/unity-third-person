using UnityEngine;

public class PlayerBlockingState : PlayerBaseState
{
    private readonly int BlockHash = Animator.StringToHash("Block");
    private float CrossFadeDuration = 0.1f;

    public PlayerBlockingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(BlockHash, CrossFadeDuration);

        stateMachine.Health.setInvulnerable(true);
    }

    public override void Exit()
    {
        stateMachine.Health.setInvulnerable(false);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        if (!stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            return;
        }

        if (stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }
    }
}
