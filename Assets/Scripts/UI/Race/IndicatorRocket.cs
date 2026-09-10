using UnityEngine;
using UnityEngine.UI;

public class IndicatorRocket : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private Image _indicatorProgress;
    [SerializeField] private RocketUI[] _rockets;    

    private void Update()
    {
        int count = _hub.Level.Race.Car.RocketGun.Armo;        

        for (int i = 0; i < _rockets.Length; i++)
        {            
            if (i >= _hub.Level.Race.Car.RocketGun.ArmoMax)
                _rockets[i].Hide();
            else
            {
                if (i >= _hub.Level.Race.Car.RocketGun.Armo)
                    _rockets[i].SetEmpty();
                else
                    _rockets[i].SetFull();
            }
        }

        _indicatorProgress.fillAmount = (float)_hub.Level.Race.Car.RocketGun.ArmoMax/ _rockets.Length;
    }
}
