using UnityEngine;

public class RestoredFromCheckpoint : MonoBehaviour
{
    //объект восстанавливает своё положение при возврате игрока на чекпоинт
    [SerializeField] private Hub _hub;
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    
    private void Start()
    {
        _hub.Level.OnLevelRestartFromCheckpoint += Level_OnLevelRestartFromCheckpoint;
        _startPosition = transform.position;
        _startRotation = transform.rotation;
    }
    private void OnDestroy()
    {
        _hub.Level.OnLevelRestartFromCheckpoint -= Level_OnLevelRestartFromCheckpoint;
    }

    protected virtual void Level_OnLevelRestartFromCheckpoint()
    {
        transform.position = _startPosition;
        transform.rotation = _startRotation;
    }
}
