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

            if (_isFiring || car.IsAI)
                car.Nitro.OnAuto();
            else
                car.Nitro.Add();
        }
    }
}
