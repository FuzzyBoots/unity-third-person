using UnityEngine;

public class PlayerHangingState : PlayerBaseState
{
    private readonly int HangingHash = Animator.StringToHash("Hanging");
    private Vector3 closestPoint;
    private Vector3 ledgeForward;
    private const float CrossFadeDuration = 0.1f;

    public PlayerHangingState(PlayerStateMachine stateMachine, Vector3 ledgeForward) : base(stateMachine)
    {
        this.ledgeForward = ledgeForward;
    }

    public override void Enter()
    {
        stateMachine.transform.rotation = Quaternion.LookRotation(ledgeForward, Vector3.up);
        stateMachine.Animator.CrossFadeInFixedTime(HangingHash, CrossFadeDuration);
    }

    public override void Exit()
    {
        
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputReader.MovementValue.y< 0)
        {
            Debug.Log("To Falling");
            stateMachine.Controller.Move(Vector3.zero);
            stateMachine.ForceReceiver.Reset();
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));
        }
        else if (stateMachine.InputReader.MovementValue.y > 0)
        {
            Debug.Log("To Pullup");
            stateMachine.SwitchState(new PlayerPullUpState(stateMachine));
        }
    }
}
