using UnityEditor.Animations;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    private readonly int JumpHash = Animator.StringToHash("Jump");
    private const float CrossFadeDuration = 0.1f;

    private Vector3 momentum;

    [SerializeField]
    AnimatorController controller;

    public PlayerJumpingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.ForceReceiver.Jump(stateMachine.JumpForce);

        momentum = stateMachine.Controller.velocity;
        momentum.y = 0f;

        stateMachine.Animator.CrossFadeInFixedTime(JumpHash, CrossFadeDuration);

        stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
    }

    public override void Exit()
    {
        stateMachine.LedgeDetector.OnLedgeDetect -= HandleLedgeDetect;
    }

    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);

        if (stateMachine.Controller.velocity.y <= 0f)
        {
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));
            return;
        }

        FaceTarget();
    }

    private void HandleLedgeDetect(Vector3 ledgeForward)
    {
        Debug.Log("Should be hanging - Jumping");
        stateMachine.SwitchState(new PlayerHangingState(stateMachine, ledgeForward));
    }
}
