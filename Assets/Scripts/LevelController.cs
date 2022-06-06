using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Signals;
using TMPro;
using UnityEngine;
using Zenject;

public class LevelController : MonoBehaviour
{
    [SerializeField] private LevelConfig levelConfig;
    [SerializeField] private Transform startPoint;
    [SerializeField] private CarController carController;
    [SerializeField] private LevelGoal levelGoal;
    [SerializeField] private GameInfoSO gameInfoSO;

    [Inject] private SignalBus _bus;
    [Inject] private PlayerInput _input;

    private bool _isStarted = false;
    private bool _isFinished = false;

    private void Awake()
    {
        ResetLevel();
        levelGoal.Reached += OnLevelGoalReached;
        _bus.Subscribe<RestartLevelSignal>(OnRestartRequested);
    }

    private void OnRestartRequested()
    {
        ResetLevel();
    }

    private void OnLevelGoalReached()
    {
        if (_isFinished) return;
        FinishLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) RequestRestart();
        if (Input.GetKeyDown(KeyCode.Escape)) RequestExit();

        if (_isFinished) return;
        if (!_isStarted)
        {
            if (Mathf.Abs(_input.Vertical) > 0.01f)
                StartLevel();
            return;
        }

        gameInfoSO.timer.Value += Time.deltaTime;
    }

    private void StartLevel()
    {
        gameInfoSO.timer.Value = 0;
        _isStarted = true;
    }

    public void RequestRestart() => _bus.Fire<RestartLevelSignal>();

    public void RequestExit() => _bus.Fire<ExitLevelSignal>();

    private void ResetLevel()
    {
        _isStarted = false;
        _isFinished = false;
        gameInfoSO.timer.Value = 0;
        ResetCarPosition();
    }

    private void FinishLevel()
    {
        _isFinished = true;
        _bus.Fire(new LevelFinishedSignal(levelConfig, gameInfoSO.timer.Value));
    }

    private void ResetCarPosition() => StartCoroutine(ResetCarCoroutine());

    private IEnumerator ResetCarCoroutine()
    {
        carController.gameObject.SetActive(false);
        carController.transform.SetPositionAndRotation(startPoint.position, startPoint.rotation);
        carController.ResetSpeed();
        yield return new WaitForFixedUpdate();
        carController.gameObject.SetActive(true);
    }
}