using UnityEngine;
using TMPro;  // Import the TextMeshPro namespace
using System.Collections;

public class Countdown : MonoBehaviour
{
    [SerializeField] private TMP_Text countdownText; // Reference to TMP_Text component
    private CarController carController;             // Reference to the CarController
    private StopWatch stopWatch;                     // Reference to the StopWatch

    private void Start()
    {
        carController = FindObjectOfType<CarController>();
        stopWatch = FindObjectOfType<StopWatch>(); // Find the StopWatch component

        if (carController != null)
        {
            carController.enabled = false; // Disable car control initially
        }

        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        int countdownTime = 3;

        while (countdownTime > 0)
        {
            countdownText.text = countdownTime.ToString(); // Update the TMP text
            yield return new WaitForSeconds(1f);          // Wait for 1 second
            countdownTime--;
        }

        countdownText.text = "Go!";                        // Display "Go!"
        
        yield return new WaitForSeconds(1f);               // Wait before removing "Go!"
        
        countdownText.text = "";                           // Clear the text

        if (carController != null)
        {
            carController.enabled = true;  // Re-enable car control
        }

        if (stopWatch != null)
        {
            stopWatch.StartStopwatch(); // Start the stopwatch
        }
    }
}
