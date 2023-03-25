using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private readonly int TargetingBlendTreeHash = Animator.StringToHash("Targeting Blend Tree");
    private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");
    private readonly int TargetingRightHash = Animator.StringToHash("TargetingRight");

    private const float FixedTransitionDuration = 0.1f;

    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) {

    }

    public override void Enter() {
        stateMachine.InputReader.CancelEvent += OnCancel;

        stateMachine.InputReader.DodgeEvent += OnDodge;

        stateMachine.InputReader.JumpEvent += OnJump;

        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, FixedTransitionDuration);
    }

    public override void Tick(float deltaTime) {
        if (stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 1));
            return;
        }

        if (stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
            return;
        }

        if (stateMachine.Targeter.CurrentTarget == null) {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }       

        Vector3 movement = CalculateMovement(deltaTime);
        Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);

        UpdateAnimator(deltaTime);

        FaceTarget();
    }

    private void UpdateAnimator(float deltaTime)
    {
        stateMachine.Animator.SetFloat(TargetingRightHash, stateMachine.InputReader.MovementValue.x, 0.1f, deltaTime);
        stateMachine.Animator.SetFloat(TargetingForwardHash, stateMachine.InputReader.MovementValue.y, 0.1f, deltaTime);
    }

    public override void Exit() {
        stateMachine.InputReader.CancelEvent -= OnCancel;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
        stateMachine.InputReader.JumpEvent -= OnJump;
    }

    public void OnCancel() {
        stateMachine.Targeter.Cancel();

        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    public void OnDodge()
    {
        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            return;
        }
        stateMachine.SwitchState(new PlayerDodgingState(stateMachine, stateMachine.InputReader.MovementValue));
    }

    private void OnJump()
    {
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
    }

    private Vector3 CalculateMovement(float deltaTime)
    {
        Vector3 movement = new Vector3();

        movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
        movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;
        

        return movement;
    }
}
