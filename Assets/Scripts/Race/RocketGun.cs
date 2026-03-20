using UnityEngine;

public class RocketGun : MonoBehaviour
{
    [SerializeField] private Car _car;
    [SerializeField] private Transform _transformGun;
    [SerializeField] private Rocket _prefabRocket;
    [SerializeField] private float _d;

    void Start()
    {
        
    }
    
    private void Update()
    {
        _d = RayDistance;
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TryShoot();
        }
    }

    private void TryShoot()
    {
        Debug.Log("Shoot");

        foreach (Car enemy in _car.Hub.Level.Race.Enemies)
        {            
            if (CanShooted(enemy))
            {
                ShootBy(enemy);
                break;
            }
        }        
    }

    private bool CanShooted(Car enemy)
    {
        Vector3 toEnemy = transform.InverseTransformPoint(enemy.transform.position);
        return toEnemy.magnitude < 40 && //Distance
                toEnemy.z > 3 && //IsForward no back
                toEnemy.x / toEnemy.z < 0.25f; //IsForward no side            
    }

    private void ShootBy(Car enemy)
    {
        Debug.Log("ShootBy: " + enemy.gameObject.name);
        Rocket rocket = Instantiate(_prefabRocket, _transformGun.position, _transformGun.rotation);
        rocket.Init(enemy.transform);
    }
    
    float RayDistance
    {
        get
        {
            RaycastHit hit;

            Vector3 direction = transform.forward;
            if (Physics.Raycast(transform.position, direction, out hit, 100))
            {
                Debug.DrawRay(transform.position, direction * hit.distance, Color.yellow);
                return hit.distance;
            }
            else
            {
                Debug.DrawRay(transform.position, direction * 100, Color.red);
                //Debug.LogWarning("Camera. Did not Hit");
                return 1000;
            }
        }
    }
}
