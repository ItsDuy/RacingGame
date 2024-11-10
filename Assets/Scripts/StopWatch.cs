using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class StopWatch : MonoBehaviour
{
    public TMP_Text timeDisplay;
    private float elapsedTime = -0.02f; 
    private bool isRunning = false;

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateDisplay();
        }
    }

    public void StartStopwatch()
    {
        elapsedTime = -0.02f; 
        isRunning = true;
    }

    public void StopStopwatch()
    {
        isRunning = false;
    }

    public void ResetStopwatch()
    {
        isRunning = false;
        elapsedTime = -0.02f; 
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        float milliseconds = (elapsedTime * 100) % 100; 

        timeDisplay.text = string.Format("{0:00}:{1:00}:{2:00}", 
            minutes, seconds, Mathf.FloorToInt(milliseconds));
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}
