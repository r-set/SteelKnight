using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip _gameAudio;
    [SerializeField] private AudioClip _menuAudio;

    [Header("Audio Toggle")]
    [SerializeField] private Toggle _soundToggle;

    private bool _isSoundOff = false;
    private const string _soundKey = "SoundState";

    [HideInInspector] public AudioSource menuAudioSource;
    [HideInInspector] public AudioSource gameAudioSource;
    [HideInInspector] public AudioSource sfxAudioSource;

    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject audioManagerObject = new GameObject("AudioManager");
                _instance = audioManagerObject.AddComponent<AudioManager>();
                DontDestroyOnLoad(audioManagerObject);
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        menuAudioSource = gameObject.AddComponent<AudioSource>();
        gameAudioSource = gameObject.AddComponent<AudioSource>();
        sfxAudioSource = gameObject.AddComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        _isSoundOff = PlayerPrefs.GetInt(_soundKey, 0) == 1;
        _soundToggle.onValueChanged.AddListener(ToggleSound);
        _soundToggle.isOn = !_isSoundOff;

        SetAudioVolume(!_isSoundOff);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void ToggleSound(bool isSoundEnabled)
    {
        _isSoundOff = !isSoundEnabled;

        PlayerPrefs.SetInt(_soundKey, _isSoundOff ? 1 : 0);
        PlayerPrefs.Save();

        SetAudioVolume(!_isSoundOff);
    }

    private void SetAudioVolume(bool isMuted)
    {
        menuAudioSource.volume = isMuted ? 0 : 0.5f;
        gameAudioSource.volume = isMuted ? 0 : 0.5f;
        sfxAudioSource.volume = isMuted ? 0 : 0.5f;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        if (sceneName.StartsWith("Menu"))
        {
            gameAudioSource.Stop();

            menuAudioSource.clip = _menuAudio;

            if (!menuAudioSource.isPlaying)
            {
                menuAudioSource.Play();
                menuAudioSource.loop = true;
            }
        }
        else if (sceneName.StartsWith("Stage"))
        {
            menuAudioSource.Stop();

            gameAudioSource.clip = _gameAudio;

            if (!gameAudioSource.isPlaying)
            {
                gameAudioSource.Play();
                gameAudioSource.loop = true;
            }
        }
        else
        {
            menuAudioSource.Stop();
            gameAudioSource.Stop();
        }
    }
}
