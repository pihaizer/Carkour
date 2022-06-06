using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LevelGoal : MonoBehaviour
{
    public event Action Reached;
    
    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var carController = other.GetComponentInParent<CarController>();
        if (!carController) return;
        Reached?.Invoke();
    }
}