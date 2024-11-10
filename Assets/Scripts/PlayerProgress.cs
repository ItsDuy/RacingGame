using UnityEngine;
using UnityEngine.UI;

public class PlayerProgress : MonoBehaviour
{
    [Header("References")]
    public LapManager lapManager; // Assign via Inspector

    [Header("Progression Settings")]
    [Tooltip("Array of sprites representing each progress stage.")]
    [SerializeField] private Sprite[] progressSprites; // Array of 6 Sprites

    [Tooltip("Image component that displays the current progress sprite.")]
    [SerializeField] private Image progressImage; // Assign the ProgressDisplay Image here

    private int totalCheckpoints;
    private int maxLap;
    private int currentCheckpoint = 0;
    private int maxCheckpoints;

    private int currentSpriteIndex = 0; // Tracks the current sprite being displayed

    private void Start()
    {
        // Validate references
        if (lapManager == null)
        {
            Debug.LogError("LapManager is not assigned in the PlayerProgress script.");
            return;
        }

        if (progressSprites == null || progressSprites.Length == 0)
        {
            Debug.LogError("Progress Sprites array is not assigned or empty in the PlayerProgress script.");
            return;
        }

        if (progressImage == null)
        {
            Debug.LogError("Progress Image is not assigned in the PlayerProgress script.");
            return;
        }

        // Validate the number of sprites
        if (progressSprites.Length != 6)
        {
            Debug.LogWarning($"Expected 6 progress sprites, but found {progressSprites.Length}. Ensure you have exactly 6 sprites.");
        }

        // Initialize variables
        totalCheckpoints = lapManager.GetTotalCheckpoint();
        maxLap = lapManager.GetMaxLap();
        maxCheckpoints = totalCheckpoints * maxLap;
        currentCheckpoint = 0;
        currentSpriteIndex = 0;

        // Initialize progress image with the first sprite
        InitializeProgressImage();
    }

    /// <summary>
    /// Initializes the progress image by setting it to the first sprite.
    /// </summary>
    private void InitializeProgressImage()
    {
        if (progressSprites.Length > 0)
        {
            progressImage.sprite = progressSprites[0];
        }
    }

    /// <summary>
    /// Call this method to update the checkpoint when the player completes a part of the race.
    /// </summary>
    public void UpdateCheckpoint()
    {
        if (currentCheckpoint < maxCheckpoints)
        {
            currentCheckpoint++;
            UpdateProgressImage();
        }
        else
        {
            Debug.Log("Maximum checkpoints reached.");
        }
    }

    /// <summary>
    /// Sets the current checkpoint to a specific value and updates the progress image.
    /// </summary>
    /// <param name="checkpoint">The checkpoint number to set.</param>
    public void SetCheckPoint(int checkpoint)
    {
        currentCheckpoint = Mathf.Clamp(checkpoint, 0, maxCheckpoints);
        UpdateProgressImage();
    }

    /// <summary>
    /// Sets the maximum number of checkpoints.
    /// </summary>
    /// <param name="maxCheckpoints">The maximum number of checkpoints.</param>
    public void SetMaxCheckPoint(int maxCheckpoints)
    {
        this.maxCheckpoints = maxCheckpoints;
        // Optionally, reset current progress
        currentCheckpoint = 0;
        currentSpriteIndex = 0;
        InitializeProgressImage();
    }

    /// <summary>
    /// Updates the displayed progress sprite based on the current checkpoint.
    /// </summary>
    private void UpdateProgressImage()
    {
        if (progressSprites.Length == 0 || progressImage == null)
            return;

        // Calculate progress fraction
        float progressFraction = (float)currentCheckpoint / maxCheckpoints;

        // Determine the corresponding sprite index
        // Ensure that when progressFraction is 1, it selects the last sprite
        int newSpriteIndex = Mathf.Clamp(Mathf.FloorToInt(progressFraction * progressSprites.Length), 0, progressSprites.Length - 1);

        if (newSpriteIndex != currentSpriteIndex)
        {
            // Update the sprite
            progressImage.sprite = progressSprites[newSpriteIndex];

            // Update the current sprite index
            currentSpriteIndex = newSpriteIndex;
        }
    }
}
