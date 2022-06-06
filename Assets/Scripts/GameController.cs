using System;
using System.Collections.Generic;
using DefaultNamespace;
using LootLocker.Requests;
using Signals;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameController : MonoBehaviour
{
    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private Canvas mainMenuCanvas;
    [SerializeField] private Canvas optionsCanvas;
    [SerializeField] private UsernameInputScreen usernameInputScreen;
    [SerializeField] private LevelSelectionScreen levelSelectionCanvas;
    [SerializeField] private LevelFinishedScreen levelFinishedScreen;

    [SerializeField] private List<LevelConfig> levelConfigs;

    [Inject] private SignalBus _bus;

    private const string _usernameKey = "username";

    private int _playerId;
    private string _playerName;
    private Scene _loadedLevel;
    private LevelConfig _loadedLevelConfig;

    private void Awake()
    {
        _bus.Subscribe<LevelFinishedSignal>(OnLevelFinished);
        _bus.Subscribe<ExitLevelSignal>(OnExitLevelSignal);
        
        usernameInputScreen.Submitted += OnUsernameSubmitted;
    }

    private void OnUsernameSubmitted(string username)
    {
        LootLockerSDKManager.SetPlayerName(username, response =>
        {
            if (!response.success)
            {
                usernameInputScreen.SetError(response.Error);
                return;
            }

            _playerName = username;
            usernameInputScreen.Close();
        });
    }

    private void Start()
    {
        CloseAllUI();
        Login();
        levelSelectionCanvas.Init(levelConfigs);
        if (SceneManager.sceneCount > 1)
        {
            _loadedLevel = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            _loadedLevelConfig = levelConfigs.Find(config => config.sceneName == _loadedLevel.name);
            gameCanvas.enabled = true;
            return;
        }

        mainMenuCanvas.enabled = true;
    }

    public void LoadLevel(LevelConfig levelConfig)
    {
        CloseAllUI();
        _loadedLevelConfig = levelConfig;
        SceneManager.LoadSceneAsync(levelConfig.sceneName, LoadSceneMode.Additive).completed += OnLevelLoaded;
    }

    public void LoadNextLevel()
    {
        OnExitLevelSignal(null);
        LoadLevel(levelConfigs.Find(
            config => config.levelNumber == _loadedLevelConfig.levelNumber + 1));
    }

    private void OnLevelLoaded(AsyncOperation asyncOperation)
    {
        _loadedLevel = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(_loadedLevel);
        gameCanvas.enabled = true;
    }

    public void RequestRestart() => _bus.Fire<RestartLevelSignal>();

    public void RequestExit() => _bus.Fire<ExitLevelSignal>();

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    private void Login()
    {
        LootLockerSDKManager.StartGuestSession(OnLogin);
    }

    private void OnLogin(LootLockerSessionResponse response)
    {
        if (!response.success)
        {
            Debug.Log("error starting LootLocker session");
            return;
        }

        Debug.Log("successfully started LootLocker session");
        _playerId = response.player_id;
        LootLockerSDKManager.GetPlayerName((response) =>
        {
            if (!response.success)
            {
                Debug.Log("error getting player name");
                return;
            }

            _playerName = response.name;
            if (_playerName == null) RequestEnterUsername();
        });
    }

    private void RequestEnterUsername()
    {
        usernameInputScreen.Open();
    }

    private void OnLevelFinished(LevelFinishedSignal signal)
    {
        Debug.Log($"{signal.LevelConfig.levelName} finished in {signal.Time}!");

        LootLockerSDKManager.SubmitScore("",
            Mathf.RoundToInt(signal.Time * 1e5f),
            signal.LevelConfig.leaderboardId,
            (response) =>
            {
                if (response.statusCode == 200)
                {
                    Debug.Log("Successful");
                    levelFinishedScreen.LeaderboardView.Fetch(signal.LevelConfig.leaderboardId);
                    levelFinishedScreen.LeaderboardView.InitPlayerLine(_playerName, response.rank, response.score);
                }
                else
                {
                    Debug.Log("failed: " + response.Error);
                }
            });

        levelFinishedScreen.Open(signal.Time);
    }

    private void OnExitLevelSignal(ExitLevelSignal obj)
    {
        SceneManager.UnloadSceneAsync(_loadedLevel);
        levelFinishedScreen.Close();
        gameCanvas.enabled = false;
        mainMenuCanvas.enabled = true;
    }

    private void CloseAllUI()
    {
        gameCanvas.enabled = false;
        mainMenuCanvas.enabled = false;
        optionsCanvas.enabled = false;
        levelSelectionCanvas.Close();
        levelFinishedScreen.Close();
        usernameInputScreen.Close();
    }
}