using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine;

    public EnemyBaseState(EnemyStateMachine stateMachine) {
        this.stateMachine = stateMachine;
    }
    protected bool IsInChaseRange()
    {
        if (stateMachine.Player.isDead) { return false; }

        Vector3 toPlayer = stateMachine.Player.transform.position - stateMachine.transform.position;

        float distance = toPlayer.sqrMagnitude;

        return distance <= Mathf.Pow(stateMachine.PlayerDetectionRange, 2);
    }
    protected bool IsInAttackRange()
    {
        if (stateMachine.Player.isDead) { return false; }

        Vector3 toPlayer = stateMachine.Player.transform.position - stateMachine.transform.position;

        float distance = toPlayer.sqrMagnitude;

        return distance <= Mathf.Pow(stateMachine.AttackRange, 2);
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void Move(float deltaTime)
    {
        stateMachine.Controller.Move(stateMachine.ForceReceiver.Movement * deltaTime);
    }

    protected void FacePlayer()
    {
        if (stateMachine.Player == null) { return; }

        Vector3 vectorToTarget = stateMachine.Player.transform.position -
            stateMachine.transform.position;
        vectorToTarget.y = 0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(vectorToTarget);
    }
}
