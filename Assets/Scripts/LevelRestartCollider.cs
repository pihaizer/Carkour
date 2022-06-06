using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LevelRestartCollider : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    
    private void OnTriggerEnter(Collider other)
    {
        var car = other.GetComponentInParent<CarController>();
        if (car != null)
        {
            gameController.RequestRestart(true);
        }
    }
}