using System;
using TMPro;
using UnityEngine;

public class LevelTimerUI : MonoBehaviour
{
    [SerializeField] private GameInfoSO gameInfoSO;
    [SerializeField] private TMP_Text timerText;

    private void Awake()
    {
        gameInfoSO.timer.Changed += TimerOnChanged;
    }

    private void TimerOnChanged(float newValue)
    {
        timerText.text = newValue.ToString("F2");
    }
}