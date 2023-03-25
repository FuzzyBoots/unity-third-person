using UnityEngine;

public class PlayerDodgingState : PlayerBaseState
{
    private Vector2 dodgingDirectionInput;

    private float remainingDodgeTime;

    private readonly int DodgingBlendTreeHash = Animator.StringToHash("Dodging Blend Tree");
    private readonly int DodgingForwardHash = Animator.StringToHash("DodgeForward");
    private readonly int DodgingRightHash = Animator.StringToHash("DodgeRight");

    private const float FixedTransitionDuration = 0.1f;


    public PlayerDodgingState(PlayerStateMachine stateMachine, Vector3 dodgingDirectionInput) : base(stateMachine)
    {
        this.dodgingDirectionInput = dodgingDirectionInput;
    }

    public override void Enter()
    {
        remainingDodgeTime = stateMachine.DodgeDuration;

        stateMachine.Animator.SetFloat(DodgingForwardHash, dodgingDirectionInput.y);
        stateMachine.Animator.SetFloat(DodgingRightHash, dodgingDirectionInput.x);
        stateMachine.Animator.CrossFadeInFixedTime(DodgingBlendTreeHash, FixedTransitionDuration);

        stateMachine.Health.setInvulnerable(true);
}

    public override void Exit()
    {
        stateMachine.Health.setInvulnerable(false);
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = new Vector3();
        movement += stateMachine.transform.right * dodgingDirectionInput.x * stateMachine.DodgeLength / stateMachine.DodgeDuration;
        movement += stateMachine.transform.forward * dodgingDirectionInput.y * stateMachine.DodgeLength / stateMachine.DodgeDuration;

        Move(movement, deltaTime);

        FaceTarget();

        remainingDodgeTime = remainingDodgeTime - deltaTime;

        if (remainingDodgeTime <= 0f)
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }
    }
}
