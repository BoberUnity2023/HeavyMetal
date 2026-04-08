using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private GameObject _blastPrefab;
    [SerializeField] private float _blastForce;
    [Range(0, 100)][SerializeField] private int _createChance;    

    private void Awake()
    {
        SetActive();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Car")
        {
            Car car = other.GetComponentInParent<Car>();
            Crash(car);
        }
    }

    private void Crash(Car car)
    {
        if (!car.IsVisible)
            return;
        Instantiate(_blastPrefab, transform.position, Quaternion.identity);

        Vector3 direction = (car.transform.position - transform.position).normalized;
        car.Rigidbody.AddForce(direction * _blastForce);
        car.DamageCounter.DamageAdd(34);

        gameObject.SetActive(false);
    }

    private void SetActive()
    {
        int rnd = Random.Range(0, 100);

        if (rnd > _createChance)
            gameObject.SetActive(false);
    }
}
