using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionScreen : CanvasScreen
{
    [SerializeField] private Transform levelsContainer;
    [SerializeField] private Button levelButtonPrefab;
    [SerializeField] private GameController gameController;

    public void Init(List<LevelConfig> levelConfigs)
    {
        foreach (var levelConfig in levelConfigs)
        {
            var levelButton = Instantiate(levelButtonPrefab, levelsContainer);
            levelButton.name = $"Level {levelConfig.levelNumber} Button";
            levelButton.GetComponentInChildren<TMP_Text>().text = levelConfig.levelNumber.ToString();
            levelButton.onClick.AddListener(() => gameController.LoadLevel(levelConfig));
        }
    }
}