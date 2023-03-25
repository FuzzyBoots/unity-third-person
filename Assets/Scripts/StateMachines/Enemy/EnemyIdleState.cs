using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;
    private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    private readonly int SpeedHash = Animator.StringToHash("Speed");

    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
    }

    public override void Exit() { }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        if (IsInChaseRange())
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }

        FacePlayer();

        stateMachine.Animator.SetFloat(SpeedHash, 0f, AnimatorDampTime, deltaTime);
    }
}
