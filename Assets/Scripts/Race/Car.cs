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
    [SerializeField] private LapsCounter _lapsCounter;
    [SerializeField] private RocketGun _rocketGun;
    [SerializeField] private DamageCounter _damageCounter;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private GameObject _prefabSparks;
    [SerializeField] private Transform _podnosPosition;
    [SerializeField] private Transform _heroPosition;    
    private ICarInputable _input;
    private InputType _inputType;
    private float _speedForward;    

    public bool IsFinished;

    public bool IsCrashed;

    public float Force { get; set; }

    public Hub Hub => _hub;

    public CarControl Control => _control;

    public CarAI AIInput => _aIInput;

    public LapsCounter LapsCounter => _lapsCounter;

    public RocketGun RocketGun => _rocketGun;

    public DamageCounter DamageCounter => _damageCounter;

    public Rigidbody Rigidbody => _rigidbody;

    public GameObject PrefabSparks => _prefabSparks;

    public Transform PodnosPosition => _podnosPosition;

    public Transform HeroPosition => _heroPosition;

    public ICarInputable Input => _input;

    public InputType InputType => _inputType;   

    public void Init(Hub hub, InputType inputType, int id)//OnlyAI
    {
        _hub = hub;
        _inputType = inputType;

        bool _isAI = InputType == InputType.AI;
        _input = _isAI ? AIInput : _hub.Input.PlayerInput;

        WayPath wayPath = Hub.PathSelector.WayPath(id);
        LapsCounter.SetWayPath(wayPath);
    }

    private void FixedUpdate()
    {        
        _speedForward = Mathf.InverseLerp(0, Control.MaxSpeed, Mathf.Abs(Speed));      // From 0 to 1 
    }

    public float Speed => Vector3.Dot(transform.forward, _rigidbody.linearVelocity); //from -Max to Max
    
    public float SpeedForward => _speedForward; //From 0 to 1//TODO: Rename

    public bool IsAI
    {
        get
        {
            return _inputType == InputType.AI;
        }        
    }    
}
