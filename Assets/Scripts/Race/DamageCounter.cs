using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DamageCounter : MonoBehaviour
{
    [SerializeField] private ParticleSystem _smoke;
    [SerializeField] private ParticleSystem _fire;
    private Car _car;
    private Transform _wheelsParent;
    private Vector3[] _wheelPositions = new Vector3[4];
    private int _damage;

    //private void Update()
    //{
        //if (_car.IsAI)
        //    return;

        //if (Input.GetKeyDown(KeyCode.M))
        //    SecondCrash();

        //if (Input.GetKeyDown(KeyCode.N))
        //    ThirdCrash();
    //}

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
    
    public void DamageAdd(int value)
    {
        _damage += value;

        if (_damage < 35)
        {
            FirstCrash();
        }

        if (_damage > 60 && _damage < 90)
        {
            SecondCrash();
        }

        if (_damage >= 100)         
        {
            ThirdCrash();
        }
    }

    private void FirstCrash()
    {        
        Emit(_smoke, true);              
    }

    private void SecondCrash()
    {        
        Emit(_fire, true);
        WheelDeattach(_car.Wheels[1]);            
    }

    private void ThirdCrash()
    {
        foreach (var wheel in _car.Wheels)
        {            
            WheelDeattach(wheel);
        }
        _car.IsCrashed = true;

        StartCoroutine(WaitRestart(3));
    }

    private IEnumerator WaitRestart(float time)
    {
        yield return new WaitForSeconds(time);
        Restart();
    }

    private void Restart()
    {
        Emit(_smoke, false);
        Emit(_fire, false);

        for (int i = 0; i < 4; i++)
        {            
            WheelAttach(_car.Wheels[i], i);
        }
        
        _car.IsCrashed = false;
    }

    private void Emit(ParticleSystem particleSystem, bool value)
    {
        ParticleSystem.EmissionModule emission = particleSystem.emission;
        emission.enabled = value;
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
