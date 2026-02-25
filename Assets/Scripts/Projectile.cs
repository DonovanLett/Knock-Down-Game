using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private float _abyssDepth;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(transform.position.y <= _abyssDepth)
        {
            Destroy(this.gameObject);
        }
    }

    public void Fire(Vector3 velocity)
    {
        rb = GetComponent<Rigidbody>();
        rb.AddRelativeForce(velocity, ForceMode.Impulse);
    }
}
