using System.Collections;
using UnityEngine;

public class DamageCounter : MonoBehaviour
{
    [SerializeField] private Car _car;    
    [SerializeField] private WheelCollider[] _wheels;
    private Transform _wheelsParent;
    private Vector3[] _wheelPositions = new Vector3[4];
    private int _damage;

    private void Start()
    {
        _wheels = GetComponentsInChildren<WheelCollider>();
        _wheelsParent = _wheels[0].transform.parent;
        for (int i = 0; i < 4; i++)
        {
            _wheelPositions[i] = _wheels[i].transform.localPosition;
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

        StartCoroutine(WaitRestart(3));
    }

    private IEnumerator WaitRestart(float time)
    {
        yield return new WaitForSeconds(time);
        Restart();
    }

    private void Restart()
    {
        for (int i = 0; i < 4; i++)
        {
            _wheels[i].transform.SetParent(_wheelsParent);
            _wheels[i].transform.localPosition = _wheelPositions[i];
            _wheels[i].transform.localRotation = Quaternion.identity;            
            _wheels[i].enabled = true;
        }

        foreach (var wheel in _wheels)
        {
                      
        }
        _car.IsCrashed = false;
    }
}
