using UnityEngine;

public enum InputType
{
    Player,
    AI
}

public enum Mode
{
    Garage,
    Track
}

public class Car : MonoBehaviour
{
    [SerializeField] private CarType _carType;
    [SerializeField] private Hub _hub;
    [SerializeField] private CarControl _control;
    [SerializeField] private CarAI _aIInput;
    [SerializeField] private Tuning _tuning;
    [SerializeField] private LapsCounter _lapsCounter;
    [SerializeField] private RocketGun _rocketGun;
    [SerializeField] private WeaponMines _weaponMines;
    [SerializeField] private DamageCounter _damageCounter;
    [SerializeField] private Visible _visible;
    [SerializeField] private Nitro _nitro;
    [SerializeField] private CarOil _oil;
    [SerializeField] private CarPaint _paint;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private GameObject[] _prefabsSparks;
    private Transform _podnosPosition;
    private Transform _heroPosition;    
    private ICarInputable _input;
    private InputType _inputType;
    private Mode _mode;
    private Vector3 _localVelocity;
    private WayPath _wayPath;
    private WheelControl[] _wheels;
    private WheelSkid[] _wheelSkids = new WheelSkid[4];
    private float _speedFactor;
    private float _slideForce;   
    private float _speed;

    public CarType CarType => _carType;    

    public Hub Hub => _hub;

    public ConfigCar Config => _hub.Game.ConfigGame.Car(_carType);

    public CarControl Control => _control;

    public CarAI AIInput => _aIInput;

    public Tuning Tuning => _tuning;

    public LapsCounter LapsCounter => _lapsCounter;

    public RocketGun RocketGun => _rocketGun;

    public WeaponMines WeaponMines => _weaponMines;

    public DamageCounter DamageCounter => _damageCounter; 
    
    public CarPaint Paint => _paint;

    public Nitro Nitro => _nitro;

    public CarOil Oil => _oil;

    public Rigidbody Rigidbody => _rigidbody;

    public GameObject[] PrefabsSparks => _prefabsSparks;

    public Transform PodnosPosition => _podnosPosition;

    public Transform HeroPosition => _heroPosition;    

    public ICarInputable Input => _input;

    public InputType InputType => _inputType;  

    public Mode Mode => _mode;
    
    public WayPath WayPath => _wayPath;

    public WheelControl[] Wheels => _wheels;

    public WheelSkid[] WheelSkids => _wheelSkids;

    public Vector3 LocalVelocity => _localVelocity;

    public float Speed => _speed;

    public float SpeedFactor => _speedFactor; //From 0 to 1

    public float SlideForce => _slideForce;

    public bool IsVisible => _visible.IsVisible;

    public bool IsAI => _inputType == InputType.AI;    

    public bool IsFinished;

    public bool IsCrashed;

    public bool IsOnEscalator { get; set; }

    public float Force { get; set; }

    public void Init(Hub hub, InputType inputType, int id, Mode mode)
    {
        _hub = hub;
        _inputType = inputType; 
        _mode = mode;

        bool _isAI = InputType == InputType.AI;
        _input = _isAI ? AIInput : _hub.Input.PlayerInput;

        if (!IsAI)
            Hub.AudioListenerMovier.Init(transform);

        _wayPath = Hub.PathSelector.WayPath(id);
        LapsCounter.SetWayPath(_wayPath);

        _wheels = GetComponentsInChildren<WheelControl>();

        for (int i = 0; i < 4; i++)
        {
            _wheelSkids[i] = Wheels[i].GetComponent<WheelSkid>();
        }

        _control.Init(this);
        _aIInput.Init(this);        
        _lapsCounter.Init(this);
        _rocketGun.Init(this);
        _weaponMines.Init(this);
        _damageCounter.Init(this);
        _paint.Init(this);
        _nitro.Init(this);
        _tuning.Init(this, Hub.Game);
        _oil.Init(this);
    }

    public void Init(Mode mode, GameController game)
    {
        _mode = mode;
        Tuning.Init(this, game);
    }

    private void FixedUpdate()
    {
        _speed = Vector3.Dot(transform.forward, _rigidbody.linearVelocity); //from -Max to Max
        _speedFactor = Mathf.InverseLerp(0, Control.MaxSpeed, Mathf.Abs(Speed));      // From 0 to 1 
        FixedUpdate_CalculateSlideForce();
        _localVelocity = transform.InverseTransformDirection(Rigidbody.linearVelocity);
    }    

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

    public int Place
    {
        get
        {
            int output = 1;
            int points = LapsCounter.Points;

            foreach (Car car in Hub.Level.Race.Cars)
            {
                bool isI = car == this;
                if (!isI)
                {
                    if (car.LapsCounter.Points > points)
                        output++;

                    if (car.LapsCounter.Points == points)
                    {
                        float distThis = Vector3.Distance(transform.position, LapsCounter.CurrentPointPosition);
                        float distEnemy = Vector3.Distance(car.transform.position, car.LapsCounter.CurrentPointPosition);

                        if (distThis > distEnemy)
                            output++;
                    }
                }
            }

            return output;
        }
    }

    public int GroundedWheels
    {
        get
        {
            int output = 0;
            foreach (WheelSkid wheel in _wheelSkids)
            {
                if (wheel.IsGrounded)
                    output++;
            }
            return output;
        }
    }
}
