using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RGUnityTools.GameSettings
{
    public class RGGameSettingsUI : MonoBehaviour
    {
        public static RGGameSettingsUI Instance;

        public RGGameSettings Settings;

        public Action<RGGameSettings> OnSettingsChanged;

        #region GENERAL_SETTINGS
        #endregion

        #region AUDIO_SETTINGS
        [Header("Audio Settings:")]
        [SerializeField] Slider masterVolumeSlider;

        [SerializeField] Slider musicVolumeSlider;

        [SerializeField] Slider effectsVolumeSlider;

        [SerializeField] Slider speechVolumeSlider;
        #endregion

        #region VIDEO_SETTINGS
        [Header("Video Settings:")]
        [SerializeField] Button windowModeButton;
        [SerializeField] TextMeshProUGUI windowModeText;
        #endregion

        #region CONTROL_SETTINGS
        [Header("Control Settings:")]
        [SerializeField] Slider mouseSensitivitySlider;
        [SerializeField] Slider gamepadSensitivitySlider;
        [SerializeField] Slider xLookSensitivitySlider;
        [SerializeField] Slider yLookSensitivitySlider;
        #endregion

        #region UNITY_METHODS
        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }

        void Start()
        {
            Initialize();
            LoadSettings();
        }
        #endregion

        #region CORE_METHODS
        public void LoadSettings()
        {
            masterVolumeSlider.value = Settings.masterVolume;
            musicVolumeSlider.value = Settings.musicVolume;
            effectsVolumeSlider.value = Settings.effectsVolume;

            mouseSensitivitySlider.value = Settings.mouseSensitivity;
            gamepadSensitivitySlider.value = Settings.gamepadSensitivity;
            xLookSensitivitySlider.value = Settings.xSensitivity;
            yLookSensitivitySlider.value = Settings.ySensitivity;

            OnSettingsChanged?.Invoke(Settings);
        }

        public void SaveSettings()
        {
            //Create OnSaveSettings action for this!
            //Audio:
            RGAudioManager.instance.SetVolumeSettings(masterVolumeSlider.value, musicVolumeSlider.value, effectsVolumeSlider.value);
            Settings.masterVolume = masterVolumeSlider.value;
            Settings.musicVolume = musicVolumeSlider.value;
            Settings.effectsVolume = effectsVolumeSlider.value;

            //Controls:
            Settings.mouseSensitivity = mouseSensitivitySlider.value;
            Settings.gamepadSensitivity = gamepadSensitivitySlider.value;
            Settings.xSensitivity = xLookSensitivitySlider.value;
            Settings.ySensitivity = yLookSensitivitySlider.value;

            OnSettingsChanged?.Invoke(Settings);
        }

        void Initialize()
        {
            //Audio
            masterVolumeSlider.minValue = 0f;
            masterVolumeSlider.maxValue = 1f;

            musicVolumeSlider.minValue = 0f;
            musicVolumeSlider.maxValue = 1f;

            effectsVolumeSlider.minValue = 0f;
            effectsVolumeSlider.maxValue = 1f;

            //Video
            windowModeButton.onClick.AddListener(OnClickWindowMode);
            windowModeText.text = Screen.fullScreenMode.ToString();

            //Controls
            mouseSensitivitySlider.minValue = .1f;
            mouseSensitivitySlider.maxValue = 1f;

            gamepadSensitivitySlider.minValue = .1f;
            gamepadSensitivitySlider.maxValue = 1f;

            xLookSensitivitySlider.minValue = .1f;
            xLookSensitivitySlider.maxValue = 1f;

            yLookSensitivitySlider.minValue = .1f;
            yLookSensitivitySlider.maxValue = 1f;
        }
        #endregion

        #region EVENTS
        void OnClickWindowMode()
        {
            if ((int)Screen.fullScreenMode >= 3)
                Screen.fullScreenMode = 0;
            else
                Screen.fullScreenMode++;

            if(windowModeText) windowModeText.text = Screen.fullScreenMode.ToString();
        }
        #endregion
    }
}
