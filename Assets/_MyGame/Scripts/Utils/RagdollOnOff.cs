using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollOnOff : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Collider mainCollider;
    [SerializeField] private Rigidbody mainRigidbody;
    [SerializeField] private Transform ragdollHolder;

    private Collider[] ragdollColliders;
    private Rigidbody[] ragdollRigidbodies;

    private void Awake()
    {
        GetRagdollBits();
        RagdollOff();
    }

    private void GetRagdollBits()
    {
        ragdollColliders = ragdollHolder.GetComponentsInChildren<Collider>();
        ragdollRigidbodies = ragdollHolder.GetComponentsInChildren<Rigidbody>();
    }

    public void RagdollOn()
    {
        anim.enabled = false;

        for (int i = 0; i < ragdollColliders.Length; i++)
        {
            ragdollColliders[i].enabled = true;
        }

        for (int i = 0; i < ragdollRigidbodies.Length; i++)
        {
            ragdollRigidbodies[i].isKinematic = false;
        }

        mainCollider.enabled = false;
        //mainRigidbody.isKinematic = true;
        
    }

    public void RagdollOff()
    {
        for (int i = 0; i < ragdollColliders.Length; i++)
        {
            ragdollColliders[i].enabled = false;
        }

        for (int i = 0; i < ragdollRigidbodies.Length; i++)
        {
            ragdollRigidbodies[i].isKinematic = true;
        }

        mainCollider.enabled = true;
        //mainRigidbody.isKinematic = false;
        anim.enabled = true;
    }

    public void ApplyForce(Vector3 force)
    {
        var rigidbody = anim.GetBoneTransform(HumanBodyBones.Hips).GetComponent<Rigidbody>();
        rigidbody.AddForce(force, ForceMode.VelocityChange);
    }


}
