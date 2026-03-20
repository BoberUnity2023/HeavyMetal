using UnityEngine;

public class DamageCounter : MonoBehaviour
{
    [SerializeField] private Car _car;

    private int _damage;
    [SerializeField] private WheelCollider[] _wheels;

    private void Start()
    {
        _wheels = GetComponentsInChildren<WheelCollider>();
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
            PrePreCrash();
        }

        if (_damage > 60 && _damage < 90)
        {
            PreCrash();
        }

        if (_damage >= 100)         
        {
            Crash();
        }
    }

    private void PrePreCrash()
    {
        //Debug.Log("PreCrash()");
        //_wheels[0].enabled = false;
        //_wheels[0].transform.SetParent(null);
    }

    private void PreCrash()
    {
        Debug.Log("PreCrash()");
        _wheels[1].enabled = false;
        _wheels[1].transform.SetParent(null);
    }

    private void Crash()
    {
        foreach (var wheel in _wheels)
        {
            wheel.enabled = false;
            wheel.transform.SetParent(null);
        }
        _car.IsCrashed = true;
    }
}
