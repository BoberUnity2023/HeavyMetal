using UnityEngine;

public class PoliceLight : MonoBehaviour
{
    [SerializeField] private Car _car;
    [SerializeField] private Light[] _lights;    

    private void Awake()
    {
        _car.OnInit += OnInit;
    }

    private void OnDestroy()
    {
        _car.OnInit -= OnInit;
    }

    private void OnInit(Mode mode)
    {
        if (mode == Mode.Track)
        {
            foreach (var light in _lights)
            {
                light.gameObject.SetActive(true);
            }
        }
    }
}
