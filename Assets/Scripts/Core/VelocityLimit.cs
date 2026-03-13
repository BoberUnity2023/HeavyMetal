using UnityEngine;

public class VelocityLimit : MonoBehaviour
{
    protected Rigidbody _rigidbody;
    private const int _maxVelocity = 3;

    private void FixedUpdate()
    {
        if (_rigidbody.linearVelocity.magnitude > _maxVelocity)
        {            
            _rigidbody.linearVelocity = _rigidbody.linearVelocity.normalized * _maxVelocity;
        }
    }

    public void SetRigidbody(Rigidbody rigidbody)
    {
        _rigidbody = rigidbody;
    }
}
