using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{

    private readonly int FallHash = Animator.StringToHash("Fall");
    private const float CrossFadeDuration = 0.1f;

    private Vector3 momentum;

    public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(FallHash, CrossFadeDuration);
        momentum = stateMachine.Controller.velocity;
        momentum.y = 0;

        stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
    }

    public override void Exit()
    {
        stateMachine.LedgeDetector.OnLedgeDetect -= HandleLedgeDetect;
    }

    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);

        if (stateMachine.Controller.isGrounded)
        {
            ReturnToLocomotion();
        }

        FaceTarget();
    }

    private void HandleLedgeDetect(Vector3 ledgeForward)
    {
        Debug.Log("Should be hanging - Falling");
        stateMachine.SwitchState(new PlayerHangingState(stateMachine, ledgeForward));
    }
}
