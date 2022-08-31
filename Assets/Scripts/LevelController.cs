using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Cinemachine;
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
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [Inject] private SignalBus _bus;
    [Inject] private PlayerInput _input;

    private bool _isStarted = false;
    private bool _isFinished = false;
    private bool _isRestarting = false;

    private void Awake()
    {
        ResetLevel();
        levelGoal.Reached += OnLevelGoalReached;
        _bus.Subscribe<RestartLevelSignal>(OnRestartRequested);
        virtualCamera.Follow = virtualCamera.LookAt = carController.transform;
    }

    private void OnRestartRequested(RestartLevelSignal signal)
    {
        if (_isRestarting) return;
        if (signal.IsFromBottomCollider && _isFinished) return;
        StartCoroutine(RestartCoroutine());
    }

    private IEnumerator RestartCoroutine()
    {
        _isRestarting = true;
        if (carController.TryGetComponent(out CarDissolveController dissolveController))
        {
            carController.SetInputBlocked(true);
            dissolveController.AnimateDissolve(true);
            yield return new WaitForSeconds(dissolveController.DissolveTime);
            dissolveController.AnimateDissolve(false);
        }

        ResetLevel();
        _isRestarting = false;
    }

    private void OnLevelGoalReached()
    {
        if (_isRestarting || _isFinished) return;
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
        carController.SetInputBlocked(false);
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