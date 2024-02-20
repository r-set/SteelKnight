using UnityEngine;
using TMPro;

public class LevelExit : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text _exitText;

    [Header("Next Level")]
    [SerializeField] private string _nameNextStage;
    [SerializeField] private int _nextCurrentLevel;

    [Header("Scripts")]
    [SerializeField] private GameSession _gameSession;
    [SerializeField] private StageManager _stageManager;

    [Header("Audio")]
    [SerializeField] private AudioClip _doorOpenSFX;

    private bool _isKeyCollect = false;
    private int _keysNeeded = 0;
    private TMP_Text _exitTextInstance;
    private Animator _doorAnimator;

    private void Awake()
    {
        _doorAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        _exitTextInstance = _exitText;
        _exitTextInstance.gameObject.SetActive(false);

    }

    private void Update()
    {
        _isKeyCollect = _gameSession.key >= _gameSession.needKeys;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_isKeyCollect)
            {
                OpenDoor();
            }
            else
            {
                _keysNeeded = _gameSession.needKeys - _gameSession.key;

                if (_keysNeeded <= _gameSession.needKeys)
                {
                    _exitTextInstance.text = "Need keys" + " - " + _keysNeeded;
                }

                _exitTextInstance.gameObject.SetActive(true);
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _exitTextInstance.gameObject.SetActive(false);
        }
    }

    private void OpenDoor()
    {
        _doorAnimator.SetTrigger("Open");
        _gameSession.key = 0;

        AudioManager.Instance.sfxAudioSource.PlayOneShot(_doorOpenSFX);

        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        _stageManager.LoadScene(_nameNextStage);
        _stageManager.currentLevel = _nextCurrentLevel;

        _gameSession.SetCurrentLevel(_nextCurrentLevel);
    }
}