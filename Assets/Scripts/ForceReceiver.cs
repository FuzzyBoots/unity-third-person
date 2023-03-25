using UnityEngine;
using UnityEngine.AI;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private CharacterController controller;

    private float verticalVelocity;

    [SerializeField] private float drag = 0.3f;

    private Vector3 impact;

    private Vector3 dampingVelocity;

    public Vector3 Movement => impact + Vector3.up * verticalVelocity;

    private void Update() {
        // Check if we're on the ground and not falling
        if (verticalVelocity < 0f && controller.isGrounded) {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        } else {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);

        if (agent != null && impact.sqrMagnitude <= 0.2f * 0.2f)
        {
            agent.enabled = true;
        }
    }

    public void AddForce(Vector3 force)
    {
        impact += force;
        if (agent != null)
        {
            agent.enabled = false;
        }

    }

    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }

    internal void Reset()
    {
        impact = Vector3.zero;
        verticalVelocity = 0f;
    }
}
