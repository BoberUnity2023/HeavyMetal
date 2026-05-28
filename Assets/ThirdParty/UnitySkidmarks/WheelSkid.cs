using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(WheelCollider))]
public class WheelSkid : MonoBehaviour 
{    
    [SerializeField] private Car _car;   
    [SerializeField] private float _skidSlideStart = 0.2f;
    [SerializeField] private float _sideSlideMin = 0.2f;
    [SerializeField] private float _sideSlideMax = 2.2f;
    //[SerializeField] private float _brakeSlideStart = 0.5f;
    [SerializeField] private float _brakeFadeSpeed = 20.0f;//На этой скорости следы от тормозов растворяются
    [SerializeField] private float _forceFadeStartSpeed = 3.0f;//На этой скорости следы от пробуксовки начинают растворяются
    [SerializeField] private float _forceFadeEndSpeed = 7.0f;//На этой скорости следы от пробуксовки растворяются    
    [SerializeField] private float _mark_width = 0.2f;//Ширина следа

    private Skidmarks _skidmarksController;
    private List<ParticleSystem> _particleSystems = new List<ParticleSystem>();
    private WheelCollider _wheelCollider;
    private WheelControl _wheelControl;    
    [SerializeField] private GroundMaterial _groundMaterial;
    [SerializeField] private int _groundMaterialID;
    private GroundMaterial _groundMaterialPrevious;
    private PhysicsMaterial _physicMaterial;
    private PhysicsMaterial _physicMaterialPrevious;
    private GroundProps _groundProps;

    private int _lastSkid = -1; // Array index for the skidmarks controller. Index of last skidmark piece this wheel used
    private float _lastFixedUpdateTime;
    private float _carSpeed;
    private float _intensity;
    private bool _isGrounded;

    public GroundMaterial GroundMaterial => _groundMaterial;

    public float Intensity => _intensity;

    public float TuningFactor {  get; set; }//1.0, 1.1, 1.2, 1.3

    public bool IsGrounded => _isGrounded;

    protected void Start() 
    {		
        _wheelControl = GetComponent<WheelControl>();
        _wheelCollider = _wheelControl.WheelCollider;
        _lastFixedUpdateTime = Time.time;
        _skidmarksController = Instantiate(_car.Hub.Game.PrefabSkidmarks, Vector3.zero, Quaternion.identity);
        _skidmarksController.Init(this);
        CreateParticles();
    }

	protected void FixedUpdate() 
    {
		_lastFixedUpdateTime = Time.time;
        GetGround_FixedUpdate();        
    }

    private void CreateParticles()
    {
        Vector3 position = transform.position + Vector3.down * _wheelCollider.radius;

        foreach (GroundProps groundProps in _car.Hub.Game.GroundPropses)
        {
            if (groundProps.GroundMaterial != GroundMaterial.Blocker)
            {
                ParticleSystem particleSystem = Instantiate(groundProps.PrefabParticles, position, Quaternion.identity, transform);
                _particleSystems.Add(particleSystem);
            }
        }
    }

    private void GetGround_FixedUpdate()
    {
        if (!_car.IsVisible)
            return;

        RaycastHit hit;        

        if (Physics.Raycast(transform.position, -transform.up, out hit))
        {
            _physicMaterial = hit.collider.material;
            if (_physicMaterial != _physicMaterialPrevious)
            {
                foreach (GroundProps groundProps in _car.Hub.Game.GroundPropses)
                {
                    if (_car.Hub.Game.IsEqualPhysicsMaterials(hit.collider.material, groundProps.PhysicMaterial))
                    {
                        _groundMaterial = groundProps.GroundMaterial;
                        _groundProps = groundProps;
                        if (_groundMaterial != _groundMaterialPrevious)
                        {
                            SetSkidColor(groundProps.Color);
                            SetFriction(groundProps.Friction);
                            _groundMaterialPrevious = _groundMaterial;
                            break;
                        }

                        _groundMaterialPrevious = _groundMaterial;
                    }
                }
            }
            _physicMaterialPrevious = _physicMaterial;
        }
    }

    public void SetSkidColor(Color color)
    {
        _skidmarksController.SetColor(color);
    }

    public void SetSkidColor()
    {
        _skidmarksController.SetColor(_groundProps.Color);
    }

    public void SetFriction()
    {
        SetFriction(_groundProps.Friction);
    }

    public void SetFrictionWithIntensity(float intensity)
    {
        SetFriction(_groundProps.Friction * intensity);
    }

