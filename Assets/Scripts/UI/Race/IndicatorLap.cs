using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class IndicatorLap : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private TMP_Text _indicator;    
    private float _positionY;

    private void Start()
    {        
        _hub.Level.Race.OnLapCompleted += OnLapCompleted;
        SetText(1);
        _positionY = transform.position.y;
    }

    private void OnDestroy()
    {
        _hub.Level.Race.OnLapCompleted -= OnLapCompleted;
    }

    private void OnLapCompleted(int lap)
    {
        float effectTime = 0.5f;
        StartScale(effectTime);
        StartCoroutine(ChangeText(effectTime, lap));
    }

    private void StartScale(float effectTime)
    {
        transform.DOScale(2, effectTime);
        transform.DOMoveY(_positionY - 200, effectTime);
    }

    private void StartReturn()
    {
        float effectTime = 1.0f;
        transform.DOScale(1, effectTime);
        transform.DOMoveY(_positionY, effectTime);
    }

    private IEnumerator ChangeText(float time, int lap)
    {
        yield return new WaitForSeconds(time);
        SetText(lap);
        StartReturn();
    }

    private void SetText(int lap)
    {
        int laps = _hub.Level.Config.Laps;
        _indicator.text = "" + Mathf.Min(lap, laps).ToString() + "/" + laps.ToString();
    }
}

