using System;
using LootLocker.Requests;
using Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Canvas))]
public class LevelFinishedScreen : MonoBehaviour
{
    [SerializeField] private LeaderboardView leaderboardView;
    
    [Inject] private SignalBus _bus;

    public LeaderboardView LeaderboardView => leaderboardView;

    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _bus.Subscribe<RestartLevelSignal>(OnRestart);
    }

    private void Start() => Close();

    public void Open(float time)
    {
        _canvas.enabled = true;
    }

    public void Close() => _canvas.enabled = false;


    private void OnRestart() => Close();
}