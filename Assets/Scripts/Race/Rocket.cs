using DG.Tweening;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private GameObject _prefabBlast;
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private float _blastForce;
    [SerializeField] private float _verticalSpeed;
    [SerializeField] private bool _isMoveOverGround;
    private Hub _hub;    
    private Car _attacker;    
    private const float _height = 1.8f;

    public void Init(Car attacker)
    {        
        _attacker = attacker;
        _hub = _attacker.Hub;
    }

    public void Shoot(Car car)
    {
        transform.parent = car.transform;
        float distance = Vector3.Distance(transform.position, car.transform.position);
        float time = distance / _speed;
        transform.DOLocalJump(Vector3.zero, 0f, 1, 1.0f).OnComplete(() => Blast(car));
    }

    public void Shoot()
    {
        Vector3 direction = transform.forward;
        float distance = RayDistance;        
        Vector3 finish = transform.position + transform.forward * distance;
        float time = distance / _speed;
        transform.DOJump(finish, 0, 1, time).
            //OnUpdate(() => OnFlyUpdate()).
            OnComplete(() => Blast());
    }

    private void FixedUpdate()
    {
        Vector3 direction = transform.forward.normalized;
        transform.position += direction * _speed * Time.fixedDeltaTime;
        FixedUpdate_MoveOverGround();
        OnFlyUpdate();
    }

    private void FixedUpdate_MoveOverGround()
    {
        if (!_isMoveOverGround)
            return;

        float heightCorrect = 0;
        
        if (RayDown < 90)
            heightCorrect = RayDown - _height;

        transform.position -= Vector3.up * Mathf.Clamp(heightCorrect, -_verticalSpeed * Time.fixedDeltaTime, _verticalSpeed * Time.fixedDeltaTime);
    }

    private void OnFlyUpdate()
    {
        foreach (Car car in _hub.Level.Race.Cars)
        {
            if (car != _attacker)
            {
                float distance = Vector3.Distance(transform.position, car.transform.position);
                if (distance < 2.5)
                {
                    Blast(car);
                    Destroy(gameObject);                    
                }
            }
        }
    }

    private void Blast()
    {
        Instantiate(_prefabBlast, transform.position, Quaternion.identity);
    }

    private void Blast(Car car)
    {
        Instantiate(_prefabBlast, transform.position, Quaternion.identity, car.transform);
        Vector3 direction = (car.transform.position - transform.position).normalized;
        car.Rigidbody.AddForce(direction * _blastForce);
        bool fromPlayer = !_attacker.IsAI;
        car.DamageCounter.DamageAdd(_damage, fromPlayer);
    }

    private float RayDistance
    {
        get
        {
            RaycastHit hit;            
            Vector3 direction = transform.forward;
            
            if (Physics.Raycast(transform.position, direction, out hit, 100))                            
                return hit.distance;
              
            return 100;            
        }
    }

    private float RayDown
    {
        get
        {            
            Vector3 direction = -transform.up;

            RaycastHit[] hits;
            hits = Physics.RaycastAll(transform.position, direction, 100.0F);

            for (int i = 0; i < hits.Length - 1; i++)//Without Blocker
            {
                RaycastHit hit = hits[i];
                if (_hub.Game.IsMaterialGround(hit.collider.material))
                {
                    return hit.distance;
                }
            }

            return 100;
        }
    }
}
