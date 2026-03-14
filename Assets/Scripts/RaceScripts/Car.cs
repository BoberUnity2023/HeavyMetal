using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType
{
    Player,
    AI
}

public class Car : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private CarControl _control;
    [SerializeField] private CarAI _aIInput;
    [SerializeField] private GameObject _prefabSparks;
    [SerializeField] private Transform _podnosPosition;
    [SerializeField] private Transform _heroPosition;    
    private ICarInputable _input;

    private InputType _inputType;

    public Hub Hub => _hub;

    public CarControl Control => _control;

    public CarAI AIInput => _aIInput;

    public GameObject PrefabSparks => _prefabSparks;

    public Transform PodnosPosition => _podnosPosition;

    public Transform HeroPosition => _heroPosition;

    public ICarInputable Input => _input;

    public InputType InputType => _inputType;

    public void Init(Hub hub, InputType inputType)
    {
        _hub = hub;
        _inputType = inputType;

        bool _isAI = InputType == InputType.AI;
        _input = _isAI ? AIInput : _hub.Input.PlayerInput;  
    }
}
