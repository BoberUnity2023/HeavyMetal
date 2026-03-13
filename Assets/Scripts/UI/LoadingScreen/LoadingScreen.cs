using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text _progressIndicator;
    [SerializeField] private Image _progressBar;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _timeHide;

    public float TimeHide => _timeHide;

    public void SetProgress(float progress)
    {
        string text = (progress * 100).ToString("f0") + "%";
        if (_progressIndicator != null)
            _progressIndicator.text = text;

        if (_progressBar != null)
            _progressBar.fillAmount = progress;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _animator.ResetTrigger("Hide");
        _animator.SetTrigger("Idle");
    }

    public void Hide()
    {
        _animator.ResetTrigger("Idle");
        _animator.SetTrigger("Hide");
        StartCoroutine(AfterHide(_timeHide));
    }

    private IEnumerator AfterHide(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
