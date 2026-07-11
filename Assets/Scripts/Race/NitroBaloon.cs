using UnityEngine;

public class NitroBaloon : MonoBehaviour
{
    [SerializeField] private bool _isFiring;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Car")
        {
            Car car = other.GetComponentInParent<Car>();            
            gameObject.SetActive(false);
            car.Nitro.Add();

            if (_isFiring || car.IsAI)
                car.Nitro.OnAuto();
        }
    }
}
