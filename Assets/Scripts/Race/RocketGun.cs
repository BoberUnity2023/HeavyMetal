using System.Collections;
using UnityEngine;

public class RocketGun : MonoBehaviour
{
    
    [SerializeField] private Transform _transformGun;
    [SerializeField] private Rocket _prefabRocket;    
    [SerializeField] private float _tryAIShootTime;
    private Car _car;
    private int _armo = 4;
    private bool _waitingNextPatron;
    private bool _isInited;

    public int Armo => _armo;

    public void Init(Car car)
    {
        _car = car;
        _isInited = true;
        _car.LapsCounter.OnLapStart += LapsCounter_OnLapStart;
        StartCoroutine(WaitAITryShoot(_tryAIShootTime));        
    }

    private void OnDestroy()
    {
        if (_isInited)
            _car.LapsCounter.OnLapStart -= LapsCounter_OnLapStart;
    }

    private void LapsCounter_OnLapStart(int obj)
    {
        _armo = 4;
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

    private void TryAIShoot()
    {
        if (_armo > 0 && _car.IsAI)
        {
            if (RayDistance < 30)
                TryShoot();
        }
    }

    private void TryShoot()
    {        
        if (_armo == 0)
            return;

        if(_car.IsFinished || !_car.Hub.Level.IsPlaying)
            return;

        if (_waitingNextPatron)
            return;

        _armo--;
        //Debug.Log("Shoot");
        bool _isShooted = false;
        Rocket rocket = Instantiate(_prefabRocket, _transformGun.position, _transformGun.rotation);
        rocket.Init(_car);
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

        StartCoroutine(WaitPatron(1.5f));
    }

    private bool CanShooted(Car enemy)
    {
        Vector3 toEnemy = transform.InverseTransformPoint(enemy.transform.position);
        return toEnemy.magnitude < 40 && //Distance
                toEnemy.z > 3 && //IsForward no back
                toEnemy.x / toEnemy.z < 0.25f; //IsForward no side            
    }

    private float RayDistance
    {
        get
        {
            RaycastHit hit;
            Vector3 from = _transformGun.position + transform.forward * 3;
            Vector3 direction = transform.forward;
            LayerMask layerMask = 1 << 11;//Layer Car

            if (Physics.Raycast(from, direction, out hit, 100, layerMask))
            {                
                return hit.distance; 
            }

            return 100;
        }
    }

    private IEnumerator WaitPatron(float time)
    {
        _waitingNextPatron = true;
        yield return new WaitForSeconds(time);
        _waitingNextPatron = false;
    }

    private IEnumerator WaitAITryShoot(float time)
    {        
        yield return new WaitForSeconds(time);
        TryAIShoot();

        if (!_car.Hub.Level.Race.Car.IsFinished)
            StartCoroutine(WaitAITryShoot(_tryAIShootTime));
    }
}
