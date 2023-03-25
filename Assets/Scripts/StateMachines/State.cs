using UnityEngine;

public abstract class State
{

    protected bool debugLog;
    public abstract void Enter();

    public abstract void Tick(float deltaTime);

    public abstract void Exit();

    protected float GetNormalizedTime(Animator animator)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (debugLog)
        {
            Debug.Log(string.Format("IsInTransition: {0}, currentAttack: {1}, nextAttack: {2}", animator.IsInTransition(0), currentInfo.IsTag("Attack"), nextInfo.IsTag("Attack")));
        }
        if (animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
        {
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0;
        }
    }

}