    private void SetFriction(float friction)
    {
        WheelFrictionCurve wheelFrictionCurve;
        wheelFrictionCurve = _wheelCollider.forwardFriction;
        wheelFrictionCurve.stiffness = friction * TuningFactor;
        _wheelCollider.forwardFriction = wheelFrictionCurve;

        wheelFrictionCurve = _wheelCollider.sidewaysFriction;
        wheelFrictionCurve.stiffness = friction * TuningFactor;
        _wheelCollider.sidewaysFriction = wheelFrictionCurve;
    }

    protected void LateUpdate()
    {
        _carSpeed = _car.Speed;
        LateUpdate_Mark();
        LateUpdate_SkidSmoke();
    }

    private void LateUpdate_Mark()
    {
        _intensity = 0;
        if (!_wheelControl.IsAttached || !_car.IsVisible)
        {
            _lastSkid = -1;
            return;
        }

        WheelHit wheelHitInfo;
        _isGrounded = _wheelCollider.GetGroundHit(out wheelHitInfo);
        if (!_isGrounded || Mathf.Abs(_carSpeed) < 0.1f)
        {
            _lastSkid = -1;
            return;
        }

        float oilIntensity = _car.Oil.Intensity;
        if (oilIntensity > 0)
            _intensity = oilIntensity;
        else
            _intensity = Mathf.Max(oilIntensity, Mathf.Clamp01(SideSlide + BrakeSlide + HandbrakeSlide + ForwardSlide));

        if (_intensity < _skidSlideStart)
        {
            _lastSkid = -1;
            return;
        }

        Vector3 skidPoint = wheelHitInfo.point + _car.Rigidbody.linearVelocity * (Time.time - _lastFixedUpdateTime) * 1.3f;
        float mark_width = _mark_width + SideSlide * _mark_width * 0.3f;
        _lastSkid = _skidmarksController.AddSkidMark(skidPoint, wheelHitInfo.normal, _intensity, _lastSkid, mark_width);
    }

    private void LateUpdate_SkidSmoke()
    {
        float rateOverTime = _intensity * RateOverTimeMax;

        for (int i = 0; i < _particleSystems.Count; i++)
        {
            ParticleSystem.EmissionModule em = _particleSystems[i].emission;
            em.rateOverTime = i == GroundMaterialID - 1 ? rateOverTime : 0;
        }
    }

    private int RateOverTimeMax
    {
        get
        {
            if (GroundMaterialID == 0)
                return 0;

            return _car.Hub.Game.GroundPropses[GroundMaterialID - 1].RateOverTimeMax;
        }
    }

    private float SideSlide
    {
        get
        {            
            float sideSlide = Mathf.Abs(_car.LocalVelocity.x); //[0...infinity]
            sideSlide = Mathf.Min(sideSlide, _sideSlideMax);//[0..._sideSlideMax]
            sideSlide = (sideSlide - _sideSlideMin) / (_sideSlideMax - _sideSlideMin);//[0...1]  
            return sideSlide;
        }
    }

    private float BrakeSlide
    {
        get
        {
            float brakeSlide = _car.Input.Brake; //[0...1]
            float speedClamped = Mathf.Min(_carSpeed, _brakeFadeSpeed);
            return brakeSlide *= (_brakeFadeSpeed - speedClamped) / _brakeFadeSpeed;//Доб. Затухание при высокой скорости
        }
    }

    private float HandbrakeSlide
    {
        get
        {
            float handbrakeSlide = _car.Input.Handbrake; //[0 ... 1]
            float speedClamped = Mathf.Min(_carSpeed, _brakeFadeSpeed);
            return handbrakeSlide *= (_brakeFadeSpeed - speedClamped) / _brakeFadeSpeed;//Доб. Затухание при высокой скорости
        }
    }

    private float ForwardSlide
    {
        get 
        {
            float speedClamped = Mathf.Min(_carSpeed, _forceFadeEndSpeed);
            float forwardSlide = _car.Input.Force + _car.Input.Reverse;//[0...1]
            if (speedClamped > _forceFadeStartSpeed)
            {
                float delta = _forceFadeEndSpeed - _forceFadeStartSpeed;
                forwardSlide *= (_forceFadeEndSpeed - speedClamped) / delta;//Доб. Затухание при высокой скорости
            }
            return Mathf.Clamp01(forwardSlide);
        }
    }

    int GroundMaterialID
    {
        get
        {
            return (int)_groundMaterial;
        }
    }
}
