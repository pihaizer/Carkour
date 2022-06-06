using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlatformTextSetter : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private List<PlatformText> platformTexts;

    private void Start()
    {
        foreach (var platformText in platformTexts)
        {
            if (!platformText.Platforms.Contains(Application.platform)) continue;
            text.text = platformText.Text;
            return;
        }
    }

    [Serializable]
    public class PlatformText
    {
        public string Text;
        public List<RuntimePlatform> Platforms;
    }
}