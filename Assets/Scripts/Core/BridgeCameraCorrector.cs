using UnityEngine;

public class BridgeCameraCorrector : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    private bool _changed;

    private void Start()
    {
        if (_hub == null)
        {
            _hub = FindObjectOfType<Hub>();
        }
        _hub.Level.OnLevelRestartFromCheckpoint += Level_OnLevelRestartFromCheckpoint;
    }

    private void OnDestroy()
    {
        _hub.Level.OnLevelRestartFromCheckpoint -= Level_OnLevelRestartFromCheckpoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Hero") &&
            _hub.Camera.TargetState == 0)
        {
            _hub.Camera.SetByTargetState(1);
            _changed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Hero") &&
            _hub.Camera.TargetState == 1 && _changed)
        {
            _hub.Camera.SetByTargetState(0);
            _changed = false;
        }
    }

    private void Level_OnLevelRestartFromCheckpoint()
    {
        if (_changed)
        {
            _hub.Camera.SetByTargetState(0);
            _changed = false;
        }
    }
}
