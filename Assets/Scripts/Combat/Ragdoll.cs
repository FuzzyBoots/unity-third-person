using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController controller;

    private Collider[] allColliders;
    private Rigidbody[] allRigidBodies;
    // Start is called before the first frame update
    private void Start()
    {
        allColliders = GetComponentsInChildren<Collider>(true);
        allRigidBodies= GetComponentsInChildren<Rigidbody>(true);

        ToggleRaddoll(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleRaddoll(bool isRagdoll)
    {
        foreach(Collider collider in allColliders)
        {
            if (collider.gameObject.CompareTag("Ragdoll")) {
                collider.enabled = isRagdoll;
            }
        }

        foreach (Rigidbody rigidbody in allRigidBodies)
        {
            if (rigidbody.gameObject.CompareTag("Ragdoll"))
            {
                rigidbody.isKinematic = !isRagdoll;
                rigidbody.useGravity = isRagdoll;
            }
        }

        controller.enabled = !isRagdoll;
        animator.enabled = !isRagdoll;
    }
}
