using System.Collections;
using UnityEngine;

public class Spirit : MonoBehaviour
{
    private Car _car;
    
    public void Init(Car car, float time)
    {        
        _car = car;        
        Hide(time);
    }

    private void Hide(float time)
    {
        _car.gameObject.SetActive(false);
        TimeLive[] particleSystems = _car.GetComponentsInChildren<TimeLive>();
        foreach (TimeLive particleSystem in particleSystems)
        {
            particleSystem.transform.parent = null;
        }
        StartCoroutine(Wait(time));
    }

    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        
        float dist = Vector3.Distance(_car.Hub.Level.Race.Car.transform.position, _car.transform.position);
        if (!_car.IsAI || dist > 2)
        {
            End();
        }
        else
            StartCoroutine(Wait(1));
    }

    private void End()
    {
        _car.gameObject.SetActive(true);
        _car.DamageCounter.SpiritEnd();
        Destroy(gameObject);
    }
}
