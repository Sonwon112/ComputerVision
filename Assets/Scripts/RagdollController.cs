using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{

    private Rigidbody[] ragdoll;
    private Rigidbody hipRb;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {   
        animator = GetComponent<Animator>();
        ragdoll = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in ragdoll)
        {
            rb.isKinematic = true;
            if(rb.tag.Equals("Hip"))
                hipRb = rb;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRagdoll()
    {
        foreach (Rigidbody rb in ragdoll)
        {
            rb.isKinematic = false;
            rb.AddForce(Vector3.down * 800, ForceMode.Impulse);
        }
        
        animator.enabled = false;
    }

    public void OffRagdoll()
    {
        animator.enabled = true;
        foreach (Rigidbody rb in ragdoll)
        {
            rb.isKinematic = true;
        }
        
    }
}
