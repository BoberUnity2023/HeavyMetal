using DG.Tweening;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private GameObject _prefabBlast;
    private const float speed = 30f;
    
    public void Shoot(Car car)
    {
        transform.parent = car.transform;
        float distance = Vector3.Distance(transform.position, car.transform.position);
        float time = distance / speed;
        transform.DOLocalJump(Vector3.zero, 2f, 1, 1.0f).OnComplete(() => Blast(car));
    }

    public void Shoot()
    {
        Vector3 direction = transform.forward;
        float distance = RayDistance;        
        Vector3 finish = transform.position + transform.forward * distance;
        float time = distance / speed;
        transform.DOJump(finish, 2, 1, time).OnComplete(() => Blast());
    }    

    private void Blast()
    {
        Instantiate(_prefabBlast, transform.position, Quaternion.identity);
    }

    private void Blast(Car car)
    {
        Instantiate(_prefabBlast, transform.position, Quaternion.identity, car.transform);
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
