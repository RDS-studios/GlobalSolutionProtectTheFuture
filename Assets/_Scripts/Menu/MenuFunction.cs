using SmallHedge.SoundManager;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuFunction : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] TMP_Dropdown dropDownResolução;
    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider musicSlider;

    [Header("Audio")]
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource musicSource;



    [SerializeField] GameObject configCanvas;

    private Resolution[] resolutions;
    private bool isFullScreen;

    void Start()
    {
        // Load available resolutions
        resolutions = Screen.resolutions;
        dropDownResolução.ClearOptions();

        int currentResIndex = 0;
        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        dropDownResolução.AddOptions(options);
        dropDownResolução.value = currentResIndex;
        dropDownResolução.RefreshShownValue();

        // Set initial fullscreen and volume states
        fullscreenToggle.isOn = Screen.fullScreen;

        if (sfxSource != null) sfxSlider.value = sfxSource.volume;
        if (musicSource != null) musicSlider.value = musicSource.volume;
    }

    public void ApplySettings()
    {
        int selectedIndex = dropDownResolução.value;
        Resolution selectedResolution = resolutions[selectedIndex];

        isFullScreen = fullscreenToggle.isOn;

        // Apply resolution and fullscreen
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, isFullScreen);

        // Apply volume to audio sources
        if (sfxSource != null) sfxSource.volume = sfxSlider.value;
        if (musicSource != null) musicSource.volume = musicSlider.value;

        Debug.Log("Settings applied");
    }

    public void ApplySettingsAndCloseConfig()
    {
        ApplySettings();
        ExitConfig();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(2); // Load the game scene
        sfxSource.Play(); // Optional: play feedback sound
        
    }

    public void BackToMenu()
    {
        sfxSource.Play(); // Optional feedback
        // Add logic if needed
    }

    public void EnterConfig()
    {
        
       configCanvas.SetActive(true); // Show configuration canvas   
       // SoundManager.PlaySound(SoundType.BtnMenu); // Play button sound
    }

    public void ExitConfig()
    {
        
        configCanvas.SetActive(false); // Hide configuration canvas 
       // SoundManager.PlaySound(SoundType.BtnBack); // Play button sound
    }   

    public void QuitGame()
    {
       
        Application.Quit();
        //SoundManager.PlaySound(SoundType.BtnMenu); // Play button sound
    }

    // Optional: Live preview of volume change
    public void OnSFXVolumeChanged(float value)
    {
        if (sfxSource != null) sfxSource.volume = value;
    }

    public void OnMusicVolumeChanged(float value)
    {
        if (musicSource != null) musicSource.volume = value;
    }
}
