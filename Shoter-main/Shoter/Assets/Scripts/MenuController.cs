using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private CameraController cameraController;
    public AudioMixerGroup mixer;
    public Slider sliderSensitivity;
    public Slider sliderVolume;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        sliderSensitivity.minValue = 0.05f;
        UpdateSettings();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnOffMenu();
        }
    }

    public void OnOffMenu()
    {
        if (panel.activeSelf == false)
        {
            panel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        }
        else if (panel.activeSelf == true)
        {
            panel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            ChangeSensitivity();
            ChangeVolume();
            SaveChanges();
            Time.timeScale = 1;
        }
    }

    private void ChangeSensitivity()
    {
        cameraController.sensX = sliderSensitivity.value * 1000f;
        cameraController.sensY = sliderSensitivity.value * 1000f;
    }
    private void ChangeVolume()
    {
        mixer.audioMixer.SetFloat("Master", Mathf.Lerp(-80, 0, sliderVolume.value));
    }
    private void SaveChanges()
    {
        PlayerPrefs.SetFloat("Sensitivity", sliderSensitivity.value);
        PlayerPrefs.SetFloat("Volume", sliderVolume.value);
    }
    private void UpdateSettings()
    {
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            sliderSensitivity.value = PlayerPrefs.GetFloat("Sensitivity");
            ChangeSensitivity();
        }
        if (PlayerPrefs.HasKey("Volume"))
        {
            sliderVolume.value = PlayerPrefs.GetFloat("Volume");
            ChangeVolume();
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
}
