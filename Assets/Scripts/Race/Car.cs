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
    [SerializeField] private Visible _visible;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private GameObject _prefabSparks;
    [SerializeField] private Transform _podnosPosition;
    [SerializeField] private Transform _heroPosition;    
    private ICarInputable _input;
    private InputType _inputType;
    private WayPath _wayPath;
    private WheelControl[] _wheels;
    private WheelSkid[] _wheelSkids = new WheelSkid[4];
    private float _speedForward;
    private float _slideForce;

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
    
    public WayPath WayPath => _wayPath;

    public WheelControl[] Wheels => _wheels;

    public bool IsVisible => _visible.IsVisible;

    public void Init(Hub hub, InputType inputType, int id)
    {
        _hub = hub;
        _inputType = inputType;

        bool _isAI = InputType == InputType.AI;
        _input = _isAI ? AIInput : _hub.Input.PlayerInput;

        _wayPath = Hub.PathSelector.WayPath(id);
        LapsCounter.SetWayPath(_wayPath);

        _wheels = GetComponentsInChildren<WheelControl>();

        for (int i = 0; i < 4; i++)
        {
            _wheelSkids[i] = Wheels[i].GetComponent<WheelSkid>();
        }
    }

    private void FixedUpdate()
    {        
        _speedForward = Mathf.InverseLerp(0, Control.MaxSpeed, Mathf.Abs(Speed));      // From 0 to 1 
        FixedUpdate_CalculateSlideForce();
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

    public float SlideForce => _slideForce;

    private void FixedUpdate_CalculateSlideForce()
    {
        float force = 0;
        foreach (var wheel in _wheelSkids)
        {
            force += wheel.Intensity;
        }

        if (Speed < 1)
            force *= Speed;

        _slideForce = force /= 4;
    }
}
