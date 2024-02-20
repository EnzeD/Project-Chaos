using TMPro;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private Light DirectionalMoonLight;
    [SerializeField] private LightingPreset Preset;
    [SerializeField, Range(0, 24)] private float TimeOfDay;
    [SerializeField] private float dayCycleLengthSeconds = 120;
    [SerializeField, Range(0, 1)] private float daylightPercentage = 0.5f; // 50% day, 50% night by default
    public TextMeshProUGUI timeText;

    // Audio sources for day and night music
    [SerializeField] private AudioSource dayMusicSource;
    [SerializeField] private AudioSource nightMusicSource;

    private void Start()
    {
        dayMusicSource.loop = true;
        nightMusicSource.loop = true;

        dayMusicSource.Play();
        nightMusicSource.Play();
        dayMusicSource.volume = 0f;
        nightMusicSource.volume = 0f;
    }

    private void Update()
    {
        if (Preset == null)
            return;

        // Calculate the length of the day and night in hours based on daylightPercentage
        float dayDurationHours = 24 * daylightPercentage;
        float nightDurationHours = 24 - dayDurationHours;

        // Determine the duration of day and night in game time
        // Assuming 6 AM to 6 PM (12 hours) is day and the rest is night
        float dayHours = 12; // 6 AM to 6 PM
        float nightHours = 12; // 6 PM to 6 AM

        if (Application.isPlaying)
        {
            TimeOfDay %= 24; // Clamp between 0-24

            // Determine the current phase (Day or Night) and calculate time increment accordingly
            if (TimeOfDay < 6 || TimeOfDay > 18) // Night Time
            {
                // Scale night hours according to the night duration defined by daylightPercentage
                float timeIncrement = (Time.deltaTime / (dayCycleLengthSeconds * (nightDurationHours / nightHours))) * 24;
                TimeOfDay += timeIncrement;
            }
            else // Day Time
            {
                // Scale day hours according to the day duration defined by daylightPercentage
                float timeIncrement = (Time.deltaTime / (dayCycleLengthSeconds * (dayDurationHours / dayHours))) * 24;
                TimeOfDay += timeIncrement;
            }

            UpdateLighting(TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }

        // Update the displayed time
        UpdateTimeDisplay();

        if (Application.isPlaying)
        {
            // Only manage music playback when the game is actually playing
            UpdateMusicPlayback();
        }
        else
        {
            // Ensure music is not playing or is muted when not in play mode
            EnsureAudioIsStoppedOrMuted();
        }
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if (DirectionalLight != null)
        {
            DirectionalLight.enabled = TimeOfDay >= 6 && TimeOfDay <= 18;
            if (DirectionalLight.enabled)
            {
                DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
                DirectionalLight.intensity = Preset.LightIntensity.Evaluate(timePercent);
                DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90, 120, 0));
            }
        }

        // Smooth transition for moonlight
        if (DirectionalMoonLight != null)
        {
            float moonIntensity = 0f;
            if (TimeOfDay >= 18 || TimeOfDay <= 6) // Night time
            {
                float nightTimePercent = 0f;
                if (TimeOfDay >= 18)
                {
                    // Transition from 18 to 24 to a 0-0.125 scale
                    nightTimePercent = (TimeOfDay - 18) / 48f;
                }
                else if (TimeOfDay <= 6)
                {
                    // Transition from 0 to 6 to a 0-0.125 scale, continuing from above
                    nightTimePercent = (6 - TimeOfDay) / 48f;
                }
                moonIntensity = Mathf.Clamp01(nightTimePercent);
            }

            DirectionalMoonLight.intensity = moonIntensity;
        }
    }

    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }

    void UpdateTimeDisplay()
    {
        // Calculate total minutes in the day
        int totalMinutes = Mathf.RoundToInt(TimeOfDay * 60);

        // Round to nearest 10 minutes
        totalMinutes = (totalMinutes / 10) * 10;

        // Convert back to hours and minutes
        int hours = totalMinutes / 60;
        int minutes = totalMinutes % 60;

        // Format time as "HH:MM"
        string formattedTime = string.Format("{0:00}:{1:00}", hours, minutes);

        // Update the UI text
        if (timeText != null)
        {
            timeText.text = formattedTime;
        }
    }

    private void UpdateMusicPlayback()
    {
        // Determine if it's day or night
        bool isDaytime = TimeOfDay >= 6 && TimeOfDay <= 18;

        // Calculate target volumes based on whether it's day or night
        float targetDayVolume = isDaytime ? 1 : 0;
        float targetNightVolume = isDaytime ? 0 : 1;

        // Smoothly transition the volume of the audio sources
        dayMusicSource.volume = Mathf.Lerp(dayMusicSource.volume, targetDayVolume, Time.deltaTime * 2); // Smooth transition for day music
        nightMusicSource.volume = Mathf.Lerp(nightMusicSource.volume, targetNightVolume, Time.deltaTime * 2); // Smooth transition for night music

        // Play or stop the audio sources based on the time of day
        if (isDaytime && !dayMusicSource.isPlaying)
        {
            if (MenuManager.IsGamePaused)
            {
                dayMusicSource.volume = 0.25f;
            }
            else
            {
                dayMusicSource.Play();
                nightMusicSource.Pause();
            }

        }
        else if (!isDaytime && !nightMusicSource.isPlaying)
        {
            if (MenuManager.IsGamePaused)
            {
                dayMusicSource.volume = 0.25f;
            }
            else
            {
                nightMusicSource.Play();
                dayMusicSource.Pause();
            }
        }
    }

    private void EnsureAudioIsStoppedOrMuted()
    {
        if (dayMusicSource.isPlaying)
            dayMusicSource.Stop(); 
        if (nightMusicSource.isPlaying)
            nightMusicSource.Stop();
    }
}
