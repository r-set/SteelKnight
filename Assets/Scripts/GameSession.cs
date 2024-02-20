using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [Header("Stat")]
    public static int playerLives = 3;
    private int _initialPlayerLives;

    [Header("Key")]
    public int key = 0;
    public int needKeys = 2;
    private int _initialKey;

    [Header("Text")]
    [SerializeField] private TMP_Text _livesText;
    [SerializeField] private TMP_Text _keyText;

    private void Start()
    {
        _initialPlayerLives = playerLives;
        _initialKey = key;

        UpdateUIText();
     }

    private void UpdateUIText()
    {
        _livesText.text = playerLives.ToString();
        _keyText.text = key.ToString() + "/" + needKeys.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    public void AddToKey(int keyToAdd)
    {
        key += keyToAdd;
        UpdateUIText();
    }

    private void TakeLife()
    {
        playerLives--;
        key = 0;
        UpdateUIText();

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void ResetGameSession()
    {
        playerLives = _initialPlayerLives;
        key = _initialKey;

        SceneManager.LoadScene(0);
    }

    public void SetCurrentLevel(int level)
    {
        PlayerPrefs.SetInt("CurrentLevel", level);
        PlayerPrefs.Save();
    }

    public int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt("CurrentLevel", 1);
    }
}