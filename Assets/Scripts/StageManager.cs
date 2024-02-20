using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _stage1Button;
    [SerializeField] private Button _stage2Button;
    [SerializeField] private Button _stage3Button;
    [SerializeField] private Button _stage4Button;
    [SerializeField] private Button _stage5Button;
    [SerializeField] private Button _stage6Button;
    [SerializeField] private Button _stage7Button;
    [SerializeField] private Button _stage8Button;
    [SerializeField] private Button _stage9Button;
    [SerializeField] private Button _stage10Button;
    [SerializeField] private Button _stage11Button;
    [SerializeField] private Button _stage12Button;
    [SerializeField] private Button _stage13Button;
    [SerializeField] private Button _stage14Button;
    [SerializeField] private Button _stage15Button;

    [Header("Audio")]
    [SerializeField] private AudioClip _buttonSFX;

    [HideInInspector] public int currentLevel = 1;

    [Header("Scripts")]
    [SerializeField] private GameSession _gameSession;

    private void Start()
    {
        currentLevel = _gameSession.GetCurrentLevel();

        for (int i = 1; i <= 15; i++)
        {
            Button stageButton = GetStageButton(i);
            if (stageButton != null)
            {
                stageButton.interactable = false;
            }
        }

        for (int i = 1; i <= currentLevel; i++)
        {
            Button stageButton = GetStageButton(i);
            if (stageButton != null)
            {
                stageButton.interactable = true;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadMainMenu();
        }
    }

    private Button GetStageButton(int level)
    {
        switch (level)
        {
            case 1:
                return _stage1Button;
            case 2:
                return _stage2Button;
            case 3:
                return _stage3Button;
            case 4:
                return _stage4Button;
            case 5:
                return _stage5Button;
            case 6:
                return _stage6Button;
            case 7:
                return _stage7Button;
            case 8:
                return _stage8Button;
            case 9:
                return _stage9Button;
            case 10:
                return _stage10Button;
            case 11:
                return _stage11Button;
            case 12:
                return _stage12Button;
            case 13:
                return _stage13Button;
            case 14:
                return _stage14Button;
            case 15:
                return _stage15Button;
 
            default:
                return null;
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadMainMenu()
    {
        LoadSceneWithButtonSFX("MenuHome");
    }

    public void LoadStages()
    {
        LoadSceneWithButtonSFX("MenuStages");
    }

    public void LoadStage(int level)
    {
        GameSession.playerLives = 3;

        if (level >= 1 && level <= 15)
        {
            string sceneName = "Stage" + level;
            LoadSceneWithButtonSFX(sceneName);
        }
        else
        {
            LoadSceneWithButtonSFX("MenuHome");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        PlayButtonSFX();
        _gameSession.SetCurrentLevel(1);
    }

    private void LoadSceneWithButtonSFX(string sceneName)
    {
        LoadScene(sceneName);
        PlayButtonSFX();
    }

    private void PlayButtonSFX()
    {
        AudioManager.Instance.sfxAudioSource.PlayOneShot(_buttonSFX);
    }

}