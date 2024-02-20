using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;

        AudioListener.pause = true;
        AudioListener.volume = 0.0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;

        AudioListener.pause = false;
        AudioListener.volume = 0.5f;
    }
}