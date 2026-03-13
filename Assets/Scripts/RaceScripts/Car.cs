using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private CarControl _carControl;
    [SerializeField] private GameObject _prefabSparks;
    [SerializeField] private Transform _podnosPosition;
    [SerializeField] private Transform _heroPosition;

    public Hub Hub => _hub;

    public CarControl CarControl => _carControl;

    public GameObject PrefabSparks => _prefabSparks;

    public Transform PodnosPosition => _podnosPosition;

    public Transform HeroPosition => _heroPosition;

    public void Initi(Hub hub)
    {
        _hub = hub;
    }
}
