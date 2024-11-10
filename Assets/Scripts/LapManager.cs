using System.Collections.Generic; // Import necessary namespace for HashSet
using UnityEngine;
using TMPro; // Import TMP for text display, if needed

public class LapManager : MonoBehaviour
{
    public int currentLap = 0;
    public int maxLap = 3;
    public int totalCheckpoints; 
    public StopWatch stopWatch;
    public LeaderBoard leaderBoard;
    public PlayerProgress playerProgress;
    private HashSet<int> checkpointsPassed = new HashSet<int>();
    public GameObject mainUI;
    public GameObject winPanel;
    public TMP_Text lapDisplay; 
    

    void Start()
    {
        UpdateLapDisplay();
        Debug.Log("Lap manager started.");
        mainUI.SetActive(true);
        winPanel.SetActive(false);
    }

    public void CheckpointPassed(int checkpointIndex)
    {
        if (!checkpointsPassed.Contains(checkpointIndex))
        {
            Debug.Log($"Checkpoint {checkpointIndex} passed.");
            checkpointsPassed.Add(checkpointIndex);
            playerProgress.UpdateCheckpoint();
        }
    }

    public void TryCompleteLap()
    {
        if (checkpointsPassed.Count == totalCheckpoints)
        {
            currentLap++;
            Debug.Log($"Lap {currentLap} completed.");

            checkpointsPassed.Clear(); 

            if (currentLap > maxLap)
            {
                Debug.Log("Race Finished!");
                stopWatch.StopStopwatch();
                mainUI.SetActive(false);
                winPanel.SetActive(true);
                leaderBoard.SetRunTime(stopWatch.GetElapsedTime());
                
            }
            else
            {
                UpdateLapDisplay();
            }
        }
        else
        {
            Debug.LogWarning($"Missed checkpoints! Passed {checkpointsPassed.Count}/{totalCheckpoints} checkpoints.");
        }
    }

    private void UpdateLapDisplay()
    {
        if (lapDisplay != null)
        {
            lapDisplay.text = $"Lap: {currentLap}/{maxLap}";
            Debug.Log($"Displayed lap: {currentLap} out of {maxLap}");
        }
    }
    public int GetMaxLap()
    {
        return maxLap;
    }
    public int GetTotalCheckpoint()
    {
        return totalCheckpoints;
    }
}
