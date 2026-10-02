using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DamageCounter : MonoBehaviour
{
    [SerializeField] private ParticleSystem _smoke;
    [SerializeField] private ParticleSystem _fire;
    [SerializeField] private int _reward;
    [SerializeField] private float _spiritTime;
    private Spirit _spirit;
    private Car _car;
    private Transform _wheelsParent;
    private Vector3[] _wheelPositions = new Vector3[4];
    private int _damage;
    private float _deadCrashTimer;
    private float _hideTimer;
    private bool _isWaitingDeadCrash;
    private bool _isWaitingHide;

    public int Damage
    {
        get => _damage;
        private set => _damage = value;
    }

    public void Init(Car car)
    {  
        _car = car;
        _wheelsParent = _car.Wheels[0].transform.parent;
        for (int i = 0; i < 4; i++)
        {
            _wheelPositions[i] = _car.Wheels[i].transform.localPosition;
        }

        enabled = false;
    }

    private void Update()
    {
        if (Time.timeScale == 0 || _car.Hub.IsPaused)
            return;

        // Логика заменяет корутину WaitDeadCrash
        if (_isWaitingDeadCrash)
        {
            _deadCrashTimer -= Time.deltaTime;
            if (_deadCrashTimer <= 0)
            {
                _isWaitingDeadCrash = false;

                for (int i = 0; i < _car.Wheels.Length; i++)
                {
                    WheelControl wheel = _car.Wheels[i];
                    WheelDeattach(wheel);
                }

                _car.IsCrashed = true;

                // Взводим второй таймер (замена корутины WaitHide)
                _hideTimer = 1f; // Ждем 1 секунду до скрытия и спавна Духа
                _isWaitingHide = true;
            }
        }

        // Логика заменяет корутину WaitHide
        if (_isWaitingHide)
        {
            _hideTimer -= Time.deltaTime;
            if (_hideTimer <= 0)
            {
                _isWaitingHide = false;

                // Таймеры закончились — выключаем Update, чтобы не тратить CPU
                enabled = false;

                _spirit = new GameObject().AddComponent<Spirit>();
                _spirit.Init(_car, _spiritTime);
            }
        }
    }


    public void DamageAdd(int value, bool fromPlayer)
    {
        if (_car.IsCrashed)
            return;

        int shields = _car.Hub.Game.Saves.GetTuning(_car.CarType, TuningType.Shields);
        value -= shields * (int)_car.Config.Tuning.Shields.Power;              

        //if (value <= 0)
        //{
        //    Debug.Log("Damage was not added. Shield more than damage");
        //    return;
        //}
        //else
        //{
        //    string t = _car.IsAI ? "AI " : "Player ";
        //    Debug.Log("Car " + t + _car.CarType.ToString() + ". Damage: " + value); 
        //}

        _damage += Mathf.Max(0, value);

        float opacity = (float)_damage / 100;
        _car.DecalOpacityController.SetOpacity(opacity);

        if (_damage < 35)
        {
            FirstCrash();
        }

        if (_damage > 35 && _damage < 65)
        {
            SecondCrash();
        }

        if (_damage > 65 && _damage >= 95)         
        {
            ThirdCrash();
        }

        if (_damage >= 95)
        {
            FourthCrash(fromPlayer);
        }
    }

    private void FirstCrash()
    {        
        Emit(_smoke, true);
        _car.Control.EngineMultiplerDamage = 0.9f; 
    }

    private void SecondCrash() 
    {
        _car.Control.EngineMultiplerDamage = 0.8f;

        for (int i = 0; i < _car.Wheels.Length; i++)
        {
            WheelControl wheel = _car.Wheels[i];
            wheel.DamageRotationSet();
        }
    }

    private void ThirdCrash()
    {        
        Emit(_fire, true);
        _car.Control.EngineMultiplerDamage = 0.7f;
    }

    private void FourthCrash(bool fromPlayer)
    {
        if (_car.IsAI)
            WheelDeattachAny();
        else
            WheelDeattachRear();

        _car.Control.EngineMultiplerDamage = 1.3f;

        if (fromPlayer)
            _car.Hub.Game.Saves.Coins += _reward;

        _deadCrashTimer = 3f; // Ждем 3 секунды до полного отрыва колес
        _isWaitingDeadCrash = true;
        enabled = true;
    }

    private IEnumerator WaitDeadCrash(float time)
    {
        yield return new WaitForSeconds(time);

        for (int i = 0; i < _car.Wheels.Length; i++)
        {
            WheelControl wheel = _car.Wheels[i];
            WheelDeattach(wheel);
        }
        
        _car.IsCrashed = true;
        StartCoroutine(WaitHide(1));
    }

    private IEnumerator WaitHide(float time)
    {
        yield return new WaitForSeconds(time);
        _spirit = new GameObject().AddComponent<Spirit>();
        _spirit.Init(_car, _spiritTime);   
    }

    public void SpiritEnd()//From Spirit
    {        
        Restart();
    }

    public void Restart()
    {
        _isWaitingDeadCrash = false;
        _isWaitingHide = false;
        enabled = false;

        Emit(_smoke, false);
        Emit(_fire, false);

        for (int i = 0; i < _car.Wheels.Length; i++)
        {
            WheelControl wheel = _car.Wheels[i];
            WheelAttach(wheel, i);
            wheel.DamageRotationReset();
        }        

        _car.IsCrashed = false;
        _car.Control.EngineMultiplerDamage = 1.0f;
        _damage = 0;        
        _car.DecalOpacityController.SetOpacity(0);

        bool inWater = transform.position.y < -10;
        if (inWater)
            _car.ReturnOnRoad.MoveToNearestReturnPoint();
        
        if (_spirit != null)
        {
            Destroy(_spirit.gameObject);
        }
    }

    private void Emit(ParticleSystem particleSystem, bool value)
    {
        ParticleSystem.EmissionModule emission = particleSystem.emission;
        emission.enabled = value;
    }

    private void WheelDeattachAny()
    {        
        int rnd = Random.Range(0, _car.Wheels.Count());
        WheelDeattach(_car.Wheels[rnd]);
    }

    private void WheelDeattachRear()
    {
        bool isIdRear = false;
        int rnd = 0;
        while (!isIdRear) 
        {
            rnd = Random.Range(0, _car.Wheels.Count());
            if (!_car.Wheels[rnd].IsSteerable)
                isIdRear = true;
        }        
        WheelDeattach(_car.Wheels[rnd]);
    }

    private void WheelDeattach(WheelControl wheelControl)
    {
        if (!wheelControl.IsAttached)
            return;

        wheelControl.enabled = false;
        wheelControl.WheelCollider.enabled = false;
        wheelControl.transform.SetParent(null);
        wheelControl.WheelModel.SetParent(null);
        wheelControl.ModelMeshCollider.enabled = true;        
        wheelControl.WheelModel.AddComponent<Rigidbody>();
    }

    private void WheelAttach(WheelControl wheelControl, int id)
    {
        wheelControl.ModelMeshCollider.enabled = false;
        Rigidbody rigidbody = wheelControl.WheelModel.GetComponent<Rigidbody>();
        Destroy(rigidbody);
        wheelControl.transform.SetParent(_wheelsParent);
        wheelControl.transform.localPosition = _wheelPositions[id];
        wheelControl.transform.localRotation = Quaternion.identity;
        wheelControl.WheelCollider.enabled = true;
        wheelControl.WheelModel.SetParent(_wheelsParent);
        wheelControl.enabled = true;

        // Сбрасываем локальные координаты модельки колеса, чтобы оно не смещалось визуально после искривления
        //wheelControl.WheelModel.localPosition = Vector3.zero;
        //wheelControl.WheelModel.localRotation = Quaternion.identity;
    }
}
