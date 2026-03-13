using System.Collections;
using UnityEngine;

public class AutoRestart : MonoBehaviour
{
    [SerializeField] private Hub _hub;

    private void Start()
    {
        _hub.Level.OnLevelLost += OnLevelLost;
        _hub.CanvasLevel.OnPressRestart += OnAnyPressed;
        _hub.CanvasLevel.OnPressRestartFromCheckpoint += OnAnyPressed;
        _hub.CanvasLevel.OnPressPanelOpen += OnAnyPressed;
    }    

    private void OnDestroy()
    {
        _hub.Level.OnLevelLost -= OnLevelLost;
        _hub.CanvasLevel.OnPressRestart -= OnAnyPressed;
        _hub.CanvasLevel.OnPressRestartFromCheckpoint -= OnAnyPressed;
        _hub.CanvasLevel.OnPressPanelOpen -= OnAnyPressed;
    }

    private void OnLevelLost()
    {
        StartCoroutine(RestartAfter(Time));
    }

    private IEnumerator RestartAfter(float time)
    {
        yield return new WaitForSeconds(time);
        
    }

    private void OnAnyPressed()
    {
        StopAllCoroutines();
    }

    float Time
    {
        get 
        {
            

            return 5;
        }
    }
}
