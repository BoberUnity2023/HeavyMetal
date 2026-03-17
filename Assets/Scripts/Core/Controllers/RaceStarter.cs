using System;
using System.Collections;
using UnityEngine;

public class RaceStarter : MonoBehaviour
{
    [SerializeField] private Hub _hub;

    public event Action<int> OnTimer;
    public event Action OnStartRace;

    private void Start()
    {
        StartCoroutine(Show3(0.3f));
    }    

    private IEnumerator Show3(float time)
    {
        yield return new WaitForSeconds(time);
        OnTimer?.Invoke(3);
        StartCoroutine(Show2(0.6f));
    }

    private IEnumerator Show2(float time)
    {
        yield return new WaitForSeconds(time);
        OnTimer?.Invoke(2);
        StartCoroutine(Show1(0.6f));
    }

    private IEnumerator Show1(float time)
    {
        yield return new WaitForSeconds(time);
        OnTimer?.Invoke(1);
        StartCoroutine(Show0(0.6f));
    }
    private IEnumerator Show0(float time)
    {
        yield return new WaitForSeconds(time);
        OnTimer?.Invoke(0);
        StartCoroutine(Hide(0.3f)); ;
    }

    private IEnumerator Hide(float time)
    {
        yield return new WaitForSeconds(time);
        OnStartRace?.Invoke();
        StartRace();
    }

    private void StartRace()
    {
        _hub.Level.Race.StartRace();
    }
}
