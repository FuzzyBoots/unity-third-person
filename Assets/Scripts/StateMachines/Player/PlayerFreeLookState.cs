using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");

    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLook Blend Tree");
    private bool shouldFade;
    private const float AnimatorDampTime = 0.1f;
    private const float FixedTransitionDuration = 0.1f;

    public PlayerFreeLookState(PlayerStateMachine stateMachine, bool shouldFade = true) : base(stateMachine) {
        this.shouldFade = shouldFade;
    }

    public override void Enter() {
        stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.InputReader.TargetEvent += OnTarget;

        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0f);

        if (shouldFade)
        {
            stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, FixedTransitionDuration);
        }
        else
        {
            stateMachine.Animator.Play(FreeLookBlendTreeHash);
        }

    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }

        Vector3 movement = CalculateMovement();

        Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);

        if (stateMachine.InputReader.MovementValue == Vector2.zero) {
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0f, AnimatorDampTime, deltaTime);
            return;
        }

        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1.0f, AnimatorDampTime, deltaTime);

        FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit() {
        stateMachine.InputReader.JumpEvent -= OnJump;
        stateMachine.InputReader.TargetEvent -= OnTarget;
    }

    public void OnJump() {
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
    }

    public void OnTarget() {
        if (!stateMachine.Targeter.SelectTarget()) {return;}

        stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
    }

    private Vector3 CalculateMovement() {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputReader.MovementValue.y +
            right * stateMachine.InputReader.MovementValue.x;
    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime) {
        stateMachine.transform.rotation = Quaternion.Lerp(
            stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltaTime * stateMachine.RotationDamping);
    }

    
}
