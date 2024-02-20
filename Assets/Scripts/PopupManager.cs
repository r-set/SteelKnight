using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PopupManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private CanvasGroup _popupPanel;
    [SerializeField] private GameObject _controlPopup;

    [Header("Audio")]
    [SerializeField] private AudioClip _buttonSFX;

    private float _fadeTime = 0.25f;
    private float _durationTime = 0.5f;
    private float _initialScale = 0f;

    private bool _isPopupAnimating = false;

    private void SetPopupActive(GameObject popup, bool isActive)
    {
        if (_isPopupAnimating)
            return;

        StartCoroutine(AnimatePopup(popup, isActive));
    }

    private IEnumerator AnimatePopup(GameObject popup, bool isActive)
    {
        _isPopupAnimating = true;

        if (isActive)
        {
            ShowPopup(popup);
        }
        else
        {
            HidePopup(popup);
        }

        yield return new WaitForSeconds(_durationTime);

        _isPopupAnimating = false;
    }

    private void ShowPopup(GameObject popup)
    {
        _popupPanel.alpha = 0f;
        _popupPanel.blocksRaycasts = true;

        RectTransform popupRect = popup.GetComponent<RectTransform>();
        popupRect.anchoredPosition = Vector2.zero;
        popup.SetActive(true);

        _popupPanel.DOFade(1, _fadeTime);
        popupRect.localScale = Vector3.one * _initialScale;
        popupRect.DOScale(Vector3.one, _durationTime).SetEase(Ease.InQuart);
    }

    private void HidePopup(GameObject popup)
    {
        _popupPanel.alpha = 1f;
        _popupPanel.blocksRaycasts = false;

        RectTransform popupRect = popup.GetComponent<RectTransform>();
        popupRect.DOScale(Vector3.one * _initialScale, _durationTime)
            .SetEase(Ease.InOutQuart)
            .OnComplete(() =>
            {
                popup.SetActive(false);
                _popupPanel.DOFade(0, _fadeTime);
            });
    }

    public void ShowPopup()
    {
        SetPopupActive(_controlPopup, true);
        AudioManager.Instance.sfxAudioSource.PlayOneShot(_buttonSFX);
    }

    public void HidePopup()
    {
        SetPopupActive(_controlPopup, false);
    }
}