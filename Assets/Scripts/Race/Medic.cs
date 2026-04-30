using UnityEngine;

public class Medic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Car")
        {
            Car car = other.GetComponentInParent<Car>();
            if (car.DamageCounter.Damage > 0)
            {
                car.DamageCounter.Restart();
                gameObject.SetActive(false);
            }            
        }
    }
}
