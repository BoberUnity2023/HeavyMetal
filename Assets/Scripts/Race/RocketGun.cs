using UnityEngine;

public class RocketGun : MonoBehaviour
{
    [SerializeField] private Car _car;
    [SerializeField] private Transform _transformGun;
    [SerializeField] private Rocket _prefabRocket;
    private int _armo = 14;

    public int Armo => _armo;

    private void Start()
    {
        _car.LapsCounter.OnLapStart += LapsCounter_OnLapStart;
    }

    private void OnDestroy()
    {
        _car.LapsCounter.OnLapStart -= LapsCounter_OnLapStart;
    }

    private void LapsCounter_OnLapStart(int obj)
    {
        _armo = 14;
    }

    private void Update()
    {
        if (_car.IsAI)
            return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            TryShoot();
        }
    }

    private void TryShoot()
    {
        if (_armo == 0)
            return;

        if(_car.IsFinished || !_car.Hub.Level.IsPlaying)
            return;

        _armo--;
        //Debug.Log("Shoot");
        bool _isShooted = false;
        Rocket rocket = Instantiate(_prefabRocket, _transformGun.position, _transformGun.rotation);
        rocket.Init(_car.Hub);
        //foreach (Car enemy in _car.Hub.Level.Race.Enemies)
        //{            
        //    if (CanShooted(enemy))
        //    {
        //        //Debug.Log("ShootBy: " + enemy.gameObject.name);
        //        rocket.Shoot(enemy);
        //        _isShooted = true;
        //        break;
        //    }            
        //}

        if (!_isShooted)
        {
            //Debug.Log("ShootFail:");
            //rocket.Shoot(); 
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
