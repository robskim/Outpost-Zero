using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    [Header("UI Components")]
    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;
    public Toggle sfxToggle;
    public TMP_Dropdown framerateDropdown;

    [Header("Audio Sources")]
    public AudioSource sfxAudioSource;

    private float masterVolume = 1f;
    private float sfxVolume = 1f;
    private int targetFrameRate = -1; // -1 indicates unlimited

    void Start()
    {
        // Load saved settings or use default
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        targetFrameRate = PlayerPrefs.GetInt("TargetFrameRate", -1);

        masterVolumeSlider.value = masterVolume;
        sfxVolumeSlider.value = sfxVolume;

        sfxToggle.isOn = PlayerPrefs.GetInt("SFXMuted", 0) == 0;

        // Set dropdown initial value
        int dropdownIndex = FramerateToDropdownIndex(targetFrameRate);
        framerateDropdown.value = dropdownIndex;

        UpdateAudioSettings();
        UpdateFramerate();
    }

    public void OnMasterVolumeChanged(float value)
    {
        masterVolume = value;
        UpdateAudioSettings();
        SaveSettings();
    }

    public void OnSFXVolumeChanged(float value)
    {
        sfxVolume = value;
        UpdateAudioSettings();
        SaveSettings();
    }

    public void OnSFXToggleChanged(bool isOn)
    {
        sfxVolume = isOn ? masterVolume : 0f;
        UpdateAudioSettings();
        SaveSettings();
    }

    public void OnFramerateDropdownChanged(int index)
    {
        targetFrameRate = DropdownIndexToFramerate(index);
        UpdateFramerate();
        SaveSettings();
    }

    private void UpdateAudioSettings()
    {
        AudioListener.volume = masterVolume;
        sfxAudioSource.volume = sfxVolume;
    }

    private void UpdateFramerate()
    {
        Application.targetFrameRate = targetFrameRate;
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetInt("SFXMuted", sfxToggle.isOn ? 0 : 1);
        PlayerPrefs.SetInt("TargetFrameRate", targetFrameRate);
        PlayerPrefs.Save();
    }

    private int DropdownIndexToFramerate(int index)
    {
        switch (index)
        {
            case 0: return 30;
            case 1: return 60;
            case 2: return 120;
            case 3: return -1; // Unlimited
            default: return -1;
        }
    }

    private int FramerateToDropdownIndex(int framerate)
    {
        switch (framerate)
        {
            case 30: return 0;
            case 60: return 1;
            case 120: return 2;
            case -1: return 3; // Unlimited
            default: return 3; // Default to Unlimited
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
