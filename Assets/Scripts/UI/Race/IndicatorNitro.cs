using UnityEngine;
using UnityEngine.UI;

public class IndicatorNitro : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private Image _indicatorNitro;

    private void Update()
    {
        _indicatorNitro.fillAmount = _hub.Level.Race.Car.Nitro.FillProgress;
    }
}
