using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerStateMachine stateMachine) {
        this.stateMachine = stateMachine;
    }

    protected void Move(Vector3 motion, float deltaTime) {
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void Move(float deltaTime)
    {
        stateMachine.Controller.Move(stateMachine.ForceReceiver.Movement * deltaTime);
    }

    protected void FaceTarget()
    {
        // Verify we have a target
        // Get the Vector3 pointing to the target (other.position - our.position)
        // Clear the Y.
        if (stateMachine.Targeter.CurrentTarget == null) { return; }

        Vector3 vectorToTarget = stateMachine.Targeter.CurrentTarget.transform.position -
            stateMachine.transform.position;
        vectorToTarget.y = 0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(vectorToTarget);
    }

    protected void ReturnToLocomotion()
    {
        if (stateMachine.Targeter.CurrentTarget != null) 
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }
        else
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }
    }
}
