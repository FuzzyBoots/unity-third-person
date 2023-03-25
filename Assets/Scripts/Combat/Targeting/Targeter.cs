using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup cineTargetGroup;

    private List<Target> targets = new List<Target>();

    private Camera mainCamera;

    public Target CurrentTarget {get; private set;}

    private void Start()
    {
       mainCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other) {
        Target target = other.GetComponent<Target>();
        if (target) {
            targets.Add(target);
            target.OnDestroyed  += RemoveTarget;
        }
    }

    private void OnTriggerExit(Collider other) {
        Target target = other.GetComponent<Target>();
        if (target) {
            RemoveTarget(target);
        }
    }

    public bool SelectTarget() {
        if (targets.Count == 0) {return false;}

        Target closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;

        foreach (Target target in targets) {
            Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);

            if (!target.GetComponentInChildren<Renderer>().isVisible)
            {
                continue;
            }

            Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);

            float distance = toCenter.magnitude;
            if (distance < closestTargetDistance)
            {
                closestTarget = target;
                closestTargetDistance = distance;
            }
        }

        if (closestTarget == null) { return false; }

        CurrentTarget = closestTarget;
        cineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);

        return true;
    }

    private void RemoveTarget(Target target) {
        if (CurrentTarget == target) {
            cineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }

        target.OnDestroyed -= RemoveTarget;
        targets.Remove(target);
    }

    public void Cancel() {
        if (CurrentTarget == null) {return;}

        cineTargetGroup.RemoveMember(CurrentTarget.transform);

        CurrentTarget = null;
    }
}
