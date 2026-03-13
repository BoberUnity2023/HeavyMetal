using UnityEngine;

public class RestoredFromCheckpointRigidbody : RestoredFromCheckpoint
{
    [SerializeField] private Rigidbody _rigidbody;

    protected override void Level_OnLevelRestartFromCheckpoint()
    {
        base.Level_OnLevelRestartFromCheckpoint();
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }
}
