using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class CoinsIndicator : MonoBehaviour
{
    [SerializeField] private SceneController _hub;
    [SerializeField] private TMP_Text _indicator;
    [SerializeField] private TMP_Text _indicatorAdd;
    private int _previousCoins;
    private bool _isInited;

    private void Start()
    {
        _hub.Game.Saves.OnCoinsChanged += OnCoinsChanged;
        _previousCoins = _hub.Game.Saves.Coins;
        _indicator.text = _hub.Game.Saves.Coins.ToString();
        _indicatorAdd.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _hub.Game.Saves.OnCoinsChanged -= OnCoinsChanged;
    }

    private void OnCoinsChanged(int value)
    {
        int change = value - _previousCoins;
        string text = change > 0 ? "+" : "";
        text = text + change;
        _previousCoins = value;

        if (change < 0)
            _indicator.text = value.ToString();

        if (_isInited)
        {
            _indicatorAdd.text = text;
            _indicatorAdd.gameObject.SetActive(true);

            _indicatorAdd.transform.DOMoveY(transform.position.y, 1.2f).SetEase(Ease.InCirc);

            _indicatorAdd.transform.parent.DOScale(1.2f, 0.4f);
            StartCoroutine(AfterCoinsChanged(1.2f, value));
        }
        else
            _isInited = true;
    }

    private IEnumerator AfterCoinsChanged(float time, int value)
    {
        yield return new WaitForSeconds(time);
        _indicator.text = value.ToString();
        _indicatorAdd.gameObject.SetActive(false);
        _indicatorAdd.transform.position = _indicatorAdd.transform.parent.position - Vector3.up * 100;
        _indicatorAdd.transform.parent.transform.DOScale(1.0f, 0.4f);
    }
}
