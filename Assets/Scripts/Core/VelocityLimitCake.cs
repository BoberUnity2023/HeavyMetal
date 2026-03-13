using UnityEngine;

public class VelocityLimitCake : VelocityLimit
{
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
}
