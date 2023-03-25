using UnityEditorInternal;
using UnityEngine;

public class PlayerPullUpState : PlayerBaseState
{
    private readonly int PullUpHash = Animator.StringToHash("Pull Up");
    private const float CrossFadeDuration = 0.1f;

    [SerializeField]
    private Vector3 offset = new Vector3(0f, 2.325f, 0.65f);

    public PlayerPullUpState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        stateMachine.Animator.CrossFadeInFixedTime(PullUpHash, CrossFadeDuration);
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        stateMachine.Controller.Move(Vector3.zero);
        stateMachine.ForceReceiver.Reset();
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f) { return; }

        stateMachine.Controller.enabled = false;
        stateMachine.transform.Translate(offset, Space.Self);
        stateMachine.Controller.enabled = true;

        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine, false));
    }
}
