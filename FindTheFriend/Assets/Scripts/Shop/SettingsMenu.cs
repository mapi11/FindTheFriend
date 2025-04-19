using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [Header("Настройки звука")]
    public Slider volumeSlider;
    public TMP_Text volumeText;

    [Header("Настройки мыши")]
    public Slider sensitivitySlider;
    public TMP_Text sensitivityText;
    public float minSensitivity = 0.5f;
    public float maxSensitivity = 5f;

    private AudioSource musicPlayer;
    private MouseLook mouseLook;

    private void Start()
    {
        // Находим компоненты
        musicPlayer = GameObject.Find("MusicPlayer").GetComponent<AudioSource>();
        mouseLook = FindObjectOfType<MouseLook>();

        InitializeVolumeSettings();
        InitializeSensitivitySettings();
    }

    private void InitializeVolumeSettings()
    {
        if (musicPlayer != null && volumeSlider != null)
        {
            volumeSlider.value = musicPlayer.volume;
            volumeSlider.onValueChanged.AddListener(ChangeVolume);
            UpdateVolumeText();
        }
    }

    private void InitializeSensitivitySettings()
    {
        if (mouseLook != null && sensitivitySlider != null)
        {
            // Настраиваем слайдер чувствительности
            sensitivitySlider.minValue = minSensitivity;
            sensitivitySlider.maxValue = maxSensitivity;
            sensitivitySlider.value = mouseLook.sensitivity;

            sensitivitySlider.onValueChanged.AddListener(ChangeSensitivity);
            UpdateSensitivityText();
        }
    }

    public void ChangeVolume(float volume)
    {
        if (musicPlayer != null)
        {
            musicPlayer.volume = volume;
            UpdateVolumeText();
        }
    }

    public void ChangeSensitivity(float sensitivity)
    {
        if (mouseLook != null)
        {
            mouseLook.sensitivity = sensitivity;
            UpdateSensitivityText();
        }
    }

    private void UpdateVolumeText()
    {
        if (volumeText != null)
        {
            volumeText.text = Mathf.RoundToInt(volumeSlider.value * 100) + "%";
        }
    }

    private void UpdateSensitivityText()
    {
        if (sensitivityText != null)
        {
            sensitivityText.text = sensitivitySlider.value.ToString("0.0");
        }
    }

    private void OnDestroy()
    {
        // Отписываемся от событий при уничтожении
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveListener(ChangeVolume);
        }

        if (sensitivitySlider != null)
        {
            sensitivitySlider.onValueChanged.RemoveListener(ChangeSensitivity);
        }
    }
}
