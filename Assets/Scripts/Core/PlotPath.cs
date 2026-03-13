using System;
using System.Collections;
using UnityEngine;

public class PlotPath : MonoBehaviour
{
    [SerializeField] private Hub _hub;    

    [SerializeField] private Transform _start;
    [SerializeField] private Transform _end;
    [SerializeField] private float _timeMove;
    [SerializeField] private float _activeDistance;

    private void Start()
    {
        StartCoroutine(MoveForwardCoroutine(0));
    }

    private IEnumerator MoveBackCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(MoveForwardCoroutine(_timeMove+0.2f));

        
    }

    private IEnumerator MoveForwardCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(MoveBackCoroutine(_timeMove + 0.2f));

        
    }

    private float DistanceToHero
    {
        get
        {
            return 0;// Vector3.Distance(_hub.Hero.transform.position, transform.position);
        }
    }
}
