using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReplayManager : MonoBehaviour
{
    [Header("Replay Settings")]
    public KeyCode replayKey = KeyCode.R;         

    [Header("UI Elements")]
    public Button replayButton;                  

    void Start()
    {
        if (replayButton != null)
        {
            replayButton.onClick.AddListener(Replay);
            Debug.Log("Replay Button listener added.");
        }
        else
        {
            Debug.LogWarning("Replay Button is not assigned in the Inspector.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(replayKey))
        {
            Debug.Log("Replay key pressed.");
            Replay();
        }
    }
    
    public void Replay()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopGameplayMusic();
            AudioManager.Instance.PlayPreGameplayMusic();
            Debug.Log("AudioManager: Stopped Gameplay Music and Played Pre-Gameplay Music.");
        }
        else
        {
            Debug.LogWarning("AudioManager instance is null during Replay.");
        }

        Scene currentScene = SceneManager.GetActiveScene();
        Debug.Log($"Reloading scene: {currentScene.name}");
        SceneManager.LoadScene(currentScene.name);
    }
}
