using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Fire(Vector3 velocity)
    {
        rb = GetComponent<Rigidbody>();
        rb.AddRelativeForce(velocity, ForceMode.Impulse);
    }
}
