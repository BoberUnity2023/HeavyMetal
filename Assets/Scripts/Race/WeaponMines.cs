using System.Collections;
using UnityEngine;

public class WeaponMines : MonoBehaviour
{
    [SerializeField] private Transform _transformGun;
    [SerializeField] private Mine _prefabMine;    
    [SerializeField] private float _tryAIShootTime;
    private Car _car;
    private float _aiShootTimer;
    private float _patronCooldownTimer;
    private int _armo;
    private int _tuningArmo;
    private bool _waitingNextPatron;
    private bool _isInited;

    public int Armo => _armo;

    public int ArmoMax => _car.Game.ConfigGame.Car(_car.CarType).StartMines + _tuningArmo;

    public void Init(Car car)
    {
        _car = car;
        if (_car.Mode == Mode.Track)
        {
            enabled = true;
            _isInited = true;
            ConfigCar configCar = _car.Game.ConfigGame.Car(_car.CarType);
            _armo = configCar.StartMines;
            _car.LapsCounter.OnLapStart += LapsCounter_OnLapStart;
            _aiShootTimer = _tryAIShootTime;
        }
    }

    public void SetTuningWeapon(int weapons)
    {
        _tuningArmo = weapons;
        _armo = ArmoMax;
    }

    private void OnDestroy()
    {
        if (_isInited)
            _car.LapsCounter.OnLapStart -= LapsCounter_OnLapStart;
    }

    private void LapsCounter_OnLapStart(int obj)
    {
        _armo = ArmoMax;
    }

    private void Update()
    {
        if (Time.timeScale == 0 || _car.Hub.IsPaused)
            return;
        
        if (_waitingNextPatron)
        {
            _patronCooldownTimer -= Time.deltaTime;
            if (_patronCooldownTimer <= 0)
            {
                _waitingNextPatron = false;
            }
        }

        Update_AI();
        Update_Player();
    }

    private void Update_AI()
    {
        if (_car.IsAI)
        {
            // ОПТИМИЗАЦИЯ: Если бот далеко/вне экрана — не пускаем луч и экономим CPU
            if (!_car.IsVisible)
                return;

            _aiShootTimer += Time.deltaTime;

            // Вычисляем целевой интервал в зависимости от текущего круга бота
            float currentTargetTime = _car.LapsCounter.Lap == 1 ? _tryAIShootTime * 3 : _tryAIShootTime;

            if (_aiShootTimer >= currentTargetTime)
            {
                _aiShootTimer = 0f;
                if (!_car.Hub.Level.Race.Car.IsFinished)
                {
                    TryAIShoot();
                }
            }
            return; // Выходим из Update для ИИ
        }
    }

    private void Update_Player()
    {
        if (!_car.IsAI)
        {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                TryShoot();
            }
        }           
    }

    private void TryAIShoot()
    {
        if (_armo > 0 && _car.IsAI && _car.IsVisible)
        {
            if (RayDistance < 30)
                TryShoot();
        }
    }

    private void TryShoot()
    {
        if (_armo == 0)
            return;

        if (_car.IsFinished || !_car.Hub.Level.IsPlaying)
            return;

        if (_waitingNextPatron)
            return;

        _armo--;
        //Debug.Log("Shoot");
        bool _isShooted = false;
        Mine mine = Instantiate(_prefabMine, _transformGun.position, _transformGun.rotation);

        _waitingNextPatron = true;
        _patronCooldownTimer = 0.8f;
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
            Vector3 direction = -transform.forward;
            LayerMask layerMask = 1 << 11;//Layer Car

            if (Physics.Raycast(from, direction, out hit, 100, layerMask))
            {
                return hit.distance;
            }

            return 100;
        }
    }
}
