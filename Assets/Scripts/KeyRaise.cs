using UnityEngine;

public class KeyRaise : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip _keyRaiseSFX;

    [Header("Scripts")]
    [SerializeField] private GameSession _gameSession;

    private int _quantityKeyRaise = 1;
    private bool _wasCollected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_wasCollected && _gameSession.key < _gameSession.needKeys)
        {
            _wasCollected = true;
            _gameSession.AddToKey(_quantityKeyRaise);
            AudioManager.Instance.sfxAudioSource.PlayOneShot(_keyRaiseSFX);
            Destroy(gameObject);
        }
    }
}