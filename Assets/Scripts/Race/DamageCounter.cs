using System.Collections;
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

        _damage += value;

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

        if (fromPlayer)
            _car.Hub.Game.Saves.Coins += _rewards[0];
    }

    private void SecondCrash(bool fromPlayer)
    {        
        Emit(_fire, true);
        int rnd = Random.Range(0, 4);
        WheelDeattach(_car.Wheels[rnd]);
        _car.Control.EngineMultiplerDamage = 1.4f;

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
        
        _car.IsCrashed = false;
        _car.Control.EngineMultiplerDamage = 1.0f;
        _damage = 0;
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
