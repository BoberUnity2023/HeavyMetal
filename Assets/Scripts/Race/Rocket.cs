using DG.Tweening;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private GameObject _prefabBlast;
    [SerializeField] private float _blastForce;
    private Hub _hub;    
    private Car _attacker;
    private const float _speed = 50f;

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
        transform.DOLocalJump(Vector3.zero, 2f, 1, 1.0f).OnComplete(() => Blast(car));
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
        OnFlyUpdate();
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
        car.DamageCounter.DamageAdd(34);
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
}
