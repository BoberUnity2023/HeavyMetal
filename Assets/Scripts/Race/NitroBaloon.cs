using UnityEngine;

public class NitroBaloon : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Car")
        {
            Car car = other.GetComponentInParent<Car>();
            car.Nitro.On();
            gameObject.SetActive(false);
        }
    }
}
