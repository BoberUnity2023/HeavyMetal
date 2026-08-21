using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DamageCounter : MonoBehaviour
{
    [SerializeField] private ParticleSystem _smoke;
    [SerializeField] private ParticleSystem _fire;
    [SerializeField] private int[] _rewards;
    private Spirit _spirit;
    private Car _car;
    private Transform _wheelsParent;
    private Vector3[] _wheelPositions = new Vector3[4];
    private int _damage;
    
    public void Init(Car car)
    {  
        _car = car;
        _wheelsParent = _car.Wheels[0].transform.parent;
        for (int i = 0; i < 4; i++)
        {
            _wheelPositions[i] = _car.Wheels[i].transform.localPosition;
        }
    }

    public int Damage 
    {         
        get => _damage; 
        private set => _damage = value;  
    }
    
    public void DamageAdd(int value, bool fromPlayer)
    {
        if (_car.IsCrashed)
            return;

        int shields = _car.Hub.Game.Saves.GetTuning(_car.CarType, TuningType.Shields);
        value -= shields;

        if (value <= 0)
        {
            Debug.Log("Damage was not added. Shield more than damage");
            return;
        }

        _damage += Mathf.Max(0, value);

        if (_damage < 35)
        {
            FirstCrash(fromPlayer);
        }

        if (_damage > 60 && _damage < 90)
        {
            SecondCrash(fromPlayer);
        }

        if (_damage >= 100)         
        {
            ThirdCrash(fromPlayer);
        }
    }

    private void FirstCrash(bool fromPlayer)
    {        
        Emit(_smoke, true);
        _car.Control.EngineMultiplerDamage = 0.85f;
        if (fromPlayer)
            _car.Hub.Game.Saves.Coins += _rewards[0];

        foreach (WheelControl wheel in _car.Wheels)
        {
            wheel.DamageRotationSet();
        }
    }

    private void SecondCrash(bool fromPlayer)
    {        
        Emit(_fire, true);

        if (_car.IsAI)
            WheelDeattachAny();
        else
            WheelDeattachRear();

        _car.Control.EngineMultiplerDamage = 1.3f;

        if (fromPlayer)
            _car.Hub.Game.Saves.Coins += _rewards[1];
    }

    private void ThirdCrash(bool fromPlayer)
    {
        foreach (var wheel in _car.Wheels)
        {            
            WheelDeattach(wheel);
        }
        _car.IsCrashed = true;
        StartCoroutine(WaitHide(1));
        
        if (fromPlayer)
            _car.Hub.Game.Saves.Coins += _rewards[2];
    }

    private IEnumerator WaitHide(float time)
    {
        yield return new WaitForSeconds(time);
        _spirit = new GameObject().AddComponent<Spirit>();
        _spirit.Init(_car, 3);   
    }

    public void SpiritEnd()//From Spirit
    {        
        Restart();
    }

    public void Restart()
    {
        Emit(_smoke, false);
        Emit(_fire, false);

        for (int i = 0; i < 4; i++)
        {            
            WheelAttach(_car.Wheels[i], i);
        }

        foreach (WheelControl wheel in _car.Wheels)
        {
            wheel.DamageRotationReset();
        }

        _car.IsCrashed = false;
        _car.Control.EngineMultiplerDamage = 1.0f;
        _damage = 0;

        bool inWater = transform.position.y < -10;
        if (inWater)
            _car.ReturnOnRoad.MoveToNearestReturnPoint();
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
    }
}
