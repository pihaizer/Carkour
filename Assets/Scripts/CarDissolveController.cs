using System;
using System.Collections.Generic;
using DG.Tweening;
using Signals;
using UnityEngine;
using Zenject;

public class CarDissolveController : MonoBehaviour
{
    [SerializeField] private List<Renderer> renderers;
    [SerializeField] private float dissolveTime = 1.5f;

    [Inject] private SignalBus _bus;

    public float DissolveTime => dissolveTime;
    
    private List<Material> _dissolveMaterials = new();
    
    private static readonly int _dissolveId = Shader.PropertyToID("_Dissolve");
    private float _currentDissolve;

    public void AnimateDissolve(bool toDissolved)
    {
        SetDissolve(toDissolved ? 0f : 1f);
        DOTween.To(() => _currentDissolve, SetDissolve, toDissolved ? 1f : 0f, dissolveTime);
    }

    private void Start()
    {
        foreach (var rend in renderers)
        {
            foreach (var material in rend.sharedMaterials)
            {
                if (material.HasFloat(_dissolveId))
                {
                    _dissolveMaterials.Add(material);
                }
            }
        }

        SetDissolve(0);
    }

    private void SetDissolve(float value)
    {
        _currentDissolve = value;
        foreach (var material in _dissolveMaterials)
        {
            material.SetFloat(_dissolveId, value);
        }
    }
}