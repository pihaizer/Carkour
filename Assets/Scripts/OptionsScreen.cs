using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class OptionsScreen : MonoBehaviour
{
    [SerializeField] private SegmentedControl graphicsSegmentedControl;
    [SerializeField] private Toggle mobileControlsToggle;

    [SerializeField] private MobileControls mobileControls;

    private const string _mobileControlsKey = "mobileControlsEnabled";

    private void Awake()
    {
        graphicsSegmentedControl.onValueChanged.AddListener(OnGraphicsValueChanged);
        mobileControlsToggle.onValueChanged.AddListener(SetMobileControlsEnabled);
    }

    private void Start()
    {
        graphicsSegmentedControl.selectedSegmentIndex = QualitySettings.GetQualityLevel();
        
        bool mobileControlsTurnedOn;
        if (PlayerPrefs.HasKey(_mobileControlsKey))
        {
            mobileControlsTurnedOn = PlayerPrefs.GetInt(_mobileControlsKey) != 0;
        }
        else
        {
            mobileControlsTurnedOn = Application.platform is RuntimePlatform.Android or RuntimePlatform.IPhonePlayer;
        }
        mobileControlsToggle.isOn = mobileControlsTurnedOn;
        SetMobileControlsEnabled(mobileControlsTurnedOn);
    }

    private void SetMobileControlsEnabled(bool value)
    {
        mobileControls.gameObject.SetActive(value);
        PlayerPrefs.SetInt(_mobileControlsKey, value ? 1 : 0);
    }

    private void OnGraphicsValueChanged(int qualityLevel)
    {
        QualitySettings.SetQualityLevel(qualityLevel);
    }
}