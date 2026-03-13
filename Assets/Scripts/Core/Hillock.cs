using UnityEngine;

public class Hillock : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private float _speed;
    private float _y;
    private bool _moveDown;

    private void Start()
    {
        _y = transform.position.y;
        gameObject.tag = "Hillock";
    }

    private void FixedUpdate()
    {
        if (_moveDown)
            transform.position -= Delta;
        else
        {
            if (transform.position.y < _y && !_hub.Level.IsLost)
            {
                transform.position += Delta / 2;
            }
        }
        _moveDown = false;
    }

    public void MoveDown()
    {
        _moveDown = true;        
    }

    Vector3 Delta
    {
        get
        {
            return Vector3.up * _speed * Time.fixedDeltaTime;
        }
    }
}
