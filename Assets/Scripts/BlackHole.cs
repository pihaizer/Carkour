using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(Collider))]
public class BlackHole : MonoBehaviour
{
    [SerializeField] private float finalRadius = 0.2f;
    [SerializeField] private float dragVelocity = 5;
    
    private List<Rigidbody> _affectedRigidbodies = new();


    private void FixedUpdate()
    {
        foreach (var rb in _affectedRigidbodies)
        {
            
            float distance = Vector3.Distance(transform.position, rb.position);
            if (distance < finalRadius)
            {
                rb.position = transform.position;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                return;
            }

            rb.velocity = Vector3.Lerp(rb.velocity, (transform.position - rb.position) * dragVelocity, 0.02f);

            // float force = dragForce / Mathf.Pow(distance, 2);
            // rb.AddForce((transform.position - rb.position).normalized * force, ForceMode.Acceleration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var rb = other.GetComponentInParent<Rigidbody>();
        if (rb == null) return;
        _affectedRigidbodies.Add(rb);
    }

    private void OnTriggerExit(Collider other)
    {
        var rb = other.GetComponentInParent<Rigidbody>();
        if (rb == null || !_affectedRigidbodies.Contains(rb)) return;
        _affectedRigidbodies.Remove(rb);
    }
}