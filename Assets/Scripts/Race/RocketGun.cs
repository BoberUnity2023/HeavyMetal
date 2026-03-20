using UnityEngine;

public class RocketGun : MonoBehaviour
{
    [SerializeField] private Car _car;
    [SerializeField] private Transform _transformGun;
    [SerializeField] private Rocket _prefabRocket;
    
    private void Update()
    {        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TryShoot();
        }
    }

    private void TryShoot()
    {
        //Debug.Log("Shoot");
        bool _isShooted = false;
        Rocket rocket = Instantiate(_prefabRocket, _transformGun.position, _transformGun.rotation);
        foreach (Car enemy in _car.Hub.Level.Race.Enemies)
        {            
            if (CanShooted(enemy))
            {
                //Debug.Log("ShootBy: " + enemy.gameObject.name);
                rocket.Shoot(enemy.transform);
                _isShooted = true;
                break;
            }            
        }

        if (!_isShooted)
        {
            //Debug.Log("ShootFail:");
            rocket.Shoot(); 
        }
    }

    private bool CanShooted(Car enemy)
    {
        Vector3 toEnemy = transform.InverseTransformPoint(enemy.transform.position);
        return toEnemy.magnitude < 40 && //Distance
                toEnemy.z > 3 && //IsForward no back
                toEnemy.x / toEnemy.z < 0.25f; //IsForward no side            
    }
}
