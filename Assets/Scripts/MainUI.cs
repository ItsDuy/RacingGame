using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainUI : MonoBehaviour
{
    public TMP_Text countdownText;
    public GameObject countdownPanel;
    public StopWatch stopWatch;

    void Start()
    {
        StartCoroutine(StartRace());
    }

    IEnumerator StartRace()
    {
        Time.timeScale = 0f; 
        countdownPanel.SetActive(true);
        stopWatch.ResetStopwatch();

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1); 
        }

        countdownText.text = "GO!";
        yield return new WaitForSecondsRealtime(1);

        countdownPanel.SetActive(false);
        Time.timeScale = 1f; 
        stopWatch.StartStopwatch();
    }
}
