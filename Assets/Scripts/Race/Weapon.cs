using System.Collections;
using UnityEngine;

public enum WeaponType
{
    MachineGun,
    RocketLauncher
}

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponType _type;
    [SerializeField] private GameObject[] _tunings;
    [SerializeField] private Rocket _prefabRocket;
    [SerializeField] private Transform _shootPosition;
    [SerializeField] private int _queue;
    [SerializeField] private float _reloadTime;
    private Car _car;
    private bool _waitingReload;

    public WeaponType Type => _type;

    public GameObject[] Tunings => _tunings;

    public Rocket PrefabRocket => _prefabRocket;

    public Transform ShootPosition => _shootPosition;

    public void Init(Car car)
    {
        _car = car;
    }

    public void SetTuning(int tuning)
    {
        for (int i = 0; i < _tunings.Length; i++)
        {
            _tunings[i].SetActive(i == tuning);
        }
    }

    public void TryShoot()
    {
        if (_waitingReload)
            return;

        CreateRocket(_shootPosition);

        for (int i = 0; i < _queue; i++)
        {
            StartCoroutine(NextShoot(0.1f * i, _shootPosition));
        }

        StartCoroutine(WaitPatron(_reloadTime));
    }

    private void CreateRocket(Transform shootPosition)
    {
        Rocket rocket = Instantiate(PrefabRocket, shootPosition.position, shootPosition.rotation);
        rocket.Init(_car);
    }

    private IEnumerator NextShoot(float time, Transform shootPosition)
    {
        yield return new WaitForSeconds(time);
        CreateRocket(shootPosition);        
    }

    private IEnumerator WaitPatron(float time)
    {
        _waitingReload = true;
        yield return new WaitForSeconds(time);
        _waitingReload = false;
    }
}
