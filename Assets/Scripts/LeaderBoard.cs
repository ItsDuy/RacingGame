using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text currentRunTimeText;
    public TMP_Text bestRunTimeText;
    public Image[] starImages;
    public Sprite filledStar;
    public Sprite emptyStar;

    private float currentRunTime = 0f;
    private float bestRunTime = Mathf.Infinity;

    private const string BestRunTimeKey = "BestRunTime";

    private void Start()
    {
        LoadBestRunTime();
        UpdateBestRunTimeUI();
    }

    public void SetRunTime(float runTime)
    {
        currentRunTime = runTime;
        Debug.Log($"Current Run Time Set: {currentRunTime}");
        UpdateCurrentRunTimeUI();

        if (currentRunTime < bestRunTime)
        {
            bestRunTime = currentRunTime;
            Debug.Log($"New Best Run Time: {bestRunTime}");
            SaveBestRunTime();
            UpdateBestRunTimeUI();
        }
        else
        {
            Debug.Log($"Current Run Time is not better than Best Run Time: {bestRunTime}");
        }

        int starsEarned = CalculateStars(runTime);
        UpdateStarsUI(starsEarned);
    }

    private void UpdateCurrentRunTimeUI()
    {
        if (currentRunTimeText != null)
        {
            currentRunTimeText.text = "Current Time: " + FormatTime(currentRunTime);
            Debug.Log($"Current Run Time UI Updated: {currentRunTimeText.text}");
        }
        else
        {
            Debug.LogError("CurrentRunTimeText is not assigned in the LeaderBoard.");
        }
    }

    private void UpdateBestRunTimeUI()
    {
        if (bestRunTimeText != null)
        {
            string bestTimeString = bestRunTime < Mathf.Infinity ? FormatTime(bestRunTime) : "N/A";
            bestRunTimeText.text = "Best Time: " + bestTimeString;
            Debug.Log($"Best Run Time UI Updated: {bestRunTimeText.text}");
        }
        else
        {
            Debug.LogError("BestRunTimeText is not assigned in the LeaderBoard.");
        }
    }

    private void UpdateStarsUI(int starsEarned)
    {
        for (int i = 0; i < starImages.Length; i++)
        {
            starImages[i].sprite = i < starsEarned ? filledStar : emptyStar;
        }
        Debug.Log($"Stars UI Updated: {starsEarned} stars earned.");
    }

    private int CalculateStars(float runTime)
    {
        if (runTime < 90f)
        {
            return 3;
        }
        else if (runTime < 120f)
        {
            return 2;
        }
        else if (runTime < 150f)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 100) % 100);
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    private void LoadBestRunTime()
    {
        if (PlayerPrefs.HasKey(BestRunTimeKey))
        {
            bestRunTime = PlayerPrefs.GetFloat(BestRunTimeKey, Mathf.Infinity);
            Debug.Log($"Loaded Best Run Time: {bestRunTime}");
        }
        else
        {
            Debug.Log("No Best Run Time found. Starting fresh.");
        }
    }

    private void SaveBestRunTime()
    {
        PlayerPrefs.SetFloat(BestRunTimeKey, bestRunTime);
        PlayerPrefs.Save();
        Debug.Log($"Saved Best Run Time: {bestRunTime}");
    }

    public void ResetBestRunTime()
    {
        bestRunTime = Mathf.Infinity;
        SaveBestRunTime();
        UpdateBestRunTimeUI();
        Debug.Log("Best Run Time has been reset.");
    }
}
