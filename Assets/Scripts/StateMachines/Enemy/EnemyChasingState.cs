using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;
    private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    private readonly int SpeedHash = Animator.StringToHash("Speed");

    public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
    }

    public override void Exit() {
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity = Vector3.zero;
    }

    public override void Tick(float deltaTime)
    {
        MoveToPlayer(deltaTime);

        FacePlayer();
        
        if (!IsInChaseRange())
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        } else if (IsInAttackRange())
        {
            stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
            return;
        }

        stateMachine.Animator.SetFloat(SpeedHash, 1f, AnimatorDampTime, deltaTime);
    }

    public void MoveToPlayer(float deltaTime)
    {
        if (stateMachine.Agent.isOnNavMesh)
        {
            Vector3 destination = this.stateMachine.Player.transform.position;
            stateMachine.Agent.destination = destination;

            Move(stateMachine.Agent.desiredVelocity.normalized * this.stateMachine.MovementSpeed, deltaTime);
        }

        stateMachine.Agent.nextPosition = stateMachine.transform.position;
    }
}
