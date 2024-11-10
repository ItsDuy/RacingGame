using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CarAudioScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CarController carController; // Assign via Inspector
    private AudioManager audioManager;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip engineClip; // Assign via Inspector
    private AudioSource engineAudioSource;

    [Header("Speed Audio Mapping")]
    [SerializeField] private float minPitch = 0.8f;
    [SerializeField] private float maxPitch = 1.5f;

    // Optionally, you can expose speed ranges if needed
    // [SerializeField] private float minSpeed = 0f;
    // [SerializeField] private float maxSpeed = 200f;

    private void Awake()
    {
        InitializeComponents();
    }

    private void OnEnable()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        // Initialize AudioManager using Singleton
        if (audioManager == null)
        {
            audioManager = AudioManager.Instance;
            if (audioManager == null)
            {
                Debug.LogError("AudioManager instance not found! Ensure that an AudioManager is present in the scene.");
                // Early exit as AudioManager is critical
                enabled = false;
                return;
            }
        }

        // Assign CarController if not assigned via Inspector
        if (carController == null)
        {
            carController = FindObjectOfType<CarController>();
            if (carController == null)
            {
                Debug.LogError("CarController not found in the scene! Please assign it manually.");
                // Early exit as CarController is critical
                enabled = false;
                return;
            }
            else
            {
                Debug.Log("CarController successfully found.");
            }
        }
        else
        {
            Debug.Log("CarController already assigned via Inspector.");
        }

        // Initialize AudioSource
        if (engineAudioSource == null)
        {
            engineAudioSource = GetComponent<AudioSource>();
            if (engineAudioSource == null)
            {
                Debug.LogError("AudioSource component missing from the car! This should not happen due to [RequireComponent].");
                // Early exit as AudioSource is critical
                enabled = false;
                return;
            }
            else
            {
                if (engineClip != null)
                {
                    engineAudioSource.clip = engineClip;
                }
                else
                {
                    Debug.LogError("EngineClip is not assigned! Please assign an engine sound AudioClip.");
                }

                engineAudioSource.loop = true;
                engineAudioSource.playOnAwake = false;
                engineAudioSource.volume = 0.5f;
            }
        }
    }

    private void Start()
    {
        if (engineAudioSource != null && engineClip != null)
        {
            if (!engineAudioSource.isPlaying)
            {
                engineAudioSource.Play();
                Debug.Log("Engine sound started playing on loop.");
            }
        }
        else
        {
            Debug.LogWarning("EngineAudioSource or EngineClip is not properly assigned. Engine sound will not play.");
        }
    }

    private void Update()
    {
        if (carController == null || audioManager == null || engineAudioSource == null || engineClip == null)
        {
            // Use Debug.LogWarning to avoid spamming the console with errors
            if (carController == null)
                Debug.LogWarning("CarController reference is missing in CarAudioScript.");

            if (audioManager == null)
                Debug.LogWarning("AudioManager reference is missing in CarAudioScript.");

            if (engineAudioSource == null)
                Debug.LogWarning("Engine AudioSource is not initialized in CarAudioScript.");

            if (engineClip == null)
                Debug.LogWarning("EngineClip is not assigned in CarAudioScript.");

            return; // Exit early to prevent further errors
        }

        // Adjust engine sound based on car speed
        float speed = carController.GetCurrentSpeed();

        float maxSpeed = carController.GetMaxSpeed();
        if (maxSpeed <= 0f)
        {
            Debug.LogWarning("MaxSpeed is zero or negative in CarController. Cannot normalize speed.");
            return;
        }

        float normalizedSpeed = Mathf.Clamp01(speed / maxSpeed);

        // Adjust pitch based on speed
        float targetPitch = Mathf.Lerp(minPitch, maxPitch, normalizedSpeed);
        engineAudioSource.pitch = targetPitch;

        // Optional: Adjust volume or other audio properties based on speed
        float targetVolume = Mathf.Lerp(0.3f, 1f, normalizedSpeed);
        engineAudioSource.volume = targetVolume;

        // Debugging purposes (optional)
        // Debug.Log($"Speed: {speed}, NormalizedSpeed: {normalizedSpeed}, Pitch: {engineAudioSource.pitch}");
    }
}
