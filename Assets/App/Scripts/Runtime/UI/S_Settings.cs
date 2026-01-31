using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class S_Settings : MonoBehaviour
{
    [TabGroup("References")]
    [Title("Script")]
    [SerializeField] private S_LoadUISettings loadUISettings;

    [TabGroup("References")]
    [Title("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [TabGroup("Outputs")]
    [SerializeField] private RSO_SettingsSaved rsoSettingsSaved;

    private bool isLoaded = false;
    private bool isSave = false;

    private List<Slider> listSlidersAudios = new();
    private List<TextMeshProUGUI> listTextAudios = new();

    private RSO_SettingsSaved rsoSettingsSavedOld;

    private int step = 1;
    private float stepFloat = 1f;
    private bool startMove = false;
    private float holdTime = 0f;
    private bool isStick = false;
    private float maxStep = 3f;
    private float accelerationTime = 3f;

    private void OnEnable()
    {
        rsoSettingsSavedOld = ScriptableObject.CreateInstance<RSO_SettingsSaved>();
        rsoSettingsSavedOld.Value = rsoSettingsSaved.Value.Clone();

        isLoaded = false;
        isSave = false;
    }

    private void Update()
    {
        if (isLoaded && (EventSystem.current.currentSelectedGameObject == listSlidersAudios[0].gameObject || EventSystem.current.currentSelectedGameObject == listSlidersAudios[1].gameObject || EventSystem.current.currentSelectedGameObject == listSlidersAudios[2].gameObject || EventSystem.current.currentSelectedGameObject == listSlidersAudios[3].gameObject) && Gamepad.current != null && startMove)
        {
            Vector2 dpad = Gamepad.current.dpad.ReadValue();
            Vector2 leftStick = Gamepad.current.leftStick.ReadValue();
            Vector2 rightStick = Gamepad.current.rightStick.ReadValue();

            bool stickActive = Mathf.Abs(leftStick.x) > 0.5f;
            bool stick2Active = Mathf.Abs(rightStick.x) > 0.5f;

            if (stickActive || stick2Active)
            {
                if (!isStick)
                {
                    isStick = true;
                    holdTime = 0f;
                    step = 1;
                    stepFloat = 1f;
                }

                holdTime += Time.unscaledDeltaTime;

                float t = Mathf.Clamp01(holdTime / accelerationTime);
                stepFloat = Mathf.Lerp(1f, maxStep, t);

                step = (int)Mathf.Clamp(Mathf.FloorToInt(stepFloat), 1, maxStep);
            }
            else if (isStick)
            {
                startMove = false;
                step = 1;
                stepFloat = 1f;
                holdTime = 0f;
            }
            else
            {
                step = 1;
                stepFloat = 1f;
                holdTime = 0f;
            }
        }
        else
        {
            isStick = false;
            step = 1;
            stepFloat = 1f;
            holdTime = 0f;
        }
    }

    public void Setup(List<Slider> listSlidersVolumes, List<TextMeshProUGUI> listTextVolumes)
    {
        listSlidersAudios = listSlidersVolumes;
        listTextAudios = listTextVolumes;

        isLoaded = true;
    }

    private Resolution GetResolutions(int index)
    {
        List<Resolution> resolutionsPC = new(Screen.resolutions);

        resolutionsPC = resolutionsPC
            .Where(r => r.width >= 1280 && r.height >= 720)
            .OrderByDescending(r => r.width * r.height)
            .ThenByDescending(r => r.refreshRateRatio.value)
            .ToList();

        Resolution resolution = resolutionsPC[0];

        for (int i = 0; i < resolutionsPC.Count; i++)
        {
            Resolution res = resolutionsPC[i];

            if (i == index) resolution = res;
        }

        return resolution;
    }

    public void UpdateResolutions(int index)
    {
        if (isLoaded && rsoSettingsSaved.Value.resolutionIndex != index) rsoSettingsSaved.Value.resolutionIndex = index;
    }

    public void UpdateFullscreen(bool value)
    {
        if (isLoaded && rsoSettingsSaved.Value.fullScreen != value) rsoSettingsSaved.Value.fullScreen = value;
    }

    public void UpdateMainVolume(float value)
    {
        if (isLoaded && rsoSettingsSaved.Value.listVolumes[0].volume != value)
        {
            float newValue = 0;

            if (Gamepad.current != null)
            {
                startMove = true;

                float diff = Mathf.Sign(value - rsoSettingsSaved.Value.listVolumes[0].volume);
                newValue = Mathf.RoundToInt(diff * step);
                newValue = Mathf.Clamp(rsoSettingsSaved.Value.listVolumes[0].volume + newValue, listSlidersAudios[0].minValue, listSlidersAudios[0].maxValue);
                listSlidersAudios[0].SetValueWithoutNotify(newValue);
            }
            else newValue = value;

            rsoSettingsSaved.Value.listVolumes[0].volume = newValue;

            listTextAudios[0].text = $"{newValue}%";

            audioMixer.SetFloat(rsoSettingsSaved.Value.listVolumes[0].name, 40 * Mathf.Log10(Mathf.Max(rsoSettingsSaved.Value.listVolumes[0].volume, 1) / 100));
        }
    }

    public void UpdateMusicVolume(float value)
    {
        if (isLoaded && rsoSettingsSaved.Value.listVolumes[1].volume != value)
        {
            float newValue = 0;

            if (Gamepad.current != null)
            {
                startMove = true;

                float diff = Mathf.Sign(value - rsoSettingsSaved.Value.listVolumes[1].volume);
                newValue = Mathf.RoundToInt(diff * step);
                newValue = Mathf.Clamp(rsoSettingsSaved.Value.listVolumes[1].volume + newValue, listSlidersAudios[1].minValue, listSlidersAudios[1].maxValue);
                listSlidersAudios[1].SetValueWithoutNotify(newValue);
            }
            else newValue = value;

            rsoSettingsSaved.Value.listVolumes[1].volume = newValue;

            listTextAudios[1].text = $"{newValue}%";

            audioMixer.SetFloat(rsoSettingsSaved.Value.listVolumes[1].name, 40 * Mathf.Log10(Mathf.Max(rsoSettingsSaved.Value.listVolumes[1].volume, 1) / 100));
        }
    }

    public void UpdateSoundsVolume(float value)
    {
        if (isLoaded && rsoSettingsSaved.Value.listVolumes[2].volume != value)
        {
            float newValue = 0;

            if (Gamepad.current != null)
            {
                startMove = true;

                float diff = Mathf.Sign(value - rsoSettingsSaved.Value.listVolumes[2].volume);
                newValue = Mathf.RoundToInt(diff * step);
                newValue = Mathf.Clamp(rsoSettingsSaved.Value.listVolumes[2].volume + newValue, listSlidersAudios[2].minValue, listSlidersAudios[2].maxValue);
                listSlidersAudios[2].SetValueWithoutNotify(newValue);
            }
            else newValue = value;

            rsoSettingsSaved.Value.listVolumes[2].volume = newValue;

            listTextAudios[2].text = $"{newValue}%";

            audioMixer.SetFloat(rsoSettingsSaved.Value.listVolumes[2].name, 40 * Mathf.Log10(Mathf.Max(rsoSettingsSaved.Value.listVolumes[2].volume, 1) / 100));
        }
    }

    public void UpdateUIVolume(float value)
    {
        if (isLoaded && rsoSettingsSaved.Value.listVolumes[3].volume != value)
        {
            float newValue = 0;

            if (Gamepad.current != null)
            {
                startMove = true;

                float diff = Mathf.Sign(value - rsoSettingsSaved.Value.listVolumes[3].volume);
                newValue = Mathf.RoundToInt(diff * step);
                newValue = Mathf.Clamp(rsoSettingsSaved.Value.listVolumes[3].volume + newValue, listSlidersAudios[3].minValue, listSlidersAudios[3].maxValue);
                listSlidersAudios[3].SetValueWithoutNotify(newValue);
            }
            else newValue = value;

            rsoSettingsSaved.Value.listVolumes[3].volume = newValue;

            listTextAudios[3].text = $"{newValue}%";

            audioMixer.SetFloat(rsoSettingsSaved.Value.listVolumes[3].name, 40 * Mathf.Log10(Mathf.Max(rsoSettingsSaved.Value.listVolumes[3].volume, 1) / 100));
        }
    }

    public void SaveSettings()
    {
        isSave = true;

        Resolution resolution = GetResolutions(rsoSettingsSaved.Value.resolutionIndex);

        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode, resolution.refreshRateRatio);

        if (rsoSettingsSaved.Value.fullScreen) Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        else Screen.fullScreenMode = FullScreenMode.Windowed;

        Screen.fullScreen = rsoSettingsSaved.Value.fullScreen;
    }

    public void Close()
    {
        if (!isSave)
        {
            rsoSettingsSaved.Value = rsoSettingsSavedOld.Value.Clone();

            audioMixer.SetFloat(rsoSettingsSaved.Value.listVolumes[0].name, 40 * Mathf.Log10(Mathf.Max(rsoSettingsSaved.Value.listVolumes[0].volume, 1) / 100));
            audioMixer.SetFloat(rsoSettingsSaved.Value.listVolumes[1].name, 40 * Mathf.Log10(Mathf.Max(rsoSettingsSaved.Value.listVolumes[1].volume, 1) / 100));
            audioMixer.SetFloat(rsoSettingsSaved.Value.listVolumes[2].name, 40 * Mathf.Log10(Mathf.Max(rsoSettingsSaved.Value.listVolumes[2].volume, 1) / 100));
            audioMixer.SetFloat(rsoSettingsSaved.Value.listVolumes[3].name, 40 * Mathf.Log10(Mathf.Max(rsoSettingsSaved.Value.listVolumes[3].volume, 1) / 100));

            loadUISettings.LoadUI();
        }
    }
}