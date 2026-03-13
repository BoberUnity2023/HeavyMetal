using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(WheelCollider))]
public class WheelSkid : MonoBehaviour 
{    
    [SerializeField] private Car _car;    	
    [SerializeField] private Rigidbody _rigidbody;    
    [SerializeField] private float _skidSlideStart = 0.2f;
    [SerializeField] private float _sideSlideMin = 0.2f;
    [SerializeField] private float _sideSlideMax = 2.2f;
    [SerializeField] private float _brakeSlideStart = 0.5f;
    [SerializeField] private float _brakeFadeSpeed = 20.0f;//На этой скорости следы от тормозов растворяются
    [SerializeField] private float _forceFadeStartSpeed = 3.0f;//На этой скорости следы от пробуксовки начинают растворяются
    [SerializeField] private float _forceFadeEndSpeed = 7.0f;//На этой скорости следы от пробуксовки растворяются
    [SerializeField] private float _smokeMax = 10;
    [SerializeField] private float _mark_width = 0.2f;//Ширина следа

    private Skidmarks _skidmarksController;
    /*private ParticleSystem _smoke;
    private ParticleSystem _smokeSand;
    private ParticleSystem _smokeSnow;*/
    private List<ParticleSystem> _particleSystems = new List<ParticleSystem>();
    //private ParticleSystem.EmissionModule _emissionModuleSand;
    //private ParticleSystem.EmissionModule _emissionModuleSnow;
    private WheelCollider _wheelCollider;
    private WheelHit _wheelHitInfo;
    [SerializeField] private GroundMaterial _groundMaterial;
    [SerializeField] private int _groundMaterialID;
    private GroundMaterial _groundMaterialPrevious;

    private int _lastSkid = -1; // Array index for the skidmarks controller. Index of last skidmark piece this wheel used
    private float lastFixedUpdateTime;
    private float _carForwardVelocity;

    public GroundMaterial GroundMaterial => _groundMaterial;

    protected void Start() 
    {
		_wheelCollider = GetComponent<WheelCollider>();
		lastFixedUpdateTime = Time.time;
        _skidmarksController = Instantiate(_car.Hub.Game.PrefabSkidmarks, Vector3.zero, Quaternion.identity);
        _skidmarksController.Init(this);
        CreateParticles();
    }

	protected void FixedUpdate() 
    {
		lastFixedUpdateTime = Time.time;
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
        RaycastHit hit;        

        if (Physics.Raycast(transform.position, -transform.up, out hit))
        {
            PhysicsMaterial physicMaterial = hit.collider.material;
            
            foreach (GroundProps groundProps in _car.Hub.Game.GroundPropses)
            {                
                if (physicMaterial.name.Contains(groundProps.PhysicMaterial.name))
                {
                    _groundMaterial = groundProps.GroundMaterial;
                    if (_groundMaterial != _groundMaterialPrevious)
                    {
                        SetSkidColor(groundProps);
                        SetFriction(groundProps);
                        _groundMaterialPrevious = _groundMaterial;
                        break;
                    }

                    _groundMaterialPrevious = _groundMaterial;
                }                
            }
        }
    }

    private void SetSkidColor(GroundProps groundProps)
    {
        _skidmarksController.SetColor(groundProps.Color);
    }

    private void SetFriction(GroundProps groundProps)
    {
        WheelFrictionCurve wheelFrictionCurve;
        wheelFrictionCurve = _wheelCollider.forwardFriction;
        wheelFrictionCurve.stiffness = groundProps.Friction;
        _wheelCollider.forwardFriction = wheelFrictionCurve;

        wheelFrictionCurve = _wheelCollider.sidewaysFriction;
        wheelFrictionCurve.stiffness = groundProps.Friction;
        _wheelCollider.sidewaysFriction = wheelFrictionCurve;
    }

    protected void LateUpdate() 
	{
        Mark();        
    }

	private void Mark()
	{
        float intensity = 0;
        if (_wheelCollider.GetGroundHit(out _wheelHitInfo))
        {
            _carForwardVelocity = Vector3.Dot(_rigidbody.linearVelocity, transform.forward);    

            intensity = Mathf.Clamp01(SideSlide + BrakeSlide + HandbrakeSlide + ForwardSlide);            
            
            if (intensity >= _skidSlideStart)
            {                                                 
                Vector3 skidPoint = _wheelHitInfo.point + _rigidbody.linearVelocity * (Time.time - lastFixedUpdateTime) * 1.3f;
                float mark_width = _mark_width + SideSlide * _mark_width * 0.3f;
                _lastSkid = _skidmarksController.AddSkidMark(skidPoint, _wheelHitInfo.normal, intensity, _lastSkid, mark_width);
            }
            else
            {
                _lastSkid = -1;                
            }
        }
        else
        {
            _lastSkid = -1;            
        }

        //Smoke
        float rateOverTime = intensity * _smokeMax;

        for (int i = 0; i < _particleSystems.Count; i++)
        {
            ParticleSystem.EmissionModule em = _particleSystems[i].emission;
            em.rateOverTime = i == GroundMaterialID - 1 ? rateOverTime : 0;
        }      
    }

    private float SideSlide
    {
        get
        {
            Vector3 localVelocity = transform.InverseTransformDirection(_rigidbody.linearVelocity);
            float sideSlide = Mathf.Abs(localVelocity.x); //[0...infinity]
            sideSlide = Mathf.Min(sideSlide, _sideSlideMax);//[0..._sideSlideMax]
            sideSlide = (sideSlide - _sideSlideMin) / (_sideSlideMax - _sideSlideMin);//[0...1]  
            return sideSlide;
        }
    }

    private float BrakeSlide
    {
        get
        {
            float brakeSlide = _car.Hub.Input.CarInput.Brake; //[0...1]
            float speedClamped = Mathf.Min(_carForwardVelocity, _brakeFadeSpeed);
            return brakeSlide *= (_brakeFadeSpeed - speedClamped) / _brakeFadeSpeed;//Доб. Затухание при высокой скорости
        }
    }

    private float HandbrakeSlide
    {
        get
        {
            float handbrakeSlide = _car.Hub.Input.CarInput.Handbrake; //[0 ... 1]
            float speedClamped = Mathf.Min(_carForwardVelocity, _brakeFadeSpeed);
            return handbrakeSlide *= (_brakeFadeSpeed - speedClamped) / _brakeFadeSpeed;//Доб. Затухание при высокой скорости
        }
    }

    private float ForwardSlide
    {
        get 
        {
            float speedClamped = Mathf.Min(_carForwardVelocity, _forceFadeEndSpeed);
            float forwardSlide = _car.Hub.Input.CarInput.Force + _car.Hub.Input.CarInput.Reverse;//[0...1]
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
